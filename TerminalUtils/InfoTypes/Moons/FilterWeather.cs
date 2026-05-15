using System.Collections.Generic;
using System.Linq;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class FilterWeather : FilterInfoType<SelectableLevel>
	{
		public FilterWeather()
			: base("Weather") { }

		public override List<SelectableLevel> Filter(List<SelectableLevel> inputList)
		{
			return inputList.Where(lvl => ContentManager.RouteDictionary.GetRoute(lvl).Price >= ContentManager.Terminal.groupCredits).ToList();
		}
	}
}
