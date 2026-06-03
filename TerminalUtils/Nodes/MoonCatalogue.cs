using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrovLib;
using TerminalUtils.Definitions;
using UnityEngine;

namespace TerminalUtils.Nodes
{
	public class MoonCatalogue : TerminalNodeReplacement
	{
		public MoonCatalogue()
			: base("Moon Catalogue", TerminalManager.MoonsPage)
		{
			this.HelpText = " Welcome to the exomoons catalogue! \n Use ROUTE to set the autopilot. \n Use INFO to learn about a moon.";
		}

		public override string GetNodeText()
		{
			List<SelectableLevel> currentlySelectedLevels = TerminalManager.CurrentFilterInfoType.Filter(LevelHelper.Levels);
			currentlySelectedLevels = TerminalManager.CurrentSortInfoType.Sort(currentlySelectedLevels);

			Plugin.debugLogger.LogDebug(
				$"Current preview types: {string.Join(", ", TerminalManager.CurrentPreviewInfoType.Select(info => info.Name))}"
			);

			var outputString = new StringBuilder();
			var table = new ConsoleTables.ConsoleTable(TerminalManager.CurrentPreviewInfoType.Select(info => info.Name).ToArray());

			int itemCount = 1;

			List<PreviewInfoType<SelectableLevel>> currentPreviewTypes = TerminalManager.CurrentPreviewInfoType;
			Plugin.debugLogger.LogDebug($"Current preview types: {string.Join(", ", currentPreviewTypes.Select(info => info.Name))}");

			foreach (SelectableLevel level in currentlySelectedLevels)
			{
				if (LevelHelper.IsHidden(level))
				{
					continue;
				}

				if (MrovLib.Defaults.VanillaHiddenMoons.Contains(StringResolver.GetNumberlessName(level)))
				{
					continue;
				}

				string[] rowItems = currentPreviewTypes.Select(info => info.Value(level)).ToArray();
				table.AddRow(rowItems);

				if (itemCount % 3 == 0)
				{
					itemCount = 1;
					table.AddRow(currentPreviewTypes.Select(_ => "").ToArray());
				}
				else
				{
					itemCount++;
				}
			}

			outputString.AppendLine(this.HelpText != null ? $"\n{this.HelpText}\n\n" : "");

			outputString.Append($" The Company // Buying at {Mathf.RoundToInt(StartOfRound.Instance.companyBuyingRate * 100f)}% \n\n");

			outputString.Append(table.ToStringCustomDecoration());

			outputString.AppendLine();
			outputString.AppendLine();

			outputString.AppendLine(
				$"PREVIEW: {string.Join(", ", currentPreviewTypes.Select(info => info.Name))}\nSORT: {TerminalManager.CurrentSortInfoType.Name}; FILTER: {TerminalManager.CurrentFilterInfoType.Name}"
			);

			return outputString.ToString().TrimEnd();

			// return $"Moon Catalogue: {TerminalManager.CurrentFilterInfoType.Name}, {TerminalManager.CurrentSortInfoType.Name}, {string.Join(", ", TerminalManager.CurrentPreviewInfoType.Select(info => info.Name))}";
		}
	}
}
