namespace RustExtended
{
    using System;

    [Flags]
    public enum ClanMemberFlags
    {
        dismiss = 2,
        expdetails = 0x100,
        invite = 1,
        management = 4
    }
}

