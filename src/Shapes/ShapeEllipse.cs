using Designer.Utility;
using Love;

namespace Designer.Shapes {
	public class ShapeEllipse : ShapeBase {

		public ShapeEllipse(BoundingBox boundingBox) : base(boundingBox) { }

		public override void Draw() {
			base.Draw();

			Vector2 position = this.GetPosition();
			Vector2 size = this.GetSize();
			
			Vector2 halfSize = size/2.0f;

			Graphics.Ellipse(DrawMode.Line, position.X + halfSize.X, position.Y + halfSize.Y, halfSize.X, halfSize.Y);
		}
	}
}