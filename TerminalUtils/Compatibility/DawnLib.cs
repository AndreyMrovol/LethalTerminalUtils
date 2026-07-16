using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Dawn;
using HarmonyLib;

namespace TerminalUtils.Compatibility
{
	public class DawnLibCompatibility : MrovLib.CompatibilityHandler
	{
		public DawnLibCompatibility(string guid, string version = null)
			: base(guid, version) { }

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
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

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public object GetLevelDawnInfo(SelectableLevel level)
		{
			return level.GetDawnInfo();
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public (bool locked, bool hidden) GetLevelStatus(SelectableLevel level)
		{
			if (!this.IsModPresent)
			{
				return (false, false);
			}

			return level.GetDawnInfo().DawnPurchaseInfo.PurchasePredicate.CanPurchase() switch
			{
				Dawn.TerminalPurchaseResult.HiddenPurchaseResult hiddenResult => (hiddenResult.IsFailure, true),
				Dawn.TerminalPurchaseResult.FailedPurchaseResult => (true, false),
				Dawn.TerminalPurchaseResult.SuccessPurchaseResult => (false, false),
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

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
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
					Dawn.Internal.TimeOfDayRefs.GetCurrentWeatherEffect(relevantMoonInfo.Level)?.GetDawnInfo()
				).WithExtra(SpawnWeightExtraKeys.RoutingPriceKey, relevantMoonInfo.DawnPurchaseInfo.Cost.Provide());

				int rarity = dungeonInfo.Weights.GetFor(ctx) ?? 0;
				if (rarity > 0)
				{
					possibleDungeons.Add(dungeonInfo);
					possibleDungeonWeights.Add(rarity);
				}
			}

			for (int i = 0; i < possibleDungeons.Count; i++)
			{
				string dungeonName = possibleDungeons[i].DungeonFlow.name;
				int weight = (int)possibleDungeonWeights[i];

				result[dungeonName] = weight;
			}

			return result;
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public string GetStoreItemNameOverride(Item item)
		{
			if (item == null)
			{
				return null;
			}

			DawnItemInfo info = item.GetDawnInfo();
			DawnShopItemInfo shopInfo = info.ShopInfo;
			if (shopInfo == null)
			{
				return item.itemName;
			}

			TerminalPurchaseResult result = shopInfo.DawnPurchaseInfo.PurchasePredicate.CanPurchase();
			if (result is TerminalPurchaseResult.FailedPurchaseResult failedResult)
			{
				if (failedResult.OverrideName != null)
				{
					Plugin.debugLogger.LogCustom(
						$"Overriding name of {info.Key} with {failedResult.OverrideName}",
						BepInEx.Logging.LogLevel.Debug,
						MrovLib.LoggingType.Debug
					);
				}
				return failedResult.OverrideName ?? item.itemName;
			}

			return item.itemName;
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public bool IsItemInStore(Item item)
		{
			if (item == null)
			{
				return false;
			}

			DawnItemInfo info = item.GetDawnInfo();
			ITerminalPurchasePredicate shopInfo = info.ShopInfo.DawnPurchaseInfo.PurchasePredicate;
			if (shopInfo == null)
			{
				return false;
			}

			return shopInfo.CanPurchase() is not TerminalPurchaseResult.HiddenPurchaseResult;
		}
	}
}
