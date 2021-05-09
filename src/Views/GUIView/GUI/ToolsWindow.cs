using System.Text;
using System.Collections.Generic;
using System.Numerics;
using Designer.Controllers.App;
using Designer.Models;
using Designer.Models.Shapes;
using ImGuiNET;

namespace Designer.Views.GUIView.GUI {
	public class ToolsWindow {
		private GUIView view = null;
		private AppController controller = null;

		private string input = string.Empty;
		private int v = 0;

		private List<IShape> shapesToDestroy = null;

		public ToolsWindow(GUIView view, AppController controller) {
			this.view = view;
			this.controller = controller;

			this.shapesToDestroy = new List<IShape>();
		}

		public void Run() {
			shapesToDestroy.Clear();

			if (ImGui.Begin("Tools")) {
				if (ImGui.Button("Add group")) {
					this.controller.AddShapeGroup(view.GetSelectedGroup());
				}

				ImGui.SameLine();

				if (ImGui.Button("Select root group")) {
					view.SetSelectedGroup(this.controller.GetRootGroup());
				}

				if (ImGui.Button("Import")) {
					var importer = new Importer();
					importer.Import(this.controller, "export.titty", Encoding.UTF8);
				}

				ImGui.SameLine();

				if (ImGui.Button("Export")) {
					var rootGroup = controller.GetRootGroup();

					var exporter = new Exporter("export.titty", Encoding.UTF8);
					rootGroup.Accept(exporter);
				}

				ImGui.Spacing();
				ImGui.Spacing();

				ImGui.Text("Selection");
				var selection = controller.GetSelection();
				ShapeNode(selection.GetAsGroup(), true);

				ImGui.Spacing();
				ImGui.Spacing();

				ImGui.Text("Shapes");
				var shapeWrappers = controller.GetRootShapes();
				this.ShapeHierarchy(shapeWrappers);
			}

			foreach (var shapeToDestroy in shapesToDestroy) {
				controller.DeleteShape(shapeToDestroy.GetIdentifier());
			}
		}

		private void ShapeHierarchy(IReadOnlyList<ShapeWrapper> shapeWrappers) {
			foreach (var shapeWrapper in shapeWrappers) {
				var shape = shapeWrapper.GetShape();

				this.ShapeNode(shape);
			}
		}

		private void ShapeNode(IShape shape, bool hideExtras = false) {
			var name = string.Empty;

			while (shape is AnnotationShapeDecorator) {
				shape = ((AnnotationShapeDecorator)shape).GetShape();
			}

			if (shape is BaseShape) {
				name = ((BaseShape)shape).GetStrategy().GetName();
			} else if (shape is GroupShape) {
				name = "group";
			}

			var displayName = name + ": " + shape.GetIdentifier().ToString();
			if (ImGui.TreeNode(displayName)) {
				if (shape is GroupShape) {
					if (!hideExtras) {
						if (ImGui.RadioButton("Selected group", shape == view.GetSelectedGroup())) {
							view.SetSelectedGroup((GroupShape)shape);
						}

						ImGui.Spacing();
						ImGui.Spacing();
						ImGui.Spacing();
					}
				}

				var position = new Love.Vector2(
					shape.GetPosition().X,
					shape.GetPosition().Y
				);
				ImGui.DragFloat2("Position", ref position);
				shape.SetPosition(new Vector2(position.X, position.Y));

				var size = new Love.Vector2(
					shape.GetSize().X,
					shape.GetSize().Y
				);
				ImGui.DragFloat2("Size", ref size);
				shape.SetSize(new Vector2(size.X, size.Y));

				if (!hideExtras) {
					ImGui.Spacing();
					ImGui.Spacing();
					ImGui.Spacing();

					ImGui.RadioButton("Top", ref v, 0); ImGui.SameLine();
					ImGui.RadioButton("Bottom", ref v, 1); ImGui.SameLine();
					ImGui.RadioButton("Left", ref v, 2); ImGui.SameLine();
					ImGui.RadioButton("Right", ref v, 3);

					ImGui.InputText("Annotation Text", ref input, 50);

					if (ImGui.Button("Create Annotation")) {
						this.controller.AddAnnotation(shape.GetIdentifier(), input, (Annotation.Position)v);
					}

					ImGui.Spacing();
					ImGui.Spacing();
					ImGui.Spacing();

					if (ImGui.Button("Delete")) {
						shapesToDestroy.Add(shape);
					}

					ImGui.Spacing();
					ImGui.Spacing();
					ImGui.Spacing();

					if (shape is GroupShape) {
						var shapeGroup = (GroupShape)shape;

						if (shapeGroup.GetChildren().Count != 0)
							ImGui.Text("Children:");

						var childWrappers = shapeGroup.GetChildren();
						foreach (var childWrapper in childWrappers) {
							var child = childWrapper.GetShape();
							this.ShapeNode(child);
						}
					}
				}

				ImGui.TreePop();
			}
		}
	}
}