using System;
using System.Collections.Generic;
using Love;

namespace Designer {
	public class Program : Scene {
		private Canvas canvas = null;
		private List<ShapeBase> shapes = null;

		private Vector2 startLocation = Vector2.Zero;

		private ShapeBase ghostShape = null;

		public override void Load() {
			base.Load();

			canvas = new Canvas();
			shapes = new List<ShapeBase>();;

			shapes.Add(
				new ShapeRectangle(
					new Vector2(100, 100), 
					new Vector2(50, 50)
				)
			);

			shapes.Add(
				new ShapeEllipse(
					new Vector2(0, 0), 
					new Vector2(50, 25)
				)
			);

			canvas.AddElement(new Button(new Vector2(10, 10), new Vector2(100, 200), new DrawableRectangle(Color.Pink, Color.Purple)));
		}

		public override void Draw() {
			base.Draw();

			Graphics.Push(StackType.All);
			foreach (ShapeBase shape in shapes)
				shape.Draw();
			Graphics.Pop();

			if (ghostShape != null) {
				Graphics.Push(StackType.All);
				ghostShape.Draw();
				Graphics.Pop();
			}

			Graphics.Push(StackType.All);
			canvas.Draw();
			Graphics.Pop();
		}

		public override void MousePressed(float x, float y, int button, bool isTouch) {
			base.MousePressed(x, y, button, isTouch);

			if (canvas.OnMousePressed((MouseButton)button, x, y))
				return;


			if ((MouseButton)button == MouseButton.LeftButton) {
				ghostShape = new ShapeEllipse(startLocation, Vector2.Zero);

				startLocation = new Vector2(x, y);
				ghostShape.SetPosition(new Vector2(x, y));
				ghostShape.SetSize(Vector2.Zero);
			}
		}

		public override void MouseReleased(float x, float y, int button, bool isTouch) {
			base.MouseReleased(x, y, button, isTouch);

			canvas.OnMouseReleased((MouseButton)button, x, y);

			if ((MouseButton)button == MouseButton.LeftButton && ghostShape != null) {
				shapes.Add(ghostShape);
				ghostShape = null;
			}
		}

		public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
			base.MouseMoved(x, y, dx, dy, isTouch);

			if (canvas.OnMouseMoved(x, y))
				return;

			if (ghostShape != null) {
				Vector2 newPosition = Vector2.Min(new Vector2(x, y), startLocation);
				Vector2 newSize = (startLocation - new Vector2(x, y)).Abs();

				ghostShape.SetPosition(newPosition);
				ghostShape.SetSize(newSize);
			}
		}
	}
}