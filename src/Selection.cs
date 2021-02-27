using System.Collections.Generic;
using Love;
using Designer.Shapes;
using Designer.Utility;
using System;

namespace Designer {
	public class Selection {
		private ShapeGroup selectedGroup = null;

		private Vector2? moveRelativeStartPos = null;
		private Vector2? moveRelativeBottomRight = null;

		private Vector2? originalTopLeft = null;

		private Vector2 oldSize = Vector2.Zero;

		private DirtyValue<List<SelectionPoint>> selectionPoints = null;

		public Selection() {
			this.selectionPoints = new DirtyValue<List<SelectionPoint>>(new List<SelectionPoint>(), CleanSelectionPoints);
		}

		public void SetSelectedGroup(ShapeGroup selectedGroup) {
			this.selectedGroup = selectedGroup;

			this.selectionPoints.MakeDirty();
		}

		public void Update(Vector2 mousePos) {
			if (selectedGroup == null)
				return;

			foreach (SelectionPoint selectionPoint in selectionPoints.GetValue())
				selectionPoint.Update(mousePos);
		}

		public void MousePressed(MouseButton button, Vector2 mousePos) {
			if (button == MouseButton.LeftButton) {
				foreach (SelectionPoint selectionPoint in selectionPoints.GetValue())
					selectionPoint.MousePressed(mousePos);
				// if (hoveredBoundingPoint != null) {
				// 	resizeRelativeStartPos = mousePos - this.GetTopLeftAnchor();

				// 	originalTopLeft = this.GetTopLeftAnchor();
				// } else {
				// 	moveRelativeStartPos = mousePos - this.GetTopLeftAnchor();
				// }
			}
		}

		public void MouseReleased(MouseButton button, Vector2 mousePos) {
			//resizeRelativeStartPos = null;
			moveRelativeStartPos = null;

			foreach (SelectionPoint selectionPoint in selectionPoints.GetValue())
				selectionPoint.MouseReleased(mousePos);
		}

		public void MouseMoved(Vector2 mousePos) {
			/*
			if (moveRelativeStartPos != null) {
				Vector2 size = this.GetSize();

				this.SetTopLeftAnchor(mousePos - (Vector2)moveRelativeStartPos);
				this.SetBottomRightAnchor(mousePos + size - (Vector2)moveRelativeStartPos);
			}
			*/

			/*
			if (resizeRelativeStartPos != null) {
				Vector2 newPos = mousePos - (Vector2)resizeRelativeStartPos;
				this.SetTopLeftAnchor(newPos);
			}
			*/
			foreach (SelectionPoint selectionPoint in selectionPoints.GetValue())
				selectionPoint.MouseMoved(mousePos);
		}

		public void Draw() {
			if (selectedGroup == null)
				return;

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			Vector2 size = bottomRight - topLeft;

			Graphics.SetColor(0.808f, 0.788f, 0.435f, 0.750f);
			Graphics.Rectangle(DrawMode.Line, topLeft.X, topLeft.Y, size.X, size.Y);

			foreach (SelectionPoint selectionPoint in selectionPoints.GetValue()) {
				selectionPoint.Draw();
			}
		}

		private Vector2 GetTopLeftAnchor() {
			return this.selectedGroup.GetTopLeftAnchor();
		}

		private void SetTopLeftAnchor(Vector2 topLeft) {
			this.selectedGroup.SetTopLeftAnchor(topLeft);

			selectionPoints.MakeDirty();
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

		private void SetBottomRightAnchor(Vector2 bottomRight) {
			this.selectedGroup.SetBottomRightAnchor(bottomRight);

			selectionPoints.MakeDirty();
		}

		private Vector2 GetSize() {
			return this.selectedGroup.GetSize();
		}

		private List<SelectionPoint> CleanSelectionPoints(List<SelectionPoint> selectionPoints) {
			

			selectionPoints.Clear();

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 topRight = this.GetTopRightAnchor();
			Vector2 bottomLeft = this.GetBottomLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			Vector2 size = bottomRight - topLeft;
			Vector2 halfSize = size / 2.0f;

			Vector2 blockSize = Vector2.One * 4.0f;
			Vector2 halfBlockSize = blockSize / 2.0f;

			// 4 corners
			selectionPoints.Add(MakeSelectionPoint(topLeft, halfBlockSize));
			selectionPoints.Add(MakeSelectionPoint(topRight, halfBlockSize));
			selectionPoints.Add(MakeSelectionPoint(bottomLeft, halfBlockSize));
			selectionPoints.Add(MakeSelectionPoint(bottomRight, halfBlockSize));

			// 2 mid points top left
			selectionPoints.Add(MakeSelectionPoint(topLeft + halfSize * Vector2.Right, halfBlockSize));
			selectionPoints.Add(MakeSelectionPoint(topLeft + halfSize * Vector2.Down, halfBlockSize));

			// 2 mid points bottom right
			selectionPoints.Add(MakeSelectionPoint(bottomRight + halfSize * Vector2.Left, halfBlockSize));
			selectionPoints.Add(MakeSelectionPoint(bottomRight + halfSize * Vector2.Up, halfBlockSize));

			return selectionPoints;
		}

		private SelectionPoint MakeSelectionPoint(Vector2 position, Vector2 halfSize) {
			return new SelectionPoint(new BoundingBox(position - halfSize, position + halfSize));
		}
	}
}