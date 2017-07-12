using System;

namespace RustExtended
{
	public enum RustProtectFlag : byte
	{
		Disabled,
		Enabled,
		UserHWID,
		Snapshot = 4
	}
}
