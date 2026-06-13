using System.Text;
using HarmonyLib;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.Patches
{
	[HarmonyPatch(typeof(Terminal))]
	public static class TerminalLoadNewNodePatch
	{
		[HarmonyPrefix]
		[HarmonyPatch("LoadNewNode")]
		// [HarmonyPriority(Priority.Last)]
		[HarmonyAfter("imabatby.lethallevelloader", "com.github.teamxiaolan.dawnlib", "mrov.TerminalFormatter")]
		public static bool PatchMethod(Terminal __instance, TerminalNode node)
		{
			if (!NodeReplacementManager.ReplaceNode)
			{
				return true;
			}

			if (TerminalManager.NodeReplacements.ContainsKey(node))
			{
				__instance.modifyingText = true;
				__instance.screenText.interactable = true;

				TerminalNodeReplacement replacement = TerminalManager.NodeReplacements[node];

				Plugin.logger.LogInfo($"Replacing node {__instance.currentNode.name} with {replacement.Name}");

				StringBuilder builder = new();

				if (__instance.displayingPersistentImage)
				{
					builder.Append("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
				}

				builder.Append("\n\n");
				builder.Append(replacement.GetNodeText());
				builder.Append($"\n{new string('-', 17)}\n");

				__instance.LoadTerminalImage(node);
				__instance.currentNode = node;

				__instance.screenText.text = builder.ToString();
				__instance.currentText = builder.ToString();

				__instance.textAdded = 0;

				return false;
			}

			return true;
		}

		[HarmonyPostfix]
		[HarmonyPatch("LoadNewNode")]
		public static void PatchMethod()
		{
			NodeReplacementManager.ReplaceNode = true;
		}
	}
}
