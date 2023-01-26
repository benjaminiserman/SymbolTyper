internal readonly struct Key
{
	private readonly PossibleKey _keyReceived;
	private readonly bool _shift;

	public Key(Keys key, bool shift)
	{
		_keyReceived = key switch
		{
			Keys.C => PossibleKey.C,
			Keys.G => PossibleKey.G,
			Keys.H => PossibleKey.H,
			Keys.J => PossibleKey.J,
			Keys.S => PossibleKey.S,
			Keys.U => PossibleKey.U,
			Keys.X => PossibleKey.X,
			Keys.Oem7 => PossibleKey.Apostrophe,
			_ => PossibleKey.Other,
		};

		_shift = shift;
	}

	public bool IsStart => _keyReceived is not PossibleKey.X 
		and not PossibleKey.Other 
		and not PossibleKey.Apostrophe;

	public bool IsApostrophe => _keyReceived is PossibleKey.Apostrophe 
		&& !_shift;

	public bool IsX => _keyReceived is PossibleKey.X;

	private char ToChar()
	{
		var c = _keyReceived switch
		{
			PossibleKey.C => 'ĉ',
			PossibleKey.G => 'ĝ',
			PossibleKey.H => 'ĥ',
			PossibleKey.J => 'ĵ',
			PossibleKey.S => 'ŝ',
			PossibleKey.U => 'ŭ',
			_ => throw new InvalidOperationException("Cannot convert KeyReceived.X or KeyReceived.Other into a char.")
		};

		if (_shift)
		{
			c = char.ToUpper(c);
		}

		return c;
	}

	private enum PossibleKey { C, G, H, J, S, U, X, Apostrophe, Other }

	public static explicit operator char(Key key) => key.ToChar();
}