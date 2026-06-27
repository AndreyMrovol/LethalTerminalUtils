using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using TerminalUtils.Compatibility;

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

		///																		    just change your fucking properties in .csproj	VVV 	debugging this took me 2 hours
		internal static LethalMoonUnlocksCompatibility LMUCompatibility = new(LethalMoonUnlocks.PluginMetadata.PLUGIN_GUID);
		internal static LethalConstellationsCompatibility LCCompatibility = new(LethalConstellations.Plugin.PluginInfo.PLUGIN_GUID);
		internal static LategameUpgradesCompatibility LGUCompat = new(MoreShipUpgrades.PluginInfo.PLUGIN_GUID);

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
