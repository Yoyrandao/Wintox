using System.Drawing;
using System.IO;

namespace Wintox.Helpers
{
	public static class ResourceHelper
	{
		public static Image ToImage(this byte[] data)
		{
			using var stream = new MemoryStream(data);

			return Image.FromStream(stream);
		}

		public static Icon ToIcon(this byte[] data)
		{
			using var stream = new MemoryStream(data);
			return new Icon(stream);
		}
	}
}