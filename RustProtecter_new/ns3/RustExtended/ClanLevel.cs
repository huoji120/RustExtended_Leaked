namespace RustExtended
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class ClanLevel
    {
        public uint BonusCraftingSpeed;
        public uint BonusGatheringAnimal;
        public uint BonusGatheringRock;
        public uint BonusGatheringWood;
        public uint BonusMembersDamage;
        public uint BonusMembersDefense;
        public uint BonusMembersPayMurder;
        public uint CurrencyTax;
        public bool FlagAbbr;
        public bool FlagDeclare;
        public bool FlagFFire;
        public bool FlagHouse;
        public bool FlagMotd;
        public bool FlagTax;
        public int Id;
        public int MaxMembers;
        public ulong RequireCurrency;
        public ulong RequireExperience;
        public int RequireLevel;
        public uint WarpCountdown;
        public uint WarpTimewait;

        public ClanLevel([Optional, DefaultParameterValue(0)] int level)
        {
            this.Id = level;
            this.RequireLevel = -1;
            this.RequireCurrency = 0L;
            this.RequireExperience = 0L;
            this.MaxMembers = 5;
            this.CurrencyTax = 10;
            this.WarpTimewait = 30;
            this.WarpCountdown = 0xe10;
            this.FlagMotd = false;
            this.FlagAbbr = false;
            this.FlagFFire = false;
            this.FlagTax = false;
            this.FlagHouse = false;
            this.FlagDeclare = false;
            this.BonusCraftingSpeed = 0;
            this.BonusGatheringWood = 0;
            this.BonusGatheringRock = 0;
            this.BonusGatheringAnimal = 0;
            this.BonusMembersPayMurder = 0;
            this.BonusMembersDefense = 0;
            this.BonusMembersDamage = 0;
        }
    }
}

