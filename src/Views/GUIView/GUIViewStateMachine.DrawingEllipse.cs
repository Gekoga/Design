using System.Numerics;
using Designer.Models.Shapes;
using Designer.Utility.Logging;
using Designer.Views.GUIView.GhostShape;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class DrawingEllipse : Drawing {
			public DrawingEllipse(ILogger logger, StateMachine<State, Trigger> machine, GUIView guiView) : base(logger, machine, guiView) { 

			}

			protected override IGhostShape CreateGhostShape(float x, float y) {
				return new EllipseGhostShape(new Vector2(x, y));
			}

			protected override ShapeIdentifier CreateShape(Vector2 position, Vector2 size) {
				return this.guiView.GetController().AddShapeEllipse(position, size, guiView.GetSelectedGroup());
			}

			public override string ToString() {
				return "Drawing Ellipse";
			}
		}
	}
}