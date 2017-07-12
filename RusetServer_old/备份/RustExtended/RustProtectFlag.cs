namespace RustExtended
{
    using System;

    public enum RustProtectFlag : byte
    {
        Disabled = 0,
        Enabled = 1,
        Snapshot = 4,
        UserHWID = 2
    }
}

