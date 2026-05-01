using BepInEx.Configuration;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils
{
	public class ConfigManager
	{
		public static ConfigManager Instance { get; private set; }

		public static void Init(ConfigFile config)
		{
			Instance = new ConfigManager(config);
		}

		internal static ConfigFile configFile;

		public static ConfigEntry<LoggingType> LoggingLevels { get; private set; }

		public static ConfigEntry<string> PreviewInfoType { get; private set; }
		public static ConfigEntry<string> FilterInfoType { get; private set; }
		public static ConfigEntry<string> SortInfoType { get; private set; }

		private ConfigManager(ConfigFile config)
		{
			configFile = config;

			LoggingLevels = configFile.Bind("Debug", "Logging Levels", LoggingType.Basic, "Set the logging level for the mod");

			PreviewInfoType = configFile.Bind(
				"General",
				"Preview Info Type",
				Defaults.defaultPreviewType,
				"Set the preview info type. Must be the name of an existing preview info type."
			);
			FilterInfoType = configFile.Bind(
				"General",
				"Filter Info Type",
				Defaults.defaultFilterType,
				"Set the filter info type. Must be the name of an existing filter info type."
			);
			SortInfoType = configFile.Bind(
				"General",
				"Sort Info Type",
				Defaults.defaultSortType,
				"Set the default sort info type. Must be the name of an existing sort info type."
			);
		}
	}
}
