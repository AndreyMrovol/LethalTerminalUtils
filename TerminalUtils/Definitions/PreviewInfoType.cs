using TerminalUtils.Enums;

namespace TerminalUtils.Definitions
{
	public class PreviewInfoType : TerminalInfoType
	{
		public PreviewInfoType(string Name)
		{
			this.Name = Name;
			this.Type = TerminalDisplayType.Preview;
		}
	}

	public class PreviewInfoType<T>(string Name) : PreviewInfoType(Name)
	{
		public virtual string Value(T inputValue)
		{
			return "";
		}

		public override string ToString()
		{
			return $"{Name} ({Type})";
		}
	}
}
