using System.Collections.Generic;
using Designer.Utility;
using Love;
using Designer.Shapes;

namespace Designer {
	public class SelectionBox {
		private Vector2 startPosition = Vector2.Zero;
		private Vector2 endPosition = Vector2.Zero;

		private DirtyCollection<List<ShapeBase>> selectedShapes;

		public SelectionBox(Vector2 startPosition, Vector2 endPosition) {
			this.startPosition = startPosition;
			this.endPosition = endPosition;

			this.selectedShapes = new DirtyCollection<List<ShapeBase>>(new List<ShapeBase>());
		}

		public void SetEndPosition(Vector2 endPosition) {
			this.endPosition = endPosition;

			this.selectedShapes.SetDirty(true);
		}

		public void Draw() {
			Vector2 topLeft = Vector2.Min(startPosition, endPosition);
			Vector2 bottomRight = Vector2.Max(startPosition, endPosition);
			Vector2 size = bottomRight - topLeft;

			Graphics.SetColor(0.608f, 0.369f, 1.000f, 0.518f);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X, topLeft.Y, size.X, size.Y);
		}

		public List<ShapeBase> GetSelectedShapes(List<ShapeBase> shapes) {
			if (selectedShapes.IsDirty()) {
				selectedShapes.GetCollection().Clear();

				foreach (ShapeBase shape in shapes) {
					if (DoesOverlapWithShape(shape))
						selectedShapes.GetCollection().Add(shape);
				}

				selectedShapes.SetDirty(false);
			}

			return selectedShapes.GetCollection();
		}

		private bool DoesOverlapWithShape(ShapeBase shape) {
			Vector2 topLeft = Vector2.Min(startPosition, endPosition);
			Vector2 bottomRight = Vector2.Max(startPosition, endPosition);

			foreach (Vector2 point in shape.GetBoundingBoxAnchors()) {
				if (point.X >= topLeft.X && point.X <= bottomRight.X && point.Y >= topLeft.Y && point.Y <= bottomRight.Y)
					continue;

				return false;
			}

			return true;
		}
	}
}