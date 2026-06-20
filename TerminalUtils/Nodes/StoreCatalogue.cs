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

			Dictionary<PurchaseType, List<BuyableThing>> groupedThings = ContentManager
				.Buyables.GroupBy(thing => thing.Type)
				.ToDictionary(group => group.Key, group => group.ToList());

			foreach (var group in groupedThings)
			{
				Type buyableType = group.Value.First().GetType();

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

						string discountPercent = item.Discount != 0 ? $"  (-{item.PercentOff}%)" : "";
						priceWithDiscount += discountPercent;
					}

					if (thing.Type == PurchaseType.Decoration)
					{
						BuyableDecoration decoration = (BuyableDecoration)thing;

						if (decoration.IsUnlocked)
						{
							continue;
						}
					}

					if (thing.Type == PurchaseType.Suit)
					{
						BuyableSuit suit = (BuyableSuit)thing;

						if (suit.IsUnlocked)
						{
							continue;
						}
					}

					table.AddRow(thing.Name.PadRight(30), priceWithDiscount);

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
			return tableString;
		}
	}
}
