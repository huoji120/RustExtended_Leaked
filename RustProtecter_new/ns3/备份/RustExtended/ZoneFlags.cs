namespace RustExtended
{
    using System;

    [Flags]
    public enum ZoneFlags
    {
        events = 0x80,
        nobuild = 8,
        nocraft = 0x100,
        nodecay = 2,
        noenter = 0x200,
        noleave = 0x400,
        nopvp = 0x10,
        nosleep = 0x40,
        radiation = 1,
        safe = 0x20,
        trade = 4
    }
}

