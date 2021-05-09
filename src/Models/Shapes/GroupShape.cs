using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Designer.Controllers.App;

namespace Designer.Models.Shapes {
	public class GroupShape : IShape, IShapeContainer {
		private List<ShapeWrapper> children = null;

		private ShapeWrapper shapeWrapper = null;

		private ShapeIdentifier identifier = null;

		private List<ShapeWrapper> shapesToDestroy = new List<ShapeWrapper>();

		public GroupShape() {
			this.children = new List<ShapeWrapper>();

			this.identifier = new ShapeIdentifier(this);
		}

		public void AddShape(ShapeWrapper shape) {
			this.children.Add(shape);

			shape.Destroyed += HandleDestroyed;
		}

		public void RemoveShape(ShapeWrapper shape) {
			this.children.Remove(shape);
			shape.Destroyed -= HandleDestroyed;
		}

		private void HandleDestroyed(ShapeWrapper shapeWrapper, List<IShapeContainer> interested) {
			this.RemoveShape(shapeWrapper);

			interested.Add(this);
		}

		public IReadOnlyList<ShapeWrapper> GetChildren() {
			return this.children;
		}

		public Vector2 GetPosition() {
			if (this.children.Count == 0)
				return new Vector2(0, 0);

			var positions = this.children.Select(x => x.GetShape().GetPosition());

			float minX = positions.Select(v => v.X).Min();
			float minY = positions.Select(v => v.Y).Min();

			return new Vector2(minX, minY);
		}
		
		public void SetPosition(Vector2 position) {
			Vector2 delta = position - this.GetPosition();

			foreach (var child in this.children)
				child.GetShape().SetPosition(child.GetShape().GetPosition() + delta);
		}

		public Vector2 GetSize() {
			Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 max = new Vector2(float.MinValue, float.MinValue);
			
			foreach (var childWrapper in this.children) {
				var child = childWrapper.GetShape();

				min.X = MathF.Min(min.X, child.GetPosition().X);
				min.Y = MathF.Min(min.Y, child.GetPosition().Y);

				max.X = MathF.Max(max.X, child.GetPosition().X + child.GetSize().X);
				max.Y = MathF.Max(max.Y, child.GetPosition().Y + child.GetSize().Y);
			}
			
			return max - min;
		}

		public void SetSize(Vector2 size) {
			var groupPosition = this.GetPosition();
			var relation = size / this.GetSize();

			foreach (var childWrapper in this.children) {
				var child = childWrapper.GetShape();

				var childPosition = child.GetPosition();
				var relativePosition = childPosition - groupPosition;

				child.SetSize(child.GetSize() * relation);
				child.SetPosition((relativePosition * relation) + groupPosition);
			} 
		}

		public ShapeIdentifier GetIdentifier() {
			return this.identifier;
		}
		
		public void Accept(IShapeVisitor shapeVisitor) {
			shapeVisitor.Visit(this);
		}

		public void SetWrapper(ShapeWrapper wrapper) {
			this.shapeWrapper = wrapper;
		}

		public ShapeWrapper GetWrapper() {
			return this.shapeWrapper;
		}

		public void Clear() {
			this.children.Clear();
		}

		public void Destroy() {
			shapesToDestroy.Clear();

			foreach (var childWrapper in children) {
				shapesToDestroy.Add(childWrapper);
			}

			foreach (var shapeToDestroy in shapesToDestroy) {
				shapeToDestroy.GetShape().Destroy();
			}

			this.GetWrapper().Destroy();
		}

		public void Restore() {
			this.GetWrapper().Restore();

			foreach (var shapeToDestroy in shapesToDestroy) {
				shapeToDestroy.GetShape().Restore();
			}
		}
	}
}