using System;
using Designer.Utility;
using Love;

namespace Designer.Shapes {
	public class ShapeEllipse : ShapeBB {

		public ShapeEllipse(BoundingBox boundingBox) : base(boundingBox) { }

		public override void Draw() {
			base.Draw();
			
			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 size = this.GetSize();

			Vector2 halfSize = size/2.0f;

			Graphics.SetLineJoin(LineJoin.Bevel);
			Graphics.Ellipse(DrawMode.Line, topLeft.X + halfSize.X, topLeft.Y + halfSize.Y, halfSize.X, halfSize.Y, 10);
		}
	}
}