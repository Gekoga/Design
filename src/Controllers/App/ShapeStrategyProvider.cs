using Designer.Models;
using Designer.Models.Shapes;

namespace Designer.Controllers.App {
	public class ShapeStrategyProvider { 
		private static ShapeStrategyProvider instance = null;

		public delegate void DrawDelegate(BaseShape baseShape, Selection selection, Love.Color shapeColor, Love.Color selectedColor);

		private ShapeStrategy EllipseStrategy = new ShapeStrategy(delegate (BaseShape baseShape, Selection selection, Love.Color shapeColor, Love.Color selectedColor) {
			var position = baseShape.GetPosition();
			var size = baseShape.GetSize();

			bool isSelected = false;
			foreach (var shape in selection.GetShapes()) {
				if (shape.GetShape() == baseShape)
					isSelected = true;
			}

			var color = isSelected ? selectedColor : shapeColor;

			Love.Graphics.SetColor(color);
			Love.Graphics.Ellipse(
				Love.DrawMode.Line,
				position.X + size.X / 2,
				position.Y + size.Y / 2,
				size.X / 2,
				size.Y / 2
			);
		}, "ellipse");

		private ShapeStrategy RectangleStrategy = new ShapeStrategy(delegate (BaseShape baseShape, Selection selection, Love.Color shapeColor, Love.Color selectedColor) {
			var position = baseShape.GetPosition();
			var size = baseShape.GetSize();

			bool isSelected = false;
			foreach (var shape in selection.GetShapes()) {
				if (shape.GetShape() == baseShape)
					isSelected = true;
			}

			var color = isSelected ? selectedColor : shapeColor;

			Love.Graphics.SetColor(color);
			Love.Graphics.Rectangle(
				Love.DrawMode.Line,
				position.X,
				position.Y,
				size.X,
				size.Y
			);
		}, "rectangle");

		public static ShapeStrategyProvider Instance {
			get {
				if (instance == null) {
					instance = new ShapeStrategyProvider();
				}

				return instance;
			}
		}

		private ShapeStrategyProvider() { }

		public ShapeStrategy GetEllipseStrategy() {
			return this.EllipseStrategy;
		}

		public ShapeStrategy GetRectangleStrategy() {
			return this.RectangleStrategy;
		}
	}
}
