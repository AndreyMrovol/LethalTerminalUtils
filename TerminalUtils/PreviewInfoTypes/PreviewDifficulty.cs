using TerminalUtils.Definitions;

namespace TerminalUtils.DisplayTypes
{
	public class PreviewDifficulty : PreviewInfoType<SelectableLevel>
	{
		public PreviewDifficulty()
			: base("Difficulty") { }

		public override string Value(SelectableLevel inputValue)
		{
			return inputValue.riskLevel;
		}
	}
}
