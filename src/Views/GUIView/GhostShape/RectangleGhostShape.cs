using System.Numerics;

namespace Designer.Views.GUIView.GhostShape {
	public class RectangleGhostShape : IGhostShape {
		private Vector2 position = Vector2.Zero;
		private Vector2 size = Vector2.Zero;

		public RectangleGhostShape(Vector2 position, Vector2 size) {
			this.position = position;
			this.size = size;
		}

		public RectangleGhostShape(Vector2 position): this(position, Vector2.Zero) {
			
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

		public void Draw(GUIViewSettings guiViewSettings) {
			var color = guiViewSettings.GetGhostColor();
			Love.Graphics.SetColor(color);
			
			Love.Graphics.Rectangle(
				Love.DrawMode.Fill,
				this.position.X,
				this.position.Y,
				this.size.X,
				this.size.Y
			);
		}
	}
}