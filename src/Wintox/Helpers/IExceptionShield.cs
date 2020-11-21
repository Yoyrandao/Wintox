using System;

using Serilog;

namespace Wintox.Helpers
{
	public interface IExceptionShield
	{
		T Protect<T>(Func<T> func);

		void Protect(Action func);

		void SetLogger(ILogger logger)
		{
			Logger = logger;
		}
		
		ILogger Logger { set; }
	}
}