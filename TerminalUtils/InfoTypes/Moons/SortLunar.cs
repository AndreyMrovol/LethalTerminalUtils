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
			inputList.Sort((a, b) => Plugin.LunarConfigCompat.GetMoonIndex(a).CompareTo(Plugin.LunarConfigCompat.GetMoonIndex(b)));
			return inputList;
		}
	}
}
