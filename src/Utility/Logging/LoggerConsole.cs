using System;

namespace Designer.Utility.Logging {
	public class LoggerConsole : LoggerBase, ILogger {
		public LoggerConsole(LogLevel logLevel = LogLevel.TRACE) : base(logLevel) {
			
		}
	
		protected override void Write(LogLevel logLevel, string message, int lineNumber, string filePath, string caller) {
			string relFilePath = filePath.Substring(Environment.CurrentDirectory.Length);

			ConsoleColor defaultForegroundColor = ConsoleColor.White;
			ConsoleColor defaultBackgroundColor = ConsoleColor.Black;

			Console.ForegroundColor = defaultForegroundColor;
			Console.BackgroundColor = defaultBackgroundColor;

			Console.Write($"{DateTime.Now} - [");

			Console.ForegroundColor = GetLogLevelConsoleColor(logLevel);
			Console.Write($"{logLevel}");

			Console.ForegroundColor = defaultForegroundColor;
			Console.Write($"] - ");
			
			if (logLevel == LogLevel.ERROR)
				Console.ForegroundColor = ConsoleColor.DarkRed;
			else
				Console.ForegroundColor = ConsoleColor.Yellow;

			Console.Write($"{message} ");

			Console.ForegroundColor = defaultForegroundColor;
			Console.Write($"- {caller} ({relFilePath}:{lineNumber})");

			Console.Write("\n");

			Console.ResetColor();
		}

		private ConsoleColor GetLogLevelConsoleColor(LogLevel logLevel) {
			switch (logLevel) {
				case LogLevel.NONE:
					return ConsoleColor.White;
				case LogLevel.TRACE:
					return ConsoleColor.Green;
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