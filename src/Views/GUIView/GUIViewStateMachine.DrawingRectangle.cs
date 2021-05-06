using System.Numerics;
using Designer.Models.Shapes;
using Designer.Utility.Logging;
using Designer.Views.GUIView.GhostShape;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class DrawingRectangle : Drawing {
			public DrawingRectangle(ILogger logger, StateMachine<State, Trigger> machine, GUIView guiView) : base(logger, machine, guiView) { 

			}

			protected override IGhostShape CreateGhostShape(float x, float y) {
				return new RectangleGhostShape(new Vector2(x, y));
			}

			protected override ShapeIdentifier CreateShape(Vector2 position, Vector2 size) {
				return this.guiView.GetController().AddShapeRectangle(position, size, guiView.GetSelectedGroup());
			}
		}
	}
}