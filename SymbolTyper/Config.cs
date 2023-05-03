namespace SymbolTyper;

using System.Collections.Generic;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

internal record Config
{
	public Hotkey ToggleHotkey { get; set; } = new(Keys.Alt, Keys.S);
	public Dictionary<string, string> Replacements { get; set; } = new();
}
