using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrovLib;
using TerminalUtils.Definitions;
using UnityEngine;

namespace TerminalUtils.Nodes
{
	public class Scan : TerminalNodeReplacement
	{
		public Scan()
			: base("Scan", TerminalManager.ScanPage) { }

		public bool GrabbablePredicate(GrabbableObject obj)
		{
			bool isShip = StartOfRound.Instance.inShipPhase;
			bool isCompanyMoon = LevelHelper.CompanyMoons.Contains(StartOfRound.Instance.currentLevel);

			if (isCompanyMoon)
			{
				return true;
			}

			if (isShip)
			{
				return obj.isInElevator || obj.isInShipRoom;
			}
			else
			{
				return !obj.isInShipRoom && !obj.isInElevator;
			}
		}

		public List<GrabbableObject> GetObjects()
		{
			List<GrabbableObject> allObjects = Object.FindObjectsOfType<GrabbableObject>().ToList();

			return allObjects
				.Where(item => item.itemProperties.isScrap && GrabbablePredicate(item) && !item.deactivated)
				.OrderBy(x => x.scrapValue)
				.ToList();
		}

		public override string GetNodeText(TerminalNode node)
		{
			bool detailedScan = ConfigManager.DetailedScanPage.Value;
			bool displayAccuratePrices = ConfigManager.DisplayAccuratePrices.Value;
			System.Random random = new(StartOfRound.Instance.randomMapSeed);

			var adjustedTable = new StringBuilder();
			adjustedTable.Append("\n");

			bool isShip = StartOfRound.Instance.inShipPhase;
			bool isCompanyMoon = LevelHelper.CompanyMoons.Contains(StartOfRound.Instance.currentLevel);

			List<GrabbableObject> objectsToScan = GetObjects();

			int items = objectsToScan.Count;
			int value = objectsToScan.Sum(x => x.scrapValue);

			if (isCompanyMoon)
			{
				adjustedTable.Append("Scanning all scrap:");
			}
			else
			{
				adjustedTable.Append($"Scanning scrap {(isShip ? "in the ship" : "on the moon")}:");
			}

			// don't show estimates for items already collected lol
			if (!displayAccuratePrices && (isShip || isCompanyMoon))
			{
				displayAccuratePrices = true;
			}

			if (!displayAccuratePrices)
			{
				int predictedValue = 0;

				foreach (GrabbableObject item in objectsToScan)
				{
					predictedValue += Mathf.Clamp(
						random.Next(item.itemProperties.minValue, item.itemProperties.maxValue),
						item.scrapValue - 6 * items,
						item.scrapValue + 9 * items
					);
				}

				value = predictedValue;
			}

			if (!detailedScan)
			{
				adjustedTable.Append(
					$"\nFound {items} scrap item{(items > 1 ? "s" : "")}, worth {(displayAccuratePrices ? "" : "about ")}${value}."
				);
			}
			else
			{
				adjustedTable.Append(
					$"\nFound {items} scrap item{(items > 1 ? "s" : "")}, worth {(displayAccuratePrices ? "" : "about ")}${value}."
				);
				adjustedTable.Append("\n\n");
				var table = new ConsoleTables.ConsoleTable("Name", "Price");

				foreach (var item in objectsToScan)
				{
					table.AddRow(
						item.itemProperties.itemName.PadRight(Defaults.itemNameWidth),
						$"${(displayAccuratePrices ? item.scrapValue : $"{item.itemProperties.minValue}-${item.itemProperties.maxValue}")}"
					);

					items++;
					value += item.scrapValue;
				}

				adjustedTable.Append("\n");
				adjustedTable.Append(table.ToStringCustomDecoration(header: true));
			}

			return adjustedTable.ToString();
		}
	}
}
