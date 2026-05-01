using HarmonyLib;
using MrovLib;

namespace TerminalUtils.Patches
{
	[HarmonyPatch(typeof(Terminal))]
	public static class TerminalStartPatch
	{
		[HarmonyPatch("Start")]
		[HarmonyPostfix]
		public static void Postfix(Terminal __instance)
		{
			StartupManager.Init(__instance);

			if (Plugin.LLLCompatibility.IsModPresent)
			{
				Plugin.LLLCompatibility.RemoveMoonNodeEvent();
			}
		}
	}
}
