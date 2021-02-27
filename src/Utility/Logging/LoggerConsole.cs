using System;
using System.Runtime.CompilerServices;

namespace Designer.Utility.Logging {
	public class LoggerConsole : ILogger {
		private LogLevel logLevel;

		public LoggerConsole(LogLevel logLevel = LogLevel.DEBUG) {
			this.logLevel = logLevel;
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

			PrintToConsole(logLevel, message, lineNumber, filePath, caller);
		}

		public void LogIf(LogLevel logLevel, bool expression, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null) {
			if (!expression)
				return;

			Log(logLevel, message, lineNumber, filePath, caller);
		}

		public void Flush() { }
		
		private void PrintToConsole(LogLevel logLevel, string message, int lineNumber, string filePath, string caller) {
			string relFilePath = filePath.Substring(Environment.CurrentDirectory.Length);

			ConsoleColor defaultForegroundColor = ConsoleColor.White;
			ConsoleColor defaultBackgroundColor = ConsoleColor.Black;

			Console.ForegroundColor = defaultForegroundColor;
			Console.BackgroundColor = defaultBackgroundColor;

			Console.Write($"{DateTime.Now} - [");

			Console.ForegroundColor = GetLogLevelConsoleColor(logLevel);
			Console.Write($"{logLevel}");

			Console.ForegroundColor = defaultForegroundColor;
			Console.Write($"] - {caller} ({relFilePath}:{lineNumber}) - {message}");

			Console.Write("\n");

			Console.ResetColor();
		}

		private ConsoleColor GetLogLevelConsoleColor(LogLevel logLevel) {
			switch (logLevel) {
				case LogLevel.NONE:
					return ConsoleColor.White;
				case LogLevel.DEBUG:
					return ConsoleColor.Gray;
				case LogLevel.INFO:
					return ConsoleColor.Cyan;
				case LogLevel.WARN:
					return ConsoleColor.DarkYellow;
				case LogLevel.ERROR:
					return ConsoleColor.DarkRed;
				default:
					return ConsoleColor.White;
			}
		}
	}
}