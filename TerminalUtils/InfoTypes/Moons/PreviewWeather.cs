using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewWeather : PreviewInfoType<SelectableLevel>
	{
		public PreviewWeather()
			: base("Weather") { }

		public override string Value(SelectableLevel inputValue)
		{
			return inputValue.currentWeather.ToString() == "None" ? "" : inputValue.currentWeather.ToString();
		}
	}
}
