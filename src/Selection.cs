using System.Collections.Generic;
using Love;
using System;
using Designer.Shapes;

namespace Designer {
	public class Selection {
		private Vector2 position = Vector2.Zero;
		private List<ShapeBase> selectedShapes = null;

		public void SetSelectedShapes(List<ShapeBase> selectedShapes) {
			this.selectedShapes = selectedShapes;
		}

		public Vector2 GetPosition() {
			if (selectedShapes == null)
				return Vector2.Zero;

			return this.GetTopLeftAnchor();
		}

		public void SetPosition(Vector2 position) {
			if (selectedShapes == null)
				return;

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();
			Vector2 size = bottomRight - topLeft;

			Vector2 delta = position - topLeft;

			foreach (ShapeBase shape in selectedShapes) {
				shape.SetPosition(shape.GetPosition() + delta);
			}
		}

		public void Draw() {
			if (selectedShapes == null)
				return;

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			Vector2 size = bottomRight - topLeft;

			Graphics.SetColor(1.000f, 0.369f, 1.000f, 1.000f);
			Graphics.Rectangle(DrawMode.Line, topLeft.X, topLeft.Y, size.X, size.Y);

			float blockSize = 7.0f;
			float halfBlockSize = blockSize / 2.0f;

			Graphics.SetColor(0.800f, 0.369f, 1.000f, 1.000f);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X - halfBlockSize, topLeft.Y - halfBlockSize, blockSize, blockSize);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X + size.X - halfBlockSize, topLeft.Y - halfBlockSize, blockSize, blockSize);

			Graphics.Rectangle(DrawMode.Fill, topLeft.X - halfBlockSize, topLeft.Y + size.Y - halfBlockSize, blockSize, blockSize);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X + size.X - halfBlockSize, topLeft.Y + size.Y - halfBlockSize, blockSize, blockSize);


			Graphics.Rectangle(DrawMode.Fill, topLeft.X + size.X / 2 - halfBlockSize, topLeft.Y - halfBlockSize, blockSize, blockSize);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X - halfBlockSize, topLeft.Y + size.Y / 2 - halfBlockSize, blockSize, blockSize);

			Graphics.Rectangle(DrawMode.Fill, topLeft.X + size.X / 2 - halfBlockSize, topLeft.Y + size.Y - halfBlockSize, blockSize, blockSize);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X + size.X - halfBlockSize, topLeft.Y + size.Y / 2 - halfBlockSize, blockSize, blockSize);
		}

		private Vector2 GetTopLeftAnchor() {
			Vector2 topLeft = Vector2.One * float.PositiveInfinity;

			foreach (ShapeBase shape in selectedShapes) {
				Vector2 shapePosition = shape.GetPosition();
				Vector2 shapeSize = shape.GetSize();

				topLeft.X = Math.Min(topLeft.X, shapePosition.X);
				topLeft.Y = Math.Min(topLeft.Y, shapePosition.Y);
			}

			return topLeft;
		}

		private Vector2 GetBottomRightAnchor() {
			Vector2 bottomRight = Vector2.Zero;

			foreach (ShapeBase shape in selectedShapes) {
				Vector2 shapePosition = shape.GetPosition();
				Vector2 shapeSize = shape.GetSize();

				bottomRight.X = Math.Max(bottomRight.X, shapePosition.X + shapeSize.X);
				bottomRight.Y = Math.Max(bottomRight.Y, shapePosition.Y + shapeSize.Y);
			}

			return bottomRight;
		}
	}
}