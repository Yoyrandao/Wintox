using System;
using System.Runtime.InteropServices;
using System.Text;

using Wintox.Lib.Constants;

namespace Wintox.Lib.LowLevelProcessing
{
	public static class LowLevel
	{
		[DllImport(LibraryType.USER)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWindowVisible(IntPtr hWnd);
		

		[DllImport(
			LibraryType.USER,
			EntryPoint    = "GetWindowText",
			ExactSpelling = false,
			CharSet       = CharSet.Auto,
			SetLastError  = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);
		

		[DllImport(LibraryType.USER, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
		                                       uint   uFlags);
		

		[DllImport(
			LibraryType.USER,
			EntryPoint    = "EnumDesktopWindows",
			ExactSpelling = false,
			CharSet       = CharSet.Auto,
			SetLastError  = true)]
		public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumerateFunc filter, IntPtr lParam);
		

		[DllImport(
			LibraryType.USER,
			EntryPoint   = "GetWindowModuleFileName",
			SetLastError = true,
			CharSet      = CharSet.Auto)]
		public static extern uint GetWindowModuleFileName(IntPtr hWnd, StringBuilder lpszFileName, uint cchFileNameMax);
		

		[DllImport(
			LibraryType.USER,
			EntryPoint   = "GetWindowThreadProcessId",
			CharSet      = CharSet.Auto,
			SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		

		[DllImport(
			LibraryType.USER,
			EntryPoint   = "EnumChildWindows",
			CharSet      = CharSet.Auto,
			SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumChildWindows(IntPtr hWndParent, EnumerateWindowsFunc lpEnumFunc, IntPtr lParam);
		

		[DllImport(
			LibraryType.KERNEL,
			EntryPoint   = "OpenProcess",
			CharSet      = CharSet.Auto,
			SetLastError = true)]
		public static extern IntPtr OpenProcess(
			uint                                 dwDesiredAccess,
			[MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
			int                                  dwProcessId
		);
		

		[DllImport(
			LibraryType.KERNEL,
			EntryPoint   = "QueryFullProcessImageName",
			CharSet      = CharSet.Auto,
			SetLastError = true)]
		public static extern bool QueryFullProcessImageName(
			[In]  IntPtr        hProcess,
			[In]  int           dwFlags,
			[Out] StringBuilder lpExeName,
			ref   int           lpdwSize);
	}
}