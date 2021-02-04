using Designer.GUI.Drawables;
using Love;

namespace Designer.GUI {
	public class Button {
		public delegate void ClickedHandler(Button button, MouseButton mouseButton);
		public event ClickedHandler Clicked;

		public delegate void ReleasedHandler(Button button, MouseButton mouseButton);
		public event ReleasedHandler Released;

		public delegate void HoveredHandler(Button button);
		public event HoveredHandler Hovered;

		public delegate void ExitedHandler(Button button);
		public event ExitedHandler Exited;

		protected IDrawable drawable = null;

		protected Vector2 position = Vector2.Zero;
		protected Vector2 size = Vector2.Zero;

		public Button(Vector2 position, Vector2 size, IDrawable drawable) {
			this.position = position;
			this.size = size;

			this.drawable = drawable;
		}

		public IDrawable GetDrawable() {
			return this.drawable;
		}

		public bool CheckOverlap(float x, float y) {
			return x >= position.X && x <= position.X + size.X && y >= position.Y && y <= position.Y + size.Y;
		}

		public void Draw() {
			if (drawable == null)
				return;

			drawable.Draw(this.position, this.size);
		}

		public virtual void OnClick(MouseButton mouseButton) { 
			Clicked?.Invoke(this, mouseButton);
		}

		public virtual void OnRelease(MouseButton mouseButton) { 
			Released?.Invoke(this, mouseButton);
		}

		public virtual void OnHover() { 
			Hovered?.Invoke(this);
		}

		public virtual void OnExit() { 
			Exited?.Invoke(this);
		}
	}
}