using System.Collections.Generic;
using MrovLib;
using MrovLib.ContentType;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Store
{
	public class SortNone : SortInfoType<BuyableThing>
	{
		public SortNone()
			: base("None") { }

		public override List<BuyableThing> Sort(List<BuyableThing> inputList)
		{
			return inputList;
		}
	}
}
