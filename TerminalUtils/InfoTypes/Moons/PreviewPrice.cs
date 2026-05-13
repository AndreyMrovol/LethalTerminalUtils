using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewPrice : PreviewInfoType<SelectableLevel>
	{
		public PreviewPrice()
			: base("Price") { }

		public override string Value(SelectableLevel inputValue)
		{
			return $"${ContentManager.RouteDictionary.GetRoute(inputValue).Price}";
		}
	}
}
