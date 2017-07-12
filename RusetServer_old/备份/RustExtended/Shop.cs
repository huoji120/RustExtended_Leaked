namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Shop
    {
        [CompilerGenerated]
        private static bool bool_0;
        public static bool CanBuy = false;
        public static bool CanSell = false;
        internal static Dictionary<ShopGroup, System.Collections.Generic.List<ShopItem>> dictionary_0;
        public static bool Enabled = false;
        [CompilerGenerated]
        private static int int_0;
        [CompilerGenerated]
        private static int int_1;
        internal static ShopGroup shopGroup_0;
        public static bool TradeZoneOnly = false;

        public static ShopItem FindItem(int item_index)
        {
            Predicate<ShopItem> match = null;
            Class34 class2 = new Class34 {
                int_0 = item_index
            };
            ShopItem item = null;
            foreach (ShopGroup group in dictionary_0.Keys)
            {
                if (match == null)
                {
                    match = new Predicate<ShopItem>(class2.method_0);
                }
                item = dictionary_0[group].Find(match);
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }

        public static ShopItem FindItem(string item_name)
        {
            Predicate<ShopItem> match = null;
            Predicate<ShopItem> predicate2 = null;
            Predicate<ShopItem> predicate3 = null;
            Class33 class2 = new Class33();
            ShopItem item = null;
            class2.string_0 = item_name.Trim(new char[] { '*' }).ToLower();
            foreach (ShopGroup group in dictionary_0.Keys)
            {
                if (item_name.StartsWith("*"))
                {
                    if (match == null)
                    {
                        match = new Predicate<ShopItem>(class2.method_0);
                    }
                    item = dictionary_0[group].Find(match);
                }
                if (item_name.EndsWith("*"))
                {
                    if (predicate2 == null)
                    {
                        predicate2 = new Predicate<ShopItem>(class2.method_1);
                    }
                    item = dictionary_0[group].Find(predicate2);
                }
                if (item == null)
                {
                    if (predicate3 == null)
                    {
                        predicate3 = new Predicate<ShopItem>(class2.method_2);
                    }
                    item = dictionary_0[group].Find(predicate3);
                }
                if (item != null)
                {
                    return item;
                }
            }
            return null;
        }

        public static System.Collections.Generic.List<ShopItem> GetItems(int group_index, out string group_name)
        {
            using (Dictionary<ShopGroup, System.Collections.Generic.List<ShopItem>>.KeyCollection.Enumerator enumerator = dictionary_0.Keys.GetEnumerator())
            {
                ShopGroup current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (((current.Name != null) && (current.Index != 0)) && (current.Index == group_index))
                    {
                        goto Label_0041;
                    }
                }
                goto Label_0065;
            Label_0041:
                group_name = current.Name;
                return dictionary_0[current];
            }
        Label_0065:
            group_name = null;
            return null;
        }

        public static System.Collections.Generic.List<ShopItem> GetItems(string group_name, out int group_index)
        {
            string str = group_name.Trim(new char[] { '*' }).ToLower();
            using (Dictionary<ShopGroup, System.Collections.Generic.List<ShopItem>>.KeyCollection.Enumerator enumerator = dictionary_0.Keys.GetEnumerator())
            {
                ShopGroup current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if ((current.Name != null) && (current.Index != 0))
                    {
                        if (group_name.StartsWith("*") && current.Name.ToLower().EndsWith(str))
                        {
                            goto Label_009F;
                        }
                        if (group_name.EndsWith("*") && current.Name.ToLower().StartsWith(str))
                        {
                            goto Label_00B5;
                        }
                        if (current.Name.Equals(group_name, StringComparison.OrdinalIgnoreCase))
                        {
                            goto Label_00CB;
                        }
                    }
                }
                goto Label_00EF;
            Label_009F:
                group_index = current.Index;
                return dictionary_0[current];
            Label_00B5:
                group_index = current.Index;
                return dictionary_0[current];
            Label_00CB:
                group_index = current.Index;
                return dictionary_0[current];
            }
        Label_00EF:
            group_index = 0;
            return null;
        }

        public static void Initialize()
        {
            Initialized = false;
            Config.Get("SHOP", "Enabled", ref Enabled, true);
            Config.Get("SHOP", "TradeZoneOnly", ref TradeZoneOnly, true);
            Config.Get("SHOP", "CanSell", ref CanSell, true);
            Config.Get("SHOP", "CanBuy", ref CanBuy, true);
            GroupCount = 0;
            ItemCount = 0;
            shopGroup_0 = new ShopGroup(null, 0);
            dictionary_0 = new Dictionary<ShopGroup, System.Collections.Generic.List<ShopItem>>();
            dictionary_0.Add(shopGroup_0, new System.Collections.Generic.List<ShopItem>());
            string[] result = null;
            Config.Get("SHOP.LIST", "ENTRY", ref result, true);
            foreach (string str in result)
            {
                string[] strArray2 = str.Split(new char[] { '=' });
                if (strArray2.Length >= 2)
                {
                    if (strArray2[0].Equals("SHOPITEM", StringComparison.OrdinalIgnoreCase))
                    {
                        ShopItem item = smethod_0(strArray2[1]);
                        if (item == null)
                        {
                            continue;
                        }
                        dictionary_0[shopGroup_0].Add(item);
                    }
                    if (strArray2[0].Equals("SHOPGROUP", StringComparison.OrdinalIgnoreCase))
                    {
                        strArray2[1] = strArray2[1].Trim(new char[] { '"' });
                        GroupCount++;
                        ShopGroup key = new ShopGroup(strArray2[1], GroupCount);
                        dictionary_0.Add(key, new System.Collections.Generic.List<ShopItem>());
                    }
                }
            }
            foreach (ShopGroup group2 in dictionary_0.Keys)
            {
                string[] strArray3 = null;
                if (group2.Name != null)
                {
                    if (Config.Get("SHOP.GROUP", group2.Name, ref strArray3, true))
                    {
                        foreach (string str2 in strArray3)
                        {
                            string[] strArray4 = str2.Split(new char[] { '=' });
                            string[] strArray5 = Helper.SplitQuotes(strArray4[1], ',');
                            ShopItem item2 = smethod_0(strArray4[1]);
                            if (item2 == null)
                            {
                                ConsoleSystem.PrintError("[SHOP]ERROR: Item not found or not enough of parameters for '" + strArray5[0] + "'.", false);
                            }
                            else if (dictionary_0[group2].Contains(item2))
                            {
                                ConsoleSystem.PrintError("[SHOP]ERROR: Item '" + strArray5[0] + "' already exists in group '" + group2.Name + "'.", false);
                            }
                            else
                            {
                                dictionary_0[group2].Add(item2);
                            }
                        }
                    }
                    else
                    {
                        ConsoleSystem.PrintError("[SHOP]ERROR: Group named '" + group2.Name + "' not exists in configuration. Skipped.", false);
                    }
                }
            }
            Initialized = true;
        }

        private static ShopItem smethod_0(string string_0)
        {
            string[] strArray = Helper.SplitQuotes(string_0, ',');
            if (strArray.Length < 4)
            {
                return null;
            }
            ItemDataBlock byName = DatablockDictionary.GetByName(strArray[0]);
            if (byName == null)
            {
                return null;
            }
            ItemCount++;
            int result = -1;
            int num2 = -1;
            int num3 = 1;
            int num4 = -1;
            if ((strArray.Length > 1) && !int.TryParse(strArray[1], out result))
            {
                result = -1;
            }
            if ((strArray.Length > 2) && !int.TryParse(strArray[2], out num2))
            {
                num2 = -1;
            }
            if ((strArray.Length > 3) && !int.TryParse(strArray[3], out num3))
            {
                num3 = 1;
            }
            if ((strArray.Length > 4) && !int.TryParse(strArray[4], out num4))
            {
                num4 = -1;
            }
            return new ShopItem(ItemCount, strArray[0], result, num2, num3, num4, byName);
        }

        public static int GroupCount
        {
            [CompilerGenerated]
            get
            {
                return int_0;
            }
            [CompilerGenerated]
            private set
            {
                int_0 = value;
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

        public static int ItemCount
        {
            [CompilerGenerated]
            get
            {
                return int_1;
            }
            [CompilerGenerated]
            private set
            {
                int_1 = value;
            }
        }

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
                return (shopItem_0.Index == this.int_0);
            }
        }
    }
}

