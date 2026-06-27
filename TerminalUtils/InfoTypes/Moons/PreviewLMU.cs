using LethalMoonUnlocks;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewLMU : PreviewInfoType<SelectableLevel>
	{
		public PreviewLMU()
			: base("LMU") { }

		public override string Value(SelectableLevel inputValue)
		{
			object unlockable = Plugin.LMUCompatibility.MoonUnlockables.TryGetValue(inputValue, out unlockable) ? unlockable : null;
			LMUnlockable lmu = unlockable as LMUnlockable;

			return lmu?.BuildTagString();
		}
	}
}
