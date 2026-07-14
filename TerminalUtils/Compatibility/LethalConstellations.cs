using System.Collections.Generic;
using System.Linq;

namespace TerminalUtils.Compatibility
{
	public class LethalConstellationsCompatibility : MrovLib.CompatibilityHandler
	{
		public LethalConstellationsCompatibility(string guid, string version = null)
			: base(guid, version) { }

		public Dictionary<SelectableLevel, string> Constellations { get; private set; } = [];

		public override void Init()
		{
			if (!this.IsModPresent)
			{
				return;
			}

			MrovLib.EventManager.ContentManagerReady.AddListener(GetConstellations);
		}

		public void GetConstellations()
		{
			if (!IsModPresent)
			{
				return;
			}

			if (!Plugin.LLLCompatibility.IsModPresent)
			{
				return;
			}

			Dictionary<SelectableLevel, string> constellationsDict = [];

			List<LethalConstellations.PluginCore.ClassMapper> constellations = LethalConstellations.PluginCore.Collections.ConstellationStuff;

			foreach (var constellation in constellations)
			{
				string constellationName = constellation.consName;
				List<SelectableLevel> levels = constellation
					.constelMoons.Select(moonName => MrovLib.StringResolver.ResolveStringToLevels(moonName).FirstOrDefault())
					.Where(level => level != null)
					.ToList();

				levels.ForEach(level => constellationsDict[level] = constellationName);
			}

			Constellations = constellationsDict;
		}

		public string GetConstellationName(SelectableLevel level)
		{
			if (!IsModPresent)
			{
				return null;
			}

			Constellations.TryGetValue(level, out string constellationName);
			return constellationName;
		}
	}
}
