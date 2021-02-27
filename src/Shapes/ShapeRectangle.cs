using Designer.Utility;
using Love;

namespace Designer.Shapes {
	public class ShapeRectangle : ShapeBB {
		public ShapeRectangle(BoundingBox boundingBox) : base(boundingBox) { }

		public override void Draw() {
			base.Draw();

			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 size = this.GetSize();

			Graphics.SetLineJoin(LineJoin.Miter);
			Graphics.Rectangle(DrawMode.Line, topLeft.X, topLeft.Y, size.X, size.Y);
		}
	}
}