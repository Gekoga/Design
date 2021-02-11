using System.Collections.Generic;
using System.Linq;
using Designer.Utility;
using Love;

namespace Designer.Shapes {
	public class ShapeGroup : IShape {
		private HashSet<IShape> shapes = null;

		private DirtyValue<BoundingBox> boundingBox = null;

		public ShapeGroup() {
			this.shapes = new HashSet<IShape>();

			this.boundingBox = new DirtyValue<BoundingBox>(new BoundingBox(Vector2.Zero, Vector2.Zero), CleanBoundingBox);
		}

		public bool AddShape(IShape shape) {
			return shapes.Add(shape);
		}

		public bool RemoveShape(IShape shape) {
			return shapes.Remove(shape);
		}

		public void ClearShapes() {
			shapes.Clear();
		}

		public int GetShapeCount() {
			return shapes.Count;
		}

		public List<IShape> GetShapes() {
			return shapes.ToList();
		}

		public virtual void Draw() {
			foreach (IShape shape in shapes)
				shape.Draw();
		}

		public Vector2 GetSize() {
			return this.boundingBox.GetValue().GetSize();
		}

		public Vector2 GetTopLeftAnchor() {
			return this.boundingBox.GetValue().GetTopLeftAnchor();
		}

		public void SetTopLeftAnchor(Vector2 topLeft) {
			// TODO: Resize 

			//this.GetBoundingBox().SetTopLeftAnchor(topLeft);
		}

		public Vector2 GetTopRightAnchor() {
			return this.boundingBox.GetValue().GetTopRightAnchor();
		}

		public void SetTopRightAnchor(Vector2 topRight) {
			// this.GetBoundingBox().SetTopRightAnchor(topRight);
		}

		public Vector2 GetBottomLeftAnchor() {
			return this.boundingBox.GetValue().GetBottomLeftAnchor();
		}

		public void SetBottomLeftAnchor(Vector2 bottomLeft) {
			//this.GetBoundingBox().SetBottomLeftAnchor(bottomLeft);
		}

		public Vector2 GetBottomRightAnchor() {
			return this.boundingBox.GetValue().GetBottomRightAnchor();
		}

		public void SetBottomRightAnchor(Vector2 bottomRight) {
			//this.GetBoundingBox().SetBottomRightAnchor(bottomRight);
		}

		public List<Vector2> GetBoundingBoxAnchors() {
			return this.boundingBox.GetValue().GetAnchors();
		}

		public bool DoesOverlapWith(Vector2 otherTopLeft, Vector2 otherBottomRight) {
			return this.boundingBox.GetValue().DoesOverlapWith(otherTopLeft, otherBottomRight);
		}

		public bool DoesOverlapWith(BoundingBox other) {
			return this.boundingBox.GetValue().DoesOverlapWith(other);
		}

		public bool IsEncupsulatedBy(Vector2 otherTopLeft, Vector2 otherBottomRight) {
			return this.boundingBox.GetValue().IsEncupsulatedBy(otherTopLeft, otherBottomRight);
		}

		public bool IsEncupsulatedBy(BoundingBox other) {
			return this.boundingBox.GetValue().IsEncupsulatedBy(other);
		}

		public bool DoesOverlapWithPoint(Vector2 point) {
			return this.boundingBox.GetValue().DoesOverlapWithPoint(point);
		}

		private BoundingBox CleanBoundingBox(BoundingBox boundingBox) {
			Vector2 topLeft = new Vector2(-1.0f, -1.0f) * float.PositiveInfinity;
			Vector2 bottomRight = new Vector2(1.0f, 1.0f) * float.PositiveInfinity;

			if (shapes != null && shapes.Count > 0) {
				topLeft.X = shapes.Min(shape => shape.GetTopLeftAnchor().X);
				topLeft.Y = shapes.Min(shape => shape.GetTopLeftAnchor().Y);

				bottomRight.X = shapes.Max(shape => shape.GetBottomRightAnchor().X);
				bottomRight.Y = shapes.Max(shape => shape.GetBottomRightAnchor().Y);
			}

			boundingBox.SetTopLeftAnchor(topLeft);
			boundingBox.SetBottomRightAnchor(bottomRight);

			return boundingBox;
		}
	}
}