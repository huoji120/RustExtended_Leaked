namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Economy
    {
        [CompilerGenerated]
        private static bool bool_0;
        public static float CommandSendTax = 0f;
        public static ulong CostBear = 20L;
        public static ulong CostBoar = 2L;
        public static ulong CostChicken = 1L;
        public static ulong CostMutantBear = 50L;
        public static ulong CostMutantWolf = 0x19L;
        public static ulong CostRabbit = 1L;
        public static ulong CostStag = 5L;
        public static ulong CostWolf = 10L;
        public static string CurrencySign = "$";
        public static Dictionary<ulong, UserEconomy> Database;
        public static bool Enabled = false;
        public static bool FeeDeath = true;
        public static float FeeDeathPercent = 5f;
        public static bool FeeMurder = true;
        public static float FeeMurderPercent = 10f;
        public static bool FeeSleeper = true;
        public static bool FeeSuicide = true;
        public static float FeeSuicidePercent = 1f;
        public static Dictionary<ulong, ulong> Hashdata;
        public static bool PayMurder = true;
        public static float PayMurderPercent = 10f;
        public static string SQL_DELETE_ECONOMY = "DELETE FROM `db_users_economy` WHERE `user_id`={0} LIMIT 1;";
        public static string SQL_INSERT_ECONOMY = "REPLACE INTO `db_users_economy` (`user_id`, `balance`, `killed_players`, `killed_mutants`, `killed_animals`, `deaths`) VALUES ({0}, {1}, {2}, {3}, {4}, {5});";
        public static ulong StartBalance = 500L;

        public static UserEconomy Add(ulong steam_id, [Optional, DefaultParameterValue(0)] int players_killed, [Optional, DefaultParameterValue(0)] int mutants_killed, [Optional, DefaultParameterValue(0)] int animals_killed, [Optional, DefaultParameterValue(0)] int deaths)
        {
            if (Database.ContainsKey(steam_id))
            {
                return null;
            }
            UserEconomy economy = new UserEconomy(steam_id, StartBalance) {
                PlayersKilled = players_killed,
                MutantsKilled = mutants_killed,
                AnimalsKilled = animals_killed,
                Deaths = deaths
            };
            Database.Add(steam_id, economy);
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_INSERT_ECONOMY, new object[] { steam_id, economy.Balance, economy.PlayersKilled, economy.MutantsKilled, economy.AnimalsKilled, economy.Deaths }));
            }
            return economy;
        }

        public static void Balance(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (!Enabled)
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.NotAvailable", Sender, null), 5f);
            }
            else
            {
                string newValue = "0" + CurrencySign;
                if ((Sender != null) && !Database.ContainsKey(userData.SteamID))
                {
                    Add(userData.SteamID, 0, 0, 0, 0);
                }
                if (Sender != null)
                {
                    newValue = Database[userData.SteamID].Balance.ToString("N0") + CurrencySign;
                }
                if (((Args != null) && (Args.Length > 0)) && ((Sender == null) || Sender.admin))
                {
                    userData = Users.Find(Args[0]);
                    if (userData == null)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
                    }
                    else if (!Database.ContainsKey(userData.SteamID))
                    {
                        Broadcast.Notice(Sender, "✘", "Player \"" + Args[0] + "\" not have balance", 5f);
                    }
                    else
                    {
                        ulong balance = Database[userData.SteamID].Balance;
                        bool flag = (Args.Length > 1) && Args[1].StartsWith("+");
                        bool flag2 = (Args.Length > 1) && Args[1].StartsWith("-");
                        if (Args.Length > 1)
                        {
                            Args[1] = Args[1].Replace("+", "").Replace("-", "").Trim();
                        }
                        if ((Args.Length > 1) && ulong.TryParse(Args[1], out balance))
                        {
                            if (flag2)
                            {
                                BalanceSub(userData.SteamID, balance);
                            }
                            else if (flag)
                            {
                                BalanceAdd(userData.SteamID, balance);
                            }
                            else
                            {
                                Database[userData.SteamID].Balance = balance;
                            }
                            newValue = Database[userData.SteamID].Balance.ToString("N0") + CurrencySign;
                            Broadcast.Notice(Sender, CurrencySign, "Balance of \"" + userData.Username + "\" now " + newValue, 5f);
                        }
                        else
                        {
                            newValue = Database[userData.SteamID].Balance.ToString("N0") + CurrencySign;
                            Broadcast.Notice(Sender, CurrencySign, "Balance of \"" + userData.Username + "\" is " + newValue, 5f);
                        }
                    }
                }
                else
                {
                    Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", newValue), null, 0f);
                }
            }
        }

        public static void BalanceAdd(ulong steam_id, ulong amount)
        {
            if (!Database.ContainsKey(steam_id))
            {
                Add(steam_id, 0, 0, 0, 0);
            }
            ulong balance = Database[steam_id].Balance;
            if ((balance + amount) < balance)
            {
                balance = ulong.MaxValue;
            }
            else
            {
                balance += amount;
            }
            Database[steam_id].Balance = balance;
            SQL_Update(steam_id);
        }

        public static void BalanceSub(ulong steam_id, ulong amount)
        {
            if (!Database.ContainsKey(steam_id))
            {
                Add(steam_id, 0, 0, 0, 0);
            }
            if (Database[steam_id].Balance <= amount)
            {
                amount = 0L;
            }
            else
            {
                amount = Database[steam_id].Balance - amount;
            }
            Database[steam_id].Balance = amount;
            SQL_Update(steam_id);
        }

        public static bool Delete(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                return false;
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                MySQL.Update(string.Format(SQL_DELETE_ECONOMY, steam_id));
            }
            return true;
        }

        public static UserEconomy Get(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                Add(steam_id, 0, 0, 0, 0);
            }
            return Database[steam_id];
        }

        public static ulong GetBalance(ulong steam_id)
        {
            if (!Database.ContainsKey(steam_id))
            {
                Add(steam_id, 0, 0, 0, 0);
            }
            return Database[steam_id].Balance;
        }

        public static void HurtKilled(DamageEvent damage)
        {
            ulong amount = 0L;
            string key = "";
            ulong costChicken = 0L;
            string displayName = "";
            PlayerClient client = damage.victim.client;
            PlayerClient client2 = damage.attacker.client;
            bool flag = !(damage.victim.idMain is Character);
            bool flag2 = !(damage.attacker.idMain is Character);
            UserData data = (client != null) ? Users.GetBySteamID(client.userID) : null;
            UserData data2 = (client2 != null) ? Users.GetBySteamID(client2.userID) : null;
            if (data == null)
            {
            }
            ClanData data3 = (data2 != null) ? data2.Clan : null;
            if (!flag2 || (client2 != null))
            {
                if (client2 != null)
                {
                    displayName = damage.attacker.client.netUser.displayName;
                }
                else
                {
                    displayName = Helper.NiceName(damage.attacker.character.name);
                    Config.Get("NAMES." + ((data == null) ? Core.Languages[0] : data.Language), displayName, ref displayName, true);
                }
                if (flag)
                {
                    if (!(damage.victim.idMain is SleepingAvatar) || !FeeSleeper)
                    {
                        return;
                    }
                    SleepingAvatar idMain = damage.victim.idMain as SleepingAvatar;
                    if (idMain == null)
                    {
                        return;
                    }
                    UserData bySteamID = Users.GetBySteamID(idMain.ownerID);
                    if (bySteamID == null)
                    {
                        return;
                    }
                    ulong balance = GetBalance(bySteamID.SteamID);
                    key = bySteamID.Username;
                    if (FeeMurder)
                    {
                        amount = (ulong) Math.Abs((float) ((balance * FeeMurderPercent) / 100f));
                    }
                    if (PayMurder)
                    {
                        costChicken = (ulong) Math.Abs((float) ((balance * PayMurderPercent) / 100f));
                    }
                    if (((costChicken > 0L) && Clans.Enabled) && (data3 != null))
                    {
                        if (data3.Level.BonusMembersPayMurder > 0)
                        {
                            costChicken += (costChicken * data3.Level.BonusMembersPayMurder) / ((ulong) 100L);
                        }
                        if (data3.Tax > 0)
                        {
                            ulong num4 = (costChicken * data3.Tax) / ((ulong) 100L);
                            data3.Balance += num4;
                            costChicken -= num4;
                        }
                    }
                    if (costChicken > 0L)
                    {
                        BalanceAdd(client2.userID, costChicken);
                    }
                    if (amount > 0L)
                    {
                        BalanceSub(bySteamID.SteamID, amount);
                    }
                }
                else if (client != null)
                {
                    key = damage.victim.client.netUser.displayName;
                    ulong num5 = GetBalance(client.userID);
                    if (client2 == null)
                    {
                        if (FeeDeath)
                        {
                            amount = (ulong) Math.Abs((float) ((num5 * FeeDeathPercent) / 100f));
                        }
                    }
                    else if ((client2 != client) && !flag2)
                    {
                        if ((client2 != client) && !flag2)
                        {
                            UserEconomy economy1 = Get(client2.userID);
                            economy1.PlayersKilled++;
                            if (FeeMurder)
                            {
                                amount = (ulong) Math.Abs((float) ((num5 * FeeMurderPercent) / 100f));
                            }
                            if (PayMurder)
                            {
                                costChicken = (ulong) Math.Abs((float) ((num5 * PayMurderPercent) / 100f));
                            }
                        }
                    }
                    else if (FeeSuicide)
                    {
                        amount = (ulong) Math.Abs((float) ((num5 * FeeSuicidePercent) / 100f));
                    }
                    if (((costChicken > 0L) && Clans.Enabled) && (data3 != null))
                    {
                        if (data3.Level.BonusMembersPayMurder > 0)
                        {
                            costChicken += (costChicken * data3.Level.BonusMembersPayMurder) / ((ulong) 100L);
                        }
                        if (data3.Tax > 0)
                        {
                            ulong num6 = (costChicken * data3.Tax) / ((ulong) 100L);
                            data3.Balance += num6;
                            costChicken -= num6;
                        }
                    }
                    if (costChicken > 0L)
                    {
                        BalanceAdd(client2.userID, costChicken);
                    }
                    if (amount > 0L)
                    {
                        BalanceSub(client.userID, amount);
                    }
                    UserEconomy economy2 = Get(client.userID);
                    economy2.Deaths++;
                }
                else if (client2 != null)
                {
                    key = Helper.NiceName(damage.victim.character.name);
                    if (key.Equals("Chicken", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostChicken != 0L)
                        {
                            costChicken = CostChicken;
                        }
                        UserEconomy economy3 = Get(client2.userID);
                        economy3.AnimalsKilled++;
                    }
                    else if (key.Equals("Rabbit", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostRabbit != 0L)
                        {
                            costChicken = CostRabbit;
                        }
                        UserEconomy economy4 = Get(client2.userID);
                        economy4.AnimalsKilled++;
                    }
                    else if (key.Equals("Boar", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostBoar != 0L)
                        {
                            costChicken = CostBoar;
                        }
                        UserEconomy economy5 = Get(client2.userID);
                        economy5.AnimalsKilled++;
                    }
                    else if (key.Equals("Stag", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostStag != 0L)
                        {
                            costChicken = CostStag;
                        }
                        UserEconomy economy6 = Get(client2.userID);
                        economy6.AnimalsKilled++;
                    }
                    else if (key.Equals("Wolf", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostWolf != 0L)
                        {
                            costChicken = CostWolf;
                        }
                        UserEconomy economy7 = Get(client2.userID);
                        economy7.AnimalsKilled++;
                    }
                    else if (key.Equals("Bear", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostBear != 0L)
                        {
                            costChicken = CostBear;
                        }
                        UserEconomy economy8 = Get(client2.userID);
                        economy8.AnimalsKilled++;
                    }
                    else if (key.Equals("Mutant Wolf", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostMutantWolf != 0L)
                        {
                            costChicken = CostMutantWolf;
                        }
                        UserEconomy economy9 = Get(client2.userID);
                        economy9.MutantsKilled++;
                    }
                    else if (key.Equals("Mutant Bear", StringComparison.OrdinalIgnoreCase))
                    {
                        if (CostMutantBear != 0L)
                        {
                            costChicken = CostMutantBear;
                        }
                        UserEconomy economy10 = Get(client2.userID);
                        economy10.MutantsKilled++;
                    }
                    else
                    {
                        ConsoleSystem.LogWarning("[WARNING] Economy: Creature '" + key + "' not have cost of death.");
                    }
                    Config.Get("NAMES." + ((data2 == null) ? Core.Languages[0] : data2.Language), key, ref key, true);
                    if (((costChicken > 0L) && Clans.Enabled) && (data3 != null))
                    {
                        if (data3.Level.BonusMembersPayMurder > 0)
                        {
                            costChicken += (costChicken * data3.Level.BonusMembersPayMurder) / ((ulong) 100L);
                        }
                        if (data3.Tax > 0)
                        {
                            ulong num7 = (costChicken * data3.Tax) / ((ulong) 100L);
                            data3.Balance += num7;
                            costChicken -= num7;
                        }
                    }
                    if (costChicken > 0L)
                    {
                        BalanceAdd(client2.userID, costChicken);
                    }
                }
                if ((client2 != null) && (costChicken > 0L))
                {
                    string text = Config.GetMessage("Economy.PlayerDeath.Pay", client2.netUser, null);
                    if (flag)
                    {
                        text = Config.GetMessage("Economy.SleeperDeath.Pay", client2.netUser, null);
                    }
                    text = text.Replace("%DEATHPAY%", costChicken.ToString("N0") + CurrencySign).Replace("%VICTIM%", key);
                    Broadcast.Message(client2.netPlayer, text, null, 0f);
                }
                if ((client != null) && (amount > 0L))
                {
                    string str4;
                    str4 = str4 = Config.GetMessage("Economy.PlayerDeath.Fee", client.netUser, null);
                    if ((client2 == client) || flag2)
                    {
                        str4 = Config.GetMessage("Economy.PlayerSuicide.Fee", client.netUser, null);
                    }
                    str4 = str4.Replace("%DEATHFEE%", amount.ToString("N0") + CurrencySign).Replace("%KILLER%", displayName).Replace("%VICTIM%", key);
                    Broadcast.Message(client.netPlayer, str4, null, 0f);
                }
            }
        }

        public static void Initialize()
        {
            Initialized = false;
            if (Database == null)
            {
                Database = new Dictionary<ulong, UserEconomy>();
            }
            if (Hashdata == null)
            {
                Hashdata = new Dictionary<ulong, ulong>();
            }
            Config.Get("ECONOMY", "Enabled", ref Enabled, true);
            Config.Get("ECONOMY", "CurrencySign", ref CurrencySign, true);
            Config.Get("ECONOMY", "StartBalance", ref StartBalance, true);
            Config.Get("ECONOMY", "Cost.Chicken", ref CostChicken, true);
            Config.Get("ECONOMY", "Cost.Rabbit", ref CostRabbit, true);
            Config.Get("ECONOMY", "Cost.Boar", ref CostBoar, true);
            Config.Get("ECONOMY", "Cost.Stag", ref CostStag, true);
            Config.Get("ECONOMY", "Cost.Wolf", ref CostWolf, true);
            Config.Get("ECONOMY", "Cost.Bear", ref CostBear, true);
            Config.Get("ECONOMY", "Cost.MutantWolf", ref CostMutantWolf, true);
            Config.Get("ECONOMY", "Cost.MutantBear", ref CostMutantBear, true);
            Config.Get("ECONOMY", "Fee.Sleeper", ref FeeSleeper, true);
            Config.Get("ECONOMY", "Fee.Suicide", ref FeeSuicide, true);
            Config.Get("ECONOMY", "Fee.Suicide.Percent", ref FeeSuicidePercent, true);
            Config.Get("ECONOMY", "Fee.Death", ref FeeDeath, true);
            Config.Get("ECONOMY", "Fee.Death.Percent", ref FeeDeathPercent, true);
            Config.Get("ECONOMY", "Fee.Murder", ref FeeMurder, true);
            Config.Get("ECONOMY", "Fee.Murder.Percent", ref FeeMurderPercent, true);
            Config.Get("ECONOMY", "Pay.Murder", ref PayMurder, true);
            Config.Get("ECONOMY", "Pay.Murder.Percent", ref PayMurderPercent, true);
            Config.Get("ECONOMY", "Command.Send.Tax", ref CommandSendTax, true);
            if (Enabled)
            {
                Shop.Initialize();
            }
            Initialized = true;
        }

        public static bool RunCommand(NetUser netUser, UserData userData, string Command, string[] Args)
        {
            switch (Command)
            {
                case "balance":
                    Balance(netUser, userData, Command, Args);
                    break;

                case "money":
                    Balance(netUser, userData, Command, Args);
                    break;

                case "send":
                    Send(netUser, userData, Command, Args);
                    break;

                case "shop":
                    ShopList(netUser, userData, Command, Args);
                    break;

                case "buy":
                    ShopBuy(netUser, userData, Command, Args);
                    break;

                case "sell":
                    ShopSell(netUser, userData, Command, Args);
                    break;

                default:
                    return false;
            }
            return true;
        }

        public static void Send(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (!Enabled)
            {
                Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Economy.NotAvailable", Sender, null), 5f);
            }
            else if ((Args != null) && (Args.Length != 0))
            {
                UserData data = Users.Find(Args[0]);
                if (data == null)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
                }
                else if (data == userData)
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Send.Himself", Sender, null), 5f);
                }
                else
                {
                    if (!Database.ContainsKey(userData.SteamID))
                    {
                        Add(userData.SteamID, 0, 0, 0, 0);
                    }
                    if (!Database.ContainsKey(data.SteamID))
                    {
                        Add(data.SteamID, 0, 0, 0, 0);
                    }
                    NetUser player = NetUser.FindByUserID(data.SteamID);
                    string newValue = "";
                    ulong result = 0L;
                    if ((Args.Length > 1) && !ulong.TryParse(Args[1], out result))
                    {
                        result = 0L;
                    }
                    if (result < 1L)
                    {
                        result = 0L;
                    }
                    string str2 = result.ToString("N0") + CurrencySign;
                    if (result == 0L)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Send.NoAmount", Sender, null), 5f);
                    }
                    else if (Database[userData.SteamID].Balance < result)
                    {
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Send.NoHaveAmount", Sender, null).Replace("%SENTAMOUNT%", str2), 5f);
                    }
                    else
                    {
                        BalanceSub(userData.SteamID, result);
                        if (CommandSendTax > 0f)
                        {
                            result -= ((ulong) (result * CommandSendTax)) / 100L;
                            str2 = result.ToString("N0") + CurrencySign;
                        }
                        BalanceAdd(data.SteamID, result);
                        newValue = Database[userData.SteamID].Balance.ToString("N0") + CurrencySign;
                        Broadcast.Notice(Sender, CurrencySign, Config.GetMessage("Economy.Send.SentToPlayer", null, data.Username).Replace("%SENTAMOUNT%", str2), 5f);
                        Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", newValue), null, 0f);
                        if (player != null)
                        {
                            newValue = Database[data.SteamID].Balance.ToString("N0") + CurrencySign;
                            Broadcast.Notice(player, CurrencySign, Config.GetMessage("Economy.Send.SentFromPlayer", null, userData.Username).Replace("%SENTAMOUNT%", str2), 5f);
                            Broadcast.Message(player, Config.GetMessage("Economy.Balance", player, null).Replace("%BALANCE%", newValue), null, 0f);
                        }
                    }
                }
            }
            else
            {
                Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
            }
        }

        public static void ShopBuy(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (Shop.Enabled && Shop.CanBuy)
            {
                if (Shop.TradeZoneOnly && ((userData.Zone == null) || !userData.Zone.CanTrade))
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NoTradeZone", Sender, null), 5f);
                }
                else if ((Args != null) && (Args.Length != 0))
                {
                    ShopItem item = null;
                    int result = 0;
                    if (int.TryParse(Args[0], out result))
                    {
                        item = Shop.FindItem(result);
                    }
                    else
                    {
                        item = Shop.FindItem(Args[0]);
                    }
                    if ((item != null) && (item.SellPrice != -1))
                    {
                        Inventory component = Sender.playerClient.controllable.GetComponent<Inventory>();
                        if ((component != null) && !component.noVacantSlots)
                        {
                            int num2 = item.SellPrice / item.Quantity;
                            int quantity = item.Quantity;
                            if ((Args.Length > 1) && !int.TryParse(Args[1], out quantity))
                            {
                                quantity = item.Quantity;
                            }
                            if (quantity < 1)
                            {
                                quantity = item.Quantity;
                            }
                            ulong amount = (ulong) (num2 * quantity);
                            if (amount > GetBalance(Sender.userID))
                            {
                                string text = Config.GetMessage("Economy.Shop.Buy.NotEnoughBalance", Sender, null).Replace("%TOTALPRICE%", amount.ToString("N0") + CurrencySign).Replace("%ITEMNAME%", item.Name);
                                Broadcast.Notice(Sender, CurrencySign, text, 5f);
                            }
                            else
                            {
                                quantity = Helper.GiveItem(Sender.playerClient, item.itemData, quantity, item.Slots);
                                if (quantity == 0)
                                {
                                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Player.Inventory.IsFull", Sender, null), 5f);
                                }
                                else
                                {
                                    string newValue = "\"" + item.itemData.name + "\"";
                                    if (quantity > 1)
                                    {
                                        newValue = quantity.ToString() + " " + newValue;
                                    }
                                    amount = (ulong) (quantity * num2);
                                    BalanceSub(Sender.userID, amount);
                                    string str4 = Config.GetMessage("Economy.Shop.Buy.ItemPurchased", Sender, null).Replace("%TOTALPRICE%", amount.ToString("N0") + CurrencySign).Replace("%ITEMNAME%", newValue);
                                    Broadcast.Notice(Sender, CurrencySign, str4, 5f);
                                    Balance(Sender, userData, "balance", null);
                                }
                            }
                        }
                        else
                        {
                            Broadcast.Notice(Sender, "✘", Config.GetMessage("Player.Inventory.IsFull", Sender, null), 5f);
                        }
                    }
                    else
                    {
                        string str = (item != null) ? item.Name : Args[0];
                        Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.Buy.ItemNotAvailable", Sender, null).Replace("%ITEMNAME%", str), 5f);
                    }
                }
                else
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.Buy.NotAvailable", Sender, null), 5f);
            }
        }

        public static void ShopList(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (!Shop.Enabled)
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NotAvailable", Sender, null), 5f);
            }
            else if (Shop.TradeZoneOnly && ((userData.Zone == null) || !userData.Zone.CanTrade))
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NoTradeZone", Sender, null), 5f);
            }
            else
            {
                int result = 0;
                string str = null;
                System.Collections.Generic.List<ShopItem> items = null;
                if ((Args != null) && (Args.Length > 0))
                {
                    if (int.TryParse(Args[0], out result))
                    {
                        items = Shop.GetItems(result, out str);
                    }
                    if (items == null)
                    {
                        items = Shop.GetItems(Args[0], out result);
                    }
                }
                if (items == null)
                {
                    items = Shop.dictionary_0[Shop.shopGroup_0];
                }
                foreach (ShopItem item in items)
                {
                    string newValue = (item.BuyPrice > 0) ? (item.BuyPrice + CurrencySign) : "None";
                    string str3 = (item.SellPrice > 0) ? (item.SellPrice + CurrencySign) : "None";
                    string text = Config.GetMessage("Economy.Shop.ListItem", Sender, null).Replace("%INDEX%", item.Index.ToString()).Replace("%ITEMNAME%", item.Name).Replace("%SELLPRICE%", str3).Replace("%BUYPRICE%", newValue).Replace("%QUANTITY%", item.Quantity.ToString());
                    Broadcast.Message(Sender, text, null, 0f);
                }
                if (result == 0)
                {
                    foreach (ShopGroup group in Shop.dictionary_0.Keys)
                    {
                        if ((group.Name != null) && (group.Index != 0))
                        {
                            string str5 = Config.GetMessage("Economy.Shop.ListGroup", Sender, null).Replace("%INDEX%", group.Index.ToString()).Replace("%GROUPNAME%", group.Name);
                            Broadcast.Message(Sender, str5, null, 0f);
                        }
                    }
                }
                Broadcast.Message(Sender, Config.GetMessage("Economy.Shop.Help", Sender, null), null, 0f);
            }
        }

        public static void ShopSell(NetUser Sender, UserData userData, string Command, string[] Args)
        {
            if (Shop.Enabled && Shop.CanSell)
            {
                if (Shop.TradeZoneOnly && ((userData.Zone == null) || !userData.Zone.CanTrade))
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NoTradeZone", Sender, null), 5f);
                }
                else if ((Args != null) && (Args.Length != 0))
                {
                    ShopItem item = null;
                    int result = 0;
                    int num2 = 0;
                    Inventory component = Sender.playerClient.controllable.GetComponent<Inventory>();
                    if (Args[0].Equals("ALL", StringComparison.OrdinalIgnoreCase))
                    {
                        ulong amount = 0L;
                        System.Collections.Generic.List<IInventoryItem> list = new System.Collections.Generic.List<IInventoryItem>();
                        Inventory.OccupiedIterator occupiedIterator = component.occupiedIterator;
                        while (occupiedIterator.Next())
                        {
                            item = Shop.FindItem(occupiedIterator.item.datablock.name);
                            if ((item != null) && (item.BuyPrice != -1))
                            {
                                int num4 = occupiedIterator.item.datablock._splittable ? occupiedIterator.item.uses : 1;
                                ulong num5 = (ulong) ((item.BuyPrice / item.Quantity) * num4);
                                amount += num5;
                                list.Add(occupiedIterator.item);
                            }
                        }
                        if (list.Count > 0)
                        {
                            foreach (IInventoryItem item2 in list)
                            {
                                component.RemoveItem(item2);
                            }
                            if (amount > 0L)
                            {
                                BalanceAdd(Sender.userID, amount);
                            }
                            string text = Config.GetMessage("Economy.Shop.Sell.AllSold", Sender, null).Replace("%TOTALPRICE%", amount.ToString("N0") + CurrencySign).Replace("%TOTALAMOUNT%", list.Count.ToString());
                            Broadcast.Notice(Sender, CurrencySign, text, 5f);
                        }
                        else
                        {
                            Broadcast.Notice(Sender, CurrencySign, Config.GetMessage("Economy.Shop.Sell.NoNothing", Sender, null), 5f);
                        }
                    }
                    else
                    {
                        if (int.TryParse(Args[0], out result))
                        {
                            item = Shop.FindItem(result);
                        }
                        else
                        {
                            item = Shop.FindItem(Args[0]);
                        }
                        if ((item != null) && (item.BuyPrice != -1))
                        {
                            int num6 = item.BuyPrice / item.Quantity;
                            int quantity = item.Quantity;
                            if ((Args.Length > 1) && !int.TryParse(Args[1], out quantity))
                            {
                                quantity = item.Quantity;
                            }
                            if (quantity < 1)
                            {
                                quantity = item.Quantity;
                            }
                            num2 = Helper.InventoryItemCount(component, item.itemData);
                            if (num2 == 0)
                            {
                                Broadcast.Notice(Sender, CurrencySign, Config.GetMessage("Economy.Shop.Sell.NotEnoughItem", Sender, null).Replace("%ITEMNAME%", item.Name), 5f);
                            }
                            else
                            {
                                if (quantity > num2)
                                {
                                    quantity = num2;
                                }
                                num2 = Helper.InventoryItemRemove(component, item.itemData, quantity);
                                string newValue = "\"" + item.Name + "\"";
                                if (num2 > 1)
                                {
                                    newValue = num2.ToString() + " " + newValue;
                                }
                                ulong num8 = (ulong) (num2 * num6);
                                BalanceAdd(Sender.userID, num8);
                                string str4 = Config.GetMessage("Economy.Shop.Sell.ItemSold", Sender, null).Replace("%TOTALPRICE%", num8.ToString("N0") + CurrencySign).Replace("%ITEMNAME%", newValue);
                                Broadcast.Notice(Sender, CurrencySign, str4, 5f);
                                Balance(Sender, userData, "balance", null);
                            }
                        }
                        else
                        {
                            string str2 = (item != null) ? item.Name : Args[0];
                            Broadcast.Notice(Sender, CurrencySign, Config.GetMessage("Economy.Shop.Sell.ItemNotAvailable", Sender, null).Replace("%ITEMNAME%", str2), 5f);
                        }
                    }
                }
                else
                {
                    Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
                }
            }
            else
            {
                Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.Sell.NotAvailable", Sender, null), 5f);
            }
        }

        public static void SQL_Update(ulong user_id)
        {
            if (Core.DatabaseType.Equals("MYSQL") && Database.ContainsKey(user_id))
            {
                MySQL.Update(string.Format(SQL_INSERT_ECONOMY, new object[] { Database[user_id].SteamID, Database[user_id].Balance, Database[user_id].PlayersKilled, Database[user_id].MutantsKilled, Database[user_id].AnimalsKilled, Database[user_id].Deaths }));
            }
        }

        public static bool Initialized
        {
            [CompilerGenerated]
            get
            {
                return bool_0;
            }
            [CompilerGenerated]
            private set
            {
                bool_0 = value;
            }
        }
    }
}

