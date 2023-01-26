namespace Iksokodo;
using WK.Libraries.HotkeyListenerNS;

internal record Config
{
	public Hotkey ToggleHotkey { get; set; } = new(Keys.Alt, Keys.S);
	public Keys StringStartKey { get; set; } = Keys.Alt;
	public Dictionary<string, string> Replacements { get; set; } = new();
}
