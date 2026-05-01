using System.Collections.Generic;
using System.Linq;
using TerminalUtils.Definitions;

namespace TerminalUtils
{
	public static class InfoTypeResolver
	{
		public static List<PreviewInfoType<SelectableLevel>> GetPreviewInfoType(string name)
		{
			// the default for this is "Name;Price;Weather"

			if (string.IsNullOrEmpty(name))
			{
				return [TerminalManager.PreviewInfoTypes["Name"]];
			}

			string[] types = name.Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();

			return types
				.Select(typeName =>
					TerminalManager.PreviewInfoTypes.FirstOrDefault(info => info.Key.ToLowerInvariant() == typeName.ToLowerInvariant())
				)
				.Where(info => info.Value != null)
				.Select(info => info.Value)
				.ToList();
		}

		public static FilterInfoType<SelectableLevel> GetFilterInfoType(string name)
		{
			return TerminalManager.FilterInfoTypes.FirstOrDefault(info => info.Key.ToLowerInvariant() == name.ToLowerInvariant()).Value;
		}

		public static SortInfoType<SelectableLevel> GetSortInfoType(string name)
		{
			return TerminalManager.SortInfoTypes.FirstOrDefault(info => info.Key.ToLowerInvariant() == name.ToLowerInvariant()).Value;
		}
	}
}
