using System.Numerics;
using Designer.Models.Shapes;

namespace Designer.Controllers.App.Commands {
	public class AddShapeGroupCommand : ICommand {
		private ShapeMapper shapeMapper = null;
		private GroupShape shapeGroup = null;

		private ShapeWrapper createdShape = null;

		public AddShapeGroupCommand(ShapeMapper shapeMapper, GroupShape shapeGroup) {
			this.shapeMapper = shapeMapper;
			this.shapeGroup = shapeGroup;
		}

		public void Execute() {
			this.createdShape = new ShapeWrapper(
				new GroupShape()
			);

			this.shapeMapper.AddShape(this.createdShape);
			this.shapeGroup.AddShape(this.createdShape);
		}

		public void Undo() {
			if (this.createdShape == null)
				return;

			this.shapeMapper.RemoveShape(this.createdShape);
			this.shapeGroup.RemoveShape(this.createdShape);
		}

		public void Redo() {
			this.shapeMapper.AddShape(this.createdShape);
			this.shapeGroup.AddShape(this.createdShape);
		}

		public ShapeWrapper GetCreatedShape() {
			return this.createdShape;
		}
	}
}