namespace TerminalUtils.Compatibility
{
	internal class WeatherRegistryCompatibility : MrovLib.CompatibilityHandler
	{
		public WeatherRegistryCompatibility(string guid, string version = null)
			: base(guid, version) { }

		public string GetWeather(SelectableLevel level)
		{
			return WeatherRegistry.WeatherManager.GetCurrentWeatherName(level);
		}

		public string GetWeatherShort(LevelWeatherType weather)
		{
			return WeatherRegistry.WeatherManager.GetWeather(weather).NameShort;
		}
	}
}
