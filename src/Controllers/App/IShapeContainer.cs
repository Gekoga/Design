using Designer.Models.Shapes;

namespace Designer.Controllers.App {
	public interface IShapeContainer {
		void AddShape(ShapeWrapper shapeWrapper);
	}
}