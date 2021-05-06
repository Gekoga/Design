using Designer.Utility.Logging;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class State : Love.Scene {
			protected ILogger logger = null;

			protected StateMachine<State, Trigger> machine = null;

			public State(ILogger logger, StateMachine<State, Trigger> machine) {
				this.logger = logger;

				this.machine = machine;

				this.machine.Configure(this)
					.OnEntry(this.OnEntry)
					.OnExit(this.OnExit);
			}

			public virtual void OnEntry() { }
			public virtual void OnExit() { }
		}
	}
}