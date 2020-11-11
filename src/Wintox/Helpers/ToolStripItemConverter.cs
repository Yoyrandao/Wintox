﻿using System;
using System.Windows.Forms;

using Wintox.Lib.Models;

namespace Wintox.Helpers
{
	public class ToolStripItemConverter : IConverter<OpenedWindow, ToolStripMenuItem>
	{
		public ToolStripMenuItem Convert(OpenedWindow @object, EventHandler callback)
		{
			var item = new ToolStripMenuItem
			{
				Text = @object.Title,
				Name = @object.Uid
			};

			item.Click += callback;

			return item;
		}
	}
}