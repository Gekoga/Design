using Designer.Utility.Logging;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class MovingSelection : UsingSelection {
			public MovingSelection(ILogger logger, StateMachine<State, Trigger> machine) : base(logger, machine) {
			
			}

			public override string ToString() {
				return "Moving Selection";
			}
		}
	}
}