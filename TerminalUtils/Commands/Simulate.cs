using System.Collections.Generic;
using System.Linq;
using DunGen.Graph;
using TerminalUtils.Compatibility;
using TerminalUtils.Definitions;

namespace TerminalUtils.Commands
{
	public class SimulateCommand : TerminalCommandNode
	{
		public SimulateCommand()
			: base("simulate") { }

		public override string Execute(string[] args)
		{
			SelectableLevel level = MrovLib.StringResolver.ResolveStringToLevels(args[0]).FirstOrDefault();

			Dictionary<string, int> flowsWithRarity = [];

			Dictionary<string, int> DungeonFlowsFromContentLoader = [];
			if (Plugin.DawnCompatibility.IsModPresent)
			{
				DungeonFlowsFromContentLoader = DawnLibCompatibility.GetDungeonRarities(StartOfRound.Instance.currentLevel);
			}
			else if (Plugin.LLLCompatibility.IsModPresent)
			{
				DungeonFlowsFromContentLoader = LethalLevelLoaderCompatibility.GetDungeonRarities(StartOfRound.Instance.currentLevel);
			}

			var table = new ConsoleTables.ConsoleTable("Interior", "Weight", "Chance");
			table.AddRow("", "", "");

			flowsWithRarity = DungeonFlowsFromContentLoader.OrderBy(o => -o.Value).ToDictionary(k => k.Key, v => v.Value);
			int totalRarityPool = flowsWithRarity.Values.Sum();

			Plugin.debugLogger.LogDebug(
				$"Total rarity pool for level {level.PlanetName}: {totalRarityPool}. Flows with rarity: {string.Join("; ", flowsWithRarity.Select(kv => $"{kv.Key} (rarity: {kv.Value})"))}"
			);

			foreach ((string dungeonName, int dungeonRarity) in flowsWithRarity)
			{
#pragma warning disable IDE0004
				table.AddRow(
					dungeonName.PadRight(20),
					dungeonRarity,
					$"{((float)dungeonRarity / (float)totalRarityPool * 100).ToString("F2")}%".PadLeft(4)
				);
#pragma warning restore IDE0004
			}

			table.AddRow("", "", "");
			table.AddRow("", "", "");
			table.AddRow("", totalRarityPool.ToString().PadRight(6), "100%".ToString().PadLeft(4));

			return table.ToStringCustomDecoration(header: true);
		}
	}
}
