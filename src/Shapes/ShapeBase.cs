using System.Collections.Generic;
using Love;

namespace Designer {
	public abstract class ShapeBase {
		protected Vector2 position = Vector2.Zero;
		protected Vector2 size = Vector2.Zero;

		private bool dirty = default;
		private List<Vector2> cachedBoundingBoxPoints = null;
		
		public ShapeBase(Vector2 position, Vector2 size) {
			this.position = position;
			this.size = size;

			this.dirty = true;
			this.cachedBoundingBoxPoints = new List<Vector2>();
		}
		
		public virtual void Draw() {

		}

		public Vector2 GetPosition() {
			return this.position;
		}

		public void SetPosition(Vector2 position) {
			this.position = position;
			this.dirty = true;
		}

		public Vector2 GetSize() {
			return this.size;
		}

		public void SetSize(Vector2 size) {
			this.size = size;
			this.dirty = true;
		}

		public List<Vector2> GetBoundingBoxPoints() {
			if (this.dirty) {
				this.cachedBoundingBoxPoints.Clear();

				this.cachedBoundingBoxPoints.Add(this.position + this.size * Vector2.Zero);
				this.cachedBoundingBoxPoints.Add(this.position + this.size * Vector2.Down);
				this.cachedBoundingBoxPoints.Add(this.position + this.size * Vector2.Right);
				this.cachedBoundingBoxPoints.Add(this.position + this.size * Vector2.One);

				this.dirty = false;
			}

			return this.cachedBoundingBoxPoints;
		}
	}
}