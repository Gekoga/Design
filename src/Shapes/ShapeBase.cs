using Love;

namespace Designer {
	public abstract class ShapeBase {
		protected Vector2 position = Vector2.Zero;
		protected Vector2 size = Vector2.Zero;
		
		public ShapeBase(Vector2 position, Vector2 size) {
			this.position = position;
			this.size = size;
		}
		
		public virtual void Draw() {

		}

		public Vector2 GetPosition() {
			return this.position;
		}

		public void SetPosition(Vector2 position) {
			this.position = position;
		}

		public Vector2 GetSize() {
			return this.size;
		}

		public void SetSize(Vector2 size) {
			this.size = size;
		}
	}
}