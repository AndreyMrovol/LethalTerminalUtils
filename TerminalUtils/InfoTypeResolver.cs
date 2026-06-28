using System.Collections.Generic;
using System.Linq;
using MrovLib.ContentType;
using TerminalUtils.Definitions;

namespace TerminalUtils
{
	public static class InfoTypeResolver
	{
		public static List<PreviewInfoType<SelectableLevel>> GetPreviewInfoType(string inputString)
		{
			// the default for this is "Price;Weather"

			if (string.IsNullOrEmpty(inputString))
			{
				return Defaults.defaultPreviewType.Split(";").Select(typeName => TerminalManager.PreviewInfoTypes[typeName]).ToList();
			}

			if (!inputString.ToLowerInvariant().Contains("Name".ToLowerInvariant()))
			{
				inputString = "Name;" + inputString;
				Plugin.logger.LogDebug($"Preview type did not contain 'Name', defaulting to 'Name;{inputString}'");
			}

			string[] types = inputString.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();

			return types
				.Select(typeName =>
					TerminalManager.PreviewInfoTypes.FirstOrDefault(info => info.Key.ToLowerInvariant() == typeName.ToLowerInvariant())
				)
				.Where(info => info.Value != null)
				.Select(info => info.Value)
				.ToList();
		}

		public static FilterInfoType<SelectableLevel> GetFilterInfoType(string inputString)
		{
			if (string.IsNullOrEmpty(inputString))
			{
				return TerminalManager.FilterInfoTypes["None"];
			}

			return TerminalManager.FilterInfoTypes.FirstOrDefault(info => info.Key.ToLowerInvariant() == inputString.ToLowerInvariant()).Value;
		}

		public static SortInfoType<SelectableLevel> GetSortInfoType(string inputString)
		{
			if (string.IsNullOrEmpty(inputString))
			{
				return TerminalManager.SortInfoTypes["None"];
			}

			return TerminalManager.SortInfoTypes.FirstOrDefault(info => info.Key.ToLowerInvariant() == inputString.ToLowerInvariant()).Value;
		}

		public static SortInfoType<BuyableThing> GetStoreSortInfoType(string inputString)
		{
			if (string.IsNullOrEmpty(inputString))
			{
				return TerminalManager.StoreSortInfoTypes["None"];
			}

			return TerminalManager
				.StoreSortInfoTypes.FirstOrDefault(info => info.Key.ToLowerInvariant() == inputString.ToLowerInvariant())
				.Value;
		}
	}
}
