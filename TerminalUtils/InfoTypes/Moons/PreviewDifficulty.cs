using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewDifficulty : PreviewInfoType<SelectableLevel>
	{
		public PreviewDifficulty()
			: base("Difficulty")
		{
			this.MaxLength = 5;
		}

		public override string Value(SelectableLevel inputValue)
		{
			return inputValue.riskLevel;
		}
	}
}
