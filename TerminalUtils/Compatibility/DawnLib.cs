using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Dawn;
using Dawn.Internal;
using Dawn.Utils;
using HarmonyLib;
using UnityEngine;

namespace TerminalUtils.Compatibility
{
	public class DawnLibCompatibility : MrovLib.CompatibilityHandler
	{
		public DawnLibCompatibility(string guid, string version = null)
			: base(guid, version) { }

		public override void Init()
		{
			if (!this.IsModPresent)
			{
				return;
			}

			Type dawnTerminalType = AccessTools.TypeByName("Dawn.MoonRegistrationHandler");
			Plugin.harmony.Patch(
				AccessTools.Method(dawnTerminalType, "DynamicMoonCatalogue"),
				transpiler: new HarmonyMethod(AccessTools.Method(typeof(DawnLibCompatibility), nameof(InsertMoonCatalogueSkip)))
			);
		}

		public (bool locked, bool hidden) GetLevelStatus(SelectableLevel level)
		{
			return level.GetDawnInfo().DawnPurchaseInfo.PurchasePredicate.CanPurchase() switch
			{
				TerminalPurchaseResult.HiddenPurchaseResult hiddenResult => (hiddenResult.IsFailure, true),
				TerminalPurchaseResult.FailedPurchaseResult => (true, false),
				TerminalPurchaseResult.SuccessPurchaseResult => (false, false),
				_ => (false, false)
			};
		}

		public static IEnumerable<CodeInstruction> InsertMoonCatalogueSkip(IEnumerable<CodeInstruction> instructions)
		{
			var matcher = new CodeMatcher(instructions);

			matcher
				.Start()
				.Insert(
					new CodeInstruction(OpCodes.Ldarg_0), // self (orig delegate)
					new CodeInstruction(OpCodes.Ldarg_1), // self (Terminal instance)
					new CodeInstruction(OpCodes.Ldarg_2), // modifieddisplaytext
					new CodeInstruction(OpCodes.Ldarg_3), // node
					new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(On.Terminal.orig_TextPostProcess), "Invoke")),
					new CodeInstruction(OpCodes.Ret)
				);

			return matcher.InstructionEnumeration();
		}

		public static Dictionary<string, int> GetDungeonRarities(SelectableLevel level)
		{
			Dictionary<string, int> result = [];

			DawnMoonInfo relevantMoonInfo = level.GetDawnInfo();

			List<DawnDungeonInfo> possibleDungeons = [];
			List<float> possibleDungeonWeights = [];

			foreach (DawnDungeonInfo dungeonInfo in LethalContent.Dungeons.Values)
			{
				SpawnWeightContext ctx = new SpawnWeightContext(
					relevantMoonInfo,
					null,
					TimeOfDayRefs.GetCurrentWeatherEffect(relevantMoonInfo.Level)?.GetDawnInfo()
				).WithExtra(SpawnWeightExtraKeys.RoutingPriceKey, relevantMoonInfo.DawnPurchaseInfo.Cost.Provide());

				int rarity = dungeonInfo.Weights.GetFor(ctx) ?? 0;
				if (rarity > 0)
				{
					possibleDungeons.Add(dungeonInfo);
					possibleDungeonWeights.Add(rarity);
				}
			}

			// sort by rarity
			for (int i = 0; i < possibleDungeons.Count; i++)
			{
				for (int j = i + 1; j < possibleDungeons.Count; j++)
				{
					if (possibleDungeonWeights[i] < possibleDungeonWeights[j])
					{
						(possibleDungeons[i], possibleDungeons[j]) = (possibleDungeons[j], possibleDungeons[i]);
						(possibleDungeonWeights[i], possibleDungeonWeights[j]) = (possibleDungeonWeights[j], possibleDungeonWeights[i]);
					}
				}
			}

			float sumOfWeights = possibleDungeonWeights.Sum();
			for (int i = 0; i < possibleDungeons.Count; i++)
			{
				string dungeonName = possibleDungeons[i].DungeonFlow.name;

				StringBuilder builder = new();
				int paddingNeeded = Mathf.Max(20 - dungeonName.Length, 0);
				builder.Append($"* {dungeonName}{new string(' ', paddingNeeded)}");

				int weight = (int)possibleDungeonWeights[i];

				result[dungeonName] = weight;
			}

			return result;
		}
	}
}
