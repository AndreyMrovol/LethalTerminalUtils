using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MrovLib;
using MrovLib.ContentType;
using TerminalUtils.Definitions;
using UnityEngine;

namespace TerminalUtils.Nodes
{
	public class StoreCatalogue : TerminalNodeReplacement
	{
		public StoreCatalogue()
			: base("Store Catalogue", TerminalManager.StorePage)
		{
			this.HelpText = " Welcome to the Company store. \n Use words BUY and INFO on any item. \n Order items in bulk by typing a number.";
		}

		public override string GetNodeText(TerminalNode node)
		{
			var table = new ConsoleTables.ConsoleTable("Name", "Price");
			var stringBuilder = new StringBuilder();

			stringBuilder.Append(this.HelpText != null ? $"\n{this.HelpText}\n" : "");

			PurchaseType[] desiredOrder =
			[
				PurchaseType.Item,
				PurchaseType.Vehicle,
				PurchaseType.Unlockable,
				PurchaseType.Decoration,
				PurchaseType.Suit
			];

			Dictionary<PurchaseType, List<BuyableThing>> groupedThings = TerminalManager
				.GetCurrentStoreItems()
				.GroupBy(thing => thing.Type)
				.OrderBy(group =>
				{
					int idx = Array.IndexOf(desiredOrder, group.Key);
					return idx == -1 ? int.MaxValue : idx;
				})
				.ToDictionary(
					group => group.Key,
					group =>
						group
							.Where(item =>
							{
								switch (item.Type)
								{
									case PurchaseType.Unlockable:
										BuyableUnlockable unlockable = (BuyableUnlockable)item;
										return !unlockable.IsUnlocked;
									case PurchaseType.Decoration:
										BuyableDecoration decoration = (BuyableDecoration)item;
										return decoration.InRotation && !decoration.IsUnlocked;
									case PurchaseType.Suit:
										BuyableSuit suit = (BuyableSuit)item;
										return suit.InRotation && !suit.IsUnlocked;
									default:
										return true;
								}
							})
							.ToList()
				);

			foreach (var group in groupedThings)
			{
				if (group.Value.Count == 0)
				{
					continue;
				}

				int itemCount = 1;

				table.AddRow("", "");
				table.AddRow($"[{group.Key.ToString().ToUpperInvariant()}S]", "");

				for (int i = 0; i < group.Value.Count; i++)
				{
					var thing = group.Value[i];
					string priceWithDiscount = $"${thing.Price}";

					if (thing.Type == PurchaseType.Item)
					{
						BuyableItem item = (BuyableItem)thing;
						if (item.Discount != 0)
						{
							string discountPercent = item.Discount != 0 ? $"  (-{item.PercentOff}%)" : "";
							priceWithDiscount += discountPercent;
						}
					}

					table.AddRow($"* {thing.Name.PadRight(30)}", $"{priceWithDiscount}");

					if (ConfigManager.DivideStore.Value != 0 && i != group.Value.Count - 1)
					{
						if (itemCount % ConfigManager.DivideStore.Value == 0)
						{
							itemCount = 1;
							table.AddRow("", "");
						}
						else
						{
							itemCount++;
						}
					}
				}
			}

			string tableString = table.ToStringCustomDecoration(header: false, divider: true).TrimEnd();
			stringBuilder.Append(tableString);
			return stringBuilder.ToString().TrimEnd();
		}
	}
}
