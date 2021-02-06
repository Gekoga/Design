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

		public Vector2 GetSize() {
			return this.boundingBox.GetSize();
		}

		public Vector2 GetTopLeftAnchor() {
			return this.boundingBox.GetTopLeftAnchor();
		}

		public void SetTopLeftAnchor(Vector2 topLeft) {
			this.boundingBox.SetTopLeftAnchor(topLeft);
		}
		
		public Vector2 GetTopRightAnchor() {
			return this.boundingBox.GetTopRightAnchor();
		}

		public void SetTopRightAnchor(Vector2 topRight) {
			this.boundingBox.SetTopRightAnchor(topRight);
		}

		public Vector2 GetBottomLeftAnchor() {
			return this.boundingBox.GetBottomLeftAnchor();
		}

		public void SetBottomLeftAnchor(Vector2 bottomLeft) {
			this.boundingBox.SetBottomLeftAnchor(bottomLeft);
		}

		public Vector2 GetBottomRightAnchor() {
			return this.boundingBox.GetBottomRightAnchor();
		}

		public void SetBottomRightAnchor(Vector2 bottomRight) {
			this.boundingBox.SetBottomRightAnchor(bottomRight);
		}

		public List<Vector2> GetBoundingBoxAnchors() {
			return this.boundingBox.GetBoundingBoxAnchors();
		}

		public bool DoesOverlap(Vector2 otherTopLeft, Vector2 otherBottomRight) {
			return this.boundingBox.DoesOverlap(otherTopLeft, otherBottomRight);
		}

		public bool DoesOverlap(BoundingBox other) {
			return this.boundingBox.DoesOverlap(other);
		}

		public bool DoesOverlapWithPoint(Vector2 point) {
			return this.boundingBox.DoesOverlapWithPoint(point);
		}
	}
}