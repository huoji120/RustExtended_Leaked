using System;

namespace RustExtended
{
	[Serializable]
	public class UserEconomy
	{
		public ulong SteamID;

		public ulong Balance;

		public int PlayersKilled;

		public int MutantsKilled;

		public int AnimalsKilled;

		public int Deaths;

		public ulong Hash
		{
			get
			{
				ulong num = this.SteamID;
				num += (ulong)((long)this.Balance.GetHashCode());
				num += (ulong)((long)this.PlayersKilled.GetHashCode());
				num += (ulong)((long)this.MutantsKilled.GetHashCode());
				num += (ulong)((long)this.AnimalsKilled.GetHashCode());
				return num + (ulong)((long)this.Deaths.GetHashCode());
			}
		}

		public UserEconomy(ulong steam_id, ulong balance)
		{
			this.SteamID = steam_id;
			this.Balance = balance;
			this.PlayersKilled = 0;
			this.MutantsKilled = 0;
			this.AnimalsKilled = 0;
			this.Deaths = 0;
		}
	}
}
