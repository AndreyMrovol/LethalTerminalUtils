using System.Collections.Generic;
using System.Linq;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class FilterNone : FilterInfoType<SelectableLevel>
	{
		public FilterNone()
			: base("None") { }

		public override List<SelectableLevel> Filter(List<SelectableLevel> inputList)
		{
			return inputList;
		}
	}
}
