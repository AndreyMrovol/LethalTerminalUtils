using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.DisplayTypes
{
	public class PreviewName : PreviewInfoType<SelectableLevel>
	{
		public PreviewName()
			: base("Name")
		{
			this.MaxLength = LevelHelper.LongestPlanetName.Length;
		}

		public override string Value(SelectableLevel inputValue)
		{
			return MrovLib.StringResolver.GetAlphanumericName(inputValue);
		}
	}
}
