using System;

namespace RustExtended
{
	[Flags]
	public enum ClanMemberFlags
	{
		invite = 1,
		dismiss = 2,
		management = 4,
		expdetails = 256
	}
}
