using System;

namespace Wintox.Lib.Models
{
	public class OpenedWindow
	{
		public IntPtr Hwnd { get; set; }
		
		public string Title { get; set; }
		
		public string ExecutablePath { get; set; }
	}
}