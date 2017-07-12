using System;

namespace RustExtended
{
	[Flags]
	public enum ZoneFlags
	{
		radiation = 1,
		nodecay = 2,
		trade = 4,
		nobuild = 8,
		nopvp = 16,
		safe = 32,
		nosleep = 64,
		events = 128,
		nocraft = 256,
		noenter = 512,
		noleave = 1024
	}
}
