using System.Collections.Generic;
using MrovLib;
using MrovLib.ContentType;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Store
{
	public class SortName : SortInfoType<BuyableThing>
	{
		public SortName()
			: base("Name") { }

		public override List<BuyableThing> Sort(List<BuyableThing> inputList)
		{
			inputList.Sort((a, b) => a.Name.CompareTo(b.Name));
			return inputList;
		}
	}
}
