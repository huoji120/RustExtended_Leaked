using System;

namespace RustExtended
{
	[Flags]
	public enum NamePrefix : byte
	{
		None = 0,
		Rank = 1,
		Clan = 2,
		All = 3
	}
}
