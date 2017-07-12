namespace RustExtended
{
    using System;

    [Serializable]
    public class UserEconomy
    {
        public int AnimalsKilled;
        public ulong Balance;
        public int Deaths;
        public int MutantsKilled;
        public int PlayersKilled;
        public ulong SteamID;

        public UserEconomy(ulong steam_id, ulong balance)
        {
            this.SteamID = steam_id;
            this.Balance = balance;
            this.PlayersKilled = 0;
            this.MutantsKilled = 0;
            this.AnimalsKilled = 0;
            this.Deaths = 0;
        }

        public ulong Hash
        {
            get
            {
                ulong num = this.SteamID + (ulong)Balance.GetHashCode();
                num += (ulong)PlayersKilled.GetHashCode();
                num += (ulong)MutantsKilled.GetHashCode();
                num += (ulong)AnimalsKilled.GetHashCode();
                return (num + (ulong)Deaths.GetHashCode());
            }
        }
    }
}

