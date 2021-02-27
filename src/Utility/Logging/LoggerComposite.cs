using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Designer.Utility.Logging {
	public class LoggerComposite : ILogger {
		private LogLevel logLevel;

		private HashSet<ILogger> loggers = null;

		public LoggerComposite(LogLevel logLevel) {
			this.logLevel = logLevel;

			this.loggers = new HashSet<ILogger>();
		}

		public bool AddLogger(ILogger logger) {
			bool success = this.loggers.Add(logger);

			if (!success)
				return false;
	
			logger.SetLogLevel(this.logLevel);
			return true;
		}

		public bool RemoveLogger(ILogger logger) {
			return this.loggers.Remove(logger);
		}

		public LogLevel GetLogLevel() {
			return this.logLevel;
		}

		public void SetLogLevel(LogLevel logLevel) {
			this.logLevel = logLevel;

			foreach (ILogger logger in loggers)
				logger.SetLogLevel(logLevel);
		}
		
		public void Log(LogLevel logLevel, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null) {
			if (logLevel == LogLevel.NONE)
				return;

			if (logLevel > this.logLevel)
				return;

			foreach (ILogger logger in loggers)
				logger.Log(logLevel, message, lineNumber, filePath, caller);
		}

		public void LogIf(LogLevel logLevel, bool expression, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null) {
			if (logLevel == LogLevel.NONE)
				return;

			if (logLevel > this.logLevel)
				return;

			if (!expression)
				return;

			foreach (ILogger logger in loggers)
				logger.Log(logLevel, message, lineNumber, filePath, caller);
		}

		public void Flush() { 
			foreach (ILogger logger in loggers)
				logger.Flush();
		}
	}
}