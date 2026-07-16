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

		public static Dictionary<SelectableLevel, int> newIndex = [];

		public override void Init()
		{
			Plugin.logger.LogInfo("Initializing LunarConfig compatibility...");

			Plugin.harmony.Patch(
				AccessTools.Constructor(typeof(LunarConfig.Objects.Config.LunarConfigCustomMoonOrder), [typeof(Dictionary<DawnMoonInfo, int>)]),
				transpiler: new HarmonyMethod(AccessTools.Method(typeof(LunarConfigCompatibility), nameof(LunarConfig_GetOrderingAlgorithm)))
			);
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

			// Find ctor call and insert a call right before it:
			// stack: ... dict -> PassNewCatalogueIndex(dict) -> dict -> new LunarConfigCustomMoonOrder(dict)
			matcher.MatchForward(false, new CodeMatch(ci => ci.opcode == OpCodes.Newobj && Equals(ci.operand, lunarCustomOrderCtor)));

			if (matcher.IsValid)
			{
				matcher.Insert(new CodeInstruction(OpCodes.Call, passMethod));
			}

			return matcher.InstructionEnumeration();
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private static void PassNewCatalogueIndex(Dictionary<object, int> newCatalogueIndex)
		{
			newIndex.Clear();

			MrovLib.LevelHelper.Levels.ForEach(moon =>
			{
				Dawn.DawnMoonInfo moonInfo = moon.GetDawnInfo();

				int index = newCatalogueIndex.ContainsKey(moonInfo) ? newCatalogueIndex[moonInfo] : 0;
				newIndex.Add(moon, index);
			});
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public int GetMoonIndex(SelectableLevel moon)
		{
			return newIndex.ContainsKey(moon) ? newIndex[moon] : 0;
		}
	}
}
