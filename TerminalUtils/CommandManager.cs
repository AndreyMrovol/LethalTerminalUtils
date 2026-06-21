using System.Collections.Generic;
using TerminalUtils.Definitions;

namespace TerminalUtils
{
	public static class CommandManager
	{
		public static TerminalNode CommandNode;
		internal static TerminalNode RedirectToMoonsNode;
		internal static TerminalNode RedirectToStoreNode;

		public static List<TerminalCommandNode> Commands { get; set; } = [];
		public static Dictionary<string, TerminalCommandNode> CommandLookup
		{
			get
			{
				Dictionary<string, TerminalCommandNode> lookup = [];
				Commands.ForEach(command => lookup[command.Name] = command);
				return lookup;
			}
		}

		public static void Init()
		{
			Commands.Clear();

			CommandNode = TerminalNodeManager.CreateTerminalNode("TerminalUtilsCommandNode");
			CommandNode.acceptAnything = false;

			RedirectToMoonsNode = TerminalNodeManager.CreateTerminalNode("TerminalUtilsRedirectToMoonsNode");
			RedirectToMoonsNode.acceptAnything = false;

			RedirectToStoreNode = TerminalNodeManager.CreateTerminalNode("TerminalUtilsRedirectToStoreNode");
			RedirectToStoreNode.acceptAnything = false;
		}

		public static TerminalNode RunTerminalCommand(TerminalCommandNode command, string[] args)
		{
			// CommandLookup.TryGetValue(commandName, out var command);
			string result = "";

			if (command == null)
			{
				CommandNode.displayText = $"Command '{command}' not found.";
				return CommandNode;
			}

			if (!command.ShouldRun())
			{
				CommandNode.displayText = $"Command '{command.Name}' cannot run!";
				return CommandNode;
			}

			// run the command itself when
			// there's no subcommands OR there's no subcommand specified

			if (command.Subcommands.Count == 0 || args.Length < 1)
			{
				Plugin.debugLogger.LogDebug($"Running command '{command.Name}' with no subcommand");
				result = command.Execute(args);
			}
			else
			{
				// TODO: this requires additional testing cause it's not that great

				Plugin.debugLogger.LogInfo(
					$"Looking for subcommand '{args[0]}'; available: {string.Join(", ", command.Subcommands.ConvertAll(sc => sc.Name))}"
				);

				try
				{
					TerminalCommandNode subCommand = command.Subcommands.Find(sc => sc.Name == args[0]);

					if (subCommand == null)
					{
						result = $"Subcommand '{args[0]}' not found for command '{command.Name}'.";
					}

					result = subCommand.Execute(args);
				}
				catch (System.Exception e)
				{
					Plugin.debugLogger.LogError($"Error finding subcommand: {e}");
				}
			}

			if (command.RedirectToNode != null)
			{
				Plugin.debugLogger.LogDebug($"Redirecting to node '{command.RedirectToNode.name}'");
				return command.RedirectToNode;
			}

			CommandNode.displayText = result;
			return CommandNode;
		}
	}
}
