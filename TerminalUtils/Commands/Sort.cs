using System.Collections.Generic;
using System.Linq;
using TerminalUtils.Definitions;

namespace TerminalUtils.Commands
{
	public class SortCommand : TerminalCommandNode
	{
		public SortCommand()
			: base("sort")
		{
			RedirectToNode = TerminalManager.MoonsPage;
		}

		public override string Execute(string[] args)
		{
			string sortTypeName = "none";

			// List<string> previewTypeNames = ["name"];
			Dictionary<string, SortInfoType<SelectableLevel>> infoTypes = TerminalManager
				.SortInfoTypes.Select(kv => kv.Value)
				.ToDictionary(infoType => infoType.Name.ToLowerInvariant(), infoType => infoType);

			Plugin.debugLogger.LogDebug($"Possible sort types: {string.Join(", ", infoTypes.Keys)}");

			args.ToList()
				.ForEach(arg =>
				{
					if (infoTypes.ContainsKey(arg))
					{
						sortTypeName = arg;
					}
				});

			TerminalManager.CurrentSortInfoType = infoTypes[sortTypeName];

			return "";
		}
	}
}
