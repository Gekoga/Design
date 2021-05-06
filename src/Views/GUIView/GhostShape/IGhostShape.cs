using System.Numerics;

namespace Designer.Views.GUIView.GhostShape {
	public interface IGhostShape {
		Vector2 GetPosition();
		void SetPosition(Vector2 position);

		Vector2 GetSize();
		void SetSize(Vector2 size);

		void Draw(GUIViewSettings guiViewSettings);
	}
}