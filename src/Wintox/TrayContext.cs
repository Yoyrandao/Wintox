#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Wintox.Helpers;
using Wintox.Helpers.Converters;
using Wintox.Helpers.MenuManagement;
using Wintox.Lib.Constants;
using Wintox.Lib.LowLevelProcessing;
using Wintox.Lib.Models;

namespace Wintox
{
	public class TrayContext : ApplicationContext
	{
		public TrayContext(
			ILowLevelProcessor                          processor,
			IShortcutManager                            shortcutManager,
			IConverter<OpenedWindow, ToolStripMenuItem> converter)
		{
			_processor       = processor;

			_windowsCache = new SortedSet<OpenedWindow>(_processor.GetOpenedWindows(), new OpenedWindowComparer());
			_trayManager  = new TrayMenuManager(converter, ItemClickCallback, ExitCallback);

			_trayIcon = new NotifyIcon
			{
				Icon             = Resources.MainIcon.ToIcon(),
				ContextMenuStrip = _trayManager.Init(_windowsCache),
				Visible          = true
			};

			_trayIcon.Click += TrayClickCallback;
			shortcutManager.Register(FiredShortcutCallback);
		}

		private void TrayClickCallback(object? sender, EventArgs e)
		{
			foreach (var window in _processor.GetOpenedWindows())
			{
				_windowsCache.Add(window);
			}
			_trayManager.Update(_windowsCache);
		}

		private void ItemClickCallback(object? sender, EventArgs e)
		{
			var menuItem = (sender as ToolStripItem)!;

			var index = _trayManager.ChangeStateOf(menuItem, menuItem.Image != null);
			var window = _windowsCache.ElementAt(index);

			_processor.SetTopMode(window, window.IsOnTop ? WindowTopMode.NoTopMost : WindowTopMode.TopMost);
			window.ChangeTopMode();

			_trayManager.Update(_windowsCache);
		}

		private void FiredShortcutCallback()
		{
			var active = _processor.GetActive();
			var window = _windowsCache.Single(x => x.Hwnd == active.Hwnd);
			
			_processor.SetTopMode(window, window.IsOnTop ? WindowTopMode.NoTopMost : WindowTopMode.TopMost);
			window.ChangeTopMode();
			
			_trayManager.Update(_windowsCache);
		}

		private void ExitCallback(object? sender, EventArgs e)
		{
			_trayIcon.ContextMenuStrip.Items.Clear();
			_trayIcon.Visible = false;

			Application.Exit();
		}

		private SortedSet<OpenedWindow> _windowsCache;

		private readonly ILowLevelProcessor _processor;
		private readonly ITrayMenuManager   _trayManager;

		private readonly NotifyIcon _trayIcon;
	}
}