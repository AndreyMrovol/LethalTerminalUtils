using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunGen.Graph;
using MrovLib;
using TerminalUtils.Compatibility;
using TerminalUtils.Definitions;

namespace TerminalUtils.Commands
{
	public class SimulateCommand : TerminalCommandNode
	{
		public SimulateCommand()
			: base("simulate") { }

		private static Dictionary<string, int> LLLResult = [];
		private static Dictionary<string, int> DawnResult = [];

		public override string Execute(string[] args)
		{
			SelectableLevel level = MrovLib.StringResolver.ResolveStringToLevels(args[0]).FirstOrDefault();

			if (level == null)
			{
				return $"Level \"{args[0]}\" not found!";
			}
			else
			{
				Plugin.logger.LogInfo($"Simulating level: {level.PlanetName}");
			}

			if (LevelHelper.CompanyMoons.Contains(level) || !level.spawnEnemiesAndScrap)
			{
				return $"{level.PlanetName} cannot generate interior!";
			}

			Dictionary<string, int> DungeonFlowsFromContentLoader = [];

			if (Plugin.DawnCompatibility.IsModPresent)
			{
				DungeonFlowsFromContentLoader = DawnLibCompatibility.GetDungeonRarities(level);
			}
			else if (Plugin.LLLCompatibility.IsModPresent)
			{
				DungeonFlowsFromContentLoader = LethalLevelLoaderCompatibility.GetDungeonRarities(level);
			}

			if (Plugin.DawnCompatibility.IsModPresent)
			{
				DawnResult = DawnLibCompatibility.GetDungeonRarities(level);
			}

			if (Plugin.LLLCompatibility.IsModPresent)
			{
				LLLResult = LethalLevelLoaderCompatibility.GetDungeonRarities(level);
			}

			StringBuilder output = new();
			output.AppendLine($"Simulating dungeons on moon: {LevelHelper.GetAlphanumericName(level)} \n\n");

			var table = new ConsoleTables.ConsoleTable("Interior", "Weight", "Chance");
			table.AddRow("", "", "");

			Dictionary<string, int> flowsWithRarity = DungeonFlowsFromContentLoader
				.OrderBy(o => -o.Value)
				.ToDictionary(k => k.Key, v => v.Value);
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

			output.AppendLine(table.ToStringCustomDecoration(header: true));

			return output.ToString();
		}
	}
}
