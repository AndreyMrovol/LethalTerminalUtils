using TerminalUtils.Enums;

namespace TerminalUtils.Definitions
{
	public class PreviewInfoType : TerminalInfoType
	{
		public int MaxLength { get; set; } = Defaults.terminalWidth;

		public PreviewInfoType(string Name)
		{
			this.Name = Name;
			this.Type = TerminalDisplayType.Preview;
		}
	}

	public abstract class PreviewInfoType<T>(string Name) : PreviewInfoType(Name)
	{
		public string ValueWithMaxLength(T inputValue)
		{
			string value = Value(inputValue);
			if (value.Length > MaxLength)
			{
				value = value.Substring(0, MaxLength - 3) + "...";
			}
			return value;
		}

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
