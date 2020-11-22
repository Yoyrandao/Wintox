using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Wintox.Helpers.Converters;
using Wintox.Lib.Models;

namespace Wintox.Helpers.MenuManagement
{
	public class TrayMenuManager : ITrayMenuManager
	{
		public TrayMenuManager(
			IConverter<OpenedWindow, ToolStripMenuItem> converter,
			EventHandler                                itemClickedHandler,
			EventHandler                                exitClickedHandler,
			OptionsContext                              optionsContext)
		{
			_converter = converter;

			_itemClickedHandler = itemClickedHandler;
			_exitClickedHandler = exitClickedHandler;
			_optionsContext     = optionsContext;

			_items          = new List<ToolStripItem>();
			_dropDownButton = new ToolStripDropDownButton(Resources.WindowsGroupText);
			_dropDown       = new ToolStripDropDownMenu();
			_menu           = new ContextMenuStrip();
		}

		public ContextMenuStrip Init(IEnumerable<OpenedWindow> windows)
		{
			foreach (var window in windows)
			{
				var menuItem = _converter.Convert(window, _itemClickedHandler);

				_items.Add(menuItem);
				_dropDown.Items.Add(menuItem);
			}

			_dropDownButton.DropDown = _dropDown;

			_menu.Items.Add(_dropDownButton);
			_menu.Items.Add(new ToolStripButton("Test", null,
			                                    (s, e) => { _optionsContext.Visible = !_optionsContext.Visible; }));
			
			_menu.Items.Add(new ToolStripButton(Resources.Exit, null, _exitClickedHandler));

			return _menu;
		}

		public void Update(IEnumerable<OpenedWindow> windows)
		{
			_dropDown.Items.Clear();
			_items.Clear();

			foreach (var window in windows)
			{
				var menuItem = _converter.Convert(window, _itemClickedHandler);

				_items.Add(menuItem);
				_dropDown.Items.Add(menuItem);
			}

			_dropDownButton.DropDown = _dropDown;
		}

		public int ChangeStateOf(ToolStripItem window, bool isOnTop)
		{
			var index = _items.IndexOf(window);

			_dropDown.Items[index].ChangeStatusIcon(isOnTop);
			_dropDownButton.DropDown = _dropDown;

			return index;
		}

		private List<ToolStripItem> _items;
		private ContextMenuStrip    _menu;

		private ToolStripDropDownButton _dropDownButton;
		private ToolStripDropDown       _dropDown;

		private readonly IConverter<OpenedWindow, ToolStripMenuItem> _converter;

		private readonly EventHandler _itemClickedHandler;
		private readonly EventHandler _exitClickedHandler;

		private readonly OptionsContext _optionsContext;
	}
}