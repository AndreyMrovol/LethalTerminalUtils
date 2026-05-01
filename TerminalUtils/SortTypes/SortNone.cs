using System.Collections.Generic;
using TerminalUtils.Definitions;

namespace TerminalUtils.SortTypes
{
	public class SortNone : SortInfoType<SelectableLevel>
	{
		public SortNone()
			: base("None") { }

		public override List<SelectableLevel> Sort(List<SelectableLevel> inputList)
		{
			return inputList;
		}
	}
}
