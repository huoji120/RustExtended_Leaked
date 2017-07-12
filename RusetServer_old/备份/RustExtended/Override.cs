namespace RustExtended
{
    using Facepunch.MeshBatch;
    using Facepunch.Utility;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Override
    {
        [CompilerGenerated]
        private static bool bool_0;
        [CompilerGenerated]
        private static bool bool_1;
        [CompilerGenerated]
        private static bool bool_2;
        [CompilerGenerated]
        private static bool bool_3;
        [CompilerGenerated]
        private static bool bool_4;
        [CompilerGenerated]
        private static bool bool_5;
        [CompilerGenerated]
        private static int int_0;
        [CompilerGenerated]
        private static int int_1;
        [CompilerGenerated]
        private static int int_2;
        private static System.Collections.Generic.List<string> list_0 = new System.Collections.Generic.List<string>();
        public static string OverridePath = "";
        [CompilerGenerated]
        private static Predicate<string> predicate_0;
        [CompilerGenerated]
        private static Predicate<string> predicate_1;
        [CompilerGenerated]
        private static Predicate<string> predicate_2;
        [CompilerGenerated]
        private static Predicate<string> predicate_3;
        [CompilerGenerated]
        private static Predicate<string> predicate_4;
        [CompilerGenerated]
        private static Predicate<string> predicate_5;
        [CompilerGenerated]
        private static string string_0;
        [CompilerGenerated]
        private static string string_1;
        [CompilerGenerated]
        private static string string_2;

        public static void ApplyDamageTypeList(TakeDamage takeDamage, ref DamageEvent damage, DamageTypeList damageType)
        {
            ProtectionTakeDamage damage2 = takeDamage as ProtectionTakeDamage;
            string str = damage.victim.idMain.name.Replace("(Clone)", "");
            for (int i = 0; i < 6; i++)
            {
                DamageTypeFlags flags = (DamageTypeFlags) (((int) 1) << i);
                float result = (damage2 != null) ? damage2.GetArmorValue(i) : 0f;
                string key = str + "." + flags.ToString().Replace("damage_", "");
                if (!(damage.victim.idMain is Character))
                {
                    Config.Get("OVERRIDE.ARMOR", key, ref result, true);
                }
                if ((result > 0f) && (damageType[i] > 0f))
                {
                    DamageTypeList list;
                    int num3;
                    (list = damageType)[num3 = i] = list[num3] * Mathf.Clamp01(1f - (result / 200f));
                }
                if (!Mathf.Approximately(damageType[i], 0f))
                {
                    damage.damageTypes |= flags;
                    damage.amount += damageType[i];
                }
            }
        }

        public static bool DamageOverride(TakeDamage take, ref DamageEvent damage, ref TakeDamage.Quantity quantity)
        {
            if (damage.attacker.idMain != damage.victim.idMain)
            {
                if (!Core.OverrideDamage || float.IsInfinity(damage.amount))
                {
                    return true;
                }
                if ((damage.victim.id.GetComponent<Character>() == null) && (damage.attacker.client != null))
                {
                    ulong ownerID = (damage.victim.client != null) ? damage.victim.client.userID : ((ulong) 0L);
                    if (damage.victim.idMain is DeployableObject)
                    {
                        ownerID = (damage.victim.idMain as DeployableObject).ownerID;
                    }
                    if (damage.victim.idMain is StructureComponent)
                    {
                        ownerID = (damage.victim.idMain as StructureComponent)._master.ownerID;
                    }
                    ulong num2 = (damage.attacker.client != null) ? damage.attacker.client.userID : ((ulong) 0L);
                    if (damage.attacker.idMain is DeployableObject)
                    {
                        num2 = (damage.attacker.idMain as DeployableObject).ownerID;
                    }
                    if (damage.attacker.idMain is StructureComponent)
                    {
                        num2 = (damage.attacker.idMain as StructureComponent)._master.ownerID;
                    }
                    if (((ownerID == num2) || Users.SharedGet(ownerID, num2)) && (Core.OwnershipDestroy || Core.DestoryOwnership.ContainsKey(damage.attacker.client.userID)))
                    {
                        Config.Get("OVERRIDE.DAMAGE", damage.attacker.idMain.name.Replace("(Clone)", "") + ".DAMAGE", ref damage.amount, true);
                        return true;
                    }
                }
                bool result = true;
                if ((damage.attacker.client != null) && (damage.attacker.idMain is Character))
                {
                    WeaponImpact extraData = damage.extraData as WeaponImpact;
                    string str = (extraData != null) ? extraData.dataBlock.name : "Hunting Bow";
                    string str2 = str.Replace(" ", "") + ".DAMAGE";
                    string key = str2 + "." + damage.victim.idMain.name.Replace("(Clone)", "");
                    string str4 = str2 + ".HEADSHOT";
                    if (Config.Get("OVERRIDE.DAMAGE", key, ref result, true) && !result)
                    {
                        return false;
                    }
                    float[] numArray = (extraData != null) ? new float[] { extraData.dataBlock.damageMin, extraData.dataBlock.damageMax } : new float[] { 75f, 75f };
                    Config.Get("OVERRIDE.DAMAGE", (damage.bodyPart == BodyPart.Head) ? str4 : str2, ref numArray, true);
                    damage.amount = UnityEngine.Random.Range(Math.Min(numArray[0], numArray[1]), Math.Max(numArray[0], numArray[1]));
                    if ((extraData != null) && (damage.extraData is BulletWeaponDataBlock))
                    {
                        damage.amount *= (damage.extraData as BulletWeaponDataBlock).IsSilenced(extraData.itemRep) ? 0.8f : 1f;
                    }
                    if ((extraData != null) && (damage.extraData is BulletWeaponImpact))
                    {
                        quantity = new DamageTypeList(0f, damage.amount, 0f, 0f, 0f, 0f);
                    }
                    else
                    {
                        quantity = new DamageTypeList(0f, 0f, damage.amount, 0f, 0f, 0f);
                    }
                    damage.amount = 0f;
                    if (quantity.Unit == TakeDamage.Unit.List)
                    {
                        ApplyDamageTypeList(take, ref damage, quantity.DamageTypeList);
                    }
                    Helper.Log(string.Concat(new object[] { 
                        "Damage: ", damage.attacker.idMain, "[", damage.attacker.networkViewID, "] from ", str, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", numArray[0], "-", numArray[1], 
                        ") pts."
                     }), false);
                }
                else if (!(damage.attacker.idMain is Character))
                {
                    float baseReturnDmg = 0f;
                    float explosionRadius = 0f;
                    if (damage.attacker.id is TimedGrenade)
                    {
                        baseReturnDmg = (damage.attacker.id as TimedGrenade).damage;
                        explosionRadius = (damage.attacker.id as TimedGrenade).explosionRadius;
                    }
                    if (damage.attacker.id is TimedExplosive)
                    {
                        baseReturnDmg = (damage.attacker.id as TimedExplosive).damage;
                        explosionRadius = (damage.attacker.id as TimedExplosive).explosionRadius;
                    }
                    if (damage.attacker.id is SpikeWall)
                    {
                        baseReturnDmg = (damage.attacker.id as SpikeWall).baseReturnDmg;
                        explosionRadius = 0f;
                    }
                    if (baseReturnDmg > 0f)
                    {
                        string str5 = damage.attacker.idMain.name.Replace("(Clone)", "") + ".DAMAGE";
                        string str6 = str5 + "." + damage.victim.idMain.name.Replace("(Clone)", "");
                        if (Config.Get("OVERRIDE.DAMAGE", str6, ref result, true) && !result)
                        {
                            return false;
                        }
                        Config.Get("OVERRIDE.DAMAGE", str5, ref baseReturnDmg, true);
                        if (explosionRadius > 0f)
                        {
                            RaycastHit hit;
                            bool flag2;
                            MeshBatchInstance instance;
                            Vector3 center = damage.victim.idMain.collider.bounds.center;
                            Vector3 a = damage.attacker.idMain.collider.bounds.center;
                            Vector3 direction = center - a;
                            float distance = Vector3.Distance(a, center);
                            if (Facepunch.MeshBatch.MeshBatchPhysics.Raycast(a, direction, out hit, distance, 0x10360401, out flag2, out instance))
                            {
                                IDMain main = flag2 ? instance.idMain : IDBase.GetMain(hit.collider);
                                GameObject obj2 = (main != null) ? main.gameObject : hit.collider.gameObject;
                                if (obj2 != damage.victim.idMain.gameObject)
                                {
                                    damage.amount = (1f - Mathf.Clamp01(distance / explosionRadius)) * baseReturnDmg;
                                    if (flag2)
                                    {
                                        damage.amount *= 0.1f;
                                    }
                                }
                            }
                        }
                        if (damage.attacker.id is SpikeWall)
                        {
                            quantity = new DamageTypeList(0f, 0f, damage.amount, 0f, 0f, 0f);
                        }
                        else
                        {
                            quantity = new DamageTypeList(0f, 0f, 0f, damage.amount, 0f, 0f);
                        }
                        damage.amount = 0f;
                        if (quantity.Unit == TakeDamage.Unit.List)
                        {
                            ApplyDamageTypeList(take, ref damage, quantity.DamageTypeList);
                        }
                    }
                    Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker.idMain, "[", damage.attacker.networkViewID, "] owned ", damage.attacker.client, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", baseReturnDmg, ") pts." }), false);
                }
            }
            return true;
        }

        public static void Initialize()
        {
            OverridePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Core.SavePath), @"cfg\RustOverride");
            if (!Directory.Exists(OverridePath))
            {
                Directory.CreateDirectory(OverridePath);
            }
            LootsFile = Path.Combine(OverridePath, "LootsList.cfg");
            ItemsFile = Path.Combine(OverridePath, "ItemsList.cfg");
            SpawnsFile = Path.Combine(OverridePath, "SpawnList.cfg");
            LootsInitialized = false;
            LootsFileCreated = false;
            LootsCount = 0;
            ItemsInitialized = false;
            ItemsFileCreated = false;
            ItemsCount = 0;
            SpawnsInitialized = false;
            SpawnsFileCreated = false;
            SpawnsCount = 0;
            list_0 = DatablockDictionary._lootSpawnLists.Keys.ToList<string>();
            if (!File.Exists(LootsFile))
            {
                LootsFileCreated = LootSaveFile();
            }
            else if (Core.OverrideLoots)
            {
                LootsInitialized = smethod_0();
            }
            LootsCount = DatablockDictionary._lootSpawnLists.Count;
            if (!File.Exists(ItemsFile))
            {
                ItemsFileCreated = smethod_1();
            }
            else if (Core.OverrideItems)
            {
                ItemsInitialized = smethod_2();
            }
            string path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Core.SavePath), @"cfg\rust_items.txt");
            if (!File.Exists(path))
            {
                string contents = "";
                foreach (ItemDataBlock block in DatablockDictionary.All)
                {
                    contents = contents + block.name + Environment.NewLine;
                }
                File.WriteAllText(path, contents);
            }
        }

        public static void InitializeSpawnsFile()
        {
            SpawnsInitialized = false;
            SpawnsFileCreated = false;
            SpawnsCount = 0;
            if (!File.Exists(SpawnsFile))
            {
                SpawnsFileCreated = smethod_4();
            }
            else if (Core.OverrideSpawns)
            {
                SpawnsInitialized = smethod_5();
            }
        }

        public static bool LootSaveFile()
        {
            using (StreamWriter writer = File.CreateText(LootsFile))
            {
                foreach (string str in list_0)
                {
                    LootSpawnList list = DatablockDictionary._lootSpawnLists[str];
                    writer.WriteLine("[" + list.name + "]");
                    writer.WriteLine(string.Concat(new object[] { "PackagesToSpawn=", list.minPackagesToSpawn, ",", list.maxPackagesToSpawn }));
                    writer.WriteLine("SpawnOneOfEach=" + list.spawnOneOfEach.ToString());
                    writer.WriteLine("NoDuplicates=" + list.noDuplicates.ToString());
                    writer.WriteLine("// Type   Weight\tList/Item\t\tMin\tMax");
                    foreach (LootSpawnList.LootWeightedEntry entry in list.LootPackages)
                    {
                        if (entry.obj != null)
                        {
                            if (entry.obj is ItemDataBlock)
                            {
                                writer.Write("PackageItem=");
                            }
                            else
                            {
                                writer.Write("PackageList=");
                            }
                            writer.Write(entry.weight + "\t");
                            writer.Write(entry.obj.name + new string('\t', 4 - (entry.obj.name.Length / 8)));
                            writer.Write(entry.amountMin + "\t" + entry.amountMax);
                            writer.WriteLine();
                        }
                    }
                    writer.WriteLine();
                }
            }
            return true;
        }

        private static bool smethod_0()
        {
            System.Collections.Generic.List<string> list = File.ReadAllLines(LootsFile).ToList<string>();
            if (predicate_0 == null)
            {
                predicate_0 = new Predicate<string>(Override.smethod_6);
            }
            if (!list.Exists(predicate_0))
            {
                ConsoleSystem.PrintError("ERROR: Spawn list for \"AILootList\" not found in \"lootslist.cfg\".", false);
                return false;
            }
            if (predicate_1 == null)
            {
                predicate_1 = new Predicate<string>(Override.smethod_7);
            }
            if (!list.Exists(predicate_1))
            {
                ConsoleSystem.PrintError("ERROR: Spawn list for \"AmmoSpawnList\" not found in \"lootslist.cfg\".", false);
                return false;
            }
            if (predicate_2 == null)
            {
                predicate_2 = new Predicate<string>(Override.smethod_8);
            }
            if (!list.Exists(predicate_2))
            {
                ConsoleSystem.PrintError("ERROR: Spawn list for \"JunkSpawnList\" not found in \"lootslist.cfg\".", false);
                return false;
            }
            if (predicate_3 == null)
            {
                predicate_3 = new Predicate<string>(Override.smethod_9);
            }
            if (!list.Exists(predicate_3))
            {
                ConsoleSystem.PrintError("ERROR: Spawn list for \"MedicalSpawnList\" not found in \"lootslist.cfg\".", false);
                return false;
            }
            if (predicate_4 == null)
            {
                predicate_4 = new Predicate<string>(Override.smethod_10);
            }
            if (!list.Exists(predicate_4))
            {
                ConsoleSystem.PrintError("ERROR: Spawn list for \"WeaponSpawnList\" not found in \"lootslist.cfg\".", false);
                return false;
            }
            if (predicate_5 == null)
            {
                predicate_5 = new Predicate<string>(Override.smethod_11);
            }
            if (!list.Exists(predicate_5))
            {
                ConsoleSystem.PrintError("ERROR: Spawn list for \"SupplyDropSpawnListMaster\" not found in \"lootslist.cfg\".", false);
                return false;
            }
            DatablockDictionary._lootSpawnLists.Clear();
            Dictionary<string, LootSpawnList> dictionary = new Dictionary<string, LootSpawnList>();
            foreach (string str in list)
            {
                string str2 = str.Trim();
                if (!string.IsNullOrEmpty(str2) && !str2.StartsWith("//"))
                {
                    if (str2.Contains("//"))
                    {
                        str2 = str2.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }
                    if ((!string.IsNullOrEmpty(str2) && str2.StartsWith("[")) && str2.EndsWith("]"))
                    {
                        str2 = str2.Substring(1, str2.Length - 2);
                        dictionary[str2] = ScriptableObject.CreateInstance<LootSpawnList>();
                    }
                }
            }
            LootSpawnList list2 = null;
            LootSpawnList.LootWeightedEntry item = null;
            System.Collections.Generic.List<LootSpawnList.LootWeightedEntry> list3 = null;
            foreach (string str3 in list)
            {
                string str4 = str3.Trim();
                if (!string.IsNullOrEmpty(str4) && !str4.StartsWith("//"))
                {
                    if (str4.Contains("//"))
                    {
                        str4 = str4.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(str4))
                    {
                        if (str4.StartsWith("[") && str4.EndsWith("]"))
                        {
                            string str5 = str4.Substring(1, str4.Length - 2);
                            list2 = dictionary[str5];
                            list2.name = str5;
                            DatablockDictionary._lootSpawnLists.Add(list2.name, list2);
                            list3 = new System.Collections.Generic.List<LootSpawnList.LootWeightedEntry>();
                        }
                        else if (str4.Contains("=") && (list2 != null))
                        {
                            string str6;
                            string[] strArray = str4.Split(new char[] { '=' });
                            if ((strArray.Length >= 2) && ((str6 = strArray[0].ToUpper()) != null))
                            {
                                if (str6 == "PACKAGESTOSPAWN")
                                {
                                    if (strArray[1].Contains(","))
                                    {
                                        strArray = strArray[1].Split(new char[] { ',' });
                                    }
                                    else
                                    {
                                        strArray = new string[] { strArray[1], strArray[1] };
                                    }
                                    int.TryParse(strArray[0], out list2.minPackagesToSpawn);
                                    int.TryParse(strArray[1], out list2.maxPackagesToSpawn);
                                }
                                else if (str6 == "SPAWNONEOFEACH")
                                {
                                    bool.TryParse(strArray[1], out list2.spawnOneOfEach);
                                }
                                else if (str6 == "NODUPLICATES")
                                {
                                    bool.TryParse(strArray[1], out list2.noDuplicates);
                                }
                                else if (str6 == "PACKAGELIST")
                                {
                                    strArray = strArray[1].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                    item = new LootSpawnList.LootWeightedEntry();
                                    if (!dictionary.ContainsKey(strArray[1]))
                                    {
                                        ConsoleSystem.LogError(string.Format("Package {0} has a reference to an spawn list named {1}, but it not exist.", list2.name, strArray[1]));
                                    }
                                    else
                                    {
                                        item.obj = dictionary[strArray[1]];
                                        float.TryParse(strArray[0], out item.weight);
                                        int.TryParse(strArray[2], out item.amountMin);
                                        int.TryParse(strArray[3], out item.amountMax);
                                        list3.Add(item);
                                        list2.LootPackages = list3.ToArray();
                                    }
                                }
                                else if (str6 == "PACKAGEITEM")
                                {
                                    strArray = strArray[1].Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                    item = new LootSpawnList.LootWeightedEntry {
                                        obj = DatablockDictionary.GetByName(strArray[1])
                                    };
                                    if (item.obj == null)
                                    {
                                        ConsoleSystem.LogError(string.Format("Package {0} has a reference to an item named {1}, but it not exist.", list2.name, strArray[1]));
                                    }
                                    else
                                    {
                                        float.TryParse(strArray[0], out item.weight);
                                        int.TryParse(strArray[2], out item.amountMin);
                                        int.TryParse(strArray[3], out item.amountMax);
                                        list3.Add(item);
                                        list2.LootPackages = list3.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private static bool smethod_1()
        {
            using (StreamWriter writer = File.CreateText(ItemsFile))
            {
                writer.WriteLine("// - Help of item properties -");
                writer.WriteLine("// SlotFlags = Flags [Belt|Chest|Cooked|Debris|Equip|Feet|FuelBasic|Head|Legs|Raw|Safe|Storage]");
                writer.WriteLine("// TransientMode = Flags [None|Untransferable|DoesNotSave|Full]");
                writer.WriteLine("// Changed properties change only on server side. But it work.");
                writer.WriteLine("");
                foreach (ItemDataBlock block2 in DatablockDictionary.All)
                {
                    BlueprintDataBlock block;
                    writer.WriteLine("[" + block2.name + "]");
                    writer.WriteLine("Description=" + block2.GetItemDescription());
                    writer.WriteLine("IsRepairable=" + block2.isRepairable);
                    writer.WriteLine("IsRecycleable=" + block2.isRecycleable);
                    writer.WriteLine("IsResearchable=" + block2.isResearchable);
                    writer.WriteLine("IsSplittable=" + block2._splittable);
                    writer.WriteLine("TransientMode=" + block2.transientMode);
                    writer.WriteLine("LoseDurability=" + block2.doesLoseCondition);
                    writer.WriteLine("MaxDurability=" + block2._maxCondition);
                    if (smethod_3(block2, out block))
                    {
                        string str = "";
                        foreach (BlueprintDataBlock.IngredientEntry entry in block.ingredients)
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, entry.amount, " \"", entry.Ingredient.name, "\", " });
                        }
                        if (str.Length > 0)
                        {
                            str = str.Substring(0, str.Length - 2);
                        }
                        writer.WriteLine("Crafting.Ingredients=" + str);
                        writer.WriteLine("Crafting.RequireWorkbench=" + block.RequireWorkbench);
                        writer.WriteLine("Crafting.Duration=" + block.craftingDuration);
                    }
                    writer.WriteLine("MinUses=" + block2._minUsesForDisplay);
                    writer.WriteLine("MaxUses=" + block2._maxUses);
                    writer.WriteLine();
                }
            }
            return true;
        }

        [CompilerGenerated]
        private static bool smethod_10(string string_3)
        {
            return string_3.Contains("[WeaponSpawnList]");
        }

        [CompilerGenerated]
        private static bool smethod_11(string string_3)
        {
            return string_3.Contains("[SupplyDropSpawnListMaster]");
        }

        private static bool smethod_2()
        {
            ItemDataBlock block = null;
            BlueprintDataBlock block2 = null;
            foreach (string str in File.ReadAllLines(ItemsFile).ToList<string>())
            {
                string[] strArray;
                System.Collections.Generic.List<BlueprintDataBlock.IngredientEntry> list2;
                string str4;
                string[] strArray5;
                int num2;
                string str2 = str.Trim();
                if (!string.IsNullOrEmpty(str2) && !str2.StartsWith("//"))
                {
                    if (str2.Contains("//"))
                    {
                        str2 = str2.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(str2))
                    {
                        if (str2.StartsWith("[") && str2.EndsWith("]"))
                        {
                            string name = str2.Substring(1, str2.Length - 2);
                            block = DatablockDictionary.GetByName(name);
                            if (block != null)
                            {
                                ItemsCount++;
                            }
                            else
                            {
                                ConsoleSystem.LogError(string.Format("Item named of {0} not exist in dictionary.", name));
                            }
                        }
                        else if (str2.Contains("=") && (block != null))
                        {
                            strArray = str2.Split(new char[] { '=' });
                            if (strArray.Length >= 2)
                            {
                                switch (strArray[0].ToUpper())
                                {
                                    case "DESCRIPTION":
                                        block.itemDescriptionOverride = strArray[1];
                                        break;

                                    case "ISREPAIRABLE":
                                        block.isRepairable = bool.Parse(strArray[1]);
                                        break;

                                    case "ISRECYCLEABLE":
                                        block.isRecycleable = bool.Parse(strArray[1]);
                                        break;

                                    case "ISRESEARCHABLE":
                                        block.isResearchable = bool.Parse(strArray[1]);
                                        break;

                                    case "ISSPLITTABLE":
                                        block._splittable = bool.Parse(strArray[1]);
                                        break;

                                    case "TRANSIENTMODE":
                                        goto Label_02B3;

                                    case "LOSEDURABILITY":
                                        block.doesLoseCondition = bool.Parse(strArray[1]);
                                        break;

                                    case "MAXDURABILITY":
                                        block._maxCondition = float.Parse(strArray[1]);
                                        break;

                                    case "CRAFTING.INGREDIENTS":
                                    {
                                        if (!smethod_3(block, out block2))
                                        {
                                            goto Label_0432;
                                        }
                                        string[] strArray2 = strArray[1].Split(new char[] { ',' });
                                        list2 = new System.Collections.Generic.List<BlueprintDataBlock.IngredientEntry>();
                                        strArray5 = strArray2;
                                        num2 = 0;
                                        goto Label_0415;
                                    }
                                    case "CRAFTING.REQUIREWORKBENCH":
                                        if (!smethod_3(block, out block2))
                                        {
                                            goto Label_0467;
                                        }
                                        block2.RequireWorkbench = bool.Parse(strArray[1]);
                                        break;

                                    case "CRAFTING.DURATION":
                                        if (!smethod_3(block, out block2))
                                        {
                                            goto Label_0499;
                                        }
                                        block2.craftingDuration = float.Parse(strArray[1]);
                                        break;

                                    case "MINUSES":
                                        block._minUsesForDisplay = int.Parse(strArray[1]);
                                        break;

                                    case "MAXUSES":
                                        block._maxUses = int.Parse(strArray[1]);
                                        break;
                                }
                            }
                        }
                    }
                }
                continue;
            Label_02B3:
                if (strArray[1].IndexOf("full", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    block.transientMode = ItemDataBlock.TransientMode.Full;
                }
                if (strArray[1].IndexOf("doesnotsave", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    block.transientMode = ItemDataBlock.TransientMode.DoesNotSave;
                }
                if (strArray[1].IndexOf("untransferable", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    block.transientMode = ItemDataBlock.TransientMode.Untransferable;
                }
                if (strArray[1].IndexOf("none", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    block.transientMode = ItemDataBlock.TransientMode.None;
                }
                continue;
            Label_0382:
                str4 = strArray5[num2];
                string[] strArray3 = Facepunch.Utility.String.SplitQuotesStrings(str4);
                if (strArray3.Length < 2)
                {
                    strArray3 = new string[] { "1", strArray3[0] };
                }
                ItemDataBlock byName = DatablockDictionary.GetByName(strArray3[1]);
                if (byName != null)
                {
                    BlueprintDataBlock.IngredientEntry item = new BlueprintDataBlock.IngredientEntry {
                        amount = int.Parse(strArray3[0]),
                        Ingredient = byName
                    };
                    list2.Add(item);
                }
                else
                {
                    ConsoleSystem.LogError(string.Format("Blueprint ingredient {0} not exist for item {1}.", strArray3[1], block.name));
                }
                num2++;
            Label_0415:
                if (num2 < strArray5.Length)
                {
                    goto Label_0382;
                }
                block2.ingredients = list2.ToArray();
                continue;
            Label_0432:
                ConsoleSystem.LogError(string.Format("Blueprint for item {1} not exist.", block.name));
                continue;
            Label_0467:
                ConsoleSystem.LogError(string.Format("Blueprint for item {1} not exist.", block.name));
                continue;
            Label_0499:
                ConsoleSystem.LogError(string.Format("Blueprint for item {1} not exist.", block.name));
            }
            return true;
        }

        private static bool smethod_3(ItemDataBlock itemDataBlock_0, out BlueprintDataBlock blueprintDataBlock_0)
        {
            foreach (ItemDataBlock block in DatablockDictionary.All)
            {
                BlueprintDataBlock block2 = block as BlueprintDataBlock;
                if ((block2 != null) && (block2.resultItem == itemDataBlock_0))
                {
                    blueprintDataBlock_0 = block2;
                    return true;
                }
            }
            blueprintDataBlock_0 = null;
            return false;
        }

        private static bool smethod_4()
        {
            GenericSpawner[] spawnerArray = UnityEngine.Object.FindObjectsOfType<GenericSpawner>();
            using (StreamWriter writer = File.CreateText(SpawnsFile))
            {
                foreach (GenericSpawner spawner in spawnerArray)
                {
                    writer.WriteLine("[" + spawner.name + "]");
                    writer.WriteLine("RADIUS=" + spawner.radius);
                    writer.WriteLine("THINKDELAY=" + spawner.thinkDelay);
                    writer.WriteLine("POSITION=" + spawner.transform.position.AsString());
                    writer.WriteLine("//\tPrefabName\tMaxPopulation\tSpawnPerTick\tUseNavmeshSample\tForceStatic");
                    foreach (GenericSpawnerSpawnList.GenericSpawnInstance instance in spawner._spawnList)
                    {
                        writer.Write("SPAWN=" + instance.prefabName);
                        writer.Write(",\t\t" + instance.targetPopulation);
                        writer.Write(",\t" + instance.numToSpawnPerTick);
                        writer.Write(",\t" + instance.useNavmeshSample);
                        writer.Write(",\t" + instance.forceStaticInstantiate);
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                    UnityEngine.Object.Destroy(spawner);
                }
            }
            return true;
        }

        private static bool smethod_5()
        {
            foreach (GenericSpawner spawner in UnityEngine.Object.FindObjectsOfType<GenericSpawner>())
            {
                UnityEngine.Object.Destroy(spawner);
            }
            System.Collections.Generic.List<string> list = File.ReadAllLines(SpawnsFile).ToList<string>();
            if (list.Count == 0)
            {
                return false;
            }
            GenericSpawner spawner2 = null;
            GenericSpawnerSpawnList.GenericSpawnInstance item = null;
            foreach (string str in list)
            {
                string str2 = str.Trim();
                if (!string.IsNullOrEmpty(str2) && !str2.StartsWith("//"))
                {
                    if (str2.Contains("//"))
                    {
                        str2 = str2.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(str2))
                    {
                        if (str2.StartsWith("[") && str2.EndsWith("]"))
                        {
                            str2.Substring(1, str2.Length - 2);
                            spawner2 = null;
                        }
                        else if (str2.Contains("=") && (spawner2 != null))
                        {
                            string str3;
                            string[] strArray = str2.Replace(" ", "").Split(new char[] { '=' });
                            if ((strArray.Length >= 2) && ((str3 = strArray[0].ToUpper()) != null))
                            {
                                if (str3 == "RADIUS")
                                {
                                    if (!float.TryParse(strArray[1], out spawner2.radius))
                                    {
                                        spawner2.radius = 50f;
                                    }
                                }
                                else if (str3 == "THINKDELAY")
                                {
                                    if (!float.TryParse(strArray[1], out spawner2.thinkDelay))
                                    {
                                        spawner2.thinkDelay = 60f;
                                    }
                                }
                                else if (str3 == "POSITION")
                                {
                                    strArray = strArray[1].Replace(" ", "").Split(new char[] { ',' });
                                    if (strArray.Length < 3)
                                    {
                                        UnityEngine.Object.Destroy(spawner2);
                                        spawner2 = null;
                                    }
                                    else
                                    {
                                        float result = 0f;
                                        if (!float.TryParse(strArray[0], out result))
                                        {
                                            UnityEngine.Object.Destroy(spawner2);
                                            spawner2 = null;
                                        }
                                        else
                                        {
                                            float num2 = 0f;
                                            if (!float.TryParse(strArray[1], out num2))
                                            {
                                                UnityEngine.Object.Destroy(spawner2);
                                                spawner2 = null;
                                            }
                                            else
                                            {
                                                float num3 = 0f;
                                                if (!float.TryParse(strArray[2], out num3))
                                                {
                                                    UnityEngine.Object.Destroy(spawner2);
                                                    spawner2 = null;
                                                }
                                                else
                                                {
                                                    spawner2.transform.position = new Vector3(result, num2, num3);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (str3 == "SPAWN")
                                {
                                    strArray = strArray[1].Replace(" ", "").Split(new char[] { ',' });
                                    item = new GenericSpawnerSpawnList.GenericSpawnInstance();
                                    if (strArray.Length > 0)
                                    {
                                        item.prefabName = strArray[0];
                                    }
                                    if ((strArray.Length > 1) && int.TryParse(strArray[1], out item.targetPopulation))
                                    {
                                        item.targetPopulation = 1;
                                    }
                                    if ((strArray.Length > 2) && int.TryParse(strArray[2], out item.numToSpawnPerTick))
                                    {
                                        item.numToSpawnPerTick = 1;
                                    }
                                    if (item.numToSpawnPerTick > item.targetPopulation)
                                    {
                                        item.numToSpawnPerTick = item.targetPopulation;
                                    }
                                    if ((strArray.Length > 3) && bool.TryParse(strArray[3], out item.useNavmeshSample))
                                    {
                                        item.useNavmeshSample = false;
                                    }
                                    if ((strArray.Length > 4) && bool.TryParse(strArray[4], out item.forceStaticInstantiate))
                                    {
                                        item.forceStaticInstantiate = false;
                                    }
                                    spawner2._spawnList.Add(item);
                                    SpawnsCount++;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        [CompilerGenerated]
        private static bool smethod_6(string string_3)
        {
            return string_3.Contains("[AILootList]");
        }

        [CompilerGenerated]
        private static bool smethod_7(string string_3)
        {
            return string_3.Contains("[AmmoSpawnList]");
        }

        [CompilerGenerated]
        private static bool smethod_8(string string_3)
        {
            return string_3.Contains("[JunkSpawnList]");
        }

        [CompilerGenerated]
        private static bool smethod_9(string string_3)
        {
            return string_3.Contains("[MedicalSpawnList]");
        }

        public static int ItemsCount
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

        public static string ItemsFile
        {
            [CompilerGenerated]
            get
            {
                return string_1;
            }
            [CompilerGenerated]
            private set
            {
                string_1 = value;
            }
        }

        public static bool ItemsFileCreated
        {
            [CompilerGenerated]
            get
            {
                return bool_3;
            }
            [CompilerGenerated]
            private set
            {
                bool_3 = value;
            }
        }

        public static bool ItemsInitialized
        {
            [CompilerGenerated]
            get
            {
                return bool_2;
            }
            [CompilerGenerated]
            private set
            {
                bool_2 = value;
            }
        }

        public static int LootsCount
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

        public static string LootsFile
        {
            [CompilerGenerated]
            get
            {
                return string_0;
            }
            [CompilerGenerated]
            private set
            {
                string_0 = value;
            }
        }

        public static bool LootsFileCreated
        {
            [CompilerGenerated]
            get
            {
                return bool_1;
            }
            [CompilerGenerated]
            private set
            {
                bool_1 = value;
            }
        }

        public static bool LootsInitialized
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

        public static int SpawnsCount
        {
            [CompilerGenerated]
            get
            {
                return int_2;
            }
            [CompilerGenerated]
            private set
            {
                int_2 = value;
            }
        }

        public static string SpawnsFile
        {
            [CompilerGenerated]
            get
            {
                return string_2;
            }
            [CompilerGenerated]
            private set
            {
                string_2 = value;
            }
        }

        public static bool SpawnsFileCreated
        {
            [CompilerGenerated]
            get
            {
                return bool_5;
            }
            [CompilerGenerated]
            private set
            {
                bool_5 = value;
            }
        }

        public static bool SpawnsInitialized
        {
            [CompilerGenerated]
            get
            {
                return bool_4;
            }
            [CompilerGenerated]
            private set
            {
                bool_4 = value;
            }
        }
    }
}

