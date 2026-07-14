using System.Collections.Generic;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class SortLunar : SortInfoType<SelectableLevel>
	{
		public SortLunar()
			: base("Lunar") { }

		public override List<SelectableLevel> Sort(List<SelectableLevel> inputList)
		{
			// highest to lowest
			inputList.Sort((a, b) => Plugin.LunarConfigCompat.GetMoonIndex(b).CompareTo(Plugin.LunarConfigCompat.GetMoonIndex(a)));
			return inputList;
		}
	}
}
