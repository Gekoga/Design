using Designer.Models;
using Designer.Models.Shapes;

namespace Designer.Controllers.App {
	public class ShapeStrategy {
		public delegate void DrawDelegate(BaseShape baseShape, Selection selection, Love.Color shapeColor, Love.Color selectedColor);

		private DrawDelegate drawDelegate = null;
		private string name = string.Empty;

		public ShapeStrategy(DrawDelegate drawDelegate, string name) {
			this.drawDelegate = drawDelegate;
			this.name = name;
		}

		public void Draw(BaseShape baseShape, Selection selection, Love.Color shapeColor, Love.Color selectedColor) {
			this.drawDelegate(baseShape, selection, shapeColor, selectedColor);
		}

		public string GetName() {
			return this.name;
		}

		public override string ToString() {
			return this.name;
		}
	}
}