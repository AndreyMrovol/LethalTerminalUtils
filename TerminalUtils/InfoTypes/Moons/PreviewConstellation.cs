using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewConstellation : PreviewInfoType<SelectableLevel>
	{
		public PreviewConstellation()
			: base("Constellation") { }

		public override string Value(SelectableLevel inputValue)
		{
			return Plugin.LCCompatibility.GetConstellationName(inputValue);
		}
	}
}
