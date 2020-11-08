using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Wintox.Lib.Constants;
using Wintox.Lib.Models;

namespace Wintox.Lib.LowLevelProcessing
{
	public delegate bool EnumerateFunc(IntPtr hWnd, int lParam);

	public delegate bool EnumerateWindowsFunc(IntPtr hWnd, IntPtr lParam);

	public class LowLevelProcessor
	{
		public LowLevelProcessor(List<string> excludedProcesses)
		{
			_excludedProcesses = excludedProcesses;
		}
		
		public List<OpenedWindow> GetOpenedWindows()
		{
			var openedWindows = new List<OpenedWindow>();

			var filter = new EnumerateFunc((hwnd, param) =>
			{
				var buffer = new StringBuilder(255);
				LowLevel.GetWindowText(hwnd, buffer, buffer.Capacity + 1);

				var title = buffer.ToString();

				if (!LowLevel.IsWindowVisible(hwnd) || string.IsNullOrEmpty(title))
				{
					return true;
				}

				LowLevel.GetWindowThreadProcessId(hwnd, out var processId);
				var executablePath = LowLevelHelper.GetUwpApplicationName(hwnd, processId);

				if (_excludedProcesses.Any(x => executablePath.Contains(x) || title.Contains(x)))
				{
					return true;
				}

				openedWindows.Add(new OpenedWindow
				{
					Hwnd           = hwnd,
					Title          = title,
					ExecutablePath = executablePath
				});

				return true;
			});

			LowLevel.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero);
			
			return openedWindows;
		}

		public void SetTopMode(OpenedWindow window, WindowTopMode mode)
		{
			LowLevel.SetWindowPos(window.Hwnd, (IntPtr) mode, 0, 0, 0, 0,
			                      (uint) (WindowPositionParameters.NoMove | WindowPositionParameters.NoSize));
		}

		private readonly List<string> _excludedProcesses;
	}
}