using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Designer.Models;
using Designer.Models.Shapes;

namespace Designer.Controllers.App {
	public class Importer {
		private class ImportState {
			public bool isRootGroup = true;

			public Stack<(GroupShape, int)> groups = null;
			public List<(string, Annotation.Position)> bufferedAnnotations = null;

			public ImportState() {
				this.isRootGroup = true;

				this.groups = new Stack<(GroupShape, int)>();
				this.bufferedAnnotations = new List<(string, Annotation.Position)>();
			}
		}

		public void Import(AppController controller, string filename, Encoding encoding) {
			var streamReader = new StreamReader(filename, encoding);

			var importState = new ImportState();

			controller.Clear();

			string line;
			while ((line = streamReader.ReadLine()) != null) {
				var values = line.Replace("\t", "").Split(' ');

				var type = values[0];

				values = values.Skip(1).ToArray();

				switch (type) {
					case "group":
						this.HandleGroup(values, importState, controller);
						break;
					case "ellipse":
						this.HandleEllipse(values, importState, controller);
						break;
					case "rectangle":
						this.HandleRectangle(values, importState, controller);
						break;
					case "ornament":
						this.HandleOrnament(values, importState);
						break;
				}
			}
		}

		private void HandleGroup(string[] values, ImportState importState, AppController controller) {
			var childCount = Convert.ToInt32(values[0]);

			importState.groups.TryPeek(out var group);

			if (importState.isRootGroup) {
				importState.isRootGroup = false;
				importState.groups.Push((null, childCount));
				return;
			}

			this.ConsumeChild(importState);
			
			var createdGroup = controller.AddShapeGroup(group.Item1);
			importState.groups.Push((createdGroup, childCount));

			this.UseBufferedAnnotations(createdGroup.GetIdentifier(), importState, controller);

		}

		private void HandleEllipse(string[] values, ImportState importState, AppController controller) {
			var (x, y, w, h) = ParseTransform(values);

			importState.groups.TryPeek(out var group);

			var identifier = controller.AddShapeEllipse(new Vector2(x, y), new Vector2(w, h), group.Item1);

			this.UseBufferedAnnotations(identifier, importState, controller);

			this.ConsumeChild(importState);
		}

		private void HandleRectangle(string[] values, ImportState importState, AppController controller) {
			var (x, y, w, h) = ParseTransform(values);

			importState.groups.TryPeek(out var group);
	
			var identifier = controller.AddShapeRectangle(new Vector2(x, y), new Vector2(w, h), group.Item1);

			this.UseBufferedAnnotations(identifier, importState, controller);

			this.ConsumeChild(importState);
		}

		private (int, int, int, int) ParseTransform(string[] values) {
			var x = Convert.ToInt32(values[0]);
			var y = Convert.ToInt32(values[1]);
			var w = Convert.ToInt32(values[2]);
			var h = Convert.ToInt32(values[3]);

			return (x, y, w, h);
		}

		private void ConsumeChild(ImportState importState) {
			var group = importState.groups.Pop();

			group.Item2--;

			if (group.Item2 > 0)
				importState.groups.Push(group);
		}

		private void HandleOrnament(string[] values, ImportState importState) {
			var positionStr = values[0];
			var position = Annotation.Position.TOP;

			switch (positionStr) {
				case "top":
					position = Annotation.Position.TOP;
					break;
				case "bottom":
					position = Annotation.Position.BOTTOM;
					break;
				case "left":
					position = Annotation.Position.LEFT;
					break;
				case "right":
					position = Annotation.Position.RIGHT;
					break;
			}

			var str = values[1].Replace("\"", "");

			importState.bufferedAnnotations.Add((str, position));
		}

		private void UseBufferedAnnotations(ShapeIdentifier identifier, ImportState importState, AppController controller) {
			foreach (var bufferedAnnotation in importState.bufferedAnnotations)
				controller.AddAnnotation(identifier, bufferedAnnotation.Item1, bufferedAnnotation.Item2);

			importState.bufferedAnnotations.Clear();
		}
	}
}