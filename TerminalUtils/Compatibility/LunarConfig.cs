using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Dawn;
using HarmonyLib;

namespace TerminalUtils.Compatibility
{
	public class LunarConfigCompatibility : MrovLib.CompatibilityHandler
	{
		public LunarConfigCompatibility(string guid, string version = null)
			: base(guid, version) { }

		private static Dictionary<object, int> rawIndex = [];
		private static Dictionary<SelectableLevel, int> newIndex = [];

		public override void Init()
		{
			Plugin.logger.LogDebug("Initializing LunarConfig compatibility...");

			Plugin.harmony.Patch(
				AccessTools.Method(typeof(LunarConfig.Objects.Config.LunarCentral), nameof(LunarConfig.Objects.Config.LunarCentral.InitMoons)),
				transpiler: new HarmonyMethod(AccessTools.Method(typeof(LunarConfigCompatibility), nameof(LunarConfig_GetOrderingAlgorithm)))
			);

			MrovLib.EventManager.ContentManagerReady.AddListener(ProcessNewOrder);
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public static IEnumerable<CodeInstruction> LunarConfig_GetOrderingAlgorithm(IEnumerable<CodeInstruction> instructions)
		{
			CodeMatcher matcher = new(instructions);

			var lunarCustomOrderCtor = AccessTools.Constructor(
				typeof(LunarConfig.Objects.Config.LunarConfigCustomMoonOrder),
				[typeof(Dictionary<DawnMoonInfo, int>)]
			);

			var passMethod = AccessTools.Method(typeof(LunarConfigCompatibility), nameof(PassNewCatalogueIndex));

			matcher.MatchForward(false, new CodeMatch(ci => ci.opcode == OpCodes.Newobj && Equals(ci.operand, lunarCustomOrderCtor)));

			if (matcher.IsValid)
			{
				Plugin.debugLogger.LogDebug("Found the target instruction for LunarConfig_GetOrderingAlgorithm transpiler.");
				matcher.Insert(new CodeInstruction(OpCodes.Call, passMethod));
			}
			else
			{
				Plugin.debugLogger.LogDebug("Failed to find the target instruction for LunarConfig_GetOrderingAlgorithm transpiler.");
			}

			return matcher.InstructionEnumeration();
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private static Dictionary<DawnMoonInfo, int> PassNewCatalogueIndex(Dictionary<DawnMoonInfo, int> newCatalogueIndex)
		{
			if (newCatalogueIndex == null)
			{
				return null;
			}

			newIndex.Clear();
			rawIndex.Clear();

			foreach (var kvp in newCatalogueIndex)
			{
				rawIndex[kvp.Key] = kvp.Value;
			}

			return newCatalogueIndex;
		}

		private static void ProcessNewOrder()
		{
			if (rawIndex == null || rawIndex.Count == 0)
			{
				return;
			}

			var levels = MrovLib.LevelHelper.Levels;

			foreach (var moon in levels)
			{
				if (moon == null)
				{
					continue;
				}

				DawnMoonInfo moonInfo;
				try
				{
					moonInfo = moon.GetDawnInfo();
				}
				catch
				{
					continue;
				}

				if (moonInfo == null)
				{
					continue;
				}

				newIndex[moon] = rawIndex.TryGetValue(moonInfo, out var value) ? value : 0;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public int GetMoonIndex(SelectableLevel moon)
		{
			return newIndex != null && moon != null && newIndex.TryGetValue(moon, out var value) ? value : 0;
		}
	}
}
