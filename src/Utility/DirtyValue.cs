namespace Designer.Utility {
	public class DirtyValue<T> {
		public delegate T CleanHandler(T value);

		private T value;
		private CleanHandler cleanDelegate;
		private bool isDirty;

		public DirtyValue(T value, CleanHandler cleanDelegate, bool isDirty = true) {
			this.value = value;
			this.cleanDelegate = cleanDelegate;
			this.isDirty = isDirty;
		}

		public T GetValue() {
			if (this.isDirty) {
				this.value = this.cleanDelegate(this.value);
				this.isDirty = false;
			}

			return this.value;
		}

		public void MakeDirty() {
			this.isDirty = true;
		}
	}

	public class DirtyValue<T, Y> {
		public delegate T CleanHandler(T value, Y args);

		private T value;
		private CleanHandler cleanDelegate;
		private bool isDirty;

		public DirtyValue(T value, CleanHandler cleanDelegate, bool isDirty = true) {
			this.value = value;
			this.cleanDelegate = cleanDelegate;
			this.isDirty = isDirty;
		}

		public T GetValue(Y args) {
			if (this.isDirty) {
				this.value = this.cleanDelegate(this.value, args);
				this.isDirty = false;
			}

			return this.value;
		}

		public void MakeDirty() {
			this.isDirty = true;
		}
	}
}