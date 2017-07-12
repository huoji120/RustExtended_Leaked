using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RustExtended
{
	[Serializable]
	public class UserData
	{
		[CompilerGenerated]
		private sealed class Class49
		{
			public string string_0;

			public bool method_0(Countdown countdown_0)
			{
				return countdown_0.Command == this.string_0;
			}
		}

		public ulong SteamID;

		public string Username;

		public string Password;

		public string Comments;

		public int Rank;

		public float ChatTime;

		public UserFlags Flags;

		public string Language;

		public int Violations;

		public DateTime ViolationDate;

		public string LastConnectIP;

		public DateTime LastConnectDate;

		public string FirstConnectIP;

		public DateTime FirstConnectDate;

		public DateTime PremiumDate;

		public Vector3 Position;

		public WorldZone Zone;

		public ClanData Clan;

		public int AveragePing;

		public bool CanTeleportShot;

		public bool HasUnlimitedAmmo;

		public string HasShootObject = "";

		public string LastChatCommand = "";

		public FallCheckState FallCheck;

		public float ProtectTime;

		public int ProtectTick;

		public string ProtectKickName = "";

		public int[] ProtectKickData = new int[0];

		public float ProtectLastSnapshotTime;

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
				ulong num = this.SteamID;
				num += (ulong)((long)((this.Username == null) ? 0 : this.Username.GetHashCode()));
				num += (ulong)((long)((this.Password == null) ? 0 : this.Password.GetHashCode()));
				num += (ulong)((long)((this.Comments == null) ? 0 : this.Comments.GetHashCode()));
				num += (ulong)((long)this.Rank);
				num += (ulong)((long)this.Flags.GetHashCode());
				num += (ulong)((long)((this.Language == null) ? 0 : this.Language.GetHashCode()));
				num += (ulong)this.Position.x;
				num += (ulong)this.Position.y;
				num += (ulong)this.Position.z;
				num += (ulong)((long)this.Violations);
				num += (ulong)((long)this.ViolationDate.GetHashCode());
				num += (ulong)((long)((this.LastConnectIP == null) ? 0 : this.LastConnectIP.GetHashCode()));
				num += (ulong)((long)this.LastConnectDate.GetHashCode());
				num += (ulong)((long)((this.FirstConnectIP == null) ? 0 : this.FirstConnectIP.GetHashCode()));
				num += (ulong)((long)this.FirstConnectDate.GetHashCode());
				return num + (ulong)((long)this.PremiumDate.GetHashCode());
			}
		}

		public bool HasShared(ulong user_id)
		{
			return Users.SharedList(this.SteamID).Contains(user_id);
		}

		public bool HasPersonal(string name)
		{
			return Users.PersonalList(this.SteamID).ContainsKey(name);
		}

		public bool HasCountdown(string command)
		{
			UserData.Class49 @class = new UserData.Class49();
			@class.string_0 = command;
			return Users.CountdownList(this.SteamID).Exists(new Predicate<Countdown>(@class.method_0));
		}

		public void SetFlag(UserFlags flag, bool state = true)
		{
			this.Flags = (state ? (this.Flags |= flag) : (this.Flags &= ~flag));
		}

		public bool HasFlag(UserFlags flag)
		{
			return (this.Flags & flag) == flag;
		}

		public void ToggleFlag(UserFlags flag)
		{
			this.Flags ^= flag;
		}

		public UserData(ulong steam_id = 0uL)
		{
			this.SteamID = steam_id;
			this.AveragePing = -60000;
		}
	}
}
