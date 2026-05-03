using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.Patches
{
	[HarmonyPatch(typeof(Terminal), "ParsePlayerSentence")]
	class TerminalParsePlayerSentencePatch
	{
		[HarmonyPrefix]
		[HarmonyBefore("mrov.WeatherRegistry")]
		public static bool GameMethodPatch(Terminal __instance, ref TerminalNode __result)
		{
			string input = __instance.screenText.text[^__instance.textAdded..]; // what the fuck?
			input = __instance.RemovePunctuation(input);

			List<string> words = input.Split(' ').ToList();

			if (words.Count >= 1)
			{
				// check if first word is a registered command

				if (CommandManager.CommandLookup.TryGetValue(words[0], out TerminalCommandNode commandNode))
				{
					// get the full command and pass it to the manager

					Plugin.debugLogger.LogWarning("Command detected, passing to CommandManager");

					// command arg1 arg2 ... argN
					if (words.Count >= 2)
					{
						string[] arguments = words.Skip(1).ToArray();

						TerminalNode result = CommandManager.RunWeatherCommand(commandNode, arguments);

						__result = result;
						return false;
					}

					return true;
				}
			}

			return true;
		}
	}
}
