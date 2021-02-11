using System;
using System.Collections.Generic;
using Designer.Shapes;
using Designer.Utility;
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

		private Vector2 cameraPosition = Vector2.Zero;

		private Renderer renderer = null;

		private GUI.Canvas canvas = null;
		private ShapeGroup rootShape = null;

		private Vector2 startLocation = Vector2.Zero;
		private IShape ghostShape = null;

		private SelectionBox selectionBox = null;

		private ShapeGroup selectedGroup = null;
		private Selection selection = null;

		private State state = State.SELECTING;

		private bool debugWindowShown = false;

		private Font font = null;

		public override void Load() {
			base.Load();

			font = Graphics.NewFont("Assets/font.ttf", 12, HintingMode.Normal);
			Graphics.SetFont(font);

			Graphics.SetBackgroundColor(0.200f, 0.137f, 0.208f, 1.000f);
			Graphics.SetLineWidth(2.0f);
			Graphics.SetLineStyle(LineStyle.Rough);

			this.renderer = new Renderer("Assets/font.ttf", 26);

			this.canvas = new GUI.Canvas();
			this.rootShape = new ShapeGroup();
			this.selection = new Selection();

			// canvas.AddElement(new Button(new Vector2(10, 10), new Vector2(100, 200), new DrawableRectangle(Color.Pink, Color.Purple)));
		}

		public override void Update(float dt) {
			base.Update(dt);

			if (selection != null) {
				selection.Update(this.ViewportToWorldPosition(Mouse.GetPosition()));
			}

			this.renderer.Update(dt, () => {
				if (debugWindowShown && ImGui.Begin("Debug Window")) {
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
						ImGui.Text("Camera:");
						ImGui.Spacing();

						ImGui.DragFloat2("Camera position", ref cameraPosition);
					}

					ImGui.Spacing();
					ImGui.Separator();

					{
						ImGui.Text("Selection:");
						ImGui.Spacing();

						/*
						ImGui.Text($"Selection count: {(selectedGroup == null ? 0 : selectedGroup.Count)}");

						ImGui.Text("Selected shapes:");
						if (selectedGroup != null) {
							foreach (IShape shape in selectedGroup) {
								DrawSelectedShapeUI(shape);
							}
						}
						*/
					}

					ImGui.End();
				}
			});
		}

		private void DrawSelectedShapeUI(IShape shape) {
			if (ImGui.CollapsingHeader($"Shape {shape.GetHashCode().ToString()}")) {
				ImGui.Text($"Type: {shape.GetType()}");

				ImGui.Spacing();

				Vector2 topLeft = shape.GetTopLeftAnchor();
				ImGui.SliderFloat2("Top Left Anchor", ref topLeft, 0.0f, 1280.0f);
				shape.SetTopLeftAnchor(topLeft);

				Vector2 topRight = shape.GetTopRightAnchor();
				ImGui.SliderFloat2("Top Right Anchor", ref topRight, 0.0f, 1280.0f);
				shape.SetTopRightAnchor(topRight);

				Vector2 bottomLeft = shape.GetBottomLeftAnchor();
				ImGui.SliderFloat2("Bottom Left Anchor", ref bottomLeft, 0.0f, 1280.0f);
				shape.SetBottomLeftAnchor(bottomLeft);

				Vector2 bottomRight = shape.GetBottomRightAnchor();
				ImGui.SliderFloat2("Bottom Right Anchor", ref bottomRight, 0.0f, 1280.0f);
				shape.SetBottomRightAnchor(bottomRight);
			}
		}

		private Love.Canvas loveCanvas = Love.Graphics.NewCanvas(1920, 1080); 

		public override void Draw() {
			base.Draw();
		
			loveCanvas.SetFilter(FilterMode.Nearest, FilterMode.Nearest);

			Graphics.SetCanvas(loveCanvas);
			Graphics.Clear();

			Graphics.Push();
			Graphics.Translate(cameraPosition.X, cameraPosition.Y);

			Graphics.Push(StackType.All);
			rootShape.Draw();
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
				selection.SetSelectedGroup(this.selectedGroup);
				selection.Draw();
				Graphics.Pop();
			}

			Graphics.Pop();

			Graphics.SetCanvas();

			Graphics.Draw(loveCanvas, 0, 0, 0, 4, 4);

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

			Vector2 mousePos = ViewportToWorldPosition(new Vector2(x, y));

			if ((MouseButton)button == MouseButton.LeftButton) {
				switch (this.state) {
					case State.SELECTING:
						selectionBox = new SelectionBox(mousePos, mousePos);

						break;
					case State.DRAWING_RECTANGLE:
						startLocation = mousePos;

						ghostShape = new ShapeRectangle(new BoundingBox(startLocation, Vector2.Zero));

						ghostShape.SetTopLeftAnchor(mousePos);
						ghostShape.SetBottomRightAnchor(mousePos);

						break;
					case State.DRAWING_ELLIPSE:
						startLocation = mousePos;

						ghostShape = new ShapeEllipse(new BoundingBox(startLocation, Vector2.Zero));

						
						ghostShape.SetTopLeftAnchor(mousePos);
						ghostShape.SetBottomRightAnchor(mousePos);

						break;
					case State.USING_SELECTION:
						selection.MousePressed((MouseButton)button, mousePos);

						break;
				}
			}
		}

		public override void MouseReleased(float x, float y, int button, bool isTouch) {
			base.MouseReleased(x, y, button, isTouch);

			if (ImGui.GetIO().WantCaptureMouse)
				return;

			// canvas.OnMouseReleased((MouseButton)button, x, y);

			Vector2 mousePos = ViewportToWorldPosition(new Vector2(x, y));

			if ((MouseButton)button == MouseButton.LeftButton) {
				if (ghostShape != null) {
					rootShape.AddShape(ghostShape);
					ghostShape = null;
				}

				if (state == State.SELECTING) {
					this.selectionBox = null;

					if (selectedGroup != null)
						state = State.USING_SELECTION;
				}

				if (state == State.USING_SELECTION) {
					selection.MouseReleased((MouseButton)button, mousePos);
				}
			}
		}

		public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
			base.MouseMoved(x, y, dx, dy, isTouch);

			if (ImGui.GetIO().WantCaptureMouse)
				return;

			if (canvas.OnMouseMoved(x, y))
				return;

			if (Mouse.IsDown(MouseButton.MiddleButton)) {
				cameraPosition += new Vector2(dx / 4.0f, dy / 4.0f);
				return;
			}

			Vector2 mousePos = ViewportToWorldPosition(new Vector2(x, y));

			if (ghostShape != null) {
				Vector2 topLeft = Vector2.Min(mousePos, startLocation);
				Vector2 bottomRight = Vector2.Max(mousePos, startLocation);

				ghostShape.SetTopLeftAnchor(topLeft);
				ghostShape.SetBottomRightAnchor(bottomRight);
			}

			if (state == State.SELECTING && selectionBox != null) {
				selectionBox.SetEndPosition(mousePos);

				selectedGroup = selectionBox.GetSelectedGroup(rootShape.GetShapes());
			}

			if (state == State.USING_SELECTION) {
				if (Mouse.IsDown(MouseButton.LeftButton)) {
					selection.MouseMoved(mousePos);
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
				selectedGroup = null;
			}
			else if (key == KeyConstant.Number2) {
				this.state = State.DRAWING_RECTANGLE;
				selectedGroup = null;
			}
			else if (key == KeyConstant.Number3) {
				this.state = State.DRAWING_ELLIPSE;
				selectedGroup = null;
			}

			if (Keyboard.IsDown(KeyConstant.LCtrl)) {
				if (key == KeyConstant.F3) {
					debugWindowShown = !debugWindowShown;
				}
			}
		}

		private Vector2 ViewportToWorldPosition(Vector2 position) {
			return ((position) / 4.0f) - cameraPosition;
		}
	}
}