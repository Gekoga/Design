namespace Designer.Models {
	public class Annotation {
		public enum Position {
			TOP,
			BOTTOM,
			LEFT,
			RIGHT,
		}

		private string text = string.Empty;
		private Position position = default;

		public Annotation(string text, Position position) {
			this.text = text;
			this.position = position;
		}

		public string GetText() {
			return this.text;
		}

		public Position GetPosition() {
			return this.position;
		}
	}
}