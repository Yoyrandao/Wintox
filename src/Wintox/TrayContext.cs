#nullable enable
using System;
using System.Windows.Forms;

using Wintox.Helpers;
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

			_trayIcon = new NotifyIcon
			{
				Icon             = Resources.MainIcon.ToIcon(),
				ContextMenuStrip = new ContextMenuStrip(),
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
			var windows = _processor.GetOpenedWindows();

			trayApp.ContextMenuStrip.Items.Clear();

			// TODO: Memoization
			foreach (var window in windows)
			{
				trayApp.ContextMenuStrip.Items.Add(_converter.Convert(window, ItemClickCallback));
			}
		}

		private void ItemClickCallback(object? sender, EventArgs e)
		{
			var menuItem = (sender as ToolStripMenuItem)!;

			//TODO: set top mode by index

			menuItem.Image = menuItem.Image == null
				                 ? Resources.CheckIcon.ToImage()
				                 : null;
		}

		private readonly NotifyIcon                                  _trayIcon;
		private readonly ILowLevelProcessor                          _processor;
		private readonly IConverter<OpenedWindow, ToolStripMenuItem> _converter;
	}
}