using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RustExtended
{
	public class Economy
	{
		public static string SQL_INSERT_ECONOMY = "REPLACE INTO `db_users_economy` (`user_id`, `balance`, `killed_players`, `killed_mutants`, `killed_animals`, `deaths`) VALUES ({0}, {1}, {2}, {3}, {4}, {5});";

		public static string SQL_DELETE_ECONOMY = "DELETE FROM `db_users_economy` WHERE `user_id`={0} LIMIT 1;";

		public static bool Enabled = false;

		public static string CurrencySign = "$";

		public static ulong StartBalance = 500uL;

		public static ulong CostChicken = 1uL;

		public static ulong CostRabbit = 1uL;

		public static ulong CostBoar = 2uL;

		public static ulong CostStag = 5uL;

		public static ulong CostWolf = 10uL;

		public static ulong CostBear = 20uL;

		public static ulong CostMutantWolf = 25uL;

		public static ulong CostMutantBear = 50uL;

		public static bool FeeSleeper = true;

		public static bool FeeSuicide = true;

		public static float FeeSuicidePercent = 1f;

		public static bool FeeDeath = true;

		public static float FeeDeathPercent = 5f;

		public static bool FeeMurder = true;

		public static float FeeMurderPercent = 10f;

		public static bool PayMurder = true;

		public static float PayMurderPercent = 10f;

		public static float CommandSendTax = 0f;

		public static Dictionary<ulong, UserEconomy> Database;

		public static Dictionary<ulong, ulong> Hashdata;

		[CompilerGenerated]
		private static bool bool_0;

		private static Dictionary<string, int> asad;

		public static bool Initialized
		{
			get;
			private set;
		}

		public static void Initialize()
		{
			Economy.Initialized = false;
			if (Economy.Database == null)
			{
				Economy.Database = new Dictionary<ulong, UserEconomy>();
			}
			if (Economy.Hashdata == null)
			{
				Economy.Hashdata = new Dictionary<ulong, ulong>();
			}
			Config.Get("ECONOMY", "Enabled", ref Economy.Enabled, true);
			Config.Get("ECONOMY", "CurrencySign", ref Economy.CurrencySign, true);
			Config.Get("ECONOMY", "StartBalance", ref Economy.StartBalance, true);
			Config.Get("ECONOMY", "Cost.Chicken", ref Economy.CostChicken, true);
			Config.Get("ECONOMY", "Cost.Rabbit", ref Economy.CostRabbit, true);
			Config.Get("ECONOMY", "Cost.Boar", ref Economy.CostBoar, true);
			Config.Get("ECONOMY", "Cost.Stag", ref Economy.CostStag, true);
			Config.Get("ECONOMY", "Cost.Wolf", ref Economy.CostWolf, true);
			Config.Get("ECONOMY", "Cost.Bear", ref Economy.CostBear, true);
			Config.Get("ECONOMY", "Cost.MutantWolf", ref Economy.CostMutantWolf, true);
			Config.Get("ECONOMY", "Cost.MutantBear", ref Economy.CostMutantBear, true);
			Config.Get("ECONOMY", "Fee.Sleeper", ref Economy.FeeSleeper, true);
			Config.Get("ECONOMY", "Fee.Suicide", ref Economy.FeeSuicide, true);
			Config.Get("ECONOMY", "Fee.Suicide.Percent", ref Economy.FeeSuicidePercent, true);
			Config.Get("ECONOMY", "Fee.Death", ref Economy.FeeDeath, true);
			Config.Get("ECONOMY", "Fee.Death.Percent", ref Economy.FeeDeathPercent, true);
			Config.Get("ECONOMY", "Fee.Murder", ref Economy.FeeMurder, true);
			Config.Get("ECONOMY", "Fee.Murder.Percent", ref Economy.FeeMurderPercent, true);
			Config.Get("ECONOMY", "Pay.Murder", ref Economy.PayMurder, true);
			Config.Get("ECONOMY", "Pay.Murder.Percent", ref Economy.PayMurderPercent, true);
			Config.Get("ECONOMY", "Command.Send.Tax", ref Economy.CommandSendTax, true);
			if (Economy.Enabled)
			{
				Shop.Initialize();
			}
			Economy.Initialized = true;
			if (Core.dlqshop)
			{
				MySQL.Query("DELETE FROM shop", false);
				for (int i = 1; i < 13; i++)
				{
					string text = null;
					List<ShopItem> list = Shop.GetItems(i, out text);
					if (list == null)
					{
						list = Shop.dictionary_0[Shop.shopGroup_0];
					}
					foreach (ShopItem current in list)
					{
						string arg_2C0_0 = (current.BuyPrice > 0) ? (current.BuyPrice + Economy.CurrencySign) : "None";
						string text2 = (current.SellPrice > 0) ? (current.SellPrice + Economy.CurrencySign) : "None";
						MySQL.Query(string.Concat(new string[]
						{
							"insert into shop(name,jiage) values('",
							current.Name,
							"','",
							text2,
							"')"
						}), false);
					}
				}
			}
		}

		public static void SQL_Update(ulong user_id)
		{
			if (Core.DatabaseType.Equals("MYSQL"))
			{
				if (Economy.Database.ContainsKey(user_id))
				{
					MySQL.Update(string.Format(Economy.SQL_INSERT_ECONOMY, new object[]
					{
						Economy.Database[user_id].SteamID,
						Economy.Database[user_id].Balance,
						Economy.Database[user_id].PlayersKilled,
						Economy.Database[user_id].MutantsKilled,
						Economy.Database[user_id].AnimalsKilled,
						Economy.Database[user_id].Deaths
					}));
				}
			}
		}

		public static UserEconomy Add(ulong steam_id, int players_killed = 0, int mutants_killed = 0, int animals_killed = 0, int deaths = 0)
		{
			UserEconomy result;
			if (Economy.Database.ContainsKey(steam_id))
			{
				result = null;
			}
			else
			{
				UserEconomy userEconomy = new UserEconomy(steam_id, Economy.StartBalance);
				userEconomy.PlayersKilled = players_killed;
				userEconomy.MutantsKilled = mutants_killed;
				userEconomy.AnimalsKilled = animals_killed;
				userEconomy.Deaths = deaths;
				Economy.Database.Add(steam_id, userEconomy);
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Economy.SQL_INSERT_ECONOMY, new object[]
					{
						steam_id,
						userEconomy.Balance,
						userEconomy.PlayersKilled,
						userEconomy.MutantsKilled,
						userEconomy.AnimalsKilled,
						userEconomy.Deaths
					}));
				}
				result = userEconomy;
			}
			return result;
		}

		public static bool Delete(ulong steam_id)
		{
			bool result;
			if (!Economy.Database.ContainsKey(steam_id))
			{
				result = false;
			}
			else
			{
				if (Core.DatabaseType.Equals("MYSQL"))
				{
					MySQL.Update(string.Format(Economy.SQL_DELETE_ECONOMY, steam_id));
				}
				result = true;
			}
			return result;
		}

		public static UserEconomy Get(ulong steam_id)
		{
			if (!Economy.Database.ContainsKey(steam_id))
			{
				Economy.Add(steam_id, 0, 0, 0, 0);
			}
			return Economy.Database[steam_id];
		}

		public static ulong GetBalance(ulong steam_id)
		{
			if (!Economy.Database.ContainsKey(steam_id))
			{
				Economy.Add(steam_id, 0, 0, 0, 0);
			}
			return Economy.Database[steam_id].Balance;
		}

		public static void BalanceAdd(ulong steam_id, ulong amount)
		{
			if (!Economy.Database.ContainsKey(steam_id))
			{
				Economy.Add(steam_id, 0, 0, 0, 0);
			}
			ulong num = Economy.Database[steam_id].Balance;
			if (num + amount < num)
			{
				num = 18446744073709551615uL;
			}
			else
			{
				num += amount;
			}
			Economy.Database[steam_id].Balance = num;
			Economy.SQL_Update(steam_id);
		}

		public static void BalanceSub(ulong steam_id, ulong amount)
		{
			if (!Economy.Database.ContainsKey(steam_id))
			{
				Economy.Add(steam_id, 0, 0, 0, 0);
			}
			if (Economy.Database[steam_id].Balance <= amount)
			{
				amount = 0uL;
			}
			else
			{
				amount = Economy.Database[steam_id].Balance - amount;
			}
			Economy.Database[steam_id].Balance = amount;
			Economy.SQL_Update(steam_id);
		}

		public static void HurtKilled(DamageEvent damage)
		{
			ulong num = 0uL;
			string text = "";
			ulong num2 = 0uL;
			string text2 = "";
			PlayerClient client = damage.victim.client;
			PlayerClient client2 = damage.attacker.client;
			bool flag = !(damage.victim.idMain is Character);
			bool flag2 = !(damage.attacker.idMain is Character);
			UserData userData = (client != null) ? Users.GetBySteamID(client.userID) : null;
			UserData userData2 = (client2 != null) ? Users.GetBySteamID(client2.userID) : null;
			if (userData == null)
			{
			}
			ClanData clanData = (userData2 != null) ? userData2.Clan : null;
			if (!flag2 || !(client2 == null))
			{
				if (client2 != null)
				{
					text2 = damage.attacker.client.netUser.displayName;
				}
				else
				{
					text2 = Helper.NiceName(damage.attacker.character.name);
					Config.Get("NAMES." + ((userData == null) ? Core.Languages[0] : userData.Language), text2, ref text2, true);
				}
				if (flag)
				{
					if (!(damage.victim.idMain is SleepingAvatar) || !Economy.FeeSleeper)
					{
						return;
					}
					SleepingAvatar sleepingAvatar = damage.victim.idMain as SleepingAvatar;
					if (sleepingAvatar == null)
					{
						return;
					}
					UserData bySteamID = Users.GetBySteamID(sleepingAvatar.ownerID);
					if (bySteamID == null)
					{
						return;
					}
					ulong balance = Economy.GetBalance(bySteamID.SteamID);
					text = bySteamID.Username;
					if (Economy.FeeMurder)
					{
						num = (ulong)Math.Abs(balance * Economy.FeeMurderPercent / 100f);
					}
					if (Economy.PayMurder)
					{
						num2 = (ulong)Math.Abs(balance * Economy.PayMurderPercent / 100f);
					}
					if (num2 > 0uL && Clans.Enabled && clanData != null)
					{
						if (clanData.Level.BonusMembersPayMurder > 0u)
						{
							num2 += num2 * (ulong)clanData.Level.BonusMembersPayMurder / 100uL;
						}
						if (clanData.Tax > 0u)
						{
							ulong num3 = num2 * (ulong)clanData.Tax / 100uL;
							clanData.Balance += num3;
							num2 -= num3;
						}
					}
					if (num2 > 0uL)
					{
						Economy.BalanceAdd(client2.userID, num2);
					}
					if (num > 0uL)
					{
						Economy.BalanceSub(bySteamID.SteamID, num);
					}
				}
				else if (client != null)
				{
					text = damage.victim.client.netUser.displayName;
					ulong balance2 = Economy.GetBalance(client.userID);
					if (client2 == null)
					{
						if (Economy.FeeDeath)
						{
							num = (ulong)Math.Abs(balance2 * Economy.FeeDeathPercent / 100f);
						}
					}
					else if (!(client2 == client) && !flag2)
					{
						if (client2 != client && !flag2)
						{
							Economy.Get(client2.userID).PlayersKilled++;
							if (Economy.FeeMurder)
							{
								num = (ulong)Math.Abs(balance2 * Economy.FeeMurderPercent / 100f);
							}
							if (Economy.PayMurder)
							{
								num2 = (ulong)Math.Abs(balance2 * Economy.PayMurderPercent / 100f);
							}
						}
					}
					else if (Economy.FeeSuicide)
					{
						num = (ulong)Math.Abs(balance2 * Economy.FeeSuicidePercent / 100f);
					}
					if (num2 > 0uL && Clans.Enabled && clanData != null)
					{
						if (clanData.Level.BonusMembersPayMurder > 0u)
						{
							num2 += num2 * (ulong)clanData.Level.BonusMembersPayMurder / 100uL;
						}
						if (clanData.Tax > 0u)
						{
							ulong num4 = num2 * (ulong)clanData.Tax / 100uL;
							clanData.Balance += num4;
							num2 -= num4;
						}
					}
					if (num2 > 0uL)
					{
						Economy.BalanceAdd(client2.userID, num2);
					}
					if (num > 0uL)
					{
						Economy.BalanceSub(client.userID, num);
					}
					Economy.Get(client.userID).Deaths++;
				}
				else if (client2 != null)
				{
					text = Helper.NiceName(damage.victim.character.name);
					if (text.Equals("Chicken", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostChicken != 0uL)
						{
							num2 = Economy.CostChicken;
						}
						Economy.Get(client2.userID).AnimalsKilled++;
					}
					else if (text.Equals("Rabbit", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostRabbit != 0uL)
						{
							num2 = Economy.CostRabbit;
						}
						Economy.Get(client2.userID).AnimalsKilled++;
					}
					else if (text.Equals("Boar", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostBoar != 0uL)
						{
							num2 = Economy.CostBoar;
						}
						Economy.Get(client2.userID).AnimalsKilled++;
					}
					else if (text.Equals("Stag", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostStag != 0uL)
						{
							num2 = Economy.CostStag;
						}
						Economy.Get(client2.userID).AnimalsKilled++;
					}
					else if (text.Equals("Wolf", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostWolf != 0uL)
						{
							num2 = Economy.CostWolf;
						}
						Economy.Get(client2.userID).AnimalsKilled++;
					}
					else if (text.Equals("Bear", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostBear != 0uL)
						{
							num2 = Economy.CostBear;
						}
						Economy.Get(client2.userID).AnimalsKilled++;
					}
					else if (text.Equals("Mutant Wolf", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostMutantWolf != 0uL)
						{
							num2 = Economy.CostMutantWolf;
						}
						Economy.Get(client2.userID).MutantsKilled++;
					}
					else if (text.Equals("Mutant Bear", StringComparison.OrdinalIgnoreCase))
					{
						if (Economy.CostMutantBear != 0uL)
						{
							num2 = Economy.CostMutantBear;
						}
						Economy.Get(client2.userID).MutantsKilled++;
					}
					else
					{
						ConsoleSystem.LogWarning("[WARNING] Economy: Creature '" + text + "' not have cost of death.");
					}
					Config.Get("NAMES." + ((userData2 == null) ? Core.Languages[0] : userData2.Language), text, ref text, true);
					if (num2 > 0uL && Clans.Enabled && clanData != null)
					{
						if (clanData.Level.BonusMembersPayMurder > 0u)
						{
							num2 += num2 * (ulong)clanData.Level.BonusMembersPayMurder / 100uL;
						}
						if (clanData.Tax > 0u)
						{
							ulong num5 = num2 * (ulong)clanData.Tax / 100uL;
							clanData.Balance += num5;
							num2 -= num5;
						}
					}
					if (num2 > 0uL)
					{
						Economy.BalanceAdd(client2.userID, num2);
					}
				}
				if (client2 != null && num2 > 0uL)
				{
					string text3 = Config.GetMessage("Economy.PlayerDeath.Pay", client2.netUser, null);
					if (flag)
					{
						text3 = Config.GetMessage("Economy.SleeperDeath.Pay", client2.netUser, null);
					}
					text3 = text3.Replace("%DEATHPAY%", num2.ToString("N0") + Economy.CurrencySign);
					text3 = text3.Replace("%VICTIM%", text);
					Broadcast.Message(client2.netPlayer, text3, null, 0f);
				}
				if (client != null && num > 0uL)
				{
					string text4 = Config.GetMessage("Economy.PlayerDeath.Fee", client.netUser, null);
					if (client2 == client || flag2)
					{
						text4 = Config.GetMessage("Economy.PlayerSuicide.Fee", client.netUser, null);
					}
					text4 = text4.Replace("%DEATHFEE%", num.ToString("N0") + Economy.CurrencySign);
					text4 = text4.Replace("%KILLER%", text2);
					text4 = text4.Replace("%VICTIM%", text);
					Broadcast.Message(client.netPlayer, text4, null, 0f);
				}
			}
		}

		public static bool RunCommand(NetUser netUser, UserData userData, string Command, string[] Args)
		{
			bool result;
			if (Command != null)
			{
				if (Economy.asad == null)
				{
					Economy.asad = new Dictionary<string, int>(6)
					{
						{
							"balance",
							0
						},
						{
							"money",
							1
						},
						{
							"send",
							2
						},
						{
							"shop",
							3
						},
						{
							"buy",
							4
						},
						{
							"sell",
							5
						}
					};
				}
				int num;
				if (Economy.asad.TryGetValue(Command, out num))
				{
					switch (num)
					{
					case 0:
						Economy.Balance(netUser, userData, Command, Args);
						break;
					case 1:
						Economy.Balance(netUser, userData, Command, Args);
						break;
					case 2:
						Economy.Send(netUser, userData, Command, Args);
						break;
					case 3:
						Economy.ShopList(netUser, userData, Command, Args);
						break;
					case 4:
						Economy.ShopBuy(netUser, userData, Command, Args);
						break;
					case 5:
						Economy.ShopSell(netUser, userData, Command, Args);
						break;
					default:
						result = false;
						return result;
					}
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static void Balance(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (!Economy.Enabled)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.NotAvailable", Sender, null), 5f);
			}
			else
			{
				string text = "0" + Economy.CurrencySign;
				if (Sender != null && !Economy.Database.ContainsKey(userData.SteamID))
				{
					Economy.Add(userData.SteamID, 0, 0, 0, 0);
				}
				if (Sender != null)
				{
					text = Economy.Database[userData.SteamID].Balance.ToString("N0") + Economy.CurrencySign;
				}
				if (Args == null || Args.Length <= 0 || (Sender != null && !Sender.admin))
				{
					Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", text), null, 0f);
				}
				else
				{
					userData = Users.Find(Args[0]);
					if (userData == null)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
					}
					else if (!Economy.Database.ContainsKey(userData.SteamID))
					{
						Broadcast.Notice(Sender, "✘", "Player \"" + Args[0] + "\" not have balance", 5f);
					}
					else
					{
						ulong balance = Economy.Database[userData.SteamID].Balance;
						bool flag = Args.Length > 1 && Args[1].StartsWith("+");
						bool flag2 = Args.Length > 1 && Args[1].StartsWith("-");
						if (Args.Length > 1)
						{
							Args[1] = Args[1].Replace("+", "").Replace("-", "").Trim();
						}
						if (Args.Length > 1 && ulong.TryParse(Args[1], out balance))
						{
							if (flag2)
							{
								Economy.BalanceSub(userData.SteamID, balance);
							}
							else if (flag)
							{
								Economy.BalanceAdd(userData.SteamID, balance);
							}
							else
							{
								Economy.Database[userData.SteamID].Balance = balance;
							}
							text = Economy.Database[userData.SteamID].Balance.ToString("N0") + Economy.CurrencySign;
							Broadcast.Notice(Sender, Economy.CurrencySign, "Balance of \"" + userData.Username + "\" now " + text, 5f);
						}
						else
						{
							text = Economy.Database[userData.SteamID].Balance.ToString("N0") + Economy.CurrencySign;
							Broadcast.Notice(Sender, Economy.CurrencySign, "Balance of \"" + userData.Username + "\" is " + text, 5f);
						}
					}
				}
			}
		}

		public static void Send(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (!Economy.Enabled)
			{
				Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessage("Economy.NotAvailable", Sender, null), 5f);
			}
			else if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender.networkPlayer, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				UserData userData2 = Users.Find(Args[0]);
				if (userData2 == null)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Command.PlayerNoFound", null, Args[0]), 5f);
				}
				else if (userData2 == userData)
				{
					Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Send.Himself", Sender, null), 5f);
				}
				else
				{
					if (!Economy.Database.ContainsKey(userData.SteamID))
					{
						Economy.Add(userData.SteamID, 0, 0, 0, 0);
					}
					if (!Economy.Database.ContainsKey(userData2.SteamID))
					{
						Economy.Add(userData2.SteamID, 0, 0, 0, 0);
					}
					NetUser netUser = NetUser.FindByUserID(userData2.SteamID);
					ulong num = 0uL;
					if (Args.Length > 1 && !ulong.TryParse(Args[1], out num))
					{
						num = 0uL;
					}
					if (num < 1uL)
					{
						num = 0uL;
					}
					string newValue = num.ToString("N0") + Economy.CurrencySign;
					if (num == 0uL)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Send.NoAmount", Sender, null), 5f);
					}
					else if (Economy.Database[userData.SteamID].Balance < num)
					{
						Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Send.NoHaveAmount", Sender, null).Replace("%SENTAMOUNT%", newValue), 5f);
					}
					else
					{
						Economy.BalanceSub(userData.SteamID, num);
						if (Economy.CommandSendTax > 0f)
						{
							num -= (ulong)(num * Economy.CommandSendTax) / 100uL;
							newValue = num.ToString("N0") + Economy.CurrencySign;
						}
						Economy.BalanceAdd(userData2.SteamID, num);
						string newValue2 = Economy.Database[userData.SteamID].Balance.ToString("N0") + Economy.CurrencySign;
						Broadcast.Notice(Sender, Economy.CurrencySign, Config.GetMessage("Economy.Send.SentToPlayer", null, userData2.Username).Replace("%SENTAMOUNT%", newValue), 5f);
						Broadcast.Message(Sender, Config.GetMessage("Economy.Balance", Sender, null).Replace("%BALANCE%", newValue2), null, 0f);
						if (netUser != null)
						{
							newValue2 = Economy.Database[userData2.SteamID].Balance.ToString("N0") + Economy.CurrencySign;
							Broadcast.Notice(netUser, Economy.CurrencySign, Config.GetMessage("Economy.Send.SentFromPlayer", null, userData.Username).Replace("%SENTAMOUNT%", newValue), 5f);
							Broadcast.Message(netUser, Config.GetMessage("Economy.Balance", netUser, null).Replace("%BALANCE%", newValue2), null, 0f);
						}
					}
				}
			}
		}

		public static void ShopList(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (!Shop.Enabled)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NotAvailable", Sender, null), 5f);
			}
			else if (Shop.TradeZoneOnly && (userData.Zone == null || !userData.Zone.CanTrade))
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NoTradeZone", Sender, null), 5f);
			}
			else
			{
				int num = 0;
				string text = null;
				List<ShopItem> list = null;
				if (Args != null && Args.Length > 0)
				{
					if (int.TryParse(Args[0], out num))
					{
						list = Shop.GetItems(num, out text);
					}
					if (list == null)
					{
						list = Shop.GetItems(Args[0], out num);
					}
				}
				if (list == null)
				{
					list = Shop.dictionary_0[Shop.shopGroup_0];
				}
				foreach (ShopItem current in list)
				{
					string newValue = (current.BuyPrice > 0) ? (current.BuyPrice + Economy.CurrencySign) : "None";
					string newValue2 = (current.SellPrice > 0) ? (current.SellPrice + Economy.CurrencySign) : "None";
					string text2 = Config.GetMessage("Economy.Shop.ListItem", Sender, null);
					text2 = text2.Replace("%INDEX%", current.Index.ToString());
					text2 = text2.Replace("%ITEMNAME%", current.Name);
					text2 = text2.Replace("%SELLPRICE%", newValue2);
					text2 = text2.Replace("%BUYPRICE%", newValue);
					text2 = text2.Replace("%QUANTITY%", current.Quantity.ToString());
					Broadcast.Message(Sender, text2, null, 0f);
				}
				if (num == 0)
				{
					foreach (ShopGroup current2 in Shop.dictionary_0.Keys)
					{
						if (current2.Name != null && current2.Index != 0)
						{
							string text3 = Config.GetMessage("Economy.Shop.ListGroup", Sender, null);
							text3 = text3.Replace("%INDEX%", current2.Index.ToString());
							text3 = text3.Replace("%GROUPNAME%", current2.Name);
							Broadcast.Message(Sender, text3, null, 0f);
						}
					}
				}
				Broadcast.Message(Sender, Config.GetMessage("Economy.Shop.Help", Sender, null), null, 0f);
			}
		}

		public static void ShopBuy(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (!Shop.Enabled || !Shop.CanBuy)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.Buy.NotAvailable", Sender, null), 5f);
			}
			else if (Shop.TradeZoneOnly && (userData.Zone == null || !userData.Zone.CanTrade))
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NoTradeZone", Sender, null), 5f);
			}
			else if (Args != null && Args.Length != 0)
			{
				int item_index = 0;
				ShopItem shopItem;
				if (int.TryParse(Args[0], out item_index))
				{
					shopItem = Shop.FindItem(item_index);
				}
				else
				{
					shopItem = Shop.FindItem(Args[0]);
				}
				if (shopItem != null)
				{
					if (shopItem.SellPrice != -1)
					{
						Inventory component = Sender.playerClient.controllable.GetComponent<Inventory>();
						if (component == null || component.noVacantSlots)
						{
							Broadcast.Notice(Sender, "✘", Config.GetMessage("Player.Inventory.IsFull", Sender, null), 5f);
							return;
						}
						int num = shopItem.SellPrice / shopItem.Quantity;
						int num2 = shopItem.Quantity;
						if (Args.Length > 1 && !int.TryParse(Args[1], out num2))
						{
							num2 = shopItem.Quantity;
						}
						if (num2 < 1)
						{
							num2 = shopItem.Quantity;
						}
						ulong num3 = (ulong)((long)(num * num2));
						if (num3 > Economy.GetBalance(Sender.userID))
						{
							string text = Config.GetMessage("Economy.Shop.Buy.NotEnoughBalance", Sender, null);
							text = text.Replace("%TOTALPRICE%", num3.ToString("N0") + Economy.CurrencySign);
							text = text.Replace("%ITEMNAME%", shopItem.Name);
							Broadcast.Notice(Sender, Economy.CurrencySign, text, 5f);
							return;
						}
						num2 = Helper.GiveItem(Sender.playerClient, shopItem.itemData, num2, shopItem.Slots);
						if (num2 == 0)
						{
							Broadcast.Notice(Sender, "✘", Config.GetMessage("Player.Inventory.IsFull", Sender, null), 5f);
							return;
						}
						string text2 = "\"" + shopItem.itemData.name + "\"";
						if (num2 > 1)
						{
							text2 = num2.ToString() + " " + text2;
						}
						num3 = (ulong)((long)(num2 * num));
						Economy.BalanceSub(Sender.userID, num3);
						string text3 = Config.GetMessage("Economy.Shop.Buy.ItemPurchased", Sender, null);
						text3 = text3.Replace("%TOTALPRICE%", num3.ToString("N0") + Economy.CurrencySign);
						text3 = text3.Replace("%ITEMNAME%", text2);
						Broadcast.Notice(Sender, Economy.CurrencySign, text3, 5f);
						Economy.Balance(Sender, userData, "balance", null);
						return;
					}
				}
				string newValue = (shopItem != null) ? shopItem.Name : Args[0];
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.Buy.ItemNotAvailable", Sender, null).Replace("%ITEMNAME%", newValue), 5f);
			}
			else
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
		}

		public static void ShopSell(NetUser Sender, UserData userData, string Command, string[] Args)
		{
			if (!Shop.Enabled || !Shop.CanSell)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.Sell.NotAvailable", Sender, null), 5f);
			}
			else if (Shop.TradeZoneOnly && (userData.Zone == null || !userData.Zone.CanTrade))
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessage("Economy.Shop.NoTradeZone", Sender, null), 5f);
			}
			else if (Args == null || Args.Length == 0)
			{
				Broadcast.Notice(Sender, "✘", Config.GetMessageCommand("Command.InvalidSyntax", Command, null), 5f);
			}
			else
			{
				int item_index = 0;
				Inventory component = Sender.playerClient.controllable.GetComponent<Inventory>();
				if (!Args[0].Equals("ALL", StringComparison.OrdinalIgnoreCase))
				{
					ShopItem shopItem;
					if (int.TryParse(Args[0], out item_index))
					{
						shopItem = Shop.FindItem(item_index);
					}
					else
					{
						shopItem = Shop.FindItem(Args[0]);
					}
					if (shopItem != null)
					{
						if (shopItem.BuyPrice != -1)
						{
							int num = shopItem.BuyPrice / shopItem.Quantity;
							int num2 = shopItem.Quantity;
							if (Args.Length > 1 && !int.TryParse(Args[1], out num2))
							{
								num2 = shopItem.Quantity;
							}
							if (num2 < 1)
							{
								num2 = shopItem.Quantity;
							}
							int num3 = Helper.InventoryItemCount(component, shopItem.itemData);
							if (num3 == 0)
							{
								Broadcast.Notice(Sender, Economy.CurrencySign, Config.GetMessage("Economy.Shop.Sell.NotEnoughItem", Sender, null).Replace("%ITEMNAME%", shopItem.Name), 5f);
								return;
							}
							if (num2 > num3)
							{
								num2 = num3;
							}
							num3 = Helper.InventoryItemRemove(component, shopItem.itemData, num2);
							string text = "\"" + shopItem.Name + "\"";
							if (num3 > 1)
							{
								text = num3.ToString() + " " + text;
							}
							ulong amount = (ulong)((long)(num3 * num));
							Economy.BalanceAdd(Sender.userID, amount);
							string text2 = Config.GetMessage("Economy.Shop.Sell.ItemSold", Sender, null);
							text2 = text2.Replace("%TOTALPRICE%", amount.ToString("N0") + Economy.CurrencySign);
							text2 = text2.Replace("%ITEMNAME%", text);
							Broadcast.Notice(Sender, Economy.CurrencySign, text2, 5f);
							Economy.Balance(Sender, userData, "balance", null);
							return;
						}
					}
					string newValue = (shopItem != null) ? shopItem.Name : Args[0];
					Broadcast.Notice(Sender, Economy.CurrencySign, Config.GetMessage("Economy.Shop.Sell.ItemNotAvailable", Sender, null).Replace("%ITEMNAME%", newValue), 5f);
				}
				else
				{
					ulong num4 = 0uL;
					List<IInventoryItem> list = new List<IInventoryItem>();
					Inventory.OccupiedIterator occupiedIterator = component.occupiedIterator;
					while (occupiedIterator.Next())
					{
						ShopItem shopItem = Shop.FindItem(occupiedIterator.item.datablock.name);
						if (shopItem != null && shopItem.BuyPrice != -1)
						{
							int num5 = occupiedIterator.item.datablock._splittable ? occupiedIterator.item.uses : 1;
							ulong num6 = (ulong)((long)(shopItem.BuyPrice / shopItem.Quantity) * (long)num5);
							num4 += num6;
							list.Add(occupiedIterator.item);
						}
					}
					if (list.Count > 0)
					{
						foreach (IInventoryItem current in list)
						{
							component.RemoveItem(current);
						}
						if (num4 > 0uL)
						{
							Economy.BalanceAdd(Sender.userID, num4);
						}
						string text3 = Config.GetMessage("Economy.Shop.Sell.AllSold", Sender, null);
						text3 = text3.Replace("%TOTALPRICE%", num4.ToString("N0") + Economy.CurrencySign);
						text3 = text3.Replace("%TOTALAMOUNT%", list.Count.ToString());
						Broadcast.Notice(Sender, Economy.CurrencySign, text3, 5f);
					}
					else
					{
						Broadcast.Notice(Sender, Economy.CurrencySign, Config.GetMessage("Economy.Shop.Sell.NoNothing", Sender, null), 5f);
					}
				}
			}
		}
	}
}
