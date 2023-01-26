namespace Iksokodo;
using System;
using WK.Libraries.HotkeyListenerNS;

internal static class HotkeyManager
{
	private static HotkeyListener _listener;
	private static Action<object, EventArgs> _action;
	private static Hotkey _hotkey;

	public static void RegisterHotkey(Action<object, EventArgs> action)
	{
		_listener = new();
		_action = action;

		_hotkey = Program.Config.ToggleHotkey;

		_listener.Add(_hotkey);

		_listener.HotkeyPressed += (_, e) =>
		{
			if (e.Hotkey == _hotkey)
			{
				_action(default, default);
			}
		};
	}

	public static void EnableSelector(Control control) => new HotkeySelector().Enable(control, _hotkey);

	public static void UpdateHotkey(string s)
	{
		Hotkey newHotkey;

		try
		{
			newHotkey = new(s);
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Change hotkey failed.\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			throw;
		}

		_listener.Update(_hotkey, newHotkey);

		_hotkey = newHotkey;
		Program.Config.ToggleHotkey = newHotkey;

		Program.SaveConfig();
	}
}
