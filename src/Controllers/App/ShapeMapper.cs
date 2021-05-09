using System;
using System.Collections.Generic;
using Designer.Models.Shapes;

namespace Designer.Controllers.App {
	public class ShapeMapper : IShapeContainer {
		private Dictionary<ShapeIdentifier, ShapeWrapper> shapes = null;

		public ShapeMapper() {
			this.shapes = new Dictionary<ShapeIdentifier, ShapeWrapper>();
		}

		public void AddShape(ShapeWrapper shapeWrapper) {
			this.shapes.Add(shapeWrapper.GetShape().GetIdentifier(), shapeWrapper);

			shapeWrapper.Destroyed += HandleDestroyed;
		}

		public void RemoveShape(ShapeWrapper shapeWrapper) {
			this.RemoveShape(shapeWrapper.GetShape().GetIdentifier());

			shapeWrapper.Destroyed -= HandleDestroyed;
		}

		public void RemoveShape(ShapeIdentifier identifier) {
			this.shapes.Remove(identifier);
		}

		public ShapeWrapper GetShape(ShapeIdentifier identifier) {
			return shapes[identifier];
		}

		public bool ContainsShape(ShapeIdentifier identifier) {
			return this.GetShape(identifier) != null;
		}

		public void Clear() {
			this.shapes.Clear();
		}

		public IReadOnlyList<ShapeWrapper> GetAllShapes() {
			List<ShapeWrapper> shapes = new List<ShapeWrapper>();

			foreach (var shapeKeyPair in this.shapes) {
				shapes.Add(shapeKeyPair.Value);
			}

			return shapes;
		}

		private void HandleDestroyed(ShapeWrapper shapeWrapper, List<IShapeContainer> interested) {
			this.RemoveShape(shapeWrapper);
			interested.Add(this);
		}
	}
}