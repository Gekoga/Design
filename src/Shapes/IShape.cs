using System.Collections.Generic;
using Designer.Utility;
using Love;

namespace Designer.Shapes {
	public interface IShape {
		void Draw();

		Vector2 GetSize();

		Vector2 GetTopLeftAnchor();
		void SetTopLeftAnchor(Vector2 topLeft);

		Vector2 GetTopRightAnchor();
		void SetTopRightAnchor(Vector2 topRight);

		Vector2 GetBottomLeftAnchor();
		void SetBottomLeftAnchor(Vector2 bottomLeft);

		Vector2 GetBottomRightAnchor();
		void SetBottomRightAnchor(Vector2 bottomRight);

		List<Vector2> GetBoundingBoxAnchors();

		bool DoesOverlapWith(Vector2 otherTopLeft, Vector2 otherBottomRight);
		bool DoesOverlapWith(BoundingBox other);

		bool IsEncupsulatedBy(Vector2 otherTopLeft, Vector2 otherBottomRight);
		bool IsEncupsulatedBy(BoundingBox other);

		bool DoesOverlapWithPoint(Vector2 point);
	}
}