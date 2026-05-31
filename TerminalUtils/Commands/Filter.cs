using System.Collections.Generic;
using System.Linq;
using TerminalUtils.Definitions;

namespace TerminalUtils.Commands
{
	public class FilterCommand : TerminalCommandNode
	{
		public FilterCommand()
			: base("filter")
		{
			RedirectToNode = TerminalManager.MoonsPage;
		}

		public override string Execute(string[] args)
		{
			string filterTypeName = "none";

			// List<string> previewTypeNames = ["name"];
			Dictionary<string, FilterInfoType<SelectableLevel>> infoTypes = TerminalManager
				.FilterInfoTypes.Select(kv => kv.Value)
				.ToDictionary(infoType => infoType.Name.ToLowerInvariant(), infoType => infoType);

			Plugin.debugLogger.LogDebug($"Possible filter types: {string.Join(", ", infoTypes.Keys)}");

			args.ToList()
				.ForEach(arg =>
				{
					if (infoTypes.ContainsKey(arg))
					{
						filterTypeName = arg;
					}
				});

			TerminalManager.CurrentFilterInfoType = infoTypes[filterTypeName];

			return "";
		}
	}
}
