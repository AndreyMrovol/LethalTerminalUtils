namespace TerminalUtils
{
	public static class Defaults
	{
		public static readonly int terminalWidth = 48;

		internal static readonly int planetWeatherWidth = 18;
		internal static readonly int planetNameWidth = terminalWidth + 2 - planetWeatherWidth - 9;

		internal static readonly int itemNameWidth = terminalWidth - 9 - 10;

		internal static readonly int dividerLength = 17;

		internal static readonly string defaultPreviewType = "Name;Price;Weather";
		internal static readonly string defaultFilterType = "None";
		internal static readonly string defaultSortType = "None";

		internal static readonly string defaultStoreSortType = "Name";
	}
}
