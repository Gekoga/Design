using Designer.Utility;
using Love;
using System;

namespace Designer {
	public class SelectionPoint {
		private BoundingBox boundingBox = null;

		private Vector2 relativeClickPosition = Vector2.Zero;

		private bool isHovered = false;
		private bool isClicked = false;

		public SelectionPoint(BoundingBox boundingBox) {
			this.boundingBox = boundingBox;
		}

		public void Update(Vector2 mousePos) {
			this.isHovered = boundingBox.DoesOverlapWithPoint(mousePos);
		}

		public void Draw() {
			Graphics.SetColor(0.808f, 0.788f, 0.435f, 1.000f);

			Vector2 topLeft = boundingBox.GetTopLeftAnchor();
			Vector2 size = boundingBox.GetSize();

			

			if (this.IsHovered()) {
				Graphics.Rectangle(DrawMode.Fill, topLeft.X - 1.0f, topLeft.Y - 1.0f, size.X + 2.0f, size.Y + 2.0f);
			} else {
				Graphics.Rectangle(DrawMode.Fill, topLeft.X, topLeft.Y, size.X, size.Y);
			}
		}

		public void MousePressed(Vector2 mousePos) {
			if (this.IsHovered()) {
				this.isClicked = true;

				relativeClickPosition = mousePos - boundingBox.GetTopLeftAnchor();
			}
		}

		public void MouseReleased(Vector2 mousePos) {
			if (this.IsClicked()) {
				this.isClicked = false;
			}
		}

		public void MouseMoved(Vector2 mousePos) {

		}

		public bool IsHovered() {
			return this.isHovered;
		}

		public bool IsClicked() {
			return this.isClicked;
		}
	}
}