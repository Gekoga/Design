namespace Designer.Utility {
	public class BaseDirtyValue<T> {
		protected T value;
		protected bool isDirty;
		
		public BaseDirtyValue(T value, bool isDirty = true) {
			this.value = value;
			this.isDirty = isDirty;
		}

		public void MakeDirty() {
			this.isDirty = true;
		}
	}

	public class DirtyValue<T> : BaseDirtyValue<T> {
		public delegate T CleanHandler(T value);

		private CleanHandler cleanDelegate;

		public DirtyValue(T value, CleanHandler cleanDelegate, bool isDirty = true) : base(value, isDirty) {
			this.cleanDelegate = cleanDelegate;
		}

		public T GetValue() {
			if (this.isDirty) {
				this.value = this.cleanDelegate(this.value);
				this.isDirty = false;
			}

			return this.value;
		}
	}

	public class DirtyValue<T, Y> : BaseDirtyValue<T> {
		public delegate T CleanHandler(T value, Y args);

		private CleanHandler cleanDelegate;

		public DirtyValue(T value, CleanHandler cleanDelegate, bool isDirty = true) : base(value, isDirty) {
			this.cleanDelegate = cleanDelegate;
		}

		public T GetValue(Y args) {
			if (this.isDirty) {
				this.value = this.cleanDelegate(this.value, args);
				this.isDirty = false;
			}

			return this.value;
		}
	}
}