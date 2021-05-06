using System.Collections.Generic;
using Designer.Models.Shapes;

namespace Designer.Controllers.App.Commands {
	public class DeleteShapeCommand : ICommand {
		private IShape shape = null;

		public DeleteShapeCommand(IShape shape) {
			this.shape = shape;
		}

		public void Execute() {
			this.shape.Destroy();
		}

		public void Undo() {
			this.shape.Restore();
		}

		public void Redo() {
			this.Execute();
		}
	}
}