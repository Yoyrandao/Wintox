#nullable enable
using System;
using System.Collections.Generic;
using System.Drawing;
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
			foreach (var window in _processor.GetOpenedWindows())
			{
				_windowsCache.Add(window);
			}

			UpdateMenu();
		}

		private void ItemClickCallback(object? sender, EventArgs e)
		{
			var menuItem = (sender as ToolStripMenuItem)!;

			//bad
			var selected = _windowsCache.Single(x => x.Title.Equals(menuItem.Text, StringComparison.OrdinalIgnoreCase));

			var index = _windowsCache
			            .Select((value, idx) => new {Window = value, Index = idx})
			            .Single(x => x.Window.Hwnd == selected.Hwnd).Index;

			_processor.SetTopMode(selected, selected.IsOnTop ? WindowTopMode.NoTopMost : WindowTopMode.TopMost);

			_windowsCache.ElementAt(index).IsOnTop = !selected.IsOnTop;
			(sender as ToolStripMenuItem)!.Image   = selected.IsOnTop ? Resources.CheckIcon.ToImage() : null;
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
			
			foreach (var window in _windowsCache)
			{
				for (var i = 0; i < menu.Items.Count; i++)
				{
					if (menu.Items[i].Text.Equals(window.Title, StringComparison.OrdinalIgnoreCase))
					{
						menu.Items[i].Image = menu.Items[i].Image != null
							? Image.FromFile("Resources/check.ico")
							: null;
					}
				}
			}
		}

		private SortedSet<OpenedWindow> _windowsCache;

		private readonly NotifyIcon                                  _trayIcon;
		private readonly ILowLevelProcessor                          _processor;
		private readonly IConverter<OpenedWindow, ToolStripMenuItem> _converter;
	}
}