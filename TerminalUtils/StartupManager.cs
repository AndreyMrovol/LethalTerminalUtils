using TerminalUtils.Commands;

namespace TerminalUtils
{
	public static class StartupManager
	{
		public static void Init(Terminal _instance)
		{
			TerminalManager.Init(_instance);

			CommandManager.Init();

			PreviewCommand previewCommand = new();
			CommandManager.Commands.Add(previewCommand);
		}
	}
}
