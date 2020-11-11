using System;

using NonInvasiveKeyboardHookLibrary;

namespace Wintox.Helpers
{
	public class ShortcutManager : IShortcutManager
	{
		public ShortcutManager(KeyboardHookManager keyboardHook)
		{
			_keyboardHook = keyboardHook;
		}

		public void Register(Action callback)
		{
			_keyboardHook.Start();
			_keyboardHook.RegisterHotkey(new[] {ModifierKeys.Alt}, 0x51, callback);
		}

		private readonly KeyboardHookManager _keyboardHook;
	}
}