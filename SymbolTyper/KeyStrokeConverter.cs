namespace SymbolTyper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WingSharpExtensions;

internal class KeyStrokeConverter
{
	[DllImport("User32.dll")]
	private static extern short GetAsyncKeyState(Keys ArrowKeys);

	private readonly InputSimulator _inputSimulator = new();

	public bool Paused { get; set; }

	public ManualResetEvent Handle { get; private set; } = new(false);

	private LazyDictionary<List<char>, string> DfsmKeyToStringMap { get; } = new()
	{
		AddMissingKeys = true,
		GetDefault = s => string.Join(string.Empty, s)
	};

	private Dictionary<string, string> StringReplacements { get; set; }

	private DeterministicFiniteStateMachine<char> StateMachine { get; init; }
	private List<char> _cachedMatch = null;
	private Stack<DeterministicFiniteStateMachine<char>.State> PreviousStates { get; } = new();

	public KeyStrokeConverter(Config config)
	{
		if (config.Replacements.Count == 0)
		{
			throw new("Config does not contain any replacements.");
		}

		if (config.Replacements.ContainsKey(string.Empty))
		{
			throw new("An empty string is not a valid key for replacement.");
		}

		StateMachine = DeterministicFiniteStateMachine<char>.GetDfsmFromStrings(config.Replacements.Keys);
		StringReplacements = config.Replacements;
	}

	public void Convert()
	{
		if (Paused)
		{
			return;
		}

		if (TryGetKey(out var receivedKey))
		{
			if (receivedKey.Keys == Keys.Back) // handle backspace
			{
				if (PreviousStates.Count > 1)
				{
					PreviousStates.Pop();
					StateMachine.CurrentState = PreviousStates.Peek();
					if (StateMachine.CurrentStateIsAccepting)
					{
						_cachedMatch = PreviousStates.Peek().Key;
					}
					else
					{
						_cachedMatch = null;
					}
				}
				else
				{
					StateMachine.ReturnToStart();
					_cachedMatch = null;
				}

				return;
			}
			else if (receivedKey.KeyChar == '\0') // handle control characters
			{
				StateMachine.ReturnToStart();
				PreviousStates.Clear();
				_cachedMatch = null;
			}
			else if (Key.IsStop(receivedKey.KeyChar) 
				&& _cachedMatch != null) // handle stop characters
			{
				StateMachine.ReturnToStart();

				foreach (var _ in 0..(_cachedMatch.Count + 1))
				{
					_inputSimulator.Keyboard.KeyPress(VirtualKeyCode.BACK);
				}

				GetAsyncKeyState(Keys.Back);

				_inputSimulator.Keyboard.TextEntry(StringReplacements[DfsmKeyToStringMap[_cachedMatch]]);

				PreviousStates.Clear();
				_cachedMatch = null;
			}
			else if (Process(receivedKey, out var matchedString)) // advance dfsm
			{
				_cachedMatch = matchedString;
			}
			else
			{
				_cachedMatch = null;
			}

			PreviousStates.Push(StateMachine.CurrentState);
		}
	}

	private bool Process(Key receivedKey, out List<char> matchedString)
	{
		var keyChar = receivedKey.KeyChar;

		if (Key.IsStop(keyChar))
		{
			matchedString = null;
			return StateMachine.ReturnToStart();
		}
		else
		{
			return StateMachine.Process(keyChar, out matchedString);
		}
	}

	private static Keys[] ControlKeys { get; } = new Keys[]
	{
		Keys.LMenu,
		Keys.RMenu,
		Keys.ControlKey,
		Keys.RControlKey,
		Keys.LControlKey,
		Keys.Escape,
		Keys.Left,
		Keys.Up,
		Keys.Right,
		Keys.Down,
		Keys.Left,
		Keys.Home,
		Keys.PageUp,
		Keys.PageDown,
		Keys.Tab,
		Keys.LWin,
		Keys.RWin,
		Keys.Return,
	};

	private const int DOWN_AND_PRESSED_RECENTLY = -32767; // -32767 == 0b10000000_00000001 => key is down and was pressed since last query

	private static bool TryGetKey(out Key gotKey)
	{
		var shiftKeyPressed = GetAsyncKeyState(Keys.ShiftKey) != 0;

		if (GetAsyncKeyState(Keys.Back) == DOWN_AND_PRESSED_RECENTLY)
		{
			if (shiftKeyPressed)
			{
				gotKey = new(Keys.Escape, shiftKeyPressed);
			}
			else
			{
				gotKey = new(Keys.Back, shiftKeyPressed);
			}

			return true;
		}

		if (ControlKeys.AnyOut(key => GetAsyncKeyState(key) == DOWN_AND_PRESSED_RECENTLY, out var key))
		{
			gotKey = new(key, shiftKeyPressed);
			return true;
		}

		foreach (var c in Key.PossibleCharactersWithoutShift)
		{
			var possibleKey = Key.GetKey(c);
			if (GetAsyncKeyState(possibleKey.Keys) == DOWN_AND_PRESSED_RECENTLY) 
			{
				gotKey = new(possibleKey.Keys, shiftKeyPressed);
				return true;
			}
		}

		gotKey = default;
		return false;
	}
}