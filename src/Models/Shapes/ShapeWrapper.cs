using System;
using System.Collections.Generic;
using Designer.Controllers.App;

namespace Designer.Models.Shapes {
	public class ShapeWrapper {
		public delegate void DestroyedHandler(ShapeWrapper shapeWrapper, List<IShapeContainer> interested);
		public event DestroyedHandler Destroyed;

		private List<IShapeContainer> interested = null;

		private IShape shape = null;
		
		public ShapeWrapper(IShape shape) {
			this.shape = shape;
			this.shape.SetWrapper(this);
		}

		public void SetShape(IShape shape) {
			this.shape.SetWrapper(null);

			this.shape = shape;
			this.shape.SetWrapper(this);
		}

		public List<IShapeContainer> Destroy() {
			interested = new List<IShapeContainer>();
		
			Destroyed?.Invoke(this, interested);

			return interested;
		}

		public void Restore() {
			foreach (var shapeContainer in this.interested)
				shapeContainer.AddShape(this);
		}

		public IShape GetShape() {
			return shape;
		}
	}
}