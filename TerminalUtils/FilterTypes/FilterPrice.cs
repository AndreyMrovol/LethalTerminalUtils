using System.Collections.Generic;
using System.Linq;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.DisplayTypes
{
	public class FilterPrice : FilterInfoType<SelectableLevel>
	{
		public FilterPrice()
			: base("Price") { }

		public override List<SelectableLevel> Filter(List<SelectableLevel> inputList)
		{
			return inputList.Where(lvl => ContentManager.RouteDictionary.GetRoute(lvl).Price >= ContentManager.Terminal.groupCredits).ToList();
		}
	}
}
