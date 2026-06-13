using System.Collections.Generic;
using TerminalUtils.Definitions;

namespace TerminalUtils
{
	public static class NodeReplacementManager
	{
		internal static List<TerminalNodeReplacement> RegisteredNodes = [];

		public static bool ReplaceNode = true;
	}
}
