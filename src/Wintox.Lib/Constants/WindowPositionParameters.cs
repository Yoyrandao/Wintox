using System;

namespace Wintox.Lib.Constants
{
	[Flags]
	public enum WindowPositionParameters : int
	{
		NoMove = 0x0002,
		NoSize = 0x0001
	}
}