using System.Diagnostics;
namespace Designer.Views.GUIView {
	public class GUIViewSettings {
		private Love.Color shapeColor;
		private Love.Color selectedShapeColor;
		private Love.Color ghostColor;
		private Love.Color selectionBoxColor;

		public GUIViewSettings(Love.Color shapeColor, Love.Color selectedShapeColor, Love.Color ghostColor, Love.Color selectionBoxColor) {
			this.shapeColor = shapeColor;
			this.selectedShapeColor = selectedShapeColor;
			this.ghostColor = ghostColor;
			this.selectionBoxColor = selectionBoxColor;
		} 

		public Love.Color GetShapeColor() {
			return this.shapeColor;
		}

		public Love.Color GetSelectedShapeColor() {
			return this.selectedShapeColor;
		}

		public Love.Color GetGhostColor() {
			return this.ghostColor;
		}

		public Love.Color GetSelectionBoxColor() {
			return this.selectionBoxColor;
		}
	}
}