using System.Collections.Generic;
using MrovLib;
using MrovLib.ContentType;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Store
{
	public class SortPrice : SortInfoType<BuyableThing>
	{
		public SortPrice()
			: base("Price") { }

		public override List<BuyableThing> Sort(List<BuyableThing> inputList)
		{
			inputList.Sort(
				(a, b) => a.Price.CompareTo(b.Price)
			);
			return inputList;
		}
	}
}
