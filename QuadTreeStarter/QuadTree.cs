using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace QuadTreeStarter
{
	class QuadTreeNode
	{
		#region Constants
		// The maximum number of objects in a quad
		// before a subdivision occurs
		private const int MAX_OBJECTS_BEFORE_SUBDIVIDE = 3;
		#endregion

		#region Variables
		// The game objects held at this level of the tree
		private List<GameObject> _objects;

		// This quad's rectangle area
		private Rectangle _rect;

		// This quad's divisions
		private QuadTreeNode[] _divisions;
		#endregion

		#region Properties
		/// <summary>
		/// The divisions of this quad
		/// </summary>
		public QuadTreeNode[] Divisions { get { return _divisions; } }

		/// <summary>
		/// This quad's rectangle
		/// </summary>
		public Rectangle Rectangle { get { return _rect; } }

		/// <summary>
		/// The game objects inside this quad
		/// </summary>
		public List<GameObject> GameObjects { get { return _objects; } }
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new Quad Tree
		/// </summary>
		/// <param name="x">This quad's x position</param>
		/// <param name="y">This quad's y position</param>
		/// <param name="width">This quad's width</param>
		/// <param name="height">This quad's height</param>
		public QuadTreeNode(int x, int y, int width, int height)
		{
			// Save the rectangle
			_rect = new Rectangle(x, y, width, height);

			// Create the object list
			_objects = new List<GameObject>();

			// No divisions yet
			_divisions = null;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds a game object to the quad.  If the quad has too many
		/// objects in it, and hasn't been divided already, it should
		/// be divided
		/// </summary>
		/// <param name="gameObj">The object to add</param>
		public void AddObject(GameObject gameObj)
		{
			// ACTIVITY: Complete this method
			if (_rect.Contains(gameObj.Rectangle))
			{
				if (_divisions == null)
					_objects.Add(gameObj);

				//If the gameObj is not contained within any of the divisions, add it to the current Quad
				//This deals with gameObjs that are in multiple divisions
				else if (!_divisions[0].Rectangle.Contains(gameObj.Rectangle) && !_divisions[1].Rectangle.Contains(gameObj.Rectangle)
						&& !_divisions[2].Rectangle.Contains(gameObj.Rectangle) && !_divisions[3].Rectangle.Contains(gameObj.Rectangle))
					_objects.Add(gameObj);

				//Oterwise add it to the division it is contined within
				else
				{
					foreach (QuadTreeNode division in _divisions)
					{
						if (division.Rectangle.Contains(gameObj.Rectangle))
							division.AddObject(gameObj);
					}
				}

				if (_objects.Count > MAX_OBJECTS_BEFORE_SUBDIVIDE && _divisions == null)
					Divide();
			}
		}

		/// <summary>
		/// Divides this quad into 4 smaller quads.  Moves any game objects
		/// that are completely contained within the new smaller quads into
		/// those quads and removes them from this one.
		/// </summary>
		public void Divide()
		{
			// ACTIVITY: Complete this method
			//Divides the quad
			_divisions = new QuadTreeNode[4];
			_divisions[0] = new QuadTreeNode(_rect.X, _rect.Y, _rect.Width / 2, _rect.Height / 2);
			_divisions[1] = new QuadTreeNode(_rect.X + _rect.Width / 2, _rect.Y, _rect.Width / 2, _rect.Height / 2);
			_divisions[2] = new QuadTreeNode(_rect.X, _rect.Y + _rect.Height / 2, _rect.Width / 2, _rect.Height / 2);
			_divisions[3] = new QuadTreeNode(_rect.X + _rect.Width / 2, _rect.Y + _rect.Height / 2, _rect.Width / 2, _rect.Height / 2);

			//Moves game objects into thier smaller quads
			foreach (QuadTreeNode divison in _divisions)
			{
				for (int i = 0; i < _objects.Count; i++)
				{
					if (divison.Rectangle.Contains(_objects[i].Rectangle))
					{
						divison.AddObject(_objects[i]);
						_objects.RemoveAt(i);
						i--;
					}
				}
			}
		}

		/// <summary>
		/// Recursively populates a list with all of the rectangles in this
		/// quad and any subdivision quads.  Use the "AddRange" method of
		/// the list class to add the elements from one list to another.
		/// </summary>
		/// <returns>A list of rectangles</returns>
		public List<Rectangle> GetAllRectangles()
		{
			List<Rectangle> rects = new List<Rectangle>();

			// ACTIVITY: Complete this method
			rects.Add(_rect);

			if (_divisions != null)
				foreach (QuadTreeNode division in _divisions)
					rects.AddRange(division.GetAllRectangles());

			return rects;
		}

		/// <summary>
		/// A possibly recursive method that returns the
		/// smallest quad that contains the specified rectangle
		/// </summary>
		/// <param name="rect">The rectangle to check</param>
		/// <returns>The smallest quad that contains the rectangle</returns>
		public QuadTreeNode GetContainingQuad(Rectangle rect)
		{
			// ACTIVITY: Complete this method
			if (_rect.Contains(rect))
			{
				QuadTreeNode smallest = this;

				if (_divisions != null)
					foreach (QuadTreeNode division in _divisions)
					{
						if (division.GetContainingQuad(rect) != null)
							smallest = division.GetContainingQuad(rect);
					}

				return smallest;
			}
			// Return null if this quad doesn't completely contain
			// the rectangle that was passed in
			return null;
		}
		#endregion
	}
}
