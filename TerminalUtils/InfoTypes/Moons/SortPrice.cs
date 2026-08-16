using System.Collections.Generic;
using System.Linq;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class SortPrice : SortInfoType<SelectableLevel>
	{
		public SortPrice()
			: base("Price") { }

		public override List<SelectableLevel> Sort(List<SelectableLevel> inputList)
		{
			return inputList.OrderBy(level => LevelHelper.VanillaOrder).ThenBy(a => ContentManager.RouteDictionary.GetRoute(a).Price).ToList();
		}
	}
}
