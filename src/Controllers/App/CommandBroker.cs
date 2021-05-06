using System.Collections.Generic;
using Designer.Controllers.App.Commands;

namespace Designer.Controllers.App {
	public class CommandBroker {
		private Stack<ICommand> undoStack = null;
		private Stack<ICommand> redoStack = null;

		public CommandBroker() {
			this.undoStack = new Stack<ICommand>();
			this.redoStack = new Stack<ICommand>();
		}

		public void ExecuteCommand(ICommand command) {
			command.Execute();

			this.undoStack.Push(command);
			this.redoStack.Clear();
		}

		public void Undo() {
			if (undoStack.TryPop(out var command)) {
				command.Undo();

				this.redoStack.Push(command);
			}
		}

		public void Redo() {
			if (redoStack.TryPop(out var command)) {
				command.Redo();

				this.undoStack.Push(command);
			}
		}
	}
}