using System.Collections.Generic;
using System.Windows.Forms;

using Wintox.Lib.Models;

namespace Wintox.Helpers.MenuManagement
{
	public interface ITrayMenuManager
	{
		ContextMenuStrip Init(IEnumerable<OpenedWindow> windows);
		
		void Update(IEnumerable<OpenedWindow> windows);
		
		int ChangeStateOf(ToolStripItem window, bool isOnTop);
	}
}