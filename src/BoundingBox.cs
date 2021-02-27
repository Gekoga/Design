using System.Collections.Generic;
using Love;

namespace Designer.Utility {
	public class BoundingBox {
		private Vector2 topLeft;
		private Vector2 bottomRight;

		private DirtyValue<List<Vector2>> anchors;

		public BoundingBox(Vector2 topLeft, Vector2 bottomRight) {
			this.topLeft = topLeft;
			this.bottomRight = bottomRight;

			this.anchors = new DirtyValue<List<Vector2>>(new List<Vector2>(), this.CleanAnchors);
		}

		public Vector2 GetSize() {
			return this.bottomRight - this.topLeft;
		}

		public Vector2 GetTopLeftAnchor() {
			return this.topLeft;
		}
		
		public void SetTopLeftAnchor(Vector2 topLeft) {
			this.topLeft = topLeft;

			this.anchors.MakeDirty();
		}

		public Vector2 GetTopRightAnchor() {
			return new Vector2(this.bottomRight.X, this.topLeft.Y);
		}

		public void SetTopRightAnchor(Vector2 topRight) {
			this.bottomRight.X = topRight.X;
			this.topLeft.Y = topRight.Y;

			this.anchors.MakeDirty();
		}

		public Vector2 GetBottomLeftAnchor() {
			return new Vector2(this.topLeft.X, this.bottomRight.Y);
		}

		public void SetBottomLeftAnchor(Vector2 bottomLeft) {
			this.topLeft.X = bottomLeft.X;
			this.bottomRight.Y = bottomLeft.Y;

			this.anchors.MakeDirty();
		}

		public Vector2 GetBottomRightAnchor() {
			return this.bottomRight;
		}

		public void SetBottomRightAnchor(Vector2 bottomRight) {
			this.bottomRight = bottomRight;

			this.anchors.MakeDirty();
		}

		public List<Vector2> GetAnchors() {
			return this.anchors.GetValue();
		}

		public bool DoesOverlapWith(Vector2 otherTopLeft, Vector2 otherBottomRight) {
			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();
			
			return (topLeft.X <= otherBottomRight.X && 
			        bottomRight.X >= otherTopLeft.X &&
							topLeft.Y <= otherBottomRight.Y && 
							bottomRight.Y >= otherTopLeft.Y);
		}

		public bool DoesOverlapWith(BoundingBox other) {
			return this.DoesOverlapWith(other.GetTopLeftAnchor(), other.GetBottomRightAnchor());
		}

		public bool IsEncupsulatedBy(Vector2 otherTopLeft, Vector2 otherBottomRight) {
			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			return (topLeft.X >= otherTopLeft.X && 
			        bottomRight.X <= otherBottomRight.X && 
							topLeft.Y >= otherTopLeft.Y && 
							bottomRight.Y <= otherBottomRight.Y);
		}

		public bool IsEncupsulatedBy(BoundingBox other) {
			return this.IsEncupsulatedBy(other.GetTopLeftAnchor(), other.GetBottomRightAnchor());
		}

		public bool DoesOverlapWithPoint(Vector2 point) {
			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			return (topLeft.X <= point.X && 
			        bottomRight.X >= point.X && 
							topLeft.Y <= point.Y && 
							bottomRight.Y >= point.Y);
		}

		private List<Vector2> CleanAnchors(List<Vector2> anchors) {
			anchors.Clear();

			anchors.Add(this.GetTopLeftAnchor());
			anchors.Add(this.GetTopRightAnchor());
			anchors.Add(this.GetBottomLeftAnchor());
			anchors.Add(this.GetBottomRightAnchor());

			return anchors;
		}
	}
} 