using System.Collections.Generic;

namespace TerminalUtils.Definitions
{
	public abstract class TerminalCommandNode(string Name) : MrovLib.Definitions.CommandNode(Name)
	{
		public new List<TerminalCommandNode> Subcommands { get; set; } = [];

		public bool HostOnly { get; set; } = false;
		public int TerminalSound { get; set; } = -1;

		public TerminalNode RedirectToNode { get; set; } = null;

		public virtual bool ShouldRun()
		{
			return (HostOnly && !StartOfRound.Instance.IsHost) ? false : true;
		}

		public virtual string Execute(string[] args)
		{
			return "";
		}
	}
}
