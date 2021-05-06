using System.Numerics;

namespace Designer.Views.GUIView.GhostShape {
	public class EllipseGhostShape : IGhostShape {
		private Vector2 position = Vector2.Zero;
		private Vector2 size = Vector2.Zero;

		public EllipseGhostShape(Vector2 position, Vector2 size) {
			this.position = position;
			this.size = size;
		}

		public EllipseGhostShape(Vector2 position) : this(position, Vector2.Zero) {

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

			Love.Graphics.Ellipse(
				Love.DrawMode.Fill,
				position.X + size.X / 2,
				position.Y + size.Y / 2,
				size.X / 2,
				size.Y / 2
			);
		}
	}
}