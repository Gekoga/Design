using System.Collections.Generic;
using Love;

namespace Designer {
	public class Canvas {
		private HashSet<Button> elements = null;

		private Button clickedElement = null;
		private Button hoveredElement = null;

		public Canvas() {
			this.elements = new HashSet<Button>();
		}

		public void AddElement(Button element) {
			this.elements.Add(element);
		}

		public void RemoveElement(Button element) {
			this.elements.Remove(element);
		}
		
		public void Draw() {
			foreach (Button element in elements)
				element.Draw();
		}

		public void OnMousePressed(MouseButton mouseButton, float x, float y) {
			if (hoveredElement == null)
				return;

			clickedElement = hoveredElement;
			hoveredElement.OnClick(mouseButton);
		}

		public void OnMouseReleased(MouseButton mouseButton, float x, float y) {
			if (clickedElement == null)
				return;

			clickedElement.OnRelease(mouseButton);
		}

		public void OnMouseMoved(float x, float y) {
			Button newHoveredElement = null;

			foreach (Button element in elements) {
				if (element.CheckOverlap(x, y)) {
					newHoveredElement = element;
					break;
				}
			}

			if (hoveredElement != newHoveredElement) {
				if (hoveredElement != null)
					hoveredElement.OnExit();
				
				hoveredElement = newHoveredElement;

				if (hoveredElement != null)
					hoveredElement.OnHover();
			}
		}
	}
}