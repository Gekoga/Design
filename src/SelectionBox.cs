using System.Collections.Generic;
using Designer.Utility;
using Love;
using Designer.Shapes;

namespace Designer {
	public class SelectionBox {
		private Vector2 startPosition = Vector2.Zero;
		private Vector2 endPosition = Vector2.Zero;

		private DirtyValue<ShapeGroup, List<IShape>> selectedGroup;

		public SelectionBox(Vector2 startPosition, Vector2 endPosition) {
			this.startPosition = startPosition;
			this.endPosition = endPosition;

			this.selectedGroup = new DirtyValue<ShapeGroup, List<IShape>>(new ShapeGroup(), CleanSelectedGroup);
		}

		public void SetEndPosition(Vector2 endPosition) {
			this.endPosition = endPosition;

			this.selectedGroup.MakeDirty();
		}

		public void Draw() {
			Vector2 topLeft = Vector2.Min(startPosition, endPosition);
			Vector2 bottomRight = Vector2.Max(startPosition, endPosition);
			Vector2 size = bottomRight - topLeft;

			Graphics.SetColor(0.808f, 0.788f, 0.435f, 0.500f);
			Graphics.Rectangle(DrawMode.Fill, topLeft.X, topLeft.Y, size.X, size.Y);
		}

		public ShapeGroup GetSelectedGroup(List<IShape> shapes) {
			return selectedGroup.GetValue(shapes);
		}

		private bool DoesOverlapWithShape(IShape shape) {
			Vector2 topLeft = Vector2.Min(startPosition, endPosition);
			Vector2 bottomRight = Vector2.Max(startPosition, endPosition);

			return shape.IsEncupsulatedBy(topLeft, bottomRight);
		}

		private ShapeGroup CleanSelectedGroup(ShapeGroup shapeGroup, List<IShape> shapes) {
			shapeGroup.ClearShapes();

			foreach (IShape shape in shapes) {
				if (DoesOverlapWithShape(shape))
					shapeGroup.AddShape(shape);
			}

			return shapeGroup;
		}
	}
}