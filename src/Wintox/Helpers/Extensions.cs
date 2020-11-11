#nullable enable
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Wintox.Lib.Models;

namespace Wintox.Helpers
{
	public static class Extensions
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

		public static bool CheckEquality(this ToolStripMenuItem? item, string value)
		{
			if (item == null)
			{
				return false;
			}
			
			return item.Name.Equals(value, StringComparison.OrdinalIgnoreCase);
		}

		public static void ChangeStatusIcon(this ToolStripMenuItem? item, bool @checked)
		{
			if (item == null)
			{
				return;
			}
			
			item.Image = @checked ? Image.FromFile("Resources/check.ico") : null; 
		}

		public static void ChangeTopMode(this OpenedWindow window)
		{
			window.IsOnTop = !window.IsOnTop;
		}
	}
}