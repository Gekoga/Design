using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine : Love.Scene {
		private enum Trigger {
			START_DRAWING_RECTANGLE,
			START_DRAWING_ELLIPSE,
			FINISH_DRAWING,

			START_SELECTING,
			FINISH_SELECTING,

			START_USING_SELECTION,
			START_MOVING_SELECTION,
			STOP_MOVING_SELECTION,
			START_RESIZING_SELECTION,
			STOP_RESIZING_SELECTION,
			FINISH_USING_SELECTION,

			START_PANNING,
			FINISH_PANNING,

			CANCEL,
		};

		private StateMachine<State, Trigger> machine = null;

		private State state = null;

		public GUIViewStateMachine(GUIView guiView, Context context) {
			var logger = context.GetLogger();

			this.machine = new StateMachine<State, Trigger>(
				() => this.state,
				s => this.state = s,
				FiringMode.Immediate
			);

			machine.OnUnhandledTrigger((state, trigger) => { });
			machine.OnTransitioned(t => {
				logger.LogDebug($"{t.Source.GetType().Name} -> {t.Destination.GetType().Name} via {t.Trigger}({string.Join(", ", t.Parameters)})");
			});

			var idle = new Idle(logger, machine);

			var drawingRectangle = new DrawingRectangle(logger, machine, guiView);
			var drawingEllipse = new DrawingEllipse(logger, machine, guiView);

			var selecting = new Selecting(logger, machine, guiView);

			var usingSelection = new UsingSelection(logger, machine);
			var movingSelection = new MovingSelection(logger, machine);
			var resizingSelection = new ResizingSelection(logger, machine);

			var panning = new Panning(logger, machine);

			// Idle
			this.machine.Configure(idle)
				.Permit(Trigger.START_DRAWING_RECTANGLE, drawingRectangle)
				.Permit(Trigger.START_DRAWING_ELLIPSE, drawingEllipse)
				.Permit(Trigger.START_SELECTING, selecting)
				.Permit(Trigger.START_USING_SELECTION, usingSelection)
				.Permit(Trigger.START_PANNING, panning);

			// Drawing
			this.machine.Configure(drawingRectangle)
				.Permit(Trigger.CANCEL, idle)
				.Permit(Trigger.FINISH_DRAWING, usingSelection);

			this.machine.Configure(drawingEllipse)
				.Permit(Trigger.CANCEL, idle)
				.Permit(Trigger.FINISH_DRAWING, usingSelection);

			// Selecting
			this.machine.Configure(selecting)
				.Permit(Trigger.CANCEL, idle)
				.Permit(Trigger.FINISH_SELECTING, usingSelection)
				.OnExit(() => {
					// TODO: Remove ghost selection
				});

			// Using selection
			this.machine.Configure(usingSelection)
				.Permit(Trigger.START_MOVING_SELECTION, movingSelection)
				.Permit(Trigger.START_RESIZING_SELECTION, resizingSelection)
				.Permit(Trigger.FINISH_USING_SELECTION, idle)
				.Permit(Trigger.CANCEL, idle)
				.OnEntry(() => {
					// TODO: Check if any object selected
				})
				.OnEntryFrom(Trigger.FINISH_DRAWING, () => {
					// TODO: Select the drawn shape
				})
				.OnExit(() => {
					guiView.GetController().ClearSelection();
				});

			this.machine.Configure(movingSelection)
				.SubstateOf(usingSelection)
				.Permit(Trigger.STOP_MOVING_SELECTION, usingSelection);

			this.machine.Configure(resizingSelection)
				.SubstateOf(usingSelection)
				.Permit(Trigger.STOP_RESIZING_SELECTION, usingSelection);

			// Panning
			this.machine.Configure(panning)
				.Permit(Trigger.FINISH_PANNING, idle)
				.Permit(Trigger.CANCEL, idle);

			this.state = idle;
		}

		public void startDrawingRectangle() {
			this.machine.Fire(Trigger.START_DRAWING_RECTANGLE);
		}

		public void startDrawingEllipse() {
			this.machine.Fire(Trigger.START_DRAWING_ELLIPSE);
		}

		public void startSelecting() {
			this.machine.Fire(Trigger.START_SELECTING);
		}

		public void cancel() {
			this.machine.Fire(Trigger.CANCEL);
		}


		public override void KeyPressed(Love.KeyConstant key, Love.Scancode scancode, bool isRepeat) {
			this.machine.State.KeyPressed(key, scancode, isRepeat);
		}

		public override void KeyReleased(Love.KeyConstant key, Love.Scancode scancode) {
			this.machine.State.KeyReleased(key, scancode);
		}

		public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
			this.machine.State.MouseMoved(x, y, dx, dy, isTouch);
		}

		public override void MousePressed(float x, float y, int button, bool isTouch) {
			this.machine.State.MousePressed(x, y, button, isTouch);
		}

		public override void MouseReleased(float x, float y, int button, bool isTouch) {
			this.machine.State.MouseReleased(x, y, button, isTouch);
		}

		public override void MouseFocus(bool focus) {
			this.machine.State.MouseFocus(focus);
		}

		public override void WheelMoved(int x, int y) {
			this.machine.State.WheelMoved(x, y);
		}

		public override void JoystickPressed(Love.Joystick joystick, int button) {
			this.machine.State.JoystickPressed(joystick, button);
		}

		public override void JoystickReleased(Love.Joystick joystick, int button) {
			this.machine.State.JoystickReleased(joystick, button);
		}

		public override void JoystickAxis(Love.Joystick joystick, float axis, float value) {
			this.machine.State.JoystickAxis(joystick, axis, value);
		}

		public override void JoystickHat(Love.Joystick joystick, int hat, Love.JoystickHat direction) {
			this.machine.State.JoystickHat(joystick, hat, direction);
		}

		public override void JoystickGamepadPressed(Love.Joystick joystick, Love.GamepadButton button) {
			this.machine.State.JoystickGamepadPressed(joystick, button);
		}

		public override void JoystickGamepadReleased(Love.Joystick joystick, Love.GamepadButton button) {
			this.machine.State.JoystickGamepadReleased(joystick, button);
		}

		public override void JoystickGamepadAxis(Love.Joystick joystick, Love.GamepadAxis axis, float value) {
			this.machine.State.JoystickGamepadAxis(joystick, axis, value);
		}

		public override void JoystickAdded(Love.Joystick joystick) {
			this.machine.State.JoystickAdded(joystick);
		}

		public override void JoystickRemoved(Love.Joystick joystick) {
			this.machine.State.JoystickRemoved(joystick);
		}

		public override void TouchMoved(long id, float x, float y, float dx, float dy, float pressure) {
			this.machine.State.TouchMoved(id, x, y, dx, dy, pressure);
		}

		public override void TouchPressed(long id, float x, float y, float dx, float dy, float pressure) {
			this.machine.State.TouchPressed(id, x, y, dx, dy, pressure);
		}

		public override void TouchReleased(long id, float x, float y, float dx, float dy, float pressure) {
			this.machine.State.TouchReleased(id, x, y, dx, dy, pressure);
		}

		public override void TextEditing(string text, int start, int end) {
			this.machine.State.TextEditing(text, start, end);
		}

		public override void TextInput(string text) {
			this.machine.State.TextInput(text);
		}

		public override void WindowFocus(bool focus) {
			this.machine.State.WindowFocus(focus);
		}

		public override void WindowVisible(bool visible) {
			this.machine.State.WindowVisible(visible);
		}

		public override void WindowResize(int w, int h) {
			this.machine.State.WindowResize(w, h);
		}

		public override void DirectoryDropped(string path) {
			this.machine.State.DirectoryDropped(path);
		}

		public override void FileDropped(string fileFilePath) {
			this.machine.State.FileDropped(fileFilePath);
		}

		public override void LowMemory() {
			this.machine.State.LowMemory();
		}

		public override void Update(float dt) {
			this.machine.State.Update(dt);
		}

		public override void Draw() {
			this.machine.State.Draw();
		}
	}
}