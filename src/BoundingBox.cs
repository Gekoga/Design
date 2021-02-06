using System.Collections.Generic;
using Love;

namespace Designer.Utility {
	public struct BoundingBox {
		private Vector2 topLeft;
		private Vector2 bottomRight;

		private DirtyCollection<List<Vector2>> boundingBoxPoints;

		public BoundingBox(Vector2 topLeft, Vector2 bottomRight) {
			this.topLeft = topLeft;
			this.bottomRight = bottomRight;

			this.boundingBoxPoints = new DirtyCollection<List<Vector2>>(new List<Vector2>());
		}

		public Vector2 GetSize() {
			return this.bottomRight - this.topLeft;
		}

		public Vector2 GetTopLeftAnchor() {
			return this.topLeft;
		}
		
		public void SetTopLeftAnchor(Vector2 topLeft) {
			this.topLeft = topLeft;

			this.boundingBoxPoints.SetDirty(true);
		}

		public Vector2 GetTopRightAnchor() {
			return new Vector2(this.bottomRight.X, this.topLeft.Y);
		}

		public void SetTopRightAnchor(Vector2 topRight) {
			this.bottomRight.X = topRight.X;
			this.topLeft.Y = topRight.Y;

			this.boundingBoxPoints.SetDirty(true);
		}

		public Vector2 GetBottomLeftAnchor() {
			return new Vector2(this.topLeft.X, this.bottomRight.Y);
		}

		public void SetBottomLeftAnchor(Vector2 bottomLeft) {
			this.topLeft.X = bottomLeft.X;
			this.bottomRight.Y = bottomLeft.Y;

			this.boundingBoxPoints.SetDirty(true);
		}

		public Vector2 GetBottomRightAnchor() {
			return this.bottomRight;
		}

		public void SetBottomRightAnchor(Vector2 bottomRight) {
			this.bottomRight = bottomRight;

			this.boundingBoxPoints.SetDirty(true);
		}

		public List<Vector2> GetBoundingBoxAnchors() {
			List<Vector2> collection = this.boundingBoxPoints.GetCollection();

			if (this.boundingBoxPoints.IsDirty()) {
				collection.Clear();

				collection.Add(this.GetTopLeftAnchor());
				collection.Add(this.GetTopRightAnchor());
				collection.Add(this.GetBottomLeftAnchor());
				collection.Add(this.GetBottomRightAnchor());

				this.boundingBoxPoints.SetDirty(false);
			}

			return collection;
		}

		public bool DoesOverlap(Vector2 otherTopLeft, Vector2 otherBottomRight) {
			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			return (topLeft.X <= otherBottomRight.X && 
			        bottomRight.X >= otherTopLeft.X && 
							topLeft.Y <= otherBottomRight.Y && 
							bottomRight.Y >= otherTopLeft.Y);
		}

		public bool DoesOverlap(BoundingBox other) {
			return this.DoesOverlap(other.GetTopLeftAnchor(), other.GetBottomRightAnchor());
		}

		public bool DoesOverlapWithPoint(Vector2 point) {
			Vector2 topLeft = this.GetTopLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			return (topLeft.X <= point.X && 
			        bottomRight.X >= point.X && 
							topLeft.Y <= point.Y && 
							bottomRight.Y >= point.Y);
		}
	}
} 