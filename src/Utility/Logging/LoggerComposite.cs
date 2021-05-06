using System.Collections.Generic;

namespace Designer.Utility.Logging {
	public class LoggerComposite : LoggerBase, ILogger {
		private LogLevel logLevel;

		private HashSet<ILogger> loggers = null;

		public LoggerComposite(LogLevel logLevel = LogLevel.TRACE) : base(logLevel) {
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

		protected override void Write(LogLevel logLevel, string message, int lineNumber = 0, string filePath = null, string caller = null) {
			foreach (ILogger logger in loggers)
				logger.Log(logLevel, message, lineNumber, filePath, caller);
		}

		public override void Flush() { 
			foreach (ILogger logger in loggers)
				logger.Flush();
		}
	}
}