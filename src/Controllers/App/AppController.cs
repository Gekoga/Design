using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Numerics;
using Designer.Controllers.App.Commands;
using Designer.Models;
using Designer.Models.Shapes;

namespace Designer.Controllers.App {
	public class AppController {
		public delegate void StateChangedHandler();
		public event StateChangedHandler StateChanged;

		private Context context = null;

		private CommandBroker commandBroker = null;

		private ShapeMapper shapeMapper = null;
		private GroupShape shapeGroup = null;

		private Selection selection = null;

		public AppController(Context context) {
			this.context = context;

			this.commandBroker = new CommandBroker();

			this.shapeMapper = new ShapeMapper();
			this.shapeGroup = new GroupShape();

			this.selection = new Selection();
		}
		
		public ShapeIdentifier AddShapeRectangle(Vector2 position, Vector2 size, GroupShape root) {
			root = (root != null) ? root : this.shapeGroup; 

			var command = new AddShapeRectangleCommand(this.shapeMapper, root, position, size);
			commandBroker.ExecuteCommand(command);

			var createdShape = command.GetCreatedShape();

			StateChanged?.Invoke();

			return createdShape.GetShape().GetIdentifier();
		}

		public ShapeIdentifier AddShapeEllipse(Vector2 position, Vector2 size, GroupShape root) {
			root = (root != null) ? root : this.shapeGroup; 

			var command = new AddShapeEllipseCommand(this.shapeMapper, root, position, size);
			commandBroker.ExecuteCommand(command);

			var createdShape = command.GetCreatedShape();

			StateChanged?.Invoke();

			return createdShape.GetShape().GetIdentifier();
		}

		public GroupShape AddShapeGroup(GroupShape root) {
			root = (root != null) ? root : this.shapeGroup;  

			var command = new AddShapeGroupCommand(this.shapeMapper, root);
			commandBroker.ExecuteCommand(command);

			var createdShape = command.GetCreatedShape();

			StateChanged?.Invoke();

			return (GroupShape)createdShape.GetShape();
		}

		public void AddAnnotation(ShapeIdentifier identifier, string text, Annotation.Position position) {
			var shape = shapeMapper.GetShape(identifier).GetShape();
			
			var command = new AddAnnotationCommand(shape, text, position);
			commandBroker.ExecuteCommand(command);
		}

		public void ResizeShape(int shapeIndex, Vector2 anchor, Vector2 newSize) {
			var shape = this.shapeGroup.GetChildren()[shapeIndex];
		}

		public void StartSelection() {
			this.selection.Clear();
		}

		public bool AddToSelection(ShapeIdentifier identifier) {
			var shape = shapeMapper.GetShape(identifier);

			if (shape == null)
				return false;

			this.selection.AddShape(shape);

			return true;
		}

		public void DeleteShape(ShapeIdentifier identifier) {
			var shape = shapeMapper.GetShape(identifier).GetShape();
			
			var command = new DeleteShapeCommand(shape);
			commandBroker.ExecuteCommand(command);
		}

		public bool RemoveFromSelection(ShapeIdentifier identifier) {
			var shape = shapeMapper.GetShape(identifier);

			if (shape == null)
				return false;

			this.selection.RemoveShape(shape);

			return true;
		}

		public void Undo() {
			this.commandBroker.Undo();
		}

		public void Redo() {
			this.commandBroker.Redo();
		}

		public void Clear() {
			this.shapeMapper.Clear();
			this.GetRootGroup().Clear();
		}

		public void ClearSelection() {
			this.selection.Clear();
		}

		public IReadOnlyList<ShapeWrapper> GetShapes() {
			return this.shapeGroup.GetChildren();
		}

		public Selection GetSelection() {
			return this.selection;
		} 
		
		public GroupShape GetRootGroup() {
			return this.shapeGroup;
		}
	}
}