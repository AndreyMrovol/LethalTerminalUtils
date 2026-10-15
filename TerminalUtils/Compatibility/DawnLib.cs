using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

// assuming that Dawn won't always be present,
// this code has to avoid using Dawn types and casting to them
// what a fucking mess

namespace TerminalUtils.Compatibility
{
	public class DawnLibCompatibility : MrovLib.CompatibilityHandler
	{
		public DawnLibCompatibility(string guid, string version = null)
			: base(guid, version) { }

		private static Type _lethalContentType;
		private static Type _stringExtensionsType;
		private static Type _spawnWeightContextType;

		private static Type LethalContentType => _lethalContentType ??= AccessTools.TypeByName("Dawn.LethalContent");
		private static Type StringExtensionsType => _stringExtensionsType ??= AccessTools.TypeByName("Dawn.Utils.StringExtensions");
		private static Type SpawnWeightContextType => _spawnWeightContextType ??= AccessTools.TypeByName("Dawn.SpawnWeightContext");

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

		private static object GetStatic(Type type, string member) =>
			Traverse.Create(type).Property(member).GetValue() ?? Traverse.Create(type).Field(member).GetValue();

		private static object Get(object target, string member) =>
			target == null ? null : Traverse.Create(target).Property(member).GetValue() ?? Traverse.Create(target).Field(member).GetValue();

		private static object CallStatic(Type type, string method, params object[] args) =>
			Traverse.Create(type).Method(method, args).GetValue();

		/// Finds the DawnXInfo (the Value of the KeyValuePair) in a LethalContent registry.
		private static object FindInfo(string registryName, string valueMember, object expected)
		{
			if (GetStatic(LethalContentType, registryName) is not IEnumerable registry)
			{
				return null;
			}

			foreach (object pair in registry)
			{
				object value = Get(pair, "Value");
				if (ReferenceEquals(Get(value, valueMember), expected) || Equals(Get(value, valueMember), expected))
				{
					return value;
				}
			}

			return null;
		}

		public static object GetLevelDawnInfo(SelectableLevel level) => FindInfo("Moons", "Level", level);

		public static object GetItemDawnInfo(Item item) => FindInfo("Items", "Item", item);

		public (bool locked, bool hidden) GetLevelStatus(SelectableLevel level)
		{
			if (!this.IsModPresent)
			{
				return (false, false);
			}

			object moonInfo = GetLevelDawnInfo(level);
			object predicate = Get(Get(moonInfo, "DawnPurchaseInfo"), "PurchasePredicate");
			object result = Traverse.Create(predicate).Method("CanPurchase").GetValue();

			return (result?.GetType().Name) switch
			{
				"HiddenPurchaseResult" => ((bool)Get(result, "IsFailure"), true),
				"FailedPurchaseResult" => (true, false),
				// SuccessPurchaseResult or unknown
				_ => (false, false),
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

			object relevantMoonInfo = GetLevelDawnInfo(level);
			if (relevantMoonInfo == null)
			{
				return result;
			}

			var dungeons = (IEnumerable)Traverse.Create(GetStatic(LethalContentType, "Dungeons")).Property("Values").GetValue();

			List<object> possibleDungeons = [];
			List<int> possibleDungeonWeights = [];

			foreach (object dungeonInfo in dungeons)
			{
				// new SpawnWeightContext(moon: relevantMoonInfo, dungeon: null, weather: null)
				object ctx = Activator.CreateInstance(SpawnWeightContextType, relevantMoonInfo, null, null);

				object weights = Get(dungeonInfo, "Weights");
				object rarityObj = Traverse.Create(weights).Method("GetFor", new object[] { relevantMoonInfo, ctx }).GetValue();
				int rarity = rarityObj == null ? 0 : Convert.ToInt32(rarityObj);

				if (rarity > 0)
				{
					possibleDungeons.Add(dungeonInfo);
					possibleDungeonWeights.Add(rarity);
				}
			}

			Plugin.debugLogger.LogDebug(
				$"Found {possibleDungeons.Count} possible dungeons for level {level.PlanetName}: "
					+ string.Join(", ", possibleDungeons.Select(d => Get(d, "Key")))
			);

			for (int i = 0; i < possibleDungeons.Count; i++)
			{
				string dungeonName = (string)Get(Get(possibleDungeons[i], "Key"), "Key");
				dungeonName = (string)CallStatic(StringExtensionsType, "RemoveLeadingNumbers", dungeonName);
				dungeonName = (string)CallStatic(StringExtensionsType, "ToCapitalized", dungeonName);
				dungeonName = ((string)CallStatic(StringExtensionsType, "ReplaceNumbersWithWords", dungeonName)).Replace(" ", "_");

				result[dungeonName] = possibleDungeonWeights[i];
			}

			return result;
		}

		public string GetStoreItemNameOverride(Item item)
		{
			if (item == null)
			{
				return null;
			}

			object info = GetItemDawnInfo(item);
			object shopInfo = Get(info, "ShopInfo");
			if (shopInfo == null)
			{
				return item.itemName;
			}

			object predicate = Get(Get(shopInfo, "DawnPurchaseInfo"), "PurchasePredicate");
			object result = Traverse.Create(predicate).Method("CanPurchase").GetValue();

			if (result?.GetType().Name == "FailedPurchaseResult")
			{
				string overrideName = Get(result, "OverrideName") as string;
				if (overrideName != null)
				{
					Plugin.debugLogger.LogCustom(
						$"Overriding name of {Get(info, "Key")} with {overrideName}",
						BepInEx.Logging.LogLevel.Debug,
						MrovLib.LoggingType.Debug
					);
				}
				return overrideName ?? item.itemName;
			}

			return item.itemName;
		}

		public bool IsItemInStore(Item item)
		{
			if (item == null)
			{
				return false;
			}

			object info = GetItemDawnInfo(item);
			object predicate = Get(Get(Get(info, "ShopInfo"), "DawnPurchaseInfo"), "PurchasePredicate");
			if (predicate == null)
			{
				return false;
			}

			object result = Traverse.Create(predicate).Method("CanPurchase").GetValue();
			return result?.GetType().Name != "HiddenPurchaseResult";
		}
	}
}
