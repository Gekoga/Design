using System.Collections.Generic;
using Love;
using System;
using Designer.Shapes;
using Designer.Utility;

namespace Designer {
	public class Selection {
		private Vector2 position = Vector2.Zero;
		private List<ShapeBase> selectedShapes = null;

		private Vector2? selectionMoveRelStartPoint = null;

		private BoundingBox? hoveredBoundingPoint = null;

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

		public void Update() {
			if (selectedShapes == null)
				return;


			Vector2 mousePos = Mouse.GetPosition();

			List<BoundingBox> boundingPoints = this.GetBoundingPoints();

			hoveredBoundingPoint = null;
			foreach (BoundingBox boundingPoint in boundingPoints) {				
				if (boundingPoint.DoesOverlapWithPoint(mousePos)) {
					hoveredBoundingPoint = boundingPoint;
				}
				//Vector2 topLeft = this.GetTopLeftAnchor();
				//Vector2 bottomRight = this.GetBottomRightAnchor();

				//Vector2 bpTopLeft = boundingPoint.GetTopLeftAnchor();


				/*
				if (x >= position.X && x <= position.X + size.X && y >= position.Y && y <= position.Y + size.Y) {

				}
				*/
			}
		}

		public void MousePressed(MouseButton button, float x, float y) {
			selectionMoveRelStartPoint = new Vector2(x, y) - this.GetPosition();
		}

		public void MouseReleased(MouseButton button, float x, float y) {
			selectionMoveRelStartPoint = null;
		}

		public void MouseMoved(float x, float y) {
			if (selectionMoveRelStartPoint == null)
				selectionMoveRelStartPoint = new Vector2(x, y) - this.GetPosition();

			this.SetPosition(new Vector2(x, y) - (Vector2)selectionMoveRelStartPoint);
		}

		public void Draw() {
			if (selectedShapes == null)
				return;

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			Vector2 size = bottomRight - topLeft;

			Graphics.SetColor(1.000f, 0.369f, 1.000f, 1.000f);
			Graphics.Rectangle(DrawMode.Line, topLeft.X, topLeft.Y, size.X, size.Y);

			foreach (BoundingBox boundingPoint in GetBoundingPoints()) {
				Vector2 boundingPointPosition = boundingPoint.GetPosition();
				Vector2 boundingPointSize = boundingPoint.GetSize();

				if (hoveredBoundingPoint != null) {
					BoundingBox _hoveredBoundingPoint = (BoundingBox)hoveredBoundingPoint;

					if (_hoveredBoundingPoint.GetPosition().Equals(boundingPointPosition)) {
						Graphics.Rectangle(DrawMode.Fill, boundingPointPosition.X - 5.0f, boundingPointPosition.Y - 5.0f, boundingPointSize.X + 10.0f, boundingPointSize.Y + 10.0f);
					} else {
						Graphics.Rectangle(DrawMode.Fill, boundingPointPosition.X, boundingPointPosition.Y, boundingPointSize.X, boundingPointSize.Y);
					}
				} else {
					Graphics.Rectangle(DrawMode.Fill, boundingPointPosition.X, boundingPointPosition.Y, boundingPointSize.X, boundingPointSize.Y);
				}
			}
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

		private Vector2 GetTopRightAnchor() {
			Vector2 topRight = new Vector2(-1, 1) * float.PositiveInfinity;

			foreach (ShapeBase shape in selectedShapes) {
				Vector2 shapePosition = shape.GetPosition();
				Vector2 shapeSize = shape.GetSize();

				topRight.X = Math.Max(topRight.X, shapePosition.X + shapeSize.X);
				topRight.Y = Math.Min(topRight.Y, shapePosition.Y);
			}

			return topRight;
		}

		private Vector2 GetBottomLeftAnchor() {
			Vector2 bottomLeft = new Vector2(1, -1) * float.PositiveInfinity;

			foreach (ShapeBase shape in selectedShapes) {
				Vector2 shapePosition = shape.GetPosition();
				Vector2 shapeSize = shape.GetSize();

				bottomLeft.X = Math.Min(bottomLeft.X, shapePosition.X);
				bottomLeft.Y = Math.Max(bottomLeft.Y, shapePosition.Y + shapeSize.Y);
			}

			return bottomLeft;
		}

		private Vector2 GetBottomRightAnchor() {
			Vector2 bottomRight =  Vector2.One * float.NegativeInfinity;

			foreach (ShapeBase shape in selectedShapes) {
				Vector2 shapePosition = shape.GetPosition();
				Vector2 shapeSize = shape.GetSize();

				bottomRight.X = Math.Max(bottomRight.X, shapePosition.X + shapeSize.X);
				bottomRight.Y = Math.Max(bottomRight.Y, shapePosition.Y + shapeSize.Y);
			}

			return bottomRight;
		}

		private List<BoundingBox> GetBoundingPoints() {
			List<BoundingBox> boundingPoints = new List<BoundingBox>();

			if (selectedShapes == null)
				return boundingPoints;

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 topRight = this.GetTopRightAnchor();
			Vector2 bottomLeft = this.GetBottomLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			Vector2 size = bottomRight - topLeft;
			Vector2 halfSize = size / 2.0f;

			Vector2 blockSize = Vector2.One * 7.0f;
			Vector2 halfBlockSize = blockSize / 2.0f;

			// 4 corners
			boundingPoints.Add(new BoundingBox(topLeft - halfBlockSize, blockSize));
			boundingPoints.Add(new BoundingBox(topRight - halfBlockSize, blockSize));
			boundingPoints.Add(new BoundingBox(bottomLeft - halfBlockSize, blockSize));
			boundingPoints.Add(new BoundingBox(bottomRight - halfBlockSize, blockSize));

			// 2 mid points top left
			boundingPoints.Add(new BoundingBox(topLeft + halfSize * Vector2.Right - halfBlockSize, blockSize));
			boundingPoints.Add(new BoundingBox(topLeft + halfSize * Vector2.Down - halfBlockSize, blockSize));

			// 2 mid points bottom right
			boundingPoints.Add(new BoundingBox(bottomRight + halfSize * Vector2.Left - halfBlockSize, blockSize));
			boundingPoints.Add(new BoundingBox(bottomRight + halfSize * Vector2.Up - halfBlockSize, blockSize));

			return boundingPoints;
		}
	}
}