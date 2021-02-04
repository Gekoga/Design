using Love;

namespace Designer.Shapes {
	public class ShapeEllipse : ShapeBase {

		public ShapeEllipse(Vector2 position, Vector2 size) : base(position, size) { }

		public override void Draw() {
			base.Draw();

			Graphics.Ellipse(DrawMode.Line, position.X + size.X / 2.0f, position.Y + size.Y / 2.0f, size.X / 2.0f, size.Y / 2.0f);
		}
	}
}