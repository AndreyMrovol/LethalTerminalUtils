using HarmonyLib;
using MrovLib;

namespace TerminalUtils.Patches
{
	public static class TerminalLoadNewNodePatch
	{
		[HarmonyPatch("LoadNewNode")]
		[HarmonyPostfix]
		public static void Postfix(Terminal __instance) { }
	}
}
