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

			SortCommand sortCommand = new();
			CommandManager.Commands.Add(sortCommand);

			FilterCommand filterCommand = new();
			CommandManager.Commands.Add(filterCommand);

			SimulateCommand simulateCommand = new();
			CommandManager.Commands.Add(simulateCommand);

			StoreSortCommand storeSortCommand = new();
			CommandManager.Commands.Add(storeSortCommand);
		}
	}
}
