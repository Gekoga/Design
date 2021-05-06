using Designer.Models;
using Designer.Models.Shapes;

namespace Designer.Controllers.App.Commands {
	public class AddAnnotationCommand : ICommand {
		private IShape shape = null;
		private string text = string.Empty;
		private Annotation.Position position = default;

		public AddAnnotationCommand(IShape shape, string text, Annotation.Position position) {
			this.shape = shape;
			this.text = text;
			this.position = position;
		}

		public void Execute() {
			var annotation = new Annotation(this.text, this.position);

			var wrapper = this.shape.GetWrapper();
			wrapper.SetShape(new AnnotationShapeDecorator(wrapper.GetShape(), annotation));
		}

		public void Undo() {
			var wrapper = this.shape.GetWrapper();
			wrapper.SetShape(this.shape);
		}

		public void Redo() {
			this.Execute();
		}
	}
}