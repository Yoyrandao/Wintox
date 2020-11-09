#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Wintox.Helpers;
using Wintox.Lib.Constants;
using Wintox.Lib.LowLevelProcessing;
using Wintox.Lib.Models;

namespace Wintox
{
	public class TrayContext : ApplicationContext
	{
		public TrayContext(ILowLevelProcessor processor, IConverter<OpenedWindow, ToolStripMenuItem> converter)
		{
			_processor = processor;
			_converter = converter;

			_windowsCache = _processor.GetOpenedWindows().ToList();

			_trayIcon = new NotifyIcon
			{
				Icon             = Resources.MainIcon.ToIcon(),
				ContextMenuStrip = MakeMenu(),
				Visible          = true
			};

			_trayIcon.Click += TrayClickCallback;
		}

		protected override void ExitThreadCore()
		{
			_trayIcon.Visible = false;

			Application.Exit();
		}

		private void TrayClickCallback(object? sender, EventArgs e)
		{
			var trayApp = (sender as NotifyIcon)!;
			_windowsCache = _processor.GetOpenedWindows().ToList();

			// TODO: Memoization
			trayApp.ContextMenuStrip = MakeMenu();
		}

		private void ItemClickCallback(object? sender, EventArgs e)
		{
			var menuItem = (sender as ToolStripMenuItem)!;
			
			//bad
			var selected = _windowsCache.Single(x => x.Title.Equals(menuItem.Text, StringComparison.OrdinalIgnoreCase));
			var index    = _windowsCache.IndexOf(selected);
			
			_processor.SetTopMode(selected, selected.IsOnTop ? WindowTopMode.NoTopMost : WindowTopMode.TopMost);

			_windowsCache[index].IsOnTop         = !selected.IsOnTop;
			(sender as ToolStripMenuItem)!.Image = selected.IsOnTop ? Resources.CheckIcon.ToImage() : null;
		}

		private ContextMenuStrip MakeMenu()
		{
			var menu = new ContextMenuStrip();

			foreach (var window in _windowsCache)
			{
				menu.Items.Add(_converter.Convert(window, ItemClickCallback));
			}

			return menu;
		}

		private List<OpenedWindow> _windowsCache;

		private readonly NotifyIcon                                  _trayIcon;
		private readonly ILowLevelProcessor                          _processor;
		private readonly IConverter<OpenedWindow, ToolStripMenuItem> _converter;
	}
}