using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewWeather : PreviewInfoType<SelectableLevel>
	{
		public PreviewWeather()
			: base("Weather")
		{
			this.MaxLength = Defaults.planetWeatherWidth;
		}

		public override string Value(SelectableLevel inputValue)
		{
			return Utils.GetWeatherName.GetWeather(inputValue);
		}
	}
}
