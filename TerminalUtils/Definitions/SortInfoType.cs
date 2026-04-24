using System.Collections.Generic;
using TerminalUtils.Enums;

namespace TerminalUtils.Definitions
{
	public class SortInfoType<T> : TerminalInfoType
	{
		public SortInfoType(string Name)
		{
			this.Name = Name;
			this.Type = TerminalDisplayType.Sort;
		}

		public virtual List<T> Sort(List<T> inputList)
		{
			return inputList;
		}
	}
}
