namespace SymbolTyper;

using System;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using System.Runtime.InteropServices;

internal static class Program
{
	public static Config Config { get; private set; }
	public const string CONFIG_PATH = @"config.json";

	[STAThread]
	static void Main()
	{
		ApplicationConfiguration.Initialize();

		if (!File.Exists(CONFIG_PATH))
		{
			Config = new();
			SaveConfig();
		}
		else
		{
			Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(CONFIG_PATH), new JsonSerializerOptions()
			{
				ReadCommentHandling = JsonCommentHandling.Skip,
				AllowTrailingCommas = true,
			});
		}

		SystemTrayProcess taskBarProcess = new(Config);

		AppDomain.CurrentDomain.ProcessExit += new(taskBarProcess.Exit);

		Application.Run(taskBarProcess);
	}

	public static void SaveConfig() => File.WriteAllText(CONFIG_PATH, JsonSerializer.Serialize(Config, new JsonSerializerOptions()
	{
		WriteIndented = true,
	}));
}