using Love;

namespace Designer {
	public class ShapeRectangle : ShapeBase {
		public ShapeRectangle(Vector2 position, Vector2 size) : base(position, size) { }

		public override void Draw() {
			base.Draw();

			Graphics.Rectangle(DrawMode.Line, position.X, position.Y, size.X, size.Y);
		}
	}
}