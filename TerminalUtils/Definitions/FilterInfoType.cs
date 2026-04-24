using System.Collections.Generic;

namespace TerminalUtils.Definitions
{
	public class FilterInfoType<T> : TerminalInfoType
	{
		public FilterInfoType(string Name)
		{
			this.Name = Name;
			this.Type = Enums.TerminalDisplayType.Filter;
		}

		public virtual List<T> Filter(List<T> inputList)
		{
			return inputList;
		}
	}
}
