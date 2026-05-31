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

			if (Plugin.DawnCompatibility.IsModPresent)
			{
				flowsWithRarity = DawnLibCompatibility.GetDungeonRarities(StartOfRound.Instance.currentLevel);
			}
			else if (Plugin.LLLCompatibility.IsModPresent)
			{
				flowsWithRarity = LethalLevelLoaderCompatibility.GetDungeonRarities(StartOfRound.Instance.currentLevel);
			}

			var table = new ConsoleTables.ConsoleTable("Interior", "Weight", "Chance");
			table.AddRow("", "", "");

			flowsWithRarity = flowsWithRarity.OrderBy(o => -o.Value).ToDictionary(k => k.Key, v => v.Value);
			int totalRarityPool = flowsWithRarity.Values.Sum();

			foreach ((string dungeonName, int dungeonRarity) in flowsWithRarity)
			{
				table.AddRow(dungeonName.PadRight(20), dungeonRarity, $"{(dungeonRarity / totalRarityPool * 100).ToString("F2")}%".PadLeft(4));
			}

			table.AddRow("", "", "");
			table.AddRow("", "", "");
			table.AddRow("", totalRarityPool.ToString().PadRight(6), "100%".ToString().PadLeft(4));

			Plugin.debugLogger.LogDebug(
				$"Flows with rarity for level {level.PlanetName}: {string.Join(", ", flowsWithRarity.Select(kv => $"{kv.Key} (rarity: {kv.Value})"))}"
			);

			return table.ToStringCustomDecoration(header: true);
		}
	}
}
