using System;
using System.Runtime.InteropServices;
using System.Text;

using Wintox.Lib.Constants;
using Wintox.Lib.Models;

namespace Wintox.Lib.LowLevelProcessing
{
	public static class LowLevelHelper
	{
		public static string GetUwpApplicationName(IntPtr hWnd, uint processId)
		{
			var windowInfo = new WindowInfo
			{
				OwnerPid = processId,
				ChildPid = processId
			};

			var pWindowInfo = Marshal.AllocHGlobal(Marshal.SizeOf(windowInfo));

			Marshal.StructureToPtr(windowInfo, pWindowInfo, false);
			LowLevel.EnumChildWindows(hWnd, EnumChildWindowsFunc, pWindowInfo);

			windowInfo = (WindowInfo) Marshal.PtrToStructure(pWindowInfo, typeof(WindowInfo));

			var proc = IntPtr.Zero;

			if (windowInfo != null
			    && (proc = LowLevel.OpenProcess(
				        (uint) (ProcessAccess.QueryInformation | ProcessAccess.VirtualMemoryRead),
				        false,
				        (int) windowInfo.ChildPid)) == IntPtr.Zero)
			{
				return null;
			}

			var capacity = 2048;
			var buffer   = new StringBuilder(capacity);
			LowLevel.QueryFullProcessImageName(proc, 0, buffer, ref capacity);

			Marshal.FreeHGlobal(pWindowInfo);

			return buffer.ToString(0, capacity);
		}

		private static bool EnumChildWindowsFunc(IntPtr hWnd, IntPtr lParam)
		{
			var info = (WindowInfo) Marshal.PtrToStructure(lParam, typeof(WindowInfo));

			LowLevel.GetWindowThreadProcessId(hWnd, out var processId);

			if (info != null && processId != info.OwnerPid)
			{
				info.ChildPid = processId;
			}

			Marshal.StructureToPtr(info, lParam, true);

			return true;
		}
	}
}