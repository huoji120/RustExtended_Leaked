namespace RustExtended
{
    using System;

    [Flags]
    public enum NamePrefix : byte
    {
        All = 3,
        Clan = 2,
        None = 0,
        Rank = 1
    }
}

