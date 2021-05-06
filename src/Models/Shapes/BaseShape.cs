using System;
using System.Numerics;
using Designer.Controllers.App;

namespace Designer.Models.Shapes {
	public class BaseShape : IShape {
		private Vector2 position = Vector2.Zero;
		private Vector2 size = Vector2.Zero;

		private ShapeWrapper shapeWrapper = null;

		private ShapeIdentifier identifier = null;

		private ShapeStrategy strategy = null;

		public BaseShape(Vector2 position, Vector2 size, ShapeStrategy strategy) {
			this.position = position;
			this.size = size;

			this.strategy = strategy;
			
			this.identifier = new ShapeIdentifier(this);
		}

		public Vector2 GetPosition() {
			return this.position;
		}
		
		public void SetPosition(Vector2 position) {
			this.position = position;
		}

		public Vector2 GetSize() {
			return this.size;
		}

		public void SetSize(Vector2 size) {
			this.size = size;
		}

		public ShapeIdentifier GetIdentifier() {
			return this.identifier;
		}
		
		public void Accept(IShapeVisitor shapeVisitor) {
			shapeVisitor.Visit(this);
		}
		
		public ShapeStrategy GetStrategy() {
			return this.strategy;
		}

		public void SetWrapper(ShapeWrapper wrapper) {
			this.shapeWrapper = wrapper;
		}

		public ShapeWrapper GetWrapper() {
			return this.shapeWrapper;
		}

		public void Destroy() { this.GetWrapper().Destroy(); }
		public void Restore() {
			this.GetWrapper().Restore();
		}
	}
}