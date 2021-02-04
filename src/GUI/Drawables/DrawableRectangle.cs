using Love;

namespace Designer.GUI.Drawables {
	public class DrawableRectangle : IDrawable {
		private Color fillColor = default;
		private Color lineColor = default;

		public DrawableRectangle(Color fillColor, Color lineColor) {
			this.fillColor = fillColor;
			this.lineColor = lineColor;
		}

		public void Draw(Vector2 position, Vector2 size) {
			Graphics.SetColor(fillColor);
			Graphics.Rectangle(DrawMode.Fill, position.X, position.Y, size.X, size.Y);

			Graphics.SetColor(lineColor);
			Graphics.Rectangle(DrawMode.Line, position.X, position.Y, size.X, size.Y);
		}
	}
}