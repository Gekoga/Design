using System.Collections;

namespace Designer.Utility {
	public class DirtyCollection<T> where T : ICollection {
		private T collection;
		private bool isDirty;

		public DirtyCollection(T collection, bool isDirty = true) {
			this.collection = collection;
			this.isDirty = isDirty;
		}

		public T GetCollection() {
			return this.collection;
		}

		public bool IsDirty() {
			return isDirty;
		}

		public void SetDirty(bool isDirty) {
			this.isDirty = isDirty;
		}
	}
}