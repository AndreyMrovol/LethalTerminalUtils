using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;

namespace TerminalUtils.Definitions
{
	public abstract class TerminalNodeReplacement
	{
		public string Name { get; set; }
		public string HelpText { get; set; } = null;
		public TerminalNode NodeToMatch { get; set; }
		public ConfigEntry<bool> Enabled { get; set; }

		public StringBuilder stringBuilder = new();

		public virtual bool IsNodeValid(TerminalNode node, Terminal terminal)
		{
			return true;
		}

		public abstract string GetNodeText();

		// constructor
		public TerminalNodeReplacement(string name, TerminalNode NodeToMatch)
		{
			this.Name = name;
			this.NodeToMatch = NodeToMatch;
			this.Enabled = ConfigManager.configFile.Bind("Nodes", name, true, $"Enable node {name}");

			NodeReplacementManager.RegisteredNodes.Add(this);
			Plugin.debugLogger.LogInfo($"Registered node {name}");
		}
	}
}
