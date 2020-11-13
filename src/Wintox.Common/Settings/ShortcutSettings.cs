using Microsoft.Extensions.Configuration;

namespace Wintox.Common.Settings
{
	public class ShortcutSettings
	{
		public ShortcutSettings(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		private readonly IConfiguration _configuration;
	}
}