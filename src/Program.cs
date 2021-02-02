using System;
using Love;

namespace Designer {
	public class Program : Scene {
		private Canvas canvas = null;

		public Program() {
			canvas = new Canvas();

			canvas.AddElement(new Button(
				new Vector2(10, 10),
				new Vector2(100, 100),
				new DrawableRectangle(Color.Blue, Color.White)
			));

			Button myButton = new Button(
				new Vector2(120, 10),
				new Vector2(200, 100),
				new DrawableRectangle(Color.Blue, Color.White)
			);
			myButton.Clicked += (button, mouseButton) => {
				button.SetColor(new Color(1.0f, 0.0f, 0.0f, 1.0f));
			};

			myButton.Released += (button, mouseButton) => {
				button.SetColor(new Color(0.0f, 0.0f, 1.0f, 1.0f));
			};

			canvas.AddElement(myButton);
		}

		public override void Draw() {
			base.Draw();

			canvas.Draw();
		}

		public override void MousePressed(float x, float y, int button, bool isTouch) {
			base.MousePressed(x, y, button, isTouch);

			canvas.OnMousePressed((MouseButton)button, x, y);
		}

		public override void MouseReleased(float x, float y, int button, bool isTouch) {
			base.MouseReleased(x, y, button, isTouch);

			canvas.OnMouseReleased((MouseButton)button, x, y);
		}

		public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
			base.MouseMoved(x, y, dx, dy, isTouch);

			canvas.OnMouseMoved(x, y);
		}
	}
}