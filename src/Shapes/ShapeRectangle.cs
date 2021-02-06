using Designer.Utility;
using Love;

namespace Designer.Shapes {
	public class ShapeRectangle : ShapeBase {
		public ShapeRectangle(BoundingBox boundingBox) : base(boundingBox) { }

		public override void Draw() {
			base.Draw();

			Vector2 position = this.GetPosition();
			Vector2 size = this.GetSize();

			Graphics.Rectangle(DrawMode.Line, position.X, position.Y, size.X, size.Y);
		}
	}
}