namespace Designer.Models.Shapes {
	public interface IShapeVisitor {
		void Visit(GroupShape groupShape);
		void Visit(BaseShape baseShape);
		void Visit(AnnotationShapeDecorator annotationShapeDecorator);
	}
}