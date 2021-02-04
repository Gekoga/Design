using System;
using System.Collections.Generic;
using Designer.Shapes;
using Designer.Utility.Extensions;
using ImGuiNET;
using Love;
using Love.Imgui;

namespace Designer {
	public class Program : Scene {
		public enum State {
			DRAWING_RECTANGLE,
			DRAWING_ELLIPSE,
			SELECTING,
			USING_SELECTION,
		}

		private Renderer renderer = null;

		private GUI.Canvas canvas = null;
		private List<ShapeBase> shapes = null;

		private Vector2 startLocation = Vector2.Zero;
		private ShapeBase ghostShape = null;

		private SelectionBox selectionBox = null;

		private List<ShapeBase> selectedShapes = null;
		private Selection selection = null;
		private Vector2? selectionMoveRelStartPoint = null;

		private State state = State.SELECTING;

		public override void Load() {
			base.Load();

			this.renderer = new Renderer("Firacode", 12);

			this.canvas = new GUI.Canvas();
			this.shapes = new List<ShapeBase>();
			this.selection = new Selection();

			// canvas.AddElement(new Button(new Vector2(10, 10), new Vector2(100, 200), new DrawableRectangle(Color.Pink, Color.Purple)));
		}

		public override void Update(float dt) {
			base.Update(dt);

			if (selectionBox != null) {
				//selection.Update(shapes);
			}

			this.renderer.Update(dt, () => {
				if (ImGui.Begin("Debug Window")) {
					{
						ImGui.Text("Information:");
						ImGui.Spacing();

						ImGui.Text($"Deltatime: {(dt * 1000).ToString("N2")}ms");
						ImGui.Text($"Framerate: {Timer.GetFPS()}");
						ImGui.Text($"OS: {Special.GetOS()}");
						ImGui.Text($"Processor count: {Special.GetProcessorCount()}");
						ImGui.Text($"System time: {DateTime.Now}");	
					}

					ImGui.Spacing();
					ImGui.Separator();

					{
						ImGui.Text("Selection:");
						ImGui.Spacing();

						ImGui.Text($"Selection count: {(selectedShapes == null ? 0 : selectedShapes.Count)}");

						ImGui.Text("Selected shapes:");
						if (selectedShapes != null) {
							foreach (ShapeBase shape in selectedShapes) {
								DrawSelectedShapeUI(shape);
							}
						}
					}

					ImGui.End();
				}
			});
		}

		private void DrawSelectedShapeUI(ShapeBase shape) {
			if (ImGui.CollapsingHeader($"Shape {shape.GetHashCode().ToString()}")) {
				ImGui.Text($"Type: {shape.GetType()}");

				ImGui.Spacing();

				Vector2 position = shape.GetPosition();
				ImGui.SliderFloat("X Position", ref position.X, 0.0f, 1280.0f);
				ImGui.SliderFloat("Y Position", ref position.Y, 0.0f, 720.0f);
				shape.SetPosition(position);

				ImGui.Spacing();

				Vector2 size = shape.GetSize();
				ImGui.SliderFloat("Size X", ref size.X, 0.0f, 1280.0f);
				ImGui.SliderFloat("Size Y", ref size.Y, 0.0f, 720.0f);
				shape.SetSize(size);
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

			if (selectionBox != null) {
				Graphics.Push(StackType.All);
				if (state == State.SELECTING)
					selectionBox.Draw();
				Graphics.Pop();
			}

			if (this.state == State.USING_SELECTION || this.state == State.SELECTING) {
				Graphics.Push(StackType.All);
				selection.SetSelectedShapes(this.selectedShapes);
				selection.Draw();
				Graphics.Pop();
			}

			Graphics.Push(StackType.All);
			canvas.Draw();
			Graphics.Pop();

			this.renderer.Draw();
		}

		public override void MousePressed(float x, float y, int button, bool isTouch) {
			base.MousePressed(x, y, button, isTouch);

			if (ImGui.GetIO().WantCaptureMouse)
				return;

			if (canvas.OnMousePressed((MouseButton)button, x, y))
				return;

			if ((MouseButton)button == MouseButton.LeftButton) {
				switch (this.state) {
					case State.SELECTING:
						selectionBox = new SelectionBox(new Vector2(x, y), new Vector2(x, y));

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
					case State.USING_SELECTION:
						selectionMoveRelStartPoint = new Vector2(x, y) - selection.GetPosition();
						//Console.WriteLine(selectionMoveStartPoint.ToString());
	
						break;
				}
			}
		}

		public override void MouseReleased(float x, float y, int button, bool isTouch) {
			base.MouseReleased(x, y, button, isTouch);

			if (ImGui.GetIO().WantCaptureMouse)
				return;

			canvas.OnMouseReleased((MouseButton)button, x, y);

			if ((MouseButton)button == MouseButton.LeftButton) {
				if (ghostShape != null) {
					shapes.Add(ghostShape);
					ghostShape = null;
				}

				if (state == State.SELECTING) {
					this.selectionBox = null;

					if (selectedShapes != null && selectedShapes.Count > 0)
						state = State.USING_SELECTION;
				}

				if (state == State.USING_SELECTION) {
					selectionMoveRelStartPoint = null;
				}
			}
		}

		public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
			base.MouseMoved(x, y, dx, dy, isTouch);

			if (ImGui.GetIO().WantCaptureMouse)
				return;

			if (canvas.OnMouseMoved(x, y))
				return;

			if (ghostShape != null) {
				Vector2 newPosition = Vector2.Min(new Vector2(x, y), startLocation);
				Vector2 newSize = (startLocation - new Vector2(x, y)).Abs();

				ghostShape.SetPosition(newPosition);
				ghostShape.SetSize(newSize);
			}

			if (state == State.SELECTING && selectionBox != null) {
				selectionBox.SetEndPosition(new Vector2(x, y));

				selectedShapes = selectionBox.GetSelectedShapes(shapes);
			}

			if (state == State.USING_SELECTION) {
				if (Mouse.IsDown(MouseButton.LeftButton)) {
					if (selectionMoveRelStartPoint == null)
						selectionMoveRelStartPoint = new Vector2(x, y) - selection.GetPosition();

					selection.SetPosition(new Vector2(x, y) - (Vector2)selectionMoveRelStartPoint);
				}
			}
		}

		public override void KeyPressed(KeyConstant key, Scancode scancode, bool isRepeat) {
			base.KeyPressed(key, scancode, isRepeat);

			if (ImGui.GetIO().WantCaptureKeyboard)
				return;

			if (key == KeyConstant.Number1) {
				this.state = State.SELECTING;
				selectionBox = null;
				selectedShapes = null;
			}
			else if (key == KeyConstant.Number2) {
				this.state = State.DRAWING_RECTANGLE;
				selectedShapes = null;
			}
			else if (key == KeyConstant.Number3) {
				this.state = State.DRAWING_ELLIPSE;
				selectedShapes = null;
			}
		}
	}
}