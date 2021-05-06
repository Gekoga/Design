using Designer.Controllers.App;
using Love;
using Designer.Utility.Logging;
using Designer.Views.GUIView;

namespace Designer {
	public class Entry {
		public static Context CreateContext() {
			LoggerComposite logger = new LoggerComposite(
				LogLevel.TRACE
			);

			logger.AddLogger(new LoggerConsole());
			//logger.AddLogger(new LoggerFile(System.Text.Encoding.UTF8));
	
			Context context = new Context(
				logger
			);

			return context;
		}

		public static void Main(string[] args) {
			using (Context context = CreateContext()) {
				context.GetLogger().LogInfo($"Application start");

				try {
					AppController controller = new AppController(context);

					using (GUIView viewGUI = new GUIView(context, controller)) {
						context.GetLogger().LogInfo($"Initializing LÖVE");

						Log.IsPrintError = true;
						Log.IsPrintInfo = true;
						Log.IsPrintWarnning = true;

						Boot.Init(new BootConfig() {
							WindowTitle = "Designer",
							WindowWidth = 1920,
							WindowHeight = 1080,

							WindowResizable = true,
							WindowVsync = false,
							WindowDisplay = 0,

							WindowHighdpi = true,

							WindowMSAA = 4,
						});

						context.GetLogger().LogInfo($"Running LÖVE");

						Boot.Run(viewGUI);
					}
				}
				catch (System.Exception e) {	
					context.GetLogger().LogError($"Error occured during execution: ${e.ToString()}");
				}

				context.GetLogger().LogInfo($"Application end");
			}
		}
	}
}
