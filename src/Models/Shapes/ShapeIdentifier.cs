using System;
namespace Designer.Models.Shapes {
	public class ShapeIdentifier {
		private int identifier = default;

		public ShapeIdentifier(IShape shape) {
			this.identifier = shape.GetHashCode();
		}

		public override string ToString() {
			return this.identifier.ToString();
		}
	}
}