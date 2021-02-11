using System.Collections.Generic;
using Love;
using Designer.Shapes;
using Designer.Utility;
using System.Linq;
using System;

namespace Designer {
	public class Selection {
		private struct ShapeSelectionResize {
			public IShape shape;

			public Vector2 topLeftDistance;
			public Vector2 topRightDistance;
			public Vector2 bottomLeftDistance;
			public Vector2 bottomRightDistance;

			public ShapeSelectionResize(IShape shape, Vector2 topLeftDistance, Vector2 topRightDistance, Vector2 bottomLeftDistance, Vector2 bottomRightDistance) {
				this.shape = shape;

				this.topLeftDistance = topLeftDistance;
				this.topRightDistance = topRightDistance;
				this.bottomLeftDistance = bottomLeftDistance;
				this.bottomRightDistance = bottomRightDistance;
			}
		}

		private Vector2 position = Vector2.Zero;
		private ShapeGroup selectedGroup = null;

		private Vector2? moveRelativeStartPoint = null;
		private Vector2? resizeRelativeStartPoint = null;
		private Vector2 oldSize = Vector2.Zero;

		private List<ShapeSelectionResize> shapeSelectionResizes = null;

		private BoundingBox hoveredBoundingPoint = null;

		public Selection() {
			this.shapeSelectionResizes = new List<ShapeSelectionResize>();
		}

		public void SetSelectedGroup(ShapeGroup selectedGroup) {
			this.selectedGroup = selectedGroup;
		}

		// public void SetPosition(Vector2 position) {
		// 	if (selectedGroup == null)
		// 		return;

		// 	Vector2 topLeft = this.GetTopLeftAnchor();
		// 	Vector2 bottomRight = this.GetBottomRightAnchor();
		// 	Vector2 size = bottomRight - topLeft;

		// 	Vector2 delta = position - topLeft;

		// 	foreach (IShape shape in selectedGroup) {
		// 		shape.SetTopLeftAnchor(shape.GetTopLeftAnchor() + delta);
		// 		shape.SetBottomRightAnchor(shape.GetBottomRightAnchor() + delta);
		// 	}
		// }

		public void Update(Vector2 mousePos) {
			if (selectedGroup == null)
				return;

			List<BoundingBox> boundingPoints = this.GetBoundingPoints();

			this.selectedGroup.GetBoundingBoxAnchors();

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

		public void MousePressed(MouseButton button, Vector2 mousePos) {
			if (hoveredBoundingPoint != null) {
				resizeRelativeStartPoint = mousePos;

				shapeSelectionResizes.Clear();

				/*
				Vector2 topLeft = this.GetTopLeftAnchor();
				Vector2 bottomRight = this.GetBottomRightAnchor();
				this.oldSize = bottomRight - topLeft;

				foreach (IShape shape in selectedGroup) {
					Vector2 topLeftDistance = shape.GetTopLeftAnchor() - topLeft;
					Vector2 topRightDistance = shape.GetTopRightAnchor() - topLeft;
					Vector2 bottomLeftDistance = shape.GetBottomLeftAnchor() - topLeft;
					Vector2 bottomRightDistance = shape.GetBottomRightAnchor() - topLeft;

					shapeSelectionResizes.Add(new ShapeSelectionResize(shape, topLeftDistance, topRightDistance, bottomLeftDistance, bottomRightDistance));
				}
				*/
			} else {
				moveRelativeStartPoint = mousePos - this.GetTopLeftAnchor();
			}
		}

		public void MouseReleased(MouseButton button, Vector2 mousePos) {
			resizeRelativeStartPoint = null;
			moveRelativeStartPoint = null;
		}

		public void MouseMoved(Vector2 mousePos) {
			/*
			if (moveRelativeStartPoint != null)
				this.SetPosition(mousePos - (Vector2)moveRelativeStartPoint);

			if (resizeRelativeStartPoint != null) {
				this.SetTopLeftAnchor(mousePos);
			}
			*/
		}

		public void Draw() {
			if (selectedGroup == null)
				return;

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			Vector2 size = bottomRight - topLeft;

			Graphics.SetColor(0.808f, 0.788f, 0.435f, 0.750f);
			Graphics.Rectangle(DrawMode.Line, topLeft.X, topLeft.Y, size.X, size.Y);

			Graphics.SetColor(0.808f, 0.788f, 0.435f, 1.000f);

			foreach (BoundingBox boundingPoint in GetBoundingPoints()) {
				Vector2 boundingPointPosition = boundingPoint.GetTopLeftAnchor();
				Vector2 boundingPointSize = boundingPoint.GetSize();

				if (hoveredBoundingPoint != null) {
					BoundingBox _hoveredBoundingPoint = (BoundingBox)hoveredBoundingPoint;

					if (_hoveredBoundingPoint.GetTopLeftAnchor().Equals(boundingPointPosition)) {
						Graphics.Rectangle(DrawMode.Fill, boundingPointPosition.X - 2.0f, boundingPointPosition.Y - 2.0f, boundingPointSize.X + 4.0f, boundingPointSize.Y + 4.0f);
					} else {
						Graphics.Rectangle(DrawMode.Fill, boundingPointPosition.X, boundingPointPosition.Y, boundingPointSize.X, boundingPointSize.Y);
					}
				} else {
					Graphics.Rectangle(DrawMode.Fill, boundingPointPosition.X, boundingPointPosition.Y, boundingPointSize.X, boundingPointSize.Y);
				}
			}
		}

		private Vector2 GetTopLeftAnchor() {
			return this.selectedGroup.GetTopLeftAnchor();
		}

		private void SetTopLeftAnchor(Vector2 topLeft) {
			this.selectedGroup.SetTopLeftAnchor(topLeft);

			/*
			Vector2 oldSize = this.oldSize;
			Vector2 newSize = this.GetBottomRightAnchor() - topLeft;

			Vector2 percentageIncrease = newSize / oldSize;

			foreach (ShapeSelectionResize shapeSelectionResize in shapeSelectionResizes) {
				IShape shape = shapeSelectionResize.shape;

				shape.SetTopLeftAnchor(topLeft + shapeSelectionResize.topLeftDistance * percentageIncrease);
				//shape.SetTopRightAnchor(topLeft + shapeSelectionResize.topRightDistance * percentageIncrease);
				//shape.SetBottomLeftAnchor(topLeft + shapeSelectionResize.bottomLeftDistance * percentageIncrease);
				shape.SetBottomRightAnchor(topLeft + shapeSelectionResize.bottomRightDistance * percentageIncrease);
			}
			*/
		}

		private Vector2 GetTopRightAnchor() {
			return this.selectedGroup.GetTopRightAnchor();
		}

		private Vector2 GetBottomLeftAnchor() {
			return this.selectedGroup.GetBottomLeftAnchor();
		}

		private Vector2 GetBottomRightAnchor() {
			return this.selectedGroup.GetBottomRightAnchor();
		}

		private List<BoundingBox> GetBoundingPoints() {
			List<BoundingBox> boundingPoints = new List<BoundingBox>();

			if (selectedGroup == null)
				return boundingPoints;

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 topRight = this.GetTopRightAnchor();
			Vector2 bottomLeft = this.GetBottomLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			Vector2 size = bottomRight - topLeft;
			Vector2 halfSize = size / 2.0f;

			Vector2 blockSize = Vector2.One * 6.0f;
			Vector2 halfBlockSize = blockSize / 2.0f;

			// 4 corners
			boundingPoints.Add(this.MakeBoundingPoint(topLeft, halfBlockSize));
			boundingPoints.Add(this.MakeBoundingPoint(topRight, halfBlockSize));
			boundingPoints.Add(this.MakeBoundingPoint(bottomLeft, halfBlockSize));
			boundingPoints.Add(this.MakeBoundingPoint(bottomRight, halfBlockSize));

			// 2 mid points top left
			boundingPoints.Add(this.MakeBoundingPoint(topLeft + halfSize * Vector2.Right, halfBlockSize));
			boundingPoints.Add(this.MakeBoundingPoint(topLeft + halfSize * Vector2.Down, halfBlockSize));

			// 2 mid points bottom right
			boundingPoints.Add(this.MakeBoundingPoint(bottomRight + halfSize * Vector2.Left, halfBlockSize));
			boundingPoints.Add(this.MakeBoundingPoint(bottomRight + halfSize * Vector2.Up, halfBlockSize));

			return boundingPoints;
		}

		private BoundingBox MakeBoundingPoint(Vector2 position, Vector2 halfSize) {
			return new BoundingBox(position - halfSize, position + halfSize);
		}
	}
}