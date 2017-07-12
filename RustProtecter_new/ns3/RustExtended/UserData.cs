namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class UserData
    {
        public int AveragePing;
        public bool CanTeleportShot;
        public  ClanData Clan;
        public string Comments;
        public FallCheckState FallCheck;
        public DateTime FirstConnectDate;
        public string FirstConnectIP;
        public UserFlags Flags;
        public string HasShootObject = "";
        public bool HasUnlimitedAmmo;
        public string Language;
        public string LastChatCommand = "";
        public DateTime LastConnectDate;
        public string LastConnectIP;
        public string Password;
        public Vector3 Position;
        public DateTime PremiumDate;
        public int[] ProtectKickData = new int[0];
        public string ProtectKickName = "";
        public float ProtectLastSnapshotTime;
        public int ProtectTick;
        public float ProtectTime;
        public int Rank;
        public ulong SteamID;
        public string Username;
        public DateTime ViolationDate;
        public int Violations;
        public WorldZone Zone;

        public UserData(ulong steam_id)
        {
            this.SteamID = steam_id;
            this.AveragePing = -60000;
        }

        public UserData()
        {
        }

        public bool HasCountdown(string command)
        {
            Class49 class2 = new Class49 {
                string_0 = command
            };
            return Users.CountdownList(this.SteamID).Exists(new Predicate<Countdown>(class2.method_0));
        }

        public bool HasFlag(UserFlags flag)
        {
            return ((this.Flags & flag) == flag);
        }

        public bool HasPersonal(string name)
        {
            return Users.PersonalList(this.SteamID).ContainsKey(name);
        }

        public bool HasShared(ulong user_id)
        {
            return Users.SharedList(this.SteamID).Contains(user_id);
        }

        public void SetFlag(UserFlags flag, [Optional, DefaultParameterValue(true)] bool state)
        {
            this.Flags = state ? (this.Flags |= flag) : (this.Flags &= ~flag);
        }

        public void ToggleFlag(UserFlags flag)
        {
            this.Flags ^= flag;
        }

        public bool Details
        {
            get
            {
                return this.HasFlag(UserFlags.details);
            }
        }

        public ulong Hash
        {
            get
            {
                ulong num = this.SteamID + ((this.Username == null) ? ((ulong) 0) : ((ulong) this.Username.GetHashCode()));
                num += (this.Password == null) ? ((ulong) 0) : ((ulong) this.Password.GetHashCode());
                num += (this.Comments == null) ? ((ulong) 0) : ((ulong) this.Comments.GetHashCode());
                num += (ulong)Rank;
                num += (ulong)Flags.GetHashCode();
                num += (this.Language == null) ? ((ulong) 0) : ((ulong) this.Language.GetHashCode());
                num += (ulong) this.Position.x;
                num += (ulong) this.Position.y;
                num += (ulong) this.Position.z;
                num += (ulong)Violations;
                num += (ulong)ViolationDate.GetHashCode();
                num += (this.LastConnectIP == null) ? ((ulong) 0) : ((ulong) this.LastConnectIP.GetHashCode());
                num += (ulong)LastConnectDate.GetHashCode();
                num += (this.FirstConnectIP == null) ? ((ulong) 0) : ((ulong) this.FirstConnectIP.GetHashCode());
                num += (ulong)FirstConnectDate.GetHashCode();
                return (num + (ulong)PremiumDate.GetHashCode());
            }
        }

        [CompilerGenerated]
        private sealed class Class49
        {
            public string string_0;

            public bool method_0(Countdown countdown_0)
            {
                return (countdown_0.Command == this.string_0);
            }
        }
    }
}

