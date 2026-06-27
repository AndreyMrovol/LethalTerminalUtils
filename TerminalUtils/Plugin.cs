using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TerminalUtils.Compatibility;

// TODO: https://discord.com/channels/1168655651455639582/1387434268577370324/1470399921952788623

namespace TerminalUtils
{
	[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
	[BepInDependency("MrovLib", BepInDependency.DependencyFlags.HardDependency)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static Logger debugLogger = new("Debug", MrovLib.LoggingType.Developer);

		internal static Harmony harmony = new(MyPluginInfo.PLUGIN_GUID);

		internal static LethalLevelLoaderCompatibility LLLCompatibility = new(LethalLevelLoader.Plugin.ModGUID);
		internal static DawnLibCompatibility DawnCompatibility = new(Dawn.DawnLib.PLUGIN_GUID);
		internal static LethalConstellationsCompatibility LCCompatibility = new(LethalConstellations.Plugin.PluginInfo.PLUGIN_GUID);

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			ConfigManager.Init(Config);

			// Plugin startup logic
			Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
