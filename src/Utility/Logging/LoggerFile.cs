using System;
using System.IO;
using System.Text;

namespace Designer.Utility.Logging {
	public class LoggerFile : LoggerBase, ILogger {
		private string fileName = String.Empty;
		private StreamWriter file = null;

		public LoggerFile(Encoding encoding, LogLevel logLevel = LogLevel.TRACE) : base(logLevel) {
			fileName = $"{DateTime.Now:yyyyMMddTHHmmss}-Log.txt";

			file = new StreamWriter(fileName, false, encoding);
		}

		public override void Flush() {
			file.Flush();
		}
		
		protected override void Write(LogLevel logLevel, string message, int lineNumber, string filePath, string caller) {
			string relFilePath = filePath.Substring(Environment.CurrentDirectory.Length);

			file.WriteLine($"{DateTime.Now} - [{logLevel}] - {message} ({caller} {relFilePath}:{lineNumber})");
		}
	}
}