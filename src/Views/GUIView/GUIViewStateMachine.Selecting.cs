using System.Numerics;
using Designer.Utility.Logging;
using Stateless;

namespace Designer.Views.GUIView {
	public partial class GUIViewStateMachine {
		private class Selecting : State {
			protected GUIView guiView = null;

			protected SelectionBox selectionBox = null;

			public Selecting(ILogger logger, StateMachine<State, Trigger> machine, GUIView guiView) : base(logger, machine) {
				this.guiView = guiView;
			}

			public override void Draw() {
				if (this.selectionBox == null)
					return;

				this.selectionBox.Draw(this.guiView.GetSettings());
			}

			public override void MousePressed(float x, float y, int button, bool isTouch) {
				this.selectionBox = new SelectionBox(new Vector2(x, y));
			}

			public override void MouseReleased(float x, float y, int button, bool isTouch) {
				if (this.selectionBox == null)
					return;
			
				var shapes = this.guiView.GetController().GetAllShapes();
				foreach (var shapeWrapper in shapes) {
					var shape = shapeWrapper.GetShape();

					var x1 = this.selectionBox.GetTopLeft().X;
					var y1 = this.selectionBox.GetTopLeft().Y;
					var w1 = this.selectionBox.GetSize().X;
					var h1 = this.selectionBox.GetSize().Y;

					var x2 = shape.GetPosition().X;
					var y2 = shape.GetPosition().Y;
					var w2 = shape.GetSize().X;
					var h2 = shape.GetSize().Y;

					if (x1 < x2 + w2 &&
							x1 + w1 > x2 &&
							y1 < y2 + h2 &&
							y1 + h1 > y2) {

						this.guiView.GetController().AddToSelection(shape.GetIdentifier());
					}
				}

				this.selectionBox = null;

				this.machine.Fire(Trigger.FINISH_SELECTING);
			}

			public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch) {
				if (this.selectionBox == null)
					return;
				
				this.selectionBox.SetBottomRight(new Vector2(x, y));
			}

			public override string ToString() {
				return "Selecting";
			}
		}
	}
}