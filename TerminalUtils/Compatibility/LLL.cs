using System.Collections.Generic;
using System.Linq;
using DunGen.Graph;
using HarmonyLib;
using LethalLevelLoader;

namespace TerminalUtils.Compatibility
{
	public class LethalLevelLoaderCompatibility : MrovLib.CompatibilityHandler
	{
		public LethalLevelLoaderCompatibility(string guid, string version = null)
			: base(guid, version) { }

		public void RemoveMoonNodeEvent()
		{
			if (!this.IsModPresent)
			{
				return;
			}

			LethalLevelLoader.TerminalManager.onLoadNewNodeRegisteredEventsDictionary.Clear();
		}

		public static bool IsLevelLocked(SelectableLevel level)
		{
			return MrovLib.SharedMethods.IsMoonLockedLLL(level);
		}

		public static bool IsLevelHidden(SelectableLevel level)
		{
			return MrovLib.SharedMethods.IsMoonHiddenLLL(level);
		}

		public static List<KeyValuePair<DungeonFlow, int>> GetDungeonFlowsWithRarity(SelectableLevel level)
		{
			List<KeyValuePair<DungeonFlow, int>> result = [];

			LethalLevelLoader.LevelManager.TryGetExtendedLevel(level, out var extendedLevel);

			LethalLevelLoader
				.DungeonManager.GetValidExtendedDungeonFlows(extendedLevel, false)
				.Select(flow => new KeyValuePair<DungeonFlow, int>(flow.extendedDungeonFlow.DungeonFlow, flow.rarity))
				.Do(result.Add);

			return result;
		}

		public static List<ExtendedDungeonFlowWithRarity> GetExtendedDungeonFlowsWithRarity(ExtendedLevel extendedLevel)
		{
			List<ExtendedDungeonFlowWithRarity> result = [];
			LethalLevelLoader.DungeonManager.GetValidExtendedDungeonFlows(extendedLevel, false).Do(result.Add);

			return result;
		}

		public static Dictionary<string, int> GetDungeonRarities(SelectableLevel level)
		{
			Dictionary<string, int> result = [];

			LethalLevelLoader.LevelManager.TryGetExtendedLevel(level, out var extendedLevel);
			GetExtendedDungeonFlowsWithRarity(extendedLevel).Do(flow => result[flow.extendedDungeonFlow.DungeonFlow.name] = flow.rarity);

			return result;
		}
	}
}
