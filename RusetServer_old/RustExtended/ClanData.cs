namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class ClanData
    {
        public string Abbr;
        public ulong Balance;
        public DateTime Created;
        public ulong Experience;
        public ClanFlags Flags;
        public Dictionary<uint, DateTime> Hostile;
        public uint ID;
        public ulong LeaderID;
        public ClanLevel Level;
        public Vector3 Location;
        public Dictionary<UserData, ClanMemberFlags> Members;
        public string MOTD;
        public string Name;
        public DateTime Penalty;
        public uint Tax;

        public ClanData(uint id, string name = null, string abbr = null, ulong leader_id = 0uL, DateTime created = default(DateTime))
        {
            this.ID = id;
            this.Name = name;
            this.Abbr = abbr;
            this.LeaderID = leader_id;
            this.Created = created;
            this.Balance = 0L;
            this.Tax = 10;
            this.Level = new ClanLevel(0);
            this.Experience = 0L;
            this.Location = Vector3.zero;
            this.MOTD = "";
            this.Penalty = new DateTime();
            this.Hostile = new Dictionary<uint, DateTime>();
            this.Members = new Dictionary<UserData, ClanMemberFlags>();
        }

        public void Message(string text)
        {
            Broadcast.MessageClan(this, text);
        }

        public bool SetLevel(ClanLevel level)
        {
            if (level == null)
            {
                return false;
            }
            this.Level = level;
            this.Tax = level.CurrencyTax;
            this.Flags = this.Flags.SetFlag<ClanFlags>(ClanFlags.can_motd, level.FlagMotd);
            this.Flags = this.Flags.SetFlag<ClanFlags>(ClanFlags.can_abbr, level.FlagAbbr);
            this.Flags = this.Flags.SetFlag<ClanFlags>(ClanFlags.can_ffire, level.FlagFFire);
            this.Flags = this.Flags.SetFlag<ClanFlags>(ClanFlags.can_tax, level.FlagTax);
            this.Flags = this.Flags.SetFlag<ClanFlags>(ClanFlags.can_warp, level.FlagHouse);
            this.Flags = this.Flags.SetFlag<ClanFlags>(ClanFlags.can_declare, level.FlagDeclare);
            return true;
        }

        public bool FrendlyFire
        {
            get
            {
                return ((this.Flags & ClanFlags.friendlyfire) == ClanFlags.friendlyfire);
            }
            set
            {
                this.Flags = this.Flags.SetFlag<ClanFlags>(ClanFlags.friendlyfire, value);
            }
        }

        public ulong Hash
        {
            get
            {
                ulong num = this.LeaderID + ((this.Name == null) ? ((ulong) 0) : ((ulong) this.Name.GetHashCode()));
                num += (this.Abbr == null) ? ((ulong) 0) : ((ulong) this.Abbr.GetHashCode());
                num += (ulong)Created.GetHashCode();
                num += (ulong)Flags.GetHashCode();
                num += this.Balance;
                num += this.Tax;
                num += (ulong)Level.GetHashCode();
                num += this.Experience;
                num += (ulong)Location.GetHashCode();
                num += (this.MOTD == null) ? ((ulong) 0) : ((ulong) this.MOTD.GetHashCode());
                foreach (UserData data in this.Members.Keys)
                {
                    num += (ulong) this.Members[data];
                }
                foreach (uint num2 in this.Hostile.Keys)
                {
                    num += num2;
                }
                return num;
            }
        }

        public int Online
        {
            get
            {
                int num = 0;
                foreach (UserData data in this.Members.Keys)
                {
                    if (NetUser.FindByUserID(data.SteamID) != null)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public static implicit operator ClanData(UserData v)
        {
            throw new NotImplementedException();
        }
    }
}

