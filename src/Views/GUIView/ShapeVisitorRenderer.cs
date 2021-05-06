using System;
using System.Numerics;
using Designer.Models;
using Designer.Models.Shapes;
using Designer.Views.GUIView;

public class ShapeVisitorRenderer : IShapeVisitor {
	private Selection selection = null;
	private GUIViewSettings settings = null;

	public ShapeVisitorRenderer(Selection selection, GUIViewSettings settings) {
		this.selection = selection;
		this.settings = settings;
	}

	public void Visit(BaseShape baseShape) {
		baseShape.GetStrategy().Draw(baseShape, selection, this.settings.GetShapeColor(), this.settings.GetSelectedShapeColor());
	}

	public void Visit(GroupShape shapeGroup) {
		foreach (var shapeWrapper in shapeGroup.GetChildren())
			shapeWrapper.GetShape().Accept(this);
	}

	public void Visit(AnnotationShapeDecorator annotationShapeDecorator) {
		var shape = annotationShapeDecorator.GetShape();
		shape.Accept(this);

		var shapePosition = shape.GetPosition();
		var shapeSize = shape.GetSize();

		var annotation = annotationShapeDecorator.GetAnnotation();

		var annotationText = annotation.GetText();
		var position = Vector2.Zero;

		var font = Love.Graphics.GetFont();
		var fontHeight = font.GetHeight();
		var textWidth = font.GetWidth(annotationText);

		var offset = 4;

		switch (annotation.GetPosition()) {
			case Annotation.Position.TOP: 
				position = new Vector2(shapeSize.X / 2 - textWidth / 2, -fontHeight - offset);
				break;
			case Annotation.Position.BOTTOM: 
				position = new Vector2(shapeSize.X / 2 - textWidth / 2, shapeSize.Y + offset);
				break;
			case Annotation.Position.LEFT: 
				position = new Vector2(-textWidth - offset, shapeSize.Y / 2 - fontHeight / 2);
				break;
			case Annotation.Position.RIGHT: 
				position = new Vector2(shapeSize.X + offset, shapeSize.Y / 2 - fontHeight / 2);
				break;
		}

		Love.Graphics.Print(annotationText, MathF.Floor(shapePosition.X + position.X), MathF.Floor(shapePosition.Y + position.Y));
	}
}