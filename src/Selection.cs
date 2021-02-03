using System;
using System.Collections.Generic;
using Love;

namespace Designer {
	public class Selection {
		private Vector2 startPosition = Vector2.Zero;
		private Vector2 endPosition = Vector2.Zero;

		private List<ShapeBase> selectedShapes = null;

		public Selection(Vector2 startPosition, Vector2 endPosition) {
			this.startPosition = startPosition;
			this.endPosition = endPosition;

			this.selectedShapes = new List<ShapeBase>();
		}

		public void SetEndPosition(Vector2 endPosition) {
			this.endPosition = endPosition;
		}

		public void Update(List<ShapeBase> shapes) {
			selectedShapes.Clear();

			foreach (ShapeBase shape in shapes) {
				if (DoesOverlapWithShape(shape))
					selectedShapes.Add(shape);
			}
		}

		public void DrawSelectionBox() {
			Vector2 topLeft = Vector2.Min(startPosition, endPosition);
			Vector2 bottomRight = Vector2.Max(startPosition, endPosition);
			Vector2 size = bottomRight - topLeft;

			Graphics.SetColor(0.608f, 0.369f, 1.000f, 0.518f);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X, topLeft.Y, size.X, size.Y);
		}

		public void DrawSelections() {
			Graphics.SetColor(1.000f, 0.369f, 1.000f, 1.000f);
			foreach (ShapeBase shape in selectedShapes) {
				Vector2 shapePosition = shape.GetPosition();
				Vector2 shapeSize = shape.GetSize();

				Graphics.Rectangle(DrawMode.Line, shapePosition.X, shapePosition.Y, shapeSize.X, shapeSize.Y);
			}

			Graphics.SetColor(0.800f, 0.369f, 1.000f, 1.000f);
			foreach (ShapeBase shape in selectedShapes) {
				Vector2 shapePosition = shape.GetPosition();
				Vector2 shapeSize = shape.GetSize();

				float blockSize = 7.0f;
				float halfBlockSize = blockSize / 2.0f;

				Graphics.Rectangle(DrawMode.Fill, shapePosition.X - halfBlockSize, shapePosition.Y - halfBlockSize, blockSize, blockSize);
				Graphics.Rectangle(DrawMode.Fill, shapePosition.X + shapeSize.X - halfBlockSize, shapePosition.Y - halfBlockSize, blockSize, blockSize);

				Graphics.Rectangle(DrawMode.Fill, shapePosition.X - halfBlockSize, shapePosition.Y + shapeSize.Y - halfBlockSize, blockSize, blockSize);
				Graphics.Rectangle(DrawMode.Fill, shapePosition.X + shapeSize.X - halfBlockSize, shapePosition.Y + shapeSize.Y - halfBlockSize, blockSize, blockSize);


				Graphics.Rectangle(DrawMode.Fill, shapePosition.X + shapeSize.X / 2 - halfBlockSize, shapePosition.Y - halfBlockSize, blockSize, blockSize);
				Graphics.Rectangle(DrawMode.Fill, shapePosition.X - halfBlockSize, shapePosition.Y + shapeSize.Y / 2 - halfBlockSize, blockSize, blockSize);

				Graphics.Rectangle(DrawMode.Fill, shapePosition.X + shapeSize.X / 2 - halfBlockSize, shapePosition.Y + shapeSize.Y - halfBlockSize, blockSize, blockSize);
				Graphics.Rectangle(DrawMode.Fill, shapePosition.X + shapeSize.X - halfBlockSize, shapePosition.Y + shapeSize.Y / 2 - halfBlockSize, blockSize, blockSize);
			}
		}

		private bool DoesOverlapWithShape(ShapeBase shape) {
			Vector2 topLeft = Vector2.Min(startPosition, endPosition);
			Vector2 bottomRight = Vector2.Max(startPosition, endPosition);

			foreach (Vector2 point in shape.GetBoundingBoxPoints()) {
				if (point.X >= topLeft.X && point.X <= bottomRight.X && point.Y >= topLeft.Y && point.Y <= bottomRight.Y)
					continue;

				return false;
			}

			return true;
		}
	}
}