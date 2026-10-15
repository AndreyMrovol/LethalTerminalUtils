using System.Text.RegularExpressions;

namespace TerminalUtils.Utils
{
	public static class StringUtils
	{
		private static readonly Regex LeadingNumberRegex = new(@"^[0-9]+");

		public static string RemoveLeadingNumbers(this string input)
		{
			return LeadingNumberRegex.Replace(input, string.Empty);
		}
	}
}
