
namespace Designer.Utility.Logging {
	public class LoggerVoid : LoggerBase, ILogger {
		public LoggerVoid(LogLevel logLevel = LogLevel.TRACE) : base(logLevel) {
		
		}

		protected override void Write(LogLevel logLevel, string message, int lineNumber = 0, string filePath = null, string caller = null) {
			
		}
	}
}