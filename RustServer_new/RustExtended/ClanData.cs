using System;
using System.Collections.Generic;
using UnityEngine;

namespace RustExtended
{
	[Serializable]
	public class ClanData
	{
		public uint ID;

		public DateTime Created;

		public string Name;

		public string Abbr;

		public ulong LeaderID;

		public ClanFlags Flags;

		public ulong Balance;

		public uint Tax;

		public ClanLevel Level;

		public ulong Experience;

		public Vector3 Location;

		public string MOTD;

		public DateTime Penalty;

		public Dictionary<uint, DateTime> Hostile;

		public Dictionary<UserData, ClanMemberFlags> Members;

		public bool FrendlyFire
		{
			get
			{
				return (this.Flags & ClanFlags.friendlyfire) == ClanFlags.friendlyfire;
			}
			set
			{
				this.Flags = this.Flags.SetFlag(ClanFlags.friendlyfire, value);
			}
		}

		public int Online
		{
			get
			{
				int num = 0;
				foreach (UserData current in this.Members.Keys)
				{
					if (NetUser.FindByUserID(current.SteamID) != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		public ulong Hash
		{
			get
			{
				ulong num = this.LeaderID;
				num += (ulong)((long)((this.Name == null) ? 0 : this.Name.GetHashCode()));
				num += (ulong)((long)((this.Abbr == null) ? 0 : this.Abbr.GetHashCode()));
				num += (ulong)((long)this.Created.GetHashCode());
				num += (ulong)((long)this.Flags.GetHashCode());
				num += this.Balance;
				num += (ulong)this.Tax;
				num += (ulong)((long)this.Level.GetHashCode());
				num += this.Experience;
				num += (ulong)((long)this.Location.GetHashCode());
				num += (ulong)((long)((this.MOTD == null) ? 0 : this.MOTD.GetHashCode()));
				foreach (UserData current in this.Members.Keys)
				{
					num += (ulong)((long)this.Members[current]);
				}
				foreach (uint current2 in this.Hostile.Keys)
				{
					num += (ulong)current2;
				}
				return num;
			}
		}

		public void Message(string text)
		{
			Broadcast.MessageClan(this, text);
		}

		public ClanData(uint id, string name = null, string abbr = null, ulong leader_id = 0uL, DateTime created = default(DateTime))
		{
			this.ID = id;
			this.Name = name;
			this.Abbr = abbr;
			this.LeaderID = leader_id;
			this.Created = created;
			this.Balance = 0uL;
			this.Tax = 10u;
			this.Level = new ClanLevel(0);
			this.Experience = 0uL;
			this.Location = Vector3.zero;
			this.MOTD = "";
			this.Penalty = default(DateTime);
			this.Hostile = new Dictionary<uint, DateTime>();
			this.Members = new Dictionary<UserData, ClanMemberFlags>();
		}

		public bool SetLevel(ClanLevel level)
		{
			bool result;
			if (level == null)
			{
				result = false;
			}
			else
			{
				this.Level = level;
				this.Tax = level.CurrencyTax;
				this.Flags = this.Flags.SetFlag(ClanFlags.can_motd, level.FlagMotd);
				this.Flags = this.Flags.SetFlag(ClanFlags.can_abbr, level.FlagAbbr);
				this.Flags = this.Flags.SetFlag(ClanFlags.can_ffire, level.FlagFFire);
				this.Flags = this.Flags.SetFlag(ClanFlags.can_tax, level.FlagTax);
				this.Flags = this.Flags.SetFlag(ClanFlags.can_warp, level.FlagHouse);
				this.Flags = this.Flags.SetFlag(ClanFlags.can_declare, level.FlagDeclare);
				result = true;
			}
			return result;
		}
	}
}
