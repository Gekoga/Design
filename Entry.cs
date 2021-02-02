using Love;

namespace Designer {
	class Entry {
		static void Main(string[] args) {
			Boot.Init(new BootConfig() {
				WindowTitle = "Designer",
				WindowWidth = 1280,
				WindowHeight = 720,

				WindowResizable = false,
				WindowVsync = false,
				WindowDisplay = 0,

				WindowHighdpi = true,

				WindowMSAA = 4,
			});

			Boot.Run(new Program());
		}
	}
}
