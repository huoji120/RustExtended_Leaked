namespace RustExtended
{
    using System;

    [Flags]
    public enum UserFlags
    {
        admin = 0x10,
        banned = 8,
        details = 0x1000,
        freezed = 0x800,
        godmode = 0x20,
        guest = 0,
        invis = 0x40,
        nopvp = 0x80,
        normal = 1,
        onevent = 0x400,
        online = 0x100,
        premium = 2,
        safeboxes = 0x200,
        whitelisted = 4
    }
}

