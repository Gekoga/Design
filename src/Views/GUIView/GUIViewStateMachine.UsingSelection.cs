using Designer.Utility.Logging;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class UsingSelection : State {
			public UsingSelection(ILogger logger, StateMachine<State, Trigger> machine) : base(logger, machine) {
				
			}

			public override void MouseReleased(float x, float y, int button, bool isTouch) {
				machine.Fire(Trigger.CANCEL);
			}
		}
	}
}