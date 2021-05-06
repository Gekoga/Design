using System.Numerics;
namespace Designer.Models.Shapes {
	public interface IShape {
		Vector2 GetPosition();
		void SetPosition(Vector2 position);

		Vector2 GetSize();
		void SetSize(Vector2 size);

		void SetWrapper(ShapeWrapper wrapper);
		ShapeWrapper GetWrapper();

		void Destroy();
		void Restore();

		ShapeIdentifier GetIdentifier();

		void Accept(IShapeVisitor shapeVisitor);
	}
}