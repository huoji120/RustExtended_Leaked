namespace RustExtended
{
    using System;

    [Flags]
    public enum ClanFlags
    {
        can_abbr = 2,
        can_declare = 0x20,
        can_ffire = 4,
        can_motd = 1,
        can_tax = 8,
        can_warp = 0x10,
        friendlyfire = 0x100,
        nodecayhouse = 0x200
    }
}

