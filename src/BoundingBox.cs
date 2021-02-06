using System.Collections.Generic;
using Love;

namespace Designer.Utility {
	public struct BoundingBox {
		private Vector2 position;
		private Vector2 size;

		private DirtyCollection<List<Vector2>> boundingBoxPoints;

		public BoundingBox(Vector2 position, Vector2 size) {
			this.position = position;
			this.size = size;

			this.boundingBoxPoints = new DirtyCollection<List<Vector2>>(new List<Vector2>());
		}

		public Vector2 GetPosition() {
			return this.position;
		}

		public void SetPosition(Vector2 position) {
			this.position = position;
			this.boundingBoxPoints.SetDirty(true);
		}

		public Vector2 GetSize() {
			return this.size;
		}

		public void SetSize(Vector2 size) {
			this.size = size;
			this.boundingBoxPoints.SetDirty(true);
		}

		public Vector2 GetTopLeftAnchor() {
			return this.position;
		}
		
		public Vector2 GetTopRightAnchor() {
			return this.position + this.size * Vector2.Right;
		}

		public Vector2 GetBottomLeftAnchor() {
			return this.position + this.size * Vector2.Down;
		}

		public Vector2 GetBottomRightAnchor() {
			return this.position + this.size;
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
			//Vector2 topRight = this.GetTopRightAnchor();
			//Vector2 bottomLeft = this.GetBottomLeftAnchor();
			Vector2 bottomRight = this.GetBottomRightAnchor();

			//Vector2 otherTopRight = new Vector2(otherBottomRight.X, otherTopLeft.Y); 
			//Vector2 otherBottomLeft = new Vector2(otherTopLeft.X, otherBottomRight.Y); 
	

			return (topLeft.X <= otherBottomRight.X && 
			        bottomRight.X >= otherTopLeft.X && 
							topLeft.Y <= otherBottomRight.Y && 
							bottomRight.Y >= otherTopLeft.Y);
		}

		public bool DoesOverlap(BoundingBox other) {
			return DoesOverlap(other.GetTopLeftAnchor(), other.GetBottomRightAnchor());
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