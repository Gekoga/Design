using System.Numerics;
using Designer.Models.Shapes;
using Designer.Utility.Logging;
using Designer.Views.GUIView.GhostShape;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private abstract class Drawing : State {
			protected GUIView guiView = null;

			protected IGhostShape ghostShape = null;

			public Drawing(ILogger logger, StateMachine<State, Trigger> machine, GUIView guiView) : base(logger, machine) {
				this.guiView = guiView;
			}

			public override void Draw() {
				if (this.ghostShape == null)
					return;

				this.ghostShape.Draw(this.guiView.GetSettings());
			}

			public override void MousePressed(float x, float y, int button, bool isTouch) {
				this.ghostShape = this.CreateGhostShape(x, y);
			}

			public override void MouseReleased(float x, float y, int button, bool isTouch) {
				if (this.ghostShape == null)
					return;

				var shapeIdentifier = this.CreateShape(this.ghostShape.GetPosition(), this.ghostShape.GetSize());

				this.ghostShape = null;

				this.guiView.GetController().AddToSelection(shapeIdentifier);
				this.machine.Fire(Trigger.FINISH_DRAWING);
			}

			public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
				if (this.ghostShape == null)
					return;

				this.ghostShape.SetSize(new Vector2(x, y) - this.ghostShape.GetPosition());
			}

			public override void OnExit() {
				this.ghostShape = null;
			}

			protected abstract IGhostShape CreateGhostShape(float x, float y);
			protected abstract ShapeIdentifier CreateShape(Vector2 position, Vector2 size);
		}
	}
}