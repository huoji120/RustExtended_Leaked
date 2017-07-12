using System;

namespace RustExtended
{
	[Serializable]
	public class ClanLevel
	{
		public int Id;

		public int RequireLevel;

		public ulong RequireCurrency;

		public ulong RequireExperience;

		public int MaxMembers;

		public uint CurrencyTax;

		public uint WarpTimewait;

		public uint WarpCountdown;

		public bool FlagMotd;

		public bool FlagAbbr;

		public bool FlagFFire;

		public bool FlagTax;

		public bool FlagHouse;

		public bool FlagDeclare;

		public uint BonusCraftingSpeed;

		public uint BonusGatheringWood;

		public uint BonusGatheringRock;

		public uint BonusGatheringAnimal;

		public uint BonusMembersPayMurder;

		public uint BonusMembersDefense;

		public uint BonusMembersDamage;

		public ClanLevel(int level = 0)
		{
			this.Id = level;
			this.RequireLevel = -1;
			this.RequireCurrency = 0uL;
			this.RequireExperience = 0uL;
			this.MaxMembers = 5;
			this.CurrencyTax = 10u;
			this.WarpTimewait = 30u;
			this.WarpCountdown = 3600u;
			this.FlagMotd = false;
			this.FlagAbbr = false;
			this.FlagFFire = false;
			this.FlagTax = false;
			this.FlagHouse = false;
			this.FlagDeclare = false;
			this.BonusCraftingSpeed = 0u;
			this.BonusGatheringWood = 0u;
			this.BonusGatheringRock = 0u;
			this.BonusGatheringAnimal = 0u;
			this.BonusMembersPayMurder = 0u;
			this.BonusMembersDefense = 0u;
			this.BonusMembersDamage = 0u;
		}
	}
}
