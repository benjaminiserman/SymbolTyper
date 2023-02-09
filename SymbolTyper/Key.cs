namespace SymbolTyper;

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

internal record struct Key(Keys Keys, bool Shift)
{
	public char KeyChar
	{
		get
		{
			var c = Keys switch
			{
				Keys.A => Shift ? 'A' : 'a',
				Keys.B => Shift ? 'B' : 'b',
				Keys.C => Shift ? 'C' : 'c',
				Keys.D => Shift ? 'D' : 'd',
				Keys.E => Shift ? 'E' : 'e',
				Keys.F => Shift ? 'F' : 'f',
				Keys.G => Shift ? 'G' : 'g',
				Keys.H => Shift ? 'H' : 'h',
				Keys.I => Shift ? 'I' : 'i',
				Keys.J => Shift ? 'J' : 'j',
				Keys.K => Shift ? 'K' : 'k',
				Keys.L => Shift ? 'L' : 'l',
				Keys.M => Shift ? 'M' : 'm',
				Keys.N => Shift ? 'N' : 'n',
				Keys.O => Shift ? 'O' : 'o',
				Keys.P => Shift ? 'P' : 'p',
				Keys.Q => Shift ? 'Q' : 'q',
				Keys.R => Shift ? 'R' : 'r',
				Keys.S => Shift ? 'S' : 's',
				Keys.T => Shift ? 'T' : 't',
				Keys.U => Shift ? 'U' : 'u',
				Keys.V => Shift ? 'V' : 'v',
				Keys.W => Shift ? 'W' : 'w',
				Keys.X => Shift ? 'X' : 'x',
				Keys.Y => Shift ? 'Y' : 'y',
				Keys.Z => Shift ? 'Z' : 'z',

				Keys.Oemtilde => Shift ? '~' : '`',
				Keys.D1 => Shift ? '!' : '1',
				Keys.D2 => Shift ? '@' : '2',
				Keys.D3 => Shift ? '#' : '3',
				Keys.D4 => Shift ? '$' : '4',
				Keys.D5 => Shift ? '%' : '5',
				Keys.D6 => Shift ? '^' : '6',
				Keys.D7 => Shift ? '&' : '7',
				Keys.D8 => Shift ? '*' : '8',
				Keys.D9 => Shift ? '(' : '9',
				Keys.D0 => Shift ? ')' : '0',
				Keys.OemMinus => Shift ? '_' : '-',
				Keys.Oemplus => Shift ? '+' : '=',

				Keys.OemOpenBrackets => Shift ? '{' : '[',
				Keys.OemCloseBrackets => Shift ? '}' : ']',
				Keys.OemPipe => Shift ? '|' : '\\',

				Keys.OemSemicolon => Shift ? ':' : ';',
				Keys.OemQuotes => Shift ? '"' : '\'',

				Keys.Oemcomma => Shift ? '<' : ',',
				Keys.OemPeriod => Shift ? '>' : '.',
				Keys.OemQuestion => Shift ? '?' : '/',

				Keys.Space => ' ',

				_ => '\0'
			};

			return c;
		}
	}

	public static Key GetKey(char c) => c switch
	{
		'A' => new(Keys.A, Shift: true),
		'a' => new(Keys.A, Shift: false),
		'B' => new(Keys.B, Shift: true),
		'b' => new(Keys.B, Shift: false),
		'C' => new(Keys.C, Shift: true),
		'c' => new(Keys.C, Shift: false),
		'D' => new(Keys.D, Shift: true),
		'd' => new(Keys.D, Shift: false),
		'E' => new(Keys.E, Shift: true),
		'e' => new(Keys.E, Shift: false),
		'F' => new(Keys.F, Shift: true),
		'f' => new(Keys.F, Shift: false),
		'G' => new(Keys.G, Shift: true),
		'g' => new(Keys.G, Shift: false),
		'H' => new(Keys.H, Shift: true),
		'h' => new(Keys.H, Shift: false),
		'I' => new(Keys.I, Shift: true),
		'i' => new(Keys.I, Shift: false),
		'J' => new(Keys.J, Shift: true),
		'j' => new(Keys.J, Shift: false),
		'K' => new(Keys.K, Shift: true),
		'k' => new(Keys.K, Shift: false),
		'L' => new(Keys.L, Shift: true),
		'l' => new(Keys.L, Shift: false),
		'M' => new(Keys.M, Shift: true),
		'm' => new(Keys.M, Shift: false),
		'N' => new(Keys.N, Shift: true),
		'n' => new(Keys.N, Shift: false),
		'O' => new(Keys.O, Shift: true),
		'o' => new(Keys.O, Shift: false),
		'P' => new(Keys.P, Shift: true),
		'p' => new(Keys.P, Shift: false),
		'Q' => new(Keys.Q, Shift: true),
		'q' => new(Keys.Q, Shift: false),
		'R' => new(Keys.R, Shift: true),
		'r' => new(Keys.R, Shift: false),
		'S' => new(Keys.S, Shift: true),
		's' => new(Keys.S, Shift: false),
		'T' => new(Keys.T, Shift: true),
		't' => new(Keys.T, Shift: false),
		'U' => new(Keys.U, Shift: true),
		'u' => new(Keys.U, Shift: false),
		'V' => new(Keys.V, Shift: true),
		'v' => new(Keys.V, Shift: false),
		'W' => new(Keys.W, Shift: true),
		'w' => new(Keys.W, Shift: false),
		'X' => new(Keys.X, Shift: true),
		'x' => new(Keys.X, Shift: false),
		'Y' => new(Keys.Y, Shift: true),
		'y' => new(Keys.Y, Shift: false),
		'Z' => new(Keys.Z, Shift: true),
		'z' => new(Keys.Z, Shift: false),

		'~' => new(Keys.Oemtilde, Shift: true),
		'`' => new(Keys.Oemtilde, Shift: false),
		'!' => new(Keys.D1, Shift: true),
		'1' => new(Keys.D1, Shift: false),
		'@' => new(Keys.D2, Shift: true),
		'2' => new(Keys.D2, Shift: false),
		'#' => new(Keys.D3, Shift: true),
		'3' => new(Keys.D3, Shift: false),
		'$' => new(Keys.D4, Shift: true),
		'4' => new(Keys.D4, Shift: false),
		'%' => new(Keys.D5, Shift: true),
		'5' => new(Keys.D5, Shift: false),
		'^' => new(Keys.D6, Shift: true),
		'6' => new(Keys.D6, Shift: false),
		'&' => new(Keys.D7, Shift: true),
		'7' => new(Keys.D7, Shift: false),
		'*' => new(Keys.D8, Shift: true),
		'8' => new(Keys.D8, Shift: false),
		'(' => new(Keys.D9, Shift: true),
		'9' => new(Keys.D9, Shift: false),
		')' => new(Keys.D0, Shift: true),
		'0' => new(Keys.D0, Shift: false),
		'_' => new(Keys.OemMinus, Shift: true),
		'-' => new(Keys.OemMinus, Shift: false),
		'+' => new(Keys.Oemplus, Shift: true),
		'=' => new(Keys.Oemplus, Shift: false),

		'{' => new(Keys.OemOpenBrackets, Shift: true),
		'[' => new(Keys.OemOpenBrackets, Shift: false),
		'}' => new(Keys.OemCloseBrackets, Shift: true),
		']' => new(Keys.OemCloseBrackets, Shift: false),
		'|' => new(Keys.OemPipe, Shift: true),
		'\\' => new(Keys.OemPipe, Shift: false),

		':' => new(Keys.OemSemicolon, Shift: true),
		';' => new(Keys.OemSemicolon, Shift: false),
		'"' => new(Keys.OemQuotes, Shift: true),
		'\'' => new(Keys.OemQuotes, Shift: false),

		'<' => new(Keys.Oemcomma, Shift: true),
		',' => new(Keys.Oemcomma, Shift: false),
		'>' => new(Keys.OemPeriod, Shift: true),
		'.' => new(Keys.OemPeriod, Shift: false),
		'?' => new(Keys.OemQuestion, Shift: true),
		'/' => new(Keys.OemQuestion, Shift: false),
		' ' => new(Keys.Space, Shift: false),

		_ => throw new($"Invalid character {c}")
	};

	public static string PossibleCharactersWithoutShift => @"qwertyuiopasdfghjklzxcvbnm`1234567890-=[]\;',./ ";
	public static string StopCharacters => " ";
	private static readonly HashSet<char> _stopCharacters = StopCharacters.ToHashSet();
	public static bool IsStop(char c) => _stopCharacters.Contains(c);
}
