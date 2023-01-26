namespace Iksokodo;
using System;
using System.Windows.Forms;
using System.Text.Json;

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
			Config = JsonSerializer.Deserialize<Config>(File.ReadAllText(CONFIG_PATH));
		}

		SystemTrayProcess taskBarProcess = new(Config);

		AppDomain.CurrentDomain.ProcessExit += new(taskBarProcess.Exit);

		Application.Run(taskBarProcess);
	}

	public static void SaveConfig() => File.WriteAllText(CONFIG_PATH, JsonSerializer.Serialize(Config));
}