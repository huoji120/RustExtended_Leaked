using System;

namespace RustExtended
{
	[Flags]
	public enum UserFlags
	{
		guest = 0,
		normal = 1,
		premium = 2,
		whitelisted = 4,
		banned = 8,
		admin = 16,
		godmode = 32,
		invis = 64,
		nopvp = 128,
		online = 256,
		safeboxes = 512,
		onevent = 1024,
		freezed = 2048,
		details = 4096
	}
}
