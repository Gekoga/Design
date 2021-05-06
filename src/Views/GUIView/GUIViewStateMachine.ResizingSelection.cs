using Designer.Utility.Logging;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class ResizingSelection : UsingSelection {
			public ResizingSelection(ILogger logger, StateMachine<State, Trigger> machine) : base(logger, machine) {
			
			}
		}
	}
}