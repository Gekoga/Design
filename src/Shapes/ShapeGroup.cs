using System;
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
			bool success = shapes.Add(shape);

			if (success)
				this.boundingBox.MakeDirty();

			return success;
		}

		public bool RemoveShape(IShape shape) {
			bool success = shapes.Remove(shape);

			if (success)
				this.boundingBox.MakeDirty();

			return success;
		}

		public void ClearShapes() {
			shapes.Clear();

			this.boundingBox.MakeDirty();
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
			Vector2 oldTopLeft = this.GetTopLeftAnchor();

			Vector2 size = this.GetSize();
			Vector2 newSize = this.GetBottomRightAnchor() - topLeft;

			foreach (IShape shape in this.GetShapes()) {
				Vector2 shapeOldTopLeft = shape.GetTopLeftAnchor() - oldTopLeft;
				Vector2 shapeOldBottomRight = shape.GetBottomRightAnchor() - oldTopLeft;
				
				Vector2 relTL = shapeOldTopLeft / size;
				Vector2 relBR = shapeOldBottomRight / size;

				shape.SetTopLeftAnchor(topLeft + newSize * relTL);
				shape.SetBottomRightAnchor(topLeft + newSize * relBR);
			}

			this.boundingBox.MakeDirty();
		}

		public Vector2 GetTopRightAnchor() {
			return this.boundingBox.GetValue().GetTopRightAnchor();
		}

		public void SetTopRightAnchor(Vector2 topRight) {
			this.boundingBox.MakeDirty();
			// this.GetBoundingBox().SetTopRightAnchor(topRight);
		}

		public Vector2 GetBottomLeftAnchor() {
			return this.boundingBox.GetValue().GetBottomLeftAnchor();
		}

		public void SetBottomLeftAnchor(Vector2 bottomLeft) {
			this.boundingBox.MakeDirty();
			//this.GetBoundingBox().SetBottomLeftAnchor(bottomLeft);
		}

		public Vector2 GetBottomRightAnchor() {
			return this.boundingBox.GetValue().GetBottomRightAnchor();
		}

		public void SetBottomRightAnchor(Vector2 bottomRight) {
			Vector2 oldBottomRight = this.GetBottomRightAnchor();
			
			Vector2 oldSize = this.GetSize();
			Vector2 newSize = bottomRight - this.GetTopLeftAnchor();

			if (newSize.X <= 2.0f) {
				newSize.X = 2.0f;
				bottomRight.X = this.GetTopLeftAnchor().X + 2.0f;
			}

			if (newSize.Y <= 2.0f) {
				newSize.Y = 2.0f;
				bottomRight.Y = this.GetTopLeftAnchor().Y + 2.0f;
			}

			foreach (IShape shape in this.GetShapes()) {
				Vector2 shapeOldTopLeft = shape.GetTopLeftAnchor() - oldBottomRight;
				Vector2 shapeOldBottomRight = shape.GetBottomRightAnchor() - oldBottomRight;

				Vector2 relativeOldTopLeft = shapeOldTopLeft / oldSize;
				Vector2 relativeOldBottomRight = shapeOldBottomRight / oldSize;

				shape.SetTopLeftAnchor(bottomRight + newSize * relativeOldTopLeft);
				shape.SetBottomRightAnchor(bottomRight + newSize * relativeOldBottomRight);
			}

			this.boundingBox.MakeDirty();
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