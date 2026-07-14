using System.Collections.Generic;
using System.Linq;
using Dawn;

namespace TerminalUtils.Compatibility
{
	public class LunarConfigCompatibility : MrovLib.CompatibilityHandler
	{
		public LunarConfigCompatibility(string guid, string version = null)
			: base(guid, version) { }

		public Dictionary<SelectableLevel, int> newIndex = [];

		private void GetMoonIndexes()
		{
			if (LunarConfig.Objects.Config.LunarCentral.enabledMoonSettings.Contains("Catalogue Index"))
			{
				LunarConfig.Objects.Config.LunarConfigCustomMoonOrder ordering = (LunarConfig.Objects.Config.LunarConfigCustomMoonOrder)
					Dawn.MoonRegistrationHandler.MoonGroupAlgorithm.OrderingSteps.First(step =>
						step is LunarConfig.Objects.Config.LunarConfigCustomMoonOrder
					);

				MrovLib.LevelHelper.Levels.ForEach(moon =>
				{
					Dawn.DawnMoonInfo moonInfo = moon.GetDawnInfo();

					int index = ordering.GetIndex(moonInfo);
					newIndex.Add(moon, index);
				});
			}
		}

		public int GetMoonIndex(SelectableLevel moon)
		{
			return newIndex.ContainsKey(moon) ? newIndex[moon] : 0;
		}
	}
}
