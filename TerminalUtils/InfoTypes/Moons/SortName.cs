using System.Collections.Generic;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class SortName : SortInfoType<SelectableLevel>
	{
		public SortName()
			: base("Name") { }

		public override List<SelectableLevel> Sort(List<SelectableLevel> inputList)
		{
			inputList.Sort((a, b) => MrovLib.StringResolver.GetAlphanumericName(a).CompareTo(MrovLib.StringResolver.GetAlphanumericName(b)));
			return inputList;
		}
	}
}
