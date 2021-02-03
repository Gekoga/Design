using Love;

namespace Designer {
	public static class Vector2Extensions {
		public static Vector2 Abs(this Vector2 vec) {
			Vector2 temp = new Vector2(vec);

			temp.AbsInPlace();

			return temp;
		}	

		public static void AbsInPlace(this ref Vector2 vec) {
			vec.X = Mathf.Abs(vec.X);
			vec.Y = Mathf.Abs(vec.Y);
		}
	}
}