using System.Numerics;

namespace Designer.Views.GUIView {
	public class SelectionBox {
		private Vector2 topLeft = Vector2.Zero;
		private Vector2 bottomRight = Vector2.Zero;

		public SelectionBox(Vector2 topLeft, Vector2 bottomRight) {
			this.topLeft = topLeft;
			this.bottomRight = bottomRight;
		}

		public SelectionBox(Vector2 topLeft) : this(topLeft, topLeft) {

		}

		public Vector2 GetTopLeft() {
			return this.topLeft;
		}

		public void SetTopLeft(Vector2 topLeft) {
			this.topLeft = topLeft;
		}

		public Vector2 GetBottomRight() {
			return this.bottomRight;
		}

		public void SetBottomRight(Vector2 bottomRight) {
			this.bottomRight = bottomRight;
		}
		
		public Vector2 GetSize() {
			return this.bottomRight - this.topLeft;
		}

		public void Draw(GUIViewSettings settings) {
			var color = settings.GetSelectionBoxColor();
			Love.Graphics.SetColor(color);
			
			var position = this.GetTopLeft();
			var size = this.GetSize();

			Love.Graphics.Rectangle(
				Love.DrawMode.Fill,
				position.X,
				position.Y,
				size.X,
				size.Y
			);
		}
	}
}