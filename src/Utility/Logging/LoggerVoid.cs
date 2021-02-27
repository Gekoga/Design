using System.Runtime.CompilerServices;

namespace Designer.Utility.Logging {
	public class LoggerVoid : ILogger {
		public LogLevel GetLogLevel() {
			return LogLevel.NONE;
		}

		public void SetLogLevel(LogLevel logLevel) { }
		
		public void Log(LogLevel logLevel, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null) { }

		public void LogIf(LogLevel logLevel, bool expression, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null) { }

		public void Flush() { }
	}
}