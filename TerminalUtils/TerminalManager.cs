using System.Collections.Generic;
using System.Linq;
using MrovLib;
using TerminalUtils.Definitions;
using TerminalUtils.Enums;

namespace TerminalUtils
{
	public static class TerminalManager
	{
		public static Terminal Terminal { get; private set; }

		public static TerminalNode MoonsPage { get; private set; }
		public static TerminalNode StorePage { get; private set; }

		public static void Init(Terminal terminal)
		{
			Terminal = terminal;

			MoonsPage = ContentManager.MoonsKeyword.specialKeywordResult;
			StorePage = ContentManager.Nodes.FirstOrDefault(node => node.name == "0_StoreHub");
		}
	}
}
