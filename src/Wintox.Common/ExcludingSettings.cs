using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace Wintox.Common
{
	public class ExcludingSettings
	{
		public ExcludingSettings(IConfiguration configuration) => _configuration = configuration;
		public ExcludingSettings() { }

		public List<string> Excluded => _configuration?
		                                .AsEnumerable()
		                                .Select(x => x.Value)
		                                .Where(x => x != null)
		                                .ToList() ?? new List<string>();

		private readonly IConfiguration _configuration;
	}
}