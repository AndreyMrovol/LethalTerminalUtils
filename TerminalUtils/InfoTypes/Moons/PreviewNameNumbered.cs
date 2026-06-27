using System.Text.RegularExpressions;
using MrovLib;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class PreviewNameNumbered : PreviewInfoType<SelectableLevel>
	{
		public PreviewNameNumbered()
			: base("NameNumbered")
		{
			this.MaxLength = LevelHelper.LongestPlanetName.Length + 4;
		}

		public override string Value(SelectableLevel inputValue)
		{
			string moonNumber = Regex.Match(inputValue.PlanetName, @"^\d+").Value;

			{
				return $"{moonNumber.PadLeft(3, '0')} {MrovLib.StringResolver.GetNumberlessName(inputValue)}";
			}
		}
	}
}
