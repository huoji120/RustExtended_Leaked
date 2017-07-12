using System;

namespace RustExtended
{
	[Flags]
	public enum ClanFlags
	{
		can_motd = 1,
		can_abbr = 2,
		can_ffire = 4,
		can_tax = 8,
		can_warp = 16,
		can_declare = 32,
		friendlyfire = 256,
		nodecayhouse = 512
	}
}
