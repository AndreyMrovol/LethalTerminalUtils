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
			return Plugin.LMUCompatibility.MoonUnlockables.TryGetValue(inputValue, out LMUnlockable unlockable)
				? unlockable.BuildTagString()
				: null;
		}
	}
}
