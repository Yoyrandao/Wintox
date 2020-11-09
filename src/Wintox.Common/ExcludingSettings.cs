using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Microsoft.Extensions.Configuration;

namespace Wintox.Common
{
	public class ExcludingSettings
	{
		public ExcludingSettings(IConfiguration configuration) => _configuration = configuration;

		public List<string> Excluded => _configuration
											.AsEnumerable()
											.Select(x => x.Value)
											.Where(x => x != null)
											.ToList();

		private readonly IConfiguration _configuration;
	}
}