using System.Collections.Generic;
using System.Linq;
using MonoMod.Utils;
using MrovLib;
using MrovLib.ContentType;
using TerminalUtils.Definitions;
using TerminalUtils.Enums;
using TerminalUtils.InfoTypes.Moons;
using TerminalUtils.Nodes;

namespace TerminalUtils
{
	public static class TerminalManager
	{
		public static Terminal Terminal => MrovLib.ContentManager.Terminal;

		public static TerminalNode MoonsPage { get; private set; }
		public static TerminalNode StorePage { get; private set; }

		public static Dictionary<string, PreviewInfoType<SelectableLevel>> PreviewInfoTypes = [];
		public static Dictionary<string, FilterInfoType<SelectableLevel>> FilterInfoTypes = [];
		public static Dictionary<string, SortInfoType<SelectableLevel>> SortInfoTypes = [];

		public static List<PreviewInfoType<SelectableLevel>> CurrentPreviewInfoType { get; set; }
		public static FilterInfoType<SelectableLevel> CurrentFilterInfoType { get; set; }
		public static SortInfoType<SelectableLevel> CurrentSortInfoType { get; set; }

		#region Store
		public static Dictionary<string, SortInfoType<BuyableThing>> StoreSortInfoTypes = [];
		public static SortInfoType<BuyableThing> CurrentStoreSortInfoType { get; set; }

		#endregion

		public static Dictionary<TerminalNode, TerminalNodeReplacement> NodeReplacements = [];

		internal static void Init(Terminal terminal)
		{
			MoonsPage = ContentManager.MoonsKeyword.specialKeywordResult;
			StorePage = ContentManager.Nodes.FirstOrDefault(node => node.name == "0_StoreHub");

			RegisterLocalInfoTypes();

			CurrentPreviewInfoType = InfoTypeResolver.GetPreviewInfoType(ConfigManager.PreviewInfoType.Value);
			CurrentFilterInfoType = InfoTypeResolver.GetFilterInfoType(ConfigManager.FilterInfoType.Value);
			CurrentSortInfoType = InfoTypeResolver.GetSortInfoType(ConfigManager.SortInfoType.Value);

			CurrentStoreSortInfoType = InfoTypeResolver.GetStoreSortInfoType(ConfigManager.StoreSortInfoType.Value);

			NodeReplacements = new Dictionary<TerminalNode, TerminalNodeReplacement>
			{
				{ MoonsPage, new MoonCatalogue() },
				{ StorePage, new StoreCatalogue() },
			};
		}

		private static void RegisterLocalInfoTypes()
		{
			PreviewInfoTypes.Clear();
			SortInfoTypes.Clear();
			FilterInfoTypes.Clear();
			StoreSortInfoTypes.Clear();

			PreviewInfoTypes.Add("Name", new PreviewName());
			PreviewInfoTypes.Add("Price", new PreviewPrice());
			PreviewInfoTypes.Add("Weather", new PreviewWeather());
			PreviewInfoTypes.Add("Difficulty", new PreviewDifficulty());

			if (Plugin.LCCompatibility.IsModPresent)
			{
				PreviewInfoTypes.Add("Constellation", new PreviewConstellation());
			}

			if (Plugin.LMUCompatibility.IsModPresent)
			{
				PreviewInfoTypes.Add("LMU", new PreviewLMU());
			}

			SortInfoTypes.Add("None", new SortNone());
			SortInfoTypes.Add("Name", new SortName());
			SortInfoTypes.Add("Price", new SortPrice());
			SortInfoTypes.Add("Difficulty", new SortDifficulty());

			FilterInfoTypes.Add("None", new FilterNone());
			FilterInfoTypes.Add("Price", new FilterPrice());
			FilterInfoTypes.Add("Weather", new FilterWeather());

			StoreSortInfoTypes.Add("None", new InfoTypes.Store.SortNone());
			StoreSortInfoTypes.Add("Name", new InfoTypes.Store.SortName());
			StoreSortInfoTypes.Add("Price", new InfoTypes.Store.SortPrice());
		}

		public static List<SelectableLevel> GetCurrentLevels()
		{
			List<SelectableLevel> currentlySelectedLevels = TerminalManager.CurrentFilterInfoType.Filter(LevelHelper.Levels).ToList();
			currentlySelectedLevels = TerminalManager.CurrentSortInfoType.Sort(currentlySelectedLevels);

			return currentlySelectedLevels;
		}

		public static List<BuyableThing> GetCurrentStoreItems()
		{
			List<BuyableThing> currentlySelectedItems = TerminalManager.CurrentStoreSortInfoType.Sort(ContentManager.Buyables.ToList());

			return currentlySelectedItems;
		}
	}
}
