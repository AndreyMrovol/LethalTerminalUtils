using System.Collections.Generic;
using TerminalUtils.Definitions;

namespace TerminalUtils
{
	public static class CommandManager
	{
		public static TerminalNode CommandNode;

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

			CommandNode = TerminalNodeManager.CreateTerminalNode("Weather Commands");
			CommandNode.acceptAnything = false;
		}

		public static TerminalNode RunWeatherCommand(TerminalCommandNode command, string[] args)
		{
			// CommandLookup.TryGetValue(commandName, out var command);
			string result = "";

			if (command == null)
			{
				CommandNode.displayText = $"Command '{command}' not found.";
				return CommandNode;
			}

			// run the command itself when
			// there's no subcommands OR there's no subcommand specified

			if (command.Subcommands.Count == 0 || args.Length < 1)
			{
				result = command.Execute(args);
			}
			else
			{
				var commandname = args[0];
				Plugin.debugLogger.LogInfo($"Running subcommand '{commandname}' - is null? {commandname == null}");

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

			CommandNode.displayText = result;
			return CommandNode;
		}
	}
}
