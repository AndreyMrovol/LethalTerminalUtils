using TerminalUtils.Definitions;

namespace TerminalUtils.DisplayTypes
{
	public class PreviewWeather : PreviewInfoType<SelectableLevel>
	{
		public PreviewWeather()
			: base("Weather") { }

		public override string Value(SelectableLevel inputValue)
		{
			return inputValue.currentWeather.ToString();
		}
	}
}
