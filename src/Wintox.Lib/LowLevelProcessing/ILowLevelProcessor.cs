using System.Collections.Generic;

using Wintox.Lib.Constants;
using Wintox.Lib.Models;

namespace Wintox.Lib.LowLevelProcessing
{
	public interface ILowLevelProcessor
	{
		List<OpenedWindow> GetOpenedWindows();

		void SetTopMode(OpenedWindow window, WindowTopMode mode);

		OpenedWindow GetActive();
	}
}