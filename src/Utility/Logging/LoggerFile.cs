using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Designer.Utility.Logging {
	public class LoggerFile : ILogger {
		private LogLevel logLevel = LogLevel.NONE;

		private string fileName = String.Empty;
		private StreamWriter file = null;

		public LoggerFile(Encoding encoding, LogLevel logLevel = LogLevel.DEBUG) {
			this.logLevel = logLevel;

			fileName = $"{DateTime.Now:yyyyMMddTHHmmss}-Log.txt";

			file = new StreamWriter(fileName, false, encoding);
		}

		public LogLevel GetLogLevel() {
			return this.logLevel;
		}

		public void SetLogLevel(LogLevel logLevel) {
			this.logLevel = logLevel;
		}
		
		public void Log(LogLevel logLevel, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null) {
			if (logLevel == LogLevel.NONE)
				return;

			if (logLevel < this.logLevel)
				return;

			WriteToFile(logLevel, message, lineNumber, filePath, caller);
		}

		public void LogIf(LogLevel logLevel, bool expression, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null) {
			if (!expression)
				return;

			Log(logLevel, message, lineNumber, filePath, caller);
		}

		public void Flush() {
			file.Flush();
		}
		
		private void WriteToFile(LogLevel logLevel, string message, int lineNumber, string filePath, string caller) {
			string relFilePath = filePath.Substring(Environment.CurrentDirectory.Length);

			file.WriteLine($"{DateTime.Now} - [{logLevel}] - {message} ({caller} {relFilePath}:{lineNumber})");
		}
	}
}