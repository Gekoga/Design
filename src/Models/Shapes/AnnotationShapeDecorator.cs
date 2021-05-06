using System.Numerics;

namespace Designer.Models.Shapes {
	public class AnnotationShapeDecorator : IShape {
		private IShape shape = null;
		private Annotation annotation = null;

		public AnnotationShapeDecorator(IShape shape, Annotation annotation) {
			this.shape = shape;
			this.annotation = annotation;
		}

		public void Accept(IShapeVisitor shapeVisitor) {
			shapeVisitor.Visit(this);

			this.shape.Accept(shapeVisitor);
		}

		public ShapeIdentifier GetIdentifier() {
			return this.shape.GetIdentifier();
		}

		public Vector2 GetPosition() {
			return this.shape.GetPosition();
		}

		public Vector2 GetSize() {
			return this.shape.GetSize();
		}

		public void SetPosition(Vector2 position) {
			this.shape.SetPosition(position);
		}

		public void SetSize(Vector2 size) {
			this.shape.SetSize(size);
		}

		public IShape GetShape() {
			return this.shape;
		}

		public Annotation GetAnnotation() {
			return this.annotation;
		}

		public void SetWrapper(ShapeWrapper wrapper) {
			this.shape.SetWrapper(wrapper);
		}

		public ShapeWrapper GetWrapper() {
			return this.shape.GetWrapper();
		}

		public void Destroy() { this.GetWrapper().Destroy(); }
		public void Restore() { this.GetWrapper().Restore(); }
	}
}