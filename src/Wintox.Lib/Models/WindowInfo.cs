using System.Runtime.InteropServices;

namespace Wintox.Lib.Models
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public class WindowInfo
	{
		[MarshalAs(UnmanagedType.U4, SizeConst = 32)]
		public uint OwnerPid;

		[MarshalAs(UnmanagedType.U4, SizeConst = 32)]
		public uint ChildPid;
	}
}