using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Designer.Models.Shapes;

namespace Designer.Models {
	public class Selection {
		private HashSet<ShapeWrapper> selectedShapes = null;
		private GroupShape group = null;

		public Selection() {
			this.selectedShapes = new HashSet<ShapeWrapper>();
			this.group = new GroupShape();
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

		public GroupShape GetAsGroup() {
			this.group.Clear();

			foreach (var shapeWrapper in this.selectedShapes)
				group.AddShape(shapeWrapper);

			return group;
		}
	}
}