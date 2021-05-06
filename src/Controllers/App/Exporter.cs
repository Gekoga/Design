using System.IO;
using System;
using Designer.Models.Shapes;
using System.Text;

namespace Designer.Controllers.App {
	public class Exporter : IShapeVisitor {
		private int depth = 0;

		private StreamWriter file = null;

		public Exporter(string filename, Encoding encoding) {
			file = new StreamWriter(filename, false, encoding);
		}

		public void Visit(GroupShape groupShape) {
			var childCount = groupShape.GetChildren().Count;

			this.Write($"group {childCount}");

			depth++;
			foreach (var childWrapper in groupShape.GetChildren())
				childWrapper.GetShape().Accept(this);
			depth--;
		}

		public void Visit(BaseShape baseShape) {
			var name = baseShape.GetStrategy().GetName();
			var position = baseShape.GetPosition();
			var size = baseShape.GetSize();

			this.Write($"{name} {Math.Round(position.X)} {Math.Round(position.Y)} {Math.Round(size.X)} {Math.Round(size.Y)}");
		}

		public void Visit(AnnotationShapeDecorator annotationShapeDecorator) {
			var position = annotationShapeDecorator.GetAnnotation().GetPosition().ToString().ToLower();
			var text = annotationShapeDecorator.GetAnnotation().GetText();

			this.Write($"ornament {position} \"{text}\"");
		}

		private void Write(string msg) {
			file.Write(new String('\t', this.depth));
			file.Write(msg);
			file.Write("\r\n");

			file.Flush();
		}
	}
}