#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Serilog;

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
			IExceptionShield                            shield,
			OptionsContext                              optionsContext,
			IConverter<OpenedWindow, ToolStripMenuItem> converter)
		{
			_processor = processor;
			_shield    = shield;

			_shield.SetLogger(_logger);

			_windowsCache = new SortedSet<OpenedWindow>(_processor.GetOpenedWindows(), new OpenedWindowComparer());
			_trayManager  = new TrayMenuManager(converter, ItemClickCallback, ExitCallback, optionsContext);

			_trayIcon = new NotifyIcon
			{
				Icon             = Resources.MainIcon.ToIcon(),
				ContextMenuStrip = _trayManager.Init(_windowsCache),
				Visible          = true,
				Text             = Resources.WintoxToolTip
			};

			_trayIcon.Click += TrayClickCallback;
			shortcutManager.Register(FiredShortcutCallback);
		}

		private void TrayClickCallback(object? sender, EventArgs e)
		{
			_shield.Protect(() =>
			{
				_logger.Information("Gettings opened windows.");

				foreach (var window in _processor.GetOpenedWindows())
				{
					_windowsCache.Add(window);
				}

				_trayManager.Update(_windowsCache);
			});
		}

		private void ItemClickCallback(object? sender, EventArgs e)
		{
			_shield.Protect(() =>
			{
				_logger.Information("Panel clicked.");

				var menuItem = (sender as ToolStripItem)!;

				var index  = _trayManager.ChangeStateOf(menuItem, menuItem.Image != null);
				var window = _windowsCache.ElementAt(index);

				_logger.Information(
					$"Changing state of \"{window.Title}\" (HWND: {window.Hwnd}) window to {!window.IsOnTop}");

				_processor.SetTopMode(window, window.IsOnTop ? WindowTopMode.NoTopMost : WindowTopMode.TopMost);
				window.ChangeTopMode();

				_trayManager.Update(_windowsCache);
			});
		}

		private void FiredShortcutCallback()
		{
			_shield.Protect(() =>
			{
				_logger.Information("Shortcut clicked.");

				var active = _processor.GetActive();
				var window = _windowsCache.Single(x => x.Hwnd == active.Hwnd);

				_logger.Information(
					$"Changing state of \"{window.Title}\" (HWND: {window.Hwnd}) window to {!window.IsOnTop}");

				_processor.SetTopMode(window, window.IsOnTop ? WindowTopMode.NoTopMost : WindowTopMode.TopMost);
				window.ChangeTopMode();

				_trayManager.Update(_windowsCache);
			});
		}

		private void ExitCallback(object? sender, EventArgs e)
		{
			_shield.Protect(() =>
			{
				_logger.Information("Application shutdown.");

				_trayIcon.ContextMenuStrip.Items.Clear();
				_trayIcon.Visible = false;

				Application.Exit();
			});
		}

		private volatile SortedSet<OpenedWindow> _windowsCache;

		private readonly ILowLevelProcessor _processor;
		private readonly IExceptionShield   _shield;
		private readonly ITrayMenuManager   _trayManager;

		private readonly NotifyIcon _trayIcon;

		private readonly ILogger _logger = Log.ForContext<TrayContext>();
	}
}