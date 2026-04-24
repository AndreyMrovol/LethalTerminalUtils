using TerminalUtils.Definitions;

namespace TerminalUtils.DisplayTypes
{
	public class PreviewName : PreviewInfoType<SelectableLevel>
	{
		public PreviewName()
			: base("Name") { }

		public override string Value(SelectableLevel inputValue)
		{
			return MrovLib.StringResolver.GetAlphanumericName(inputValue);
		}
	}
}
