using System.Collections.Generic;
using System.Linq;
using LethalMoonUnlocks;

namespace TerminalUtils.Compatibility
{
	public class LethalMoonUnlocksCompatibility : MrovLib.CompatibilityHandler
	{
		public LethalMoonUnlocksCompatibility(string guid, string version = null)
			: base(guid, version) { }

		public Dictionary<SelectableLevel, LMUnlockable> MoonUnlockables { get; private set; } = [];

		public override void Init()
		{
			if (!this.IsModPresent)
			{
				return;
			}

			MrovLib.EventManager.ContentManagerReady.AddListener(PopulateDictionary);
		}

		public void PopulateDictionary()
		{
			if (!IsModPresent)
			{
				return;
			}

			List<LMUnlockable> unlockables = LethalMoonUnlocks.UnlockManager.Instance.Unlocks;

			Dictionary<SelectableLevel, LMUnlockable> unlockablesDict = [];

			foreach (var unlockable in unlockables)
			{
				SelectableLevel level = unlockable.ExtendedLevel.SelectableLevel;

				if (level != null)
				{
					unlockablesDict[level] = unlockable;
				}
			}

			MoonUnlockables = unlockablesDict;
		}
	}
}
