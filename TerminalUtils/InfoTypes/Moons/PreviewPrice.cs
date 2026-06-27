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
			if (Plugin.LGUCompat.IsModPresent)
			{
				return $"${Plugin.LGUCompat.GetMoonPrice(ContentManager.RouteDictionary.GetRoute(inputValue).Price)}";
			}

			return $"${ContentManager.RouteDictionary.GetRoute(inputValue).Price}";
		}
	}
}
