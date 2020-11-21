using System;

using Serilog;

namespace Wintox.Helpers
{
	public class ExceptionShield : IExceptionShield
	{
		public T Protect<T>(Func<T> func)
		{
			try
			{
				return func();
			}
			catch (Exception e)
			{
				Logger.Error(e.Message);

				return default;
			}
		}

		public void Protect(Action func)
		{
			try
			{
				func();
			}
			catch (Exception e)
			{
				Logger.Error(e.Message);
			}
		}

		public ILogger Logger { get; set; }
	}
}