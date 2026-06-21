using System.Collections.Generic;
using System.Linq;
using MrovLib.ContentType;
using TerminalUtils.Definitions;

namespace TerminalUtils.Commands
{
	public class StoreSortCommand : TerminalCommandNode
	{
		public StoreSortCommand()
			: base("store")
		{
			RedirectToNode = TerminalManager.StorePage;
		}

		public override string Execute(string[] args)
		{
			string sortTypeName = "name";

			// List<string> previewTypeNames = ["name"];
			Dictionary<string, SortInfoType<BuyableThing>> infoTypes = TerminalManager
				.StoreSortInfoTypes.Select(kv => kv.Value)
				.ToDictionary(infoType => infoType.Name.ToLowerInvariant(), infoType => infoType);

			Plugin.debugLogger.LogDebug($"Possible store sort types: {string.Join(", ", infoTypes.Keys)}");

			args.ToList()
				.ForEach(arg =>
				{
					if (infoTypes.ContainsKey(arg))
					{
						sortTypeName = arg;
					}
				});

			TerminalManager.CurrentStoreSortInfoType = infoTypes[sortTypeName];
			ConfigManager.StoreSortInfoType.Value = infoTypes[sortTypeName].Name;

			return "";
		}
	}
}
