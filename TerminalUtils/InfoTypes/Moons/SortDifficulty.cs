using System;
using System.Collections.Generic;
using System.Linq;
using TerminalUtils.Definitions;

namespace TerminalUtils.InfoTypes.Moons
{
	public class SortDifficulty : SortInfoType<SelectableLevel>
	{
		public SortDifficulty()
			: base("Difficulty") { }

		private static readonly List<string> difficultyOrder = ["Safe", "F", "E", "D", "C", "B", "A", "S", "Unknown", "?"];

		public override List<SelectableLevel> Sort(List<SelectableLevel> inputList)
		{
			// sort by base difficulty (using difficultyOrder) then by plus/minus modifiers (more '+' => later/harder, more '-' => earlier/easier)
			inputList.Sort(
				(a, b) =>
				{
					string riskA = string.IsNullOrWhiteSpace(a.riskLevel) ? "Unknown" : a.riskLevel.Trim();
					string riskB = string.IsNullOrWhiteSpace(b.riskLevel) ? "Unknown" : b.riskLevel.Trim();

					int plusA = riskA.Count(c => c == '+');
					int minusA = riskA.Count(c => c == '-');
					int plusB = riskB.Count(c => c == '+');
					int minusB = riskB.Count(c => c == '-');

					// base key: remove '+' and '-' characters and trim
					string baseA = new string(riskA.Where(c => c != '+' && c != '-').ToArray()).Trim();
					string baseB = new string(riskB.Where(c => c != '+' && c != '-').ToArray()).Trim();

					if (string.IsNullOrEmpty(baseA))
					{
						baseA = "Unknown";
					}
					if (string.IsNullOrEmpty(baseB))
					{
						baseB = "Unknown";
					}

					int indexA = difficultyOrder.FindIndex(s => string.Equals(s, baseA, StringComparison.OrdinalIgnoreCase));
					int indexB = difficultyOrder.FindIndex(s => string.Equals(s, baseB, StringComparison.OrdinalIgnoreCase));

					if (indexA == -1)
					{
						indexA = difficultyOrder.Count;
					}
					if (indexB == -1)
					{
						indexB = difficultyOrder.Count;
					}

					if (indexA != indexB)
					{
						return indexA.CompareTo(indexB);
					}

					// same base difficulty -> compare net modifier (plus minus)
					int netA = plusA - minusA;
					int netB = plusB - minusB;
					return netA.CompareTo(netB);
				}
			);

			return inputList;
		}
	}
}
