using System;
using Designer.Utility.Logging;

namespace Designer {
	public class Context : IDisposable {
		private ILogger logger = null;

		public Context(ILogger logger) {
			this.logger = logger;
		}

		public ILogger GetLogger() {
			return this.logger;
		}

		public void Dispose() {
			this.logger.Flush();
		}
	}
}