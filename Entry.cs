﻿using Love;

namespace Designer {
	class Entry {
		static void Main(string[] args) {
			Boot.Init(new BootConfig() {
				WindowTitle = "Designer",
				WindowWidth = 1920,
				WindowHeight = 1080,

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
