using System.Collections.Generic;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class SortDifficulty : SortInfoType<SelectableLevel>
	{
		public SortDifficulty()
			: base("Difficulty") { }

		public override List<SelectableLevel> Sort(List<SelectableLevel> inputList)
		{
			inputList.Sort((a, b) => a.riskLevel.CompareTo(b.riskLevel));
			return inputList;
		}
	}
}
