using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using Wintox.Lib.Models;

namespace Wintox.Helpers
{
	public class ToolStripItemConverter : IConverter<OpenedWindow, ToolStripMenuItem>
	{
		public ToolStripMenuItem Convert(OpenedWindow @object, EventHandler callback)
		{
			using var imageStream = new MemoryStream(Resources.CheckIcon);
			var item = new ToolStripMenuItem
			{
				Text  = @object.Title,
				Image = @object.IsOnTop ? Image.FromStream(imageStream) : null
			};

			item.Click += callback;

			return item;
		}
	}
}