using System.Collections.Generic;
using System.Linq;
using TerminalUtils.Definitions;

namespace TerminalUtils.Commands
{
	public class PreviewCommand : TerminalCommandNode
	{
		public PreviewCommand()
			: base("preview")
		{
			RedirectToNode = TerminalManager.MoonsPage;
		}

		public override string Execute(string[] args)
		{
			List<string> previewTypeNames = ["name"];
			Dictionary<string, PreviewInfoType<SelectableLevel>> infoTypes = TerminalManager
				.PreviewInfoTypes.Select(kv => kv.Value)
				.ToDictionary(infoType => infoType.Name.ToLowerInvariant(), infoType => infoType);

			Plugin.debugLogger.LogDebug($"Possible preview types: {string.Join(", ", infoTypes.Keys)}");

			args.ToList()
				.ForEach(arg =>
				{
					if (infoTypes.ContainsKey(arg))
					{
						previewTypeNames.Add(arg);
					}
				});

			TerminalManager.CurrentPreviewInfoType = previewTypeNames.Select(name => infoTypes[name]).ToList();
			ConfigManager.PreviewInfoType.Value = string.Join(";", TerminalManager.CurrentPreviewInfoType.Select(info => info.Name));

			return "";
		}
	}
}
