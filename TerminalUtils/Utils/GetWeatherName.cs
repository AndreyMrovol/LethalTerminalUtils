namespace TerminalUtils.Utils
{
	public static class GetWeatherName
	{
		public static string GetWeather(SelectableLevel level, bool shortened = false)
		{
			string weather = MrovLib.SharedMethods.GetWeather(level);

			if (weather.Length >= Defaults.planetWeatherWidth || shortened)
			{
				if (Plugin.WeatherRegistryCompatibility.IsModPresent)
				{
					weather = Plugin.WeatherRegistryCompatibility.GetWeatherShort(level.currentWeather);
				}
				else
				{
					weather = weather.Substring(0, 5);
				}
			}

			if (Plugin.WeatherRegistryCompatibility.IsModPresent)
			{
				weather = Plugin.WeatherRegistryCompatibility.GetWeather(level);
			}

			return weather == "None" ? "" : weather;
		}
	}
}
