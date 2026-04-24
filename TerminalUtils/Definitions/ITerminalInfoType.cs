// using LethalLevelLoader;
// using LethalRegistry.Managers;

using TerminalUtils.Enums;

namespace TerminalUtils.Definitions
{
	public interface ITerminalInfoType
	{
		public string Name { get; }
		public TerminalDisplayType Type { get; }
	}

	public class TerminalInfoType : ITerminalInfoType
	{
		public string Name { get; set; }
		public TerminalDisplayType Type { get; set; }

		// TODO: How to resolve values? What's the type that the InfoTypes will resolve to?
		// public

		public override string ToString()
		{
			return $"{Name} ({Type})";
		}
	}
}
