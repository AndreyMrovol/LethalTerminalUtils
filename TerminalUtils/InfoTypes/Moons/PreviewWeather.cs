using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewWeather : PreviewInfoType<SelectableLevel>
	{
		public PreviewWeather()
			: base("Weather") { }

		public override string Value(SelectableLevel inputValue)
		{
			string weather = inputValue.currentWeather.ToString();

			if (Plugin.WeatherRegistryCompatibility.IsModPresent)
			{
				weather = Plugin.WeatherRegistryCompatibility.GetWeather(inputValue);
			}

			return weather == "None" ? "" : weather;
		}
	}
}
