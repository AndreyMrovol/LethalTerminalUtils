using System.Collections.Generic;
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
			inputList.Sort(
				(a, b) => ContentManager.RouteDictionary.GetRoute(a).Price.CompareTo(ContentManager.RouteDictionary.GetRoute(b).Price)
			);
			return inputList;
		}
	}
}
