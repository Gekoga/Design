using System;
using System.Collections.Generic;
using Love;

namespace Designer {
	public class Program : Scene {
		public enum State {
			DRAWING_RECTANGLE,
			DRAWING_ELLIPSE,
			SELECTING,
			USING_SELECTION,
		}

		private Canvas canvas = null;
		private List<ShapeBase> shapes = null;

		private Vector2 startLocation = Vector2.Zero;
		private ShapeBase ghostShape = null;

		private Selection selection = null;

		private State state = State.SELECTING;

		public override void Load() {
			base.Load();

			canvas = new Canvas();
			shapes = new List<ShapeBase>(); ;

			// canvas.AddElement(new Button(new Vector2(10, 10), new Vector2(100, 200), new DrawableRectangle(Color.Pink, Color.Purple)));
		}

		public override void Update(float dt) {
			base.Update(dt);

			if (selection != null) {
				selection.Update(shapes);
			}
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

			if (selection != null) {
				Graphics.Push(StackType.All);
				if (state == State.SELECTING)
					selection.DrawSelectionBox();
				selection.DrawSelections();
				Graphics.Pop();
			}

			Graphics.Push(StackType.All);
			canvas.Draw();
			Graphics.Pop();

			Graphics.Print($"Current state: {state.ToString()}", 10, 10);
		}

		public override void MousePressed(float x, float y, int button, bool isTouch) {
			base.MousePressed(x, y, button, isTouch);

			if (canvas.OnMousePressed((MouseButton)button, x, y))
				return;

			if ((MouseButton)button == MouseButton.LeftButton) {
				switch (this.state) {
					case State.SELECTING:
						selection = new Selection(new Vector2(x, y), new Vector2(x, y));

						break;
					case State.DRAWING_RECTANGLE:
						ghostShape = new ShapeRectangle(startLocation, Vector2.Zero);

						startLocation = new Vector2(x, y);
						ghostShape.SetPosition(new Vector2(x, y));
						ghostShape.SetSize(Vector2.Zero);

						break;
					case State.DRAWING_ELLIPSE:
						ghostShape = new ShapeEllipse(startLocation, Vector2.Zero);

						startLocation = new Vector2(x, y);
						ghostShape.SetPosition(new Vector2(x, y));
						ghostShape.SetSize(Vector2.Zero);

						break;
				}
			}
		}

		public override void MouseReleased(float x, float y, int button, bool isTouch) {
			base.MouseReleased(x, y, button, isTouch);

			canvas.OnMouseReleased((MouseButton)button, x, y);

			if ((MouseButton)button == MouseButton.LeftButton) {
				if (ghostShape != null) {
					shapes.Add(ghostShape);
					ghostShape = null;
				}

				if (state == State.SELECTING) {
					state = State.USING_SELECTION;
				}
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

			if (state == State.SELECTING && selection != null) {
				selection.SetEndPosition(new Vector2(x, y));
			}
		}

		public override void KeyPressed(KeyConstant key, Scancode scancode, bool isRepeat) {
			base.KeyPressed(key, scancode, isRepeat);

			if (key == KeyConstant.Number1) {
				this.state = State.SELECTING;
				selection = null;
			}
			else if (key == KeyConstant.Number2) {
				this.state = State.DRAWING_RECTANGLE;
				selection = null;
			}
			else if (key == KeyConstant.Number3) {
				this.state = State.DRAWING_ELLIPSE;
				selection = null;
			}
				
		}
	}
}