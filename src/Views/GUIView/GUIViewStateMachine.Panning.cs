using Designer.Utility.Logging;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class Panning : State {
			public Panning(ILogger logger, StateMachine<State, Trigger> machine) : base(logger, machine) {
			
			}
		}
	}
}