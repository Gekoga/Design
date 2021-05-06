using System.Numerics;
using Designer.Models.Shapes;

namespace Designer.Controllers.App.Commands {
	public class AddShapeEllipseCommand : ICommand {
		private ShapeMapper shapeMapper = null;
		private GroupShape shapeGroup = null;

		private Vector2 position = Vector2.Zero;
		private Vector2 size = Vector2.Zero;

		private ShapeWrapper createdShape = null;

		public AddShapeEllipseCommand(ShapeMapper shapeMapper, GroupShape shapeGroup, Vector2 position, Vector2 size) {
			this.shapeMapper = shapeMapper;
			this.shapeGroup = shapeGroup;

			this.position = position;
			this.size = size;
		}

		public void Execute() {
			this.createdShape = new ShapeWrapper(
				new BaseShape(position, size, ShapeStrategyProvider.Instance.GetEllipseStrategy())
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