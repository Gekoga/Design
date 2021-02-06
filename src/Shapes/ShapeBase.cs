using System.Collections.Generic;
using Designer.Utility;
using Love;

namespace Designer.Shapes {
	public abstract class ShapeBase {
		private BoundingBox boundingBox = default;

		public ShapeBase(BoundingBox boundingBox) {
			this.boundingBox = boundingBox;
		}
		
		public virtual void Draw() {

		}

		public Vector2 GetPosition() {
			return this.boundingBox.GetPosition();
		}

		public void SetPosition(Vector2 position) {
			this.boundingBox.SetPosition(position);
		}

		public Vector2 GetSize() {
			return this.boundingBox.GetSize();
		}

		public void SetSize(Vector2 size) {
			this.boundingBox.SetSize(size);
		}

		public List<Vector2> GetBoundingBoxAnchors() {
			return this.boundingBox.GetBoundingBoxAnchors();
		}
	}
}