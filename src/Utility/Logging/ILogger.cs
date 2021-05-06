using System.Runtime.CompilerServices;

namespace Designer.Utility.Logging {
	public interface ILogger {
		LogLevel GetLogLevel();
		void SetLogLevel(LogLevel logLevel);

		void Log(LogLevel logLevel, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		void LogIf(LogLevel logLevel, bool expression, string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);

		void LogTrace(string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		void LogTraceIf(string message, bool expression, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		
		void LogDebug(string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		void LogDebugIf(string message, bool expression, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
	
		void LogInfo(string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		void LogInfoIf(string message, bool expression, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		
		void LogWarn(string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		void LogWarnIf(string message, bool expression, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		
		void LogError(string message, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);
		void LogErrorIf(string message, bool expression, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filePath = null, [CallerMemberName] string caller = null);

		void Flush();
	}
}