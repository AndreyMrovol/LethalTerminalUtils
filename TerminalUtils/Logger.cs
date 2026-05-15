using BepInEx.Logging;
using MrovLib;

namespace TerminalUtils
{
	public class Logger : MrovLib.Logger
	{
		public Logger(string SourceName, LoggingType defaultLoggingType = LoggingType.Debug)
			: base(SourceName, defaultLoggingType)
		{
			ModName = SourceName;
			LogSource = BepInEx.Logging.Logger.CreateLogSource("TerminalUtils");
			_name = SourceName;
		}

		public override bool ShouldLog(LoggingType type)
		{
			return ConfigManager.LoggingLevels.Value >= type;
		}
	}
}
