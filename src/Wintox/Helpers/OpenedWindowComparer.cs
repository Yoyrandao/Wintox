using System;
using System.Collections.Generic;

using Wintox.Lib.Models;

namespace Wintox.Helpers
{
	public class OpenedWindowComparer : IComparer<OpenedWindow>
	{
		public int Compare(OpenedWindow x, OpenedWindow y)
		{
			if (ReferenceEquals(x, y))
				return 0;

			if (ReferenceEquals(null, y))
				return 1;

			if (ReferenceEquals(null, x))
				return -1;

			return string.Compare(x.Title, y.Title, StringComparison.Ordinal);
		}
	}
}