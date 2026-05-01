using BepInEx.Configuration;

namespace TerminalUtils.Definitions
{
	public abstract class ConfigHandler<T, CT> : MrovLib.ConfigHandler<T, CT>
	{
		public ConfigHandler(CT value)
		{
			DefaultValue = value;
		}
	}
}
