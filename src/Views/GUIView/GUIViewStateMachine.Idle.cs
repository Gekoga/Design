using Designer.Utility.Logging;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class Idle : State {
			public Idle(ILogger logger, StateMachine<State, Trigger> machine) : base(logger, machine) { }

			public override void MousePressed(float x, float y, int button, bool isTouch) {
				this.logger.LogTrace($"Clicked in idle! {x}, {y}");
			}

			public override string ToString() {
				return "Idle";
			}
		}
	}
}