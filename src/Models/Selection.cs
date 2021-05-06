using System.Collections.ObjectModel;
using System.Collections.Generic;
using Designer.Models.Shapes;

namespace Designer.Models {
	public class Selection {
		private HashSet<ShapeWrapper> selectedShapes = null;

		public Selection() {
			this.selectedShapes = new HashSet<ShapeWrapper>();
		}

		public void AddShape(ShapeWrapper shape) {
			this.selectedShapes.Add(shape);
		}

		public void RemoveShape(ShapeWrapper shape) {
			this.selectedShapes.Remove(shape);
		}

		public void Clear() {
			this.selectedShapes.Clear();
		}

		public IReadOnlySet<ShapeWrapper> GetShapes() {
			return this.selectedShapes;
		}
	}
}