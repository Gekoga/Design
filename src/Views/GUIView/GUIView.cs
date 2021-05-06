using System;
using System.Numerics;
using Designer.Controllers.App;
using Designer.Models.Shapes;
using Designer.Views.GUIView.GUI;
using ImGuiNET;
using Love.Imgui;

namespace Designer.Views.GUIView {
	public class GUIView : Love.Scene, IDisposable {
		private GUIViewSettings settings = null;

		private Love.Imgui.Renderer imGuiRenderer = null;

		private GUIViewStateMachine stateMachine = null;

		private const int RenderScale = 4;

		private Context context = null;

		private AppController controller = null;

		private Love.Canvas canvas = null;

		private ToolsWindow toolsWindow = null;

		private GroupShape selectedGroup = null;

		public GUIView(Context context, AppController controller) {
			this.settings = new GUIViewSettings(
				new Love.Color(1.000f, 1.000f, 1.000f, 1.0f),
				new Love.Color(1.000f, 0.259f, 0.333f, 1.0f),
				new Love.Color(1.000f, 0.922f, 0.549f, 0.7f),
				new Love.Color(0.000f, 0.922f, 0.549f, 0.7f)
			);

			this.context = context;

			this.controller = controller;
			this.controller.StateChanged += OnStateChanged;
		}

		public AppController GetController() {
			return this.controller;
		}

		public GUIViewSettings GetSettings() {
			return this.settings;
		}

		public override void Load() {
			this.imGuiRenderer = new Renderer("G:/font/msyh.ttf", 18);

			var font = Love.Graphics.NewFont("assets/small_pixel.ttf");
			font.SetFilter(Love.FilterMode.Nearest, Love.FilterMode.Nearest);
			Love.Graphics.SetFont(font);

			this.stateMachine = new GUIViewStateMachine(this, context);

			this.toolsWindow = new ToolsWindow(this, this.controller); 

			RecreateCanvas();

			Love.Graphics.SetBackgroundColor(0.282f, 0.247f, 0.337f, 1.0f);
			Love.Graphics.SetLineWidth(3);
		}

		public override void Update(float dt) {
			imGuiRenderer.Update(dt, ImGuiLayout);

			this.stateMachine.Update(dt);
		}

		public void ImGuiLayout() {
			toolsWindow.Run();	
		}

		public override void Draw() {
			Love.Graphics.Push(Love.StackType.All);

			Love.Graphics.SetColor(1.0f, 1.0f, 1.0f, 1.0f);
			Love.Graphics.SetBackgroundColor(0.1f, 0.1f, 0.1f);
			Love.Graphics.SetLineWidth(4);
			Love.Graphics.SetLineStyle(Love.LineStyle.Rough);

			Love.Graphics.SetCanvas(this.canvas);
			{
				Love.Graphics.Clear();

				var shapes = this.controller.GetShapes();
				var selectedShapes = controller.GetSelection();

				IShapeVisitor shapeVisitorRenderer = new ShapeVisitorRenderer(selectedShapes, this.settings);

				foreach (var shape in shapes)
					shape.GetShape().Accept(shapeVisitorRenderer);

				this.stateMachine.Draw();
			}
			Love.Graphics.SetColor(Love.Color.White);
			Love.Graphics.SetCanvas();

			Love.Graphics.Draw(this.canvas, 0, 0, 0, RenderScale, RenderScale);

			Love.Graphics.Pop();

			imGuiRenderer.Draw();
		}

		public override void MousePressed(float x, float y, int button, bool isTouch) {
			if (ImGui.GetIO().WantCaptureMouse)
				return;

			Vector2 position = this.ScreenToViewport(new Vector2(x, y));
			Love.MouseButton mouseButton = (Love.MouseButton)button;

			this.stateMachine.MousePressed(x / 4, y / 4, button, isTouch);
		}

		public override void MouseReleased(float x, float y, int button, bool isTouch) {
			if (ImGui.GetIO().WantCaptureMouse)
				return;

			Vector2 position = this.ScreenToViewport(new Vector2(x, y));
			Love.MouseButton mouseButton = (Love.MouseButton)button;

			this.stateMachine.MouseReleased(x / 4, y / 4, button, isTouch);
		}

		public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
			if (ImGui.GetIO().WantCaptureMouse)
				return;

			Vector2 position = this.ScreenToViewport(new Vector2(x, y));
			Vector2 delta = this.ScreenToViewport(new Vector2(x, y));

			this.stateMachine.MouseMoved(x / 4, y / 4, dx, dy, isTouch);
		}

		public override void KeyPressed(Love.KeyConstant key, Love.Scancode scancode, bool isRepeat) {
			if (ImGui.GetIO().WantCaptureKeyboard)
				return;

			if (Love.Keyboard.IsDown(Love.KeyConstant.LCtrl) || Love.Keyboard.IsDown(Love.KeyConstant.RCtrl)) {
				if (key == Love.KeyConstant.Z) {
					this.controller.Undo();
					return;
				}

				if (key == Love.KeyConstant.Y) {
					this.controller.Redo();
					return;
				}
			} else {
				if (key == Love.KeyConstant.F6) {
					Love.Event.Quit();
					return;
				}

				if (key == Love.KeyConstant.R) {
					this.stateMachine.startDrawingRectangle();
					return;
				}

				if (key == Love.KeyConstant.E) {
					this.stateMachine.startDrawingEllipse();
					return;
				}

				if (key == Love.KeyConstant.S) {
					this.stateMachine.startSelecting();
					return;
				}

				if (key == Love.KeyConstant.Escape) {
					this.stateMachine.cancel();
					return;
				}
			}
			
			this.stateMachine.KeyPressed(key, scancode, isRepeat);
		}

		public override void TextInput(string text) {
			imGuiRenderer.TextInput(text);
		}

		public override void WindowResize(int w, int h) {
			RecreateCanvas();
		}

		public void Dispose() {
			this.controller.StateChanged -= OnStateChanged;
		}

		private void OnStateChanged() {

		}

		private void RecreateCanvas() {
			this.context.GetLogger().LogDebug($"Recreating canvas");

			if (this.canvas != null)
				this.canvas.Dispose();

			this.canvas = Love.Graphics.NewCanvas(Love.Graphics.GetWidth() / RenderScale, Love.Graphics.GetHeight() / RenderScale);
			this.canvas.SetFilter(Love.FilterMode.Nearest, Love.FilterMode.Nearest);
		}

		private Vector2 ScreenToViewport(Vector2 position) {
			return position / (Vector2.One * RenderScale);
		}

		public GroupShape GetSelectedGroup() {
			return this.selectedGroup;
		}

		public void SetSelectedGroup(GroupShape groupShape) {
			this.selectedGroup = groupShape;
		}
	}
}