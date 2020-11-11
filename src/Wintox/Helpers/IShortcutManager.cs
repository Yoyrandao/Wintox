using System;

namespace Wintox.Helpers
{
	public interface IShortcutManager
	{
		void Register(Action callback);
	}
}