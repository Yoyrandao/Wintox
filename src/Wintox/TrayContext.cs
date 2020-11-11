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

			_windowsCache = new SortedSet<OpenedWindow>(_processor.GetOpenedWindows(), new OpenedWindowComparer());
			_updated      = new Queue<OpenedWindow>();

			_trayIcon = new NotifyIcon
			{
				Icon             = Resources.MainIcon.ToIcon(),
				ContextMenuStrip = MakeMenu(),
				Visible          = true
			};

			_trayIcon.Click += TrayClickCallback;
		}

		private void TrayClickCallback(object? sender, EventArgs e)
		{
			foreach (var window in _processor.GetOpenedWindows())
			{
				_windowsCache.Add(window);
			}
		}

		private void ItemClickCallback(object? sender, EventArgs e)
		{
			var menuItem = (sender as ToolStripMenuItem)!;

			var index        = _trayIcon.ContextMenuStrip.Items.IndexOf(menuItem);
			var window = _windowsCache.ElementAt(index);

			_processor.SetTopMode(window, window.IsOnTop ? WindowTopMode.NoTopMost : WindowTopMode.TopMost);
			window.ChangeTopMode();

			_updated.Enqueue(window);
			UpdateMenu();
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

		private void UpdateMenu()
		{
			var menu = _trayIcon.ContextMenuStrip;

			while (_updated.Any())
			{
				var window = _updated.Dequeue();

				foreach (ToolStripMenuItem? item in menu.Items)
				{
					if (!item!.CheckEquality(window.Uid))
						continue;

					item.ChangeStatusIcon(window.IsOnTop);
				}
			}
		}

		private SortedSet<OpenedWindow> _windowsCache;
		private Queue<OpenedWindow>     _updated;

		private readonly NotifyIcon                                  _trayIcon;
		private readonly ILowLevelProcessor                          _processor;
		private readonly IConverter<OpenedWindow, ToolStripMenuItem> _converter;
	}
}