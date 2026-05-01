namespace TerminalUtils.Compatibility
{
	public class LethalLevelLoaderCompatibility : MrovLib.CompatibilityHandler
	{
		public LethalLevelLoaderCompatibility(string guid, string version = null)
			: base(guid, version) { }

		public void RemoveMoonNodeEvent()
		{
			if (!this.IsModPresent)
			{
				return;
			}

			LethalLevelLoader.TerminalManager.onLoadNewNodeRegisteredEventsDictionary.Clear();
		}

		public static bool IsLevelLocked(SelectableLevel level)
		{
			return MrovLib.SharedMethods.IsMoonLockedLLL(level);
		}

		public static bool IsLevelHidden(SelectableLevel level)
		{
			return MrovLib.SharedMethods.IsMoonHiddenLLL(level);
		}
	}
}
