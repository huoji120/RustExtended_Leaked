using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RustExtended
{
	public class Shop
	{
		[CompilerGenerated]
		private sealed class Class33
		{
			public string string_0;

			public bool method_0(ShopItem shopItem_0)
			{
				return shopItem_0.Name.ToLower().EndsWith(this.string_0);
			}

			public bool method_1(ShopItem shopItem_0)
			{
				return shopItem_0.Name.ToLower().StartsWith(this.string_0);
			}

			public bool method_2(ShopItem shopItem_0)
			{
				return shopItem_0.Name.Equals(this.string_0, StringComparison.OrdinalIgnoreCase);
			}
		}

		[CompilerGenerated]
		private sealed class Class34
		{
			public int int_0;

			public bool method_0(ShopItem shopItem_0)
			{
				return shopItem_0.Index == this.int_0;
			}
		}

		public static bool Enabled = false;

		public static bool TradeZoneOnly = false;

		public static bool CanSell = false;

		public static bool CanBuy = false;

		internal static Dictionary<ShopGroup, List<ShopItem>> dictionary_0;

		internal static ShopGroup shopGroup_0;

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static int int_0;

		[CompilerGenerated]
		private static int int_1;

		public static bool Initialized
		{
			get;
			private set;
		}

		public static int GroupCount
		{
			get;
			private set;
		}

		public static int ItemCount
		{
			get;
			private set;
		}

		public static void Initialize()
		{
			Shop.Initialized = false;
			Config.Get("SHOP", "Enabled", ref Shop.Enabled, true);
			Config.Get("SHOP", "TradeZoneOnly", ref Shop.TradeZoneOnly, true);
			Config.Get("SHOP", "CanSell", ref Shop.CanSell, true);
			Config.Get("SHOP", "CanBuy", ref Shop.CanBuy, true);
			Shop.GroupCount = 0;
			Shop.ItemCount = 0;
			Shop.shopGroup_0 = new ShopGroup(null, 0);
			Shop.dictionary_0 = new Dictionary<ShopGroup, List<ShopItem>>();
			Shop.dictionary_0.Add(Shop.shopGroup_0, new List<ShopItem>());
			string[] array = null;
			Config.Get("SHOP.LIST", "ENTRY", ref array, true);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string[] array3 = text.Split(new char[]
				{
					'='
				});
				if (array3.Length >= 2)
				{
					if (array3[0].Equals("SHOPITEM", StringComparison.OrdinalIgnoreCase))
					{
						ShopItem shopItem = Shop.smethod_0(array3[1]);
						if (shopItem == null)
						{
							goto IL_199;
						}
						Shop.dictionary_0[Shop.shopGroup_0].Add(shopItem);
					}
					if (array3[0].Equals("SHOPGROUP", StringComparison.OrdinalIgnoreCase))
					{
						array3[1] = array3[1].Trim(new char[]
						{
							'"'
						});
						Shop.GroupCount++;
						ShopGroup key = new ShopGroup(array3[1], Shop.GroupCount);
						Shop.dictionary_0.Add(key, new List<ShopItem>());
					}
				}
				IL_199:;
			}
			foreach (ShopGroup current in Shop.dictionary_0.Keys)
			{
				string[] array4 = null;
				if (current.Name != null)
				{
					if (Config.Get("SHOP.GROUP", current.Name, ref array4, true))
					{
						string[] array5 = array4;
						for (int j = 0; j < array5.Length; j++)
						{
							string text2 = array5[j];
							string[] array6 = text2.Split(new char[]
							{
								'='
							});
							string[] array7 = Helper.SplitQuotes(array6[1], ',');
							ShopItem shopItem2 = Shop.smethod_0(array6[1]);
							if (shopItem2 == null)
							{
								ConsoleSystem.PrintError("[SHOP]ERROR: Item not found or not enough of parameters for '" + array7[0] + "'.", false);
							}
							else if (Shop.dictionary_0[current].Contains(shopItem2))
							{
								ConsoleSystem.PrintError(string.Concat(new string[]
								{
									"[SHOP]ERROR: Item '",
									array7[0],
									"' already exists in group '",
									current.Name,
									"'."
								}), false);
							}
							else
							{
								Shop.dictionary_0[current].Add(shopItem2);
							}
						}
					}
					else
					{
						ConsoleSystem.PrintError("[SHOP]ERROR: Group named '" + current.Name + "' not exists in configuration. Skipped.", false);
					}
				}
			}
			Shop.Initialized = true;
		}

		private static ShopItem smethod_0(string string_0)
		{
			string[] array = Helper.SplitQuotes(string_0, ',');
			ShopItem result;
			if (array.Length < 4)
			{
				result = null;
			}
			else
			{
				ItemDataBlock byName = DatablockDictionary.GetByName(array[0]);
				if (byName == null)
				{
					result = null;
				}
				else
				{
					Shop.ItemCount++;
					int sell_price = -1;
					int buy_price = -1;
					int quantity = 1;
					int slots = -1;
					if (array.Length > 1 && !int.TryParse(array[1], out sell_price))
					{
						sell_price = -1;
					}
					if (array.Length > 2 && !int.TryParse(array[2], out buy_price))
					{
						buy_price = -1;
					}
					if (array.Length > 3 && !int.TryParse(array[3], out quantity))
					{
						quantity = 1;
					}
					if (array.Length > 4 && !int.TryParse(array[4], out slots))
					{
						slots = -1;
					}
					result = new ShopItem(Shop.ItemCount, array[0], sell_price, buy_price, quantity, slots, byName);
				}
			}
			return result;
		}

		public static List<ShopItem> GetItems(string group_name, out int group_index)
		{
			string value = group_name.Trim(new char[]
			{
				'*'
			}).ToLower();
			List<ShopItem> result;
			foreach (ShopGroup current in Shop.dictionary_0.Keys)
			{
				if (current.Name != null && current.Index != 0)
				{
					if (group_name.StartsWith("*") && current.Name.ToLower().EndsWith(value))
					{
						group_index = current.Index;
						List<ShopItem> list = Shop.dictionary_0[current];
						result = list;
						return result;
					}
					if (group_name.EndsWith("*") && current.Name.ToLower().StartsWith(value))
					{
						group_index = current.Index;
						List<ShopItem> list = Shop.dictionary_0[current];
						result = list;
						return result;
					}
					if (current.Name.Equals(group_name, StringComparison.OrdinalIgnoreCase))
					{
						group_index = current.Index;
						List<ShopItem> list = Shop.dictionary_0[current];
						result = list;
						return result;
					}
				}
			}
			group_index = 0;
			result = null;
			return result;
		}

		public static List<ShopItem> GetItems(int group_index, out string group_name)
		{
			List<ShopItem> result;
			foreach (ShopGroup current in Shop.dictionary_0.Keys)
			{
				if (current.Name != null && current.Index != 0 && current.Index == group_index)
				{
					group_name = current.Name;
					result = Shop.dictionary_0[current];
					return result;
				}
			}
			group_name = null;
			result = null;
			return result;
		}

		public static ShopItem FindItem(string item_name)
		{
			Predicate<ShopItem> predicate = null;
			Predicate<ShopItem> predicate2 = null;
			Predicate<ShopItem> predicate3 = null;
			Shop.Class33 @class = new Shop.Class33();
			ShopItem shopItem = null;
			@class.string_0 = item_name.Trim(new char[]
			{
				'*'
			}).ToLower();
			ShopItem result;
			foreach (ShopGroup current in Shop.dictionary_0.Keys)
			{
				if (item_name.StartsWith("*"))
				{
					List<ShopItem> list = Shop.dictionary_0[current];
					if (predicate == null)
					{
						predicate = new Predicate<ShopItem>(@class.method_0);
					}
					shopItem = list.Find(predicate);
				}
				if (item_name.EndsWith("*"))
				{
					List<ShopItem> list2 = Shop.dictionary_0[current];
					if (predicate2 == null)
					{
						predicate2 = new Predicate<ShopItem>(@class.method_1);
					}
					shopItem = list2.Find(predicate2);
				}
				if (shopItem == null)
				{
					List<ShopItem> list3 = Shop.dictionary_0[current];
					if (predicate3 == null)
					{
						predicate3 = new Predicate<ShopItem>(@class.method_2);
					}
					shopItem = list3.Find(predicate3);
				}
				if (shopItem != null)
				{
					result = shopItem;
					return result;
				}
			}
			result = null;
			return result;
		}

		public static ShopItem FindItem(int item_index)
		{
			Predicate<ShopItem> predicate = null;
			Shop.Class34 @class = new Shop.Class34();
			@class.int_0 = item_index;
			ShopItem result;
			foreach (ShopGroup current in Shop.dictionary_0.Keys)
			{
				List<ShopItem> list = Shop.dictionary_0[current];
				if (predicate == null)
				{
					predicate = new Predicate<ShopItem>(@class.method_0);
				}
				ShopItem shopItem = list.Find(predicate);
				if (shopItem != null)
				{
					result = shopItem;
					return result;
				}
			}
			result = null;
			return result;
		}
	}
}
