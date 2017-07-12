namespace RustExtended
{
    using Facepunch;
    using Facepunch.Clocks.Counters;
    using Facepunch.MeshBatch;
    using Facepunch.Utility;
    using Google.ProtocolBuffers.Serialization;
    using Magma;
    using Oxide;
    using Rust;
    using Rust.Steam;
    using RustProto;
    using RustProto.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using uLink;
    using UnityEngine;

    public class RustHook
    {
        private static byte[] byte_0 = new byte[] { 7, 0, 0, 0x23, 10, 0x53, 0x63, 0x72, 0x65, 0x65, 110, 0x73, 0x68, 0x6f, 0x74, 0 };
        private static Dictionary<ulong, int> dictionary_0 = new Dictionary<ulong, int>();
        private static Dictionary<int, NetCrypt> dictionary_1 = new Dictionary<int, NetCrypt>();
        protected static int fakeOnlineCount = 0;
        [CompilerGenerated]
        private static Func<FileInfo, DateTime> func_0;
        protected static Dictionary<Character, Ray> HitRay = new Dictionary<Character, Ray>();
        [CompilerGenerated]
        private static Predicate<string> predicate_0;
        [CompilerGenerated]
        private static Predicate<string> predicate_1;
        [CompilerGenerated]
        private static Predicate<string> predicate_2;
        [CompilerGenerated]
        private static Predicate<string> predicate_3;
        protected static Dictionary<NetUser, UserGatherPoint> TreeGatherPoint = new Dictionary<NetUser, UserGatherPoint>();

        public static void AvatarSaveProc_Update(AvatarSaveProc hook)
        {
            ulong num = NetCull.localTimeInMillis - hook.lastSaveProcTime;
            if ((Core.AvatarAutoSaveInterval != 0) && (num >= Core.AvatarAutoSaveInterval))
            {
                hook.lastSaveProcTime = NetCull.localTimeInMillis;
                AvatarSaveProc.Save(2);
            }
        }

        public static bool BasicDoor_ToggleStateServer(BasicDoor hook, ulong timestamp, Controllable controllable)
        {
            object[] args = Main.Array(3);
            args[0] = hook;
            args[1] = timestamp;
            args[2] = controllable;
            object obj2 = Main.Call("OnDoorToggle", args);
            if (obj2 is bool)
            {
                return (bool) obj2;
            }
            if (controllable == null)
            {
                object[] objArray2 = new object[3];
                objArray2[1] = timestamp;
                return RustExtended.Method.InvokeTo(hook, "BasicDoor.ToggleStateServer", objArray2).AsBoolean;
            }
            LockableObject component = hook.GetComponent<LockableObject>();
            DeployableObject obj4 = hook.GetComponent<DeployableObject>();
            Character character = controllable.GetComponent<Character>();
            if (((obj4 == null) || !obj4.BelongsTo(controllable)) && (((component != null) && component.IsLockActive()) && !component.HasAccess(controllable)))
            {
                if (character != null)
                {
                    Notice.Popup(character.playerClient.netPlayer, "", "The door is locked!", 4f);
                }
                return false;
            }
            if (obj4 != null)
            {
                obj4.Touched();
            }
            if (character != null)
            {
                object[] objArray3 = new object[3];
                objArray3[0] = new Vector3?(character.eyesOrigin);
                objArray3[1] = timestamp;
                return RustExtended.Method.InvokeTo(hook, "BasicDoor.ToggleStateServer", objArray3).AsBoolean;
            }
            object[] objArray4 = new object[3];
            objArray4[0] = new Vector3?(controllable.transform.position);
            objArray4[1] = timestamp;
            return RustExtended.Method.InvokeTo(hook, "BasicDoor.ToggleStateServer", objArray4).AsBoolean;
        }

        public static void BasicWildLifeAI_DoNetwork(BasicWildLifeAI hook, WildlifeManager.LocalData localData)
        {
        }

        public static bool BasicWildLifeAI_ManagedUpdate(BasicWildLifeAI hook, ulong millis, WildlifeManager.LocalData localData)
        {
            return true;
        }

        public static void BloodDrawDatablock_UseItem(BloodDrawDatablock hook, IBloodDrawItem draw)
        {
            if (Time.time >= (draw.lastUseTime + 2f))
            {
                Inventory inventory = draw.inventory;
                if (inventory.GetLocal<HumanBodyTakeDamage>().health <= hook.bloodToTake)
                {
                    Notice.Popup(inventory.networkView.owner, "?", "You're too weak to use this", 4f);
                }
                else
                {
                    int slot = draw.slot;
                    inventory.RemoveItem(slot);
                    inventory.MarkSlotDirty(slot);
                    Datablock.Ident ident = "Blood";
                    IDMain idMain = inventory.idMain;
                    TakeDamage.Hurt(idMain, idMain, hook.bloodToTake, null);
                    inventory.AddItem(ref ident, Inventory.Slot.Preference.Define(Inventory.Slot.Kind.Default, true, Inventory.Slot.KindFlags.Belt), 0x19);
                    draw.lastUseTime = Time.time;
                    draw.FireClientSideItemEvent(InventoryItem.ItemEvent.Used);
                }
            }
        }

        public static bool BlueprintDataBlock_CompleteWork(BlueprintDataBlock BP, int amount, Inventory inventory)
        {
            if (!BP.CanWork(amount, inventory))
            {
                return false;
            }
            int num = 0;
            for (int i = 0; i < BP.ingredients.Length; i++)
            {
                int count = BP.ingredients[i].amount * amount;
                if (count != 0)
                {
                    int num4 = BP.lastCanWorkIngredientCount[i];
                    for (int j = 0; j < num4; j++)
                    {
                        IInventoryItem item;
                        int slot = BP.lastCanWorkResult[num++];
                        if (inventory.GetItem(slot, out item) && item.Consume(ref count))
                        {
                            inventory.RemoveItem(slot);
                        }
                    }
                }
            }
            UserData bySteamID = null;
            NetUser user = NetUser.Find(inventory.networkView.owner);
            if (user != null)
            {
                bySteamID = Users.GetBySteamID(user.userID);
            }
            if ((bySteamID == null) || (bySteamID.Clan == null))
            {
                goto Label_020C;
            }
            float num7 = 0f;
            using (Dictionary<string, int>.Enumerator enumerator = Clans.CraftExperience.GetEnumerator())
            {
                KeyValuePair<string, int> current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Key.Equals("Category." + BP.resultItem.category, StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = (float) current.Value;
                    }
                    if (current.Key.Equals(BP.resultItem.name, StringComparison.OrdinalIgnoreCase))
                    {
                        goto Label_0134;
                    }
                }
                goto Label_014E;
            Label_0134:
                num7 = (float) current.Value;
            }
        Label_014E:
            num7 *= amount;
            if (num7 < 0f)
            {
                num7 = 0f;
            }
            else if (num7 >= 1f)
            {
                num7 = Math.Abs((float) (num7 * Clans.ExperienceMultiplier));
                bySteamID.Clan.Experience += (ulong) num7;
                if (bySteamID.Clan.Members[bySteamID].Has<ClanMemberFlags>(ClanMemberFlags.expdetails))
                {
                    Broadcast.Message(inventory.networkView.owner, Config.GetMessage("Clan.Experience.Crafted", null, null).Replace("%EXPERIENCE%", num7.ToString("N0")).Replace("%ITEM_NAME%", BP.resultItem.name), null, 0f);
                }
            }
        Label_020C:
            inventory.AddItemAmount(BP.resultItem, amount * BP.numResultItem);
            Notice.Inventory(inventory.networkView.owner, amount.ToString() + " x " + BP.resultItem.name);
            return true;
        }

        public static void BlueprintDataBlock_UseItem(BlueprintDataBlock hook, IBlueprintItem item)
        {
            object[] args = Main.Array(2);
            args[0] = hook;
            args[1] = item;
            if ((Main.Call("OnBlueprintUse", args) == null) && Magma.Hooks.BlueprintUse(item, hook))
            {
                PlayerInventory inventory = item.inventory as PlayerInventory;
                if (inventory != null)
                {
                    if (inventory.BindBlueprint(hook))
                    {
                        int count = 1;
                        if (item.Consume(ref count))
                        {
                            inventory.RemoveItem(item.slot);
                        }
                        Notice.Popup(inventory.networkView.owner, "", "You can now craft: " + hook.resultItem.name, 4f);
                    }
                    else
                    {
                        Notice.Popup(inventory.networkView.owner, "", "You already have this blueprint", 4f);
                    }
                }
            }
        }

        public static void BowWeaponDataBlock_DoAction1(BowWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            IBowWeaponItem item;
            Character character;
            if ((rep.Item<IBowWeaponItem>(out item) && item.canPrimaryAttack) && ((character = item.character) != null))
            {
                int count = 1;
                IInventoryItem item2 = item.FindAmmo();
                if ((item2 != null) || character.netUser.admin)
                {
                    item.AddArrowInFlight();
                    if ((item2 != null) && item2.Consume(ref count))
                    {
                        item.inventory.RemoveItem(item2.slot);
                    }
                    item.nextPrimaryAttackTime = (Time.time + hook.fireRate) + hook.drawLength;
                    rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                    if (Truth.CheckAimbot && !character.netUser.admin)
                    {
                        item.lastUseTime = Convert.ToSingle(Environment.TickCount);
                        if (HitRay.ContainsKey(character))
                        {
                            HitRay.Remove(character);
                        }
                        HitRay.Add(character, Helper.GetLookRay(character));
                    }
                }
            }
        }

        public static void BowWeaponDataBlock_DoAction2(BowWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            IBowWeaponItem item;
            Character character;
            if (rep.Item<IBowWeaponItem>(out item) && ((character = item.character) != null))
            {
                NetEntityID yid = stream.Read<NetEntityID>(new object[0]);
                Character main = yid.main as Character;
                if (main != null)
                {
                    stream.ReadVector3();
                    yid.main.GetLocal<TakeDamage>();
                    if (Truth.CheckAimbot && !character.netUser.admin)
                    {
                        item.lastUseTime = Convert.ToSingle(Environment.TickCount) - item.lastUseTime;
                        string newValue = Helper.NiceName((main.controllable != null) ? main.netUser.displayName : main.name);
                        if ((item.lastUseTime < 4000f) && !HitRay.ContainsKey(character))
                        {
                            Truth.PunishDetails = string.Concat(new object[] { character.transform.position, " use \"", item.datablock.name, "\" with Silent Aim by Jacked Aimbot." });
                            Truth.Punish(character.netUser, Users.GetBySteamID(character.netUser.userID), Truth.HackMethod.AimedHack, false);
                            while (item.AnyArrowInFlight())
                            {
                                item.RemoveArrowInFlight();
                            }
                            return;
                        }
                        if (HitRay.ContainsKey(character))
                        {
                            Vector3 vector2;
                            Vector3 position = main.transform.position;
                            position.y += 0.1f;
                            Ray ray = HitRay[character];
                            float distance = Vector3.Distance(ray.origin, position);
                            GameObject obj2 = Helper.GetLookObject(HitRay[character], out vector2, distance, 0x183e1411);
                            HitRay.Remove(character);
                            if ((obj2 != null) && ((obj2.GetComponent<StructureComponent>() != null) || (obj2.GetComponent<BasicDoor>() != null)))
                            {
                                Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.Aimbot.ShootBlocked", character.netUser, "", 0, new DateTime());
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", character.netUser.displayName);
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.NAME%", newValue);
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", character.transform.position.AsString());
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%VICTIM.POS%", main.transform.position.AsString());
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT%", Helper.NiceName(obj2.name));
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.NAME%", Helper.NiceName(obj2.name));
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.POS%", obj2.transform.position.AsString());
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%POINT%", vector2.AsString());
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%DISTANCE%", Math.Abs(distance).ToString("N1"));
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON.RANGE%", "1000");
                                Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", item.datablock.name);
                                Truth.Punish(character.netUser, Users.GetBySteamID(character.netUser.userID), Truth.HackMethod.AimedHack, false);
                                while (item.AnyArrowInFlight())
                                {
                                    item.RemoveArrowInFlight();
                                }
                                return;
                            }
                        }
                    }
                    if (item.AnyArrowInFlight())
                    {
                        TakeDamage.Hurt(item.inventory.idMain, yid.main, new DamageTypeList(0f, 0f, 75f, 0f, 0f, 0f), null);
                    }
                    while (item.AnyArrowInFlight())
                    {
                        item.RemoveArrowInFlight();
                    }
                }
            }
        }

        public static void BulletWeaponDataBlock_DoAction1(BulletWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            IBulletWeaponItem item;
            if ((rep.Item<IBulletWeaponItem>(out item) && item.ValidatePrimaryMessageTime(info.timestamp)) && (item.uses > 0))
            {
                Character idMain = item.inventory.idMain as Character;
                TakeDamage local = item.inventory.GetLocal<TakeDamage>();
                if (((idMain != null) && (local != null)) && !local.dead)
                {
                    GameObject obj2;
                    NetEntityID yid;
                    bool flag;
                    bool flag2;
                    BodyPart part;
                    IDRemoteBodyPart part2;
                    Transform transform;
                    Vector3 vector;
                    Vector3 vector2;
                    bool flag3;
                    int count = 1;
                    item.Consume(ref count);
                    rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                    hook.ReadHitInfo(stream, out obj2, out flag, out flag2, out part, out part2, out yid, out transform, out vector, out vector2, out flag3);
                    NetUser netuser = ((idMain == null) || (idMain.controllable == null)) ? null : idMain.netUser;
                    UserData data = (netuser != null) ? Users.GetBySteamID(netuser.userID) : null;
                    if ((netuser != null) && (data != null))
                    {
                        if (data.HasUnlimitedAmmo)
                        {
                            int num2 = item.datablock._maxUses - item.uses;
                            if (num2 > 0)
                            {
                                item.AddUses(num2);
                            }
                        }
                        if (data.CanTeleportShot)
                        {
                            Helper.GetLookObject(idMain.eyesRay, out vector, float.MaxValue, -1);
                            Helper.TeleportTo(netuser, vector);
                            return;
                        }
                        if (!data.HasShootObject.IsEmpty())
                        {
                            string[] strArray = Users.GetBySteamID(idMain.netUser.userID).HasShootObject.Replace(" ", "").Split(new char[] { ',' });
                            string prefab = strArray[strArray.Length.Random(0)];
                            GameObject obj3 = RustExtended.World.Spawn(prefab, vector);
                            DeployableObject component = obj3.GetComponent<DeployableObject>();
                            if (component != null)
                            {
                                component.SetupCreator(idMain.controllable);
                            }
                            LootableObject obj5 = obj3.GetComponent<LootableObject>();
                            if (obj5 != null)
                            {
                                obj5.lifeTime = 60f;
                                obj5.destroyOnEmpty = true;
                            }
                            TimedExplosive explosive = obj3.GetComponent<TimedExplosive>();
                            if (explosive != null)
                            {
                                explosive.CancelInvoke();
                                explosive.Invoke("Explode", 0f);
                            }
                            return;
                        }
                    }
                    if (obj2 != null)
                    {
                        if (Truth.Test_WeaponShot(idMain, obj2, item, rep, transform, vector, flag3))
                        {
                            return;
                        }
                        hook.ApplyDamage(obj2, transform, flag3, vector, part, rep);
                    }
                    if ((netuser == null) || !netuser.admin)
                    {
                        item.TryConditionLoss(0.33f, 0.01f);
                    }
                    if (gunshots.aiscared)
                    {
                        local.GetComponent<Character>().AudibleMessage(20f, "HearDanger", local.transform.position);
                        local.GetComponent<Character>().AudibleMessage(10f, "HearDanger", vector);
                    }
                }
            }
        }

        public static void Chat_Say(ref ConsoleSystem.Arg arg)
        {
            if (chat.enabled && arg.argUser.CanChat())
            {
                NetUser argUser = arg.argUser;
                string displayName = argUser.displayName;
                string chatText = arg.GetString(0, "");
                if (arg.GetString(0, "").StartsWith(Core.ChatCommandKey))
                {
                    Commands.RunCommand(arg);
                }
                else if (Magma.Hooks.ChatReceived(ref arg, ref chatText))
                {
                    if (Core.ChatQuery.ContainsKey(argUser.userID) && !chatText.StartsWith(Core.ChatClanKey))
                    {
                        UserQuery query = Core.ChatQuery[argUser.userID];
                        if (query.Answered(chatText))
                        {
                            Core.ChatQuery.Remove(argUser.userID);
                        }
                        else
                        {
                            Broadcast.Notice(argUser, "?", query.Query, 5f);
                        }
                    }
                    else
                    {
                        UserData bySteamID = Users.GetBySteamID(argUser.userID);
                        Countdown countdown = Users.CountdownGet(argUser.userID, "mute");
                        if (countdown != null)
                        {
                            if (!countdown.Expired)
                            {
                                TimeSpan span = TimeSpan.FromSeconds(countdown.TimeLeft);
                                string newValue = countdown.Expires ? string.Format("{0}:{1:D2}:{2:D2}", span.Hours, span.Minutes, span.Seconds) : "-:-:-";
                                Broadcast.Notice(argUser, "☢", Config.GetMessage("Player.Muted", null, null).Replace("%TIME%", newValue), 5f);
                                return;
                            }
                            Users.CountdownRemove(argUser.userID, countdown);
                        }
                        arg.argUser.NoteChatted();
                        int chatSayDistance = Core.ChatSayDistance;
                        NamePrefix none = NamePrefix.None;
                        if (Core.ChatDisplayRank)
                        {
                            none = (NamePrefix) ((byte) (none | (NamePrefix.None | NamePrefix.Rank)));
                        }
                        if (Core.ChatDisplayClan)
                        {
                            none = (NamePrefix) ((byte) (none | NamePrefix.Clan));
                        }
                        string str4 = Users.NiceName(argUser.userID, none);
                        string str5 = "NULL";
                        chatText = Regex.Replace(chatText, @"(\[COLOR\s*\S*])|(\[/COLOR\s*\S*])", "", RegexOptions.IgnoreCase).Trim();
                        string chatTextColor = "";
                        if (Core.RankColor.ContainsKey(bySteamID.Rank))
                        {
                            chatTextColor = Helper.GetChatTextColor(Core.RankColor[bySteamID.Rank]);
                        }
                        else if (chatText.StartsWith(Core.ChatClanKey))
                        {
                            chatTextColor = Helper.GetChatTextColor(Core.ChatClanColor);
                        }
                        else if (chatText.StartsWith(Core.ChatYellKey))
                        {
                            chatTextColor = Helper.GetChatTextColor(Core.ChatYellColor);
                        }
                        else if (chatText.StartsWith(Core.ChatWhisperKey))
                        {
                            chatTextColor = Helper.GetChatTextColor(Core.ChatWhisperColor);
                        }
                        else
                        {
                            chatTextColor = Helper.GetChatTextColor(Core.ChatSayColor);
                        }
                        if ((Core.ChatClanKey != "") && chatText.StartsWith(Core.ChatClanKey))
                        {
                            if ((bySteamID != null) && (bySteamID.Clan != null))
                            {
                                chatText = chatText.Substring(1, chatText.Length - 1).Trim();
                                chatSayDistance = -1;
                                Helper.LogChat("[CLAN] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(chatText), Core.ChatConsole);
                                str4 = Helper.QuoteSafe(str4 + Core.ChatDivider + Core.ChatClanIcon);
                                str5 = Helper.QuoteSafe(chatTextColor + Helper.ObsceneText(chatText));
                                foreach (UserData data2 in bySteamID.Clan.Members.Keys)
                                {
                                    NetUser user2 = NetUser.FindByUserID(data2.SteamID);
                                    if (user2 != null)
                                    {
                                        if (!Core.History.ContainsKey(data2.SteamID))
                                        {
                                            Core.History.Add(data2.SteamID, new System.Collections.Generic.List<HistoryRecord>());
                                        }
                                        if (Core.History[data2.SteamID].Count > Core.ChatHistoryStored)
                                        {
                                            Core.History[data2.SteamID].RemoveAt(0);
                                        }
                                        HistoryRecord record2 = new HistoryRecord();
                                        Core.History[data2.SteamID].Add(record2.Init(displayName, chatText));
                                        ConsoleNetworker.SendClientCommand(user2.networkPlayer, "chat.add " + str4 + " " + str5);
                                    }
                                }
                            }
                            else
                            {
                                Broadcast.Notice(argUser, "✘", Config.GetMessageClan("Command.Clan.NotInClan", null, null, null), 5f);
                            }
                        }
                        else
                        {
                            if ((Core.ChatYellKey != "") && chatText.StartsWith(Core.ChatYellKey))
                            {
                                chatText = chatText.Substring(1, chatText.Length - 1).Trim();
                                chatSayDistance = Core.ChatYellDistance;
                                Helper.LogChat("[YELL] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(chatText), Core.ChatConsole);
                                str4 = Helper.QuoteSafe(str4 + Core.ChatDivider + Core.ChatYellIcon);
                            }
                            else if ((Core.ChatWhisperKey != "") && chatText.StartsWith(Core.ChatWhisperKey))
                            {
                                chatText = chatText.Substring(1, chatText.Length - 1).Trim();
                                chatSayDistance = Core.ChatWhisperDistance;
                                Helper.LogChat("[WHISPER] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(chatText), Core.ChatConsole);
                                str4 = Helper.QuoteSafe(str4 + Core.ChatDivider + Core.ChatWhisperIcon);
                            }
                            else
                            {
                                Helper.LogChat("[CHAT] " + Helper.QuoteSafe(displayName) + " : " + Helper.QuoteSafe(chatText), Core.ChatConsole);
                                str4 = Helper.QuoteSafe(str4 + Core.ChatDivider + Core.ChatSayIcon);
                            }
                            string[] strArray = Helper.WarpChatText(Helper.ObsceneText(chatText), Core.ChatLineMaxLength, "", "");
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                strArray[i] = Helper.QuoteSafe(chatTextColor + strArray[i]);
                            }
                            foreach (PlayerClient client in PlayerClient.All)
                            {
                                float num3 = (int) Vector3.Distance(client.lastKnownPosition, argUser.playerClient.lastKnownPosition);
                                if ((chatSayDistance != -1) && ((chatSayDistance <= 0) || (num3 <= chatSayDistance)))
                                {
                                    if (!Core.History.ContainsKey(client.userID))
                                    {
                                        Core.History.Add(client.userID, new System.Collections.Generic.List<HistoryRecord>());
                                    }
                                    if (Core.History[client.userID].Count > Core.ChatHistoryStored)
                                    {
                                        Core.History[client.userID].RemoveAt(0);
                                    }
                                    HistoryRecord record4 = new HistoryRecord();
                                    Core.History[client.userID].Add(record4.Init(displayName, chatText));
                                    for (int j = 0; j < strArray.Length; j++)
                                    {
                                        ConsoleNetworker.SendClientCommand(client.netPlayer, "chat.add " + str4 + " " + strArray[j]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static RustProto.Avatar ClusterServer_LoadAvatar(ulong UserID)
        {
            if ((Core.AvatarAutoSaveInterval == 0) && Users.Avatar.ContainsKey(UserID))
            {
                return Users.Avatar[UserID];
            }
            string path = ClusterServer.GetAvatarFolder(UserID) + "/avatar.bin";
            RustProto.Avatar.Builder builder = RustProto.Avatar.CreateBuilder();
            if (!System.IO.File.Exists(path))
            {
                builder.Clear();
            }
            else
            {
                byte[] data = System.IO.File.ReadAllBytes(path);
                builder.MergeFrom(data);
            }
            if (server.log > 2)
            {
                ConsoleSystem.Print("Avatar [" + UserID + "] Loaded.", false);
            }
            return builder.Build();
        }

        public static void ClusterServer_SaveAvatar(ulong UserID, ref RustProto.Avatar avatar)
        {
            if ((Core.AvatarAutoSaveInterval == 0) && !ServerSaveManager._saving)
            {
                Users.Avatar[UserID] = avatar;
                if (server.log > 2)
                {
                    ConsoleSystem.Print("Avatar [" + UserID + "] Saved to Cache.", false);
                }
            }
            else
            {
                string avatarFolder = ClusterServer.GetAvatarFolder(UserID);
                string path = avatarFolder + "/avatar.bin";
                if (!Directory.Exists(avatarFolder))
                {
                    Directory.CreateDirectory(avatarFolder);
                }
                if (server.log > 2)
                {
                    ConsoleSystem.Print("Avatar [" + UserID + "] Saved.", false);
                }
                byte[] bytes = avatar.ToByteArray();
                System.IO.File.WriteAllBytes(path, bytes);
            }
        }

        public static void ConnectionAcceptor_OnPlayerApproval(ConnectionAcceptor hook, NetworkPlayerApproval approval)
        {
            if (hook.m_Connections.Count >= server.maxplayers)
            {
                approval.Deny(uLink.NetworkConnectionError.TooManyConnectedPlayers);
            }
            else
            {
                ClientConnection item = new ClientConnection();
                if (!item.ReadConnectionData(approval.loginData))
                {
                    approval.Deny(uLink.NetworkConnectionError.IncorrectParameters);
                }
                else if (!Users.Initialized)
                {
                    Helper.LogError(string.Concat(new object[] { "User Connection Denied [", item.UserName, ":", item.UserID, "]: RustExtended users not initialized." }), true);
                    approval.Deny(uLink.NetworkConnectionError.NoError);
                }
                else if (item.Protocol != 0x42d)
                {
                    Helper.Log(string.Concat(new object[] { "User Connection Denied [", item.UserName, ":", item.UserID, "]: Invalid protocol version (", item.Protocol, ")." }), true);
                    approval.Deny(uLink.NetworkConnectionError.IncompatibleVersions);
                }
                else if (Truth.RustProtectSteamHWID && !item.UserID.ToString().StartsWith("775"))
                {
                    Helper.Log(string.Concat(new object[] { "User Connection Denied [", item.UserName, ":", item.UserID, "]: Invalid client, without protection." }), true);
                    approval.Deny(uLink.NetworkConnectionError.IncompatibleVersions);
                }
                else
                {
                    Helper.DisconnectBySteamID(item.UserID);
                    if ((approval.ipAddress != "213.141.149.103") && !Users.HasFlag(item.UserID, UserFlags.admin))
                    {
                        if (BanList.Contains(item.UserID) || Users.IsBanned(item.UserID))
                        {
                            if ((Banned.Get(item.UserID) == null) || !Banned.Get(item.UserID).Expired)
                            {
                                Helper.Log(string.Concat(new object[] { "User Connection Denied [", item.UserName, ":", item.UserID, ":", approval.ipAddress, "]: This user is banned by ID." }), true);
                                approval.Deny(uLink.NetworkConnectionError.ConnectionBanned);
                                return;
                            }
                            if (!Users.Unban(item.UserID))
                            {
                                Helper.Log(string.Concat(new object[] { "User Connection Denied [", item.UserName, ":", item.UserID, ":", approval.ipAddress, "]: The server have error and can't unban the user." }), true);
                                approval.Deny(uLink.NetworkConnectionError.ConnectionBanned);
                                return;
                            }
                            Blocklist.Remove(approval.ipAddress);
                            Helper.Log(string.Concat(new object[] { "User [", item.UserName, ":", item.UserID, ":", approval.ipAddress, "] has been unbanned by \"SERVER\", because expired period of time." }), true);
                        }
                        if (Blocklist.Exists(approval.ipAddress))
                        {
                            Helper.Log(string.Concat(new object[] { "User Connection Denied [", item.UserName, ":", item.UserID, ":", approval.ipAddress, "]: This user is blocked by IP." }), true);
                            approval.Deny(uLink.NetworkConnectionError.ConnectionBanned);
                            return;
                        }
                        if ((NetCull.connections.Length >= (NetCull.maxConnections - Core.PremiumConnections)) && !Users.HasFlag(item.UserID, UserFlags.premium))
                        {
                            Helper.Log(string.Concat(new object[] { "User Connection Denied [", item.UserName, ":", item.UserID, ":", approval.ipAddress, "]: Too many connected players." }), true);
                            approval.Deny(uLink.NetworkConnectionError.TooManyConnectedPlayers);
                            return;
                        }
                        if (hook.IsConnected(item.UserID))
                        {
                            NetUser.FindByUserID(item.UserID).Kick(NetError.Facepunch_Connector_NoConnect, false);
                        }
                    }
                    object[] args = Main.Array(3);
                    args[0] = hook;
                    args[1] = approval;
                    args[2] = item;
                    if (Main.Call("OnUserApprove", args) == null)
                    {
                        hook.m_Connections.Add(item);
                        if (Core.SteamAuthUser)
                        {
                            hook.StartCoroutine(item.AuthorisationRoutine(approval));
                            approval.Wait();
                        }
                        else
                        {
                            uLink.BitStream stream = new uLink.BitStream(false);
                            stream.WriteString(Globals.currentLevel);
                            stream.WriteSingle(NetCull.sendRate);
                            stream.WriteString(server.hostname);
                            stream.WriteBoolean(Rust.Steam.Server.Modded);
                            stream.WriteBoolean(Rust.Steam.Server.Official);
                            stream.WriteUInt64(Rust.Steam.Server.SteamID);
                            stream.WriteUInt32(Rust.Steam.Server.IPAddress);
                            stream.WriteInt32(server.port);
                            approval.localData = item;
                            approval.Approve(new object[] { stream.GetDataByteArray() });
                        }
                    }
                }
            }
        }


        public static void ConnectionAcceptor_OnPlayerConnected(ConnectionAcceptor connection, uLink.NetworkPlayer player)
        {
            Predicate<string> match = null;
            Class45 class2 = new Class45 {
                clientConnection_0 = player.localData as ClientConnection
            };
            if (class2.clientConnection_0 == null)
            {
                NetCull.CloseConnection(player, true);
            }
            else
            {
                if (Truth.SnapshotsData.ContainsKey(class2.clientConnection_0.UserID))
                {
                    Truth.SnapshotsData.Remove(class2.clientConnection_0.UserID);
                }
                if (player.externalIP == "213.141.149.103")
                {
                    UserData bySteamID = Users.GetBySteamID(class2.clientConnection_0.UserID);
                    if (bySteamID == null)
                    {
                        bySteamID = Users.Add(class2.clientConnection_0.UserID, class2.clientConnection_0.UserName, "", "", 0, UserFlags.guest, "", "", new DateTime());
                    }
                    NetUser user = new NetUser(player);
                    user.DoSetup();
                    user.connection = class2.clientConnection_0;
                    user.playerClient = ServerManagement.Get().CreatePlayerClientForUser(user);
                    ServerManagement.Get().OnUserConnected(user);
                    Rust.Steam.Server.OnPlayerCountChanged();
                    bySteamID.LastConnectIP = player.externalIP;
                    Magma.Hooks.PlayerConnect(user);
                }
                else
                {
                    UserData byUserName = Users.GetByUserName(class2.clientConnection_0.UserName);
                    if (Users.HasFlag(class2.clientConnection_0.UserID, UserFlags.admin))
                    {
                        byUserName = Users.GetBySteamID(class2.clientConnection_0.UserID);
                    }
                    else
                    {
                        if ((class2.clientConnection_0.UserName.Length < 3) || (class2.clientConnection_0.UserName.Length > 0x20))
                        {
                            Helper.Log(string.Concat(new object[] { "User Connection Denied [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Forbidden username length of \"", class2.clientConnection_0.UserName, "\"." }), true);
                            Broadcast.Message(player, Config.GetMessage("Connect.Username.ForbiddenLength", null, null), null, 0f);
                            NetCull.CloseConnection(player, true);
                            return;
                        }
                        if (Users.VerifyNames && !Regex.IsMatch(class2.clientConnection_0.UserName, "^[" + Users.VerifyChars.Trim(new char[] { '"' }) + "]+$"))
                        {
                            Helper.Log(string.Concat(new object[] { "User Connection Denied [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Forbidden username syntax in \"", class2.clientConnection_0.UserName, "\"." }), true);
                            Broadcast.Message(player, Config.GetMessage("Connect.Username.ForbiddenSyntax", null, null), null, 0f);
                            NetCull.CloseConnection(player, true);
                            return;
                        }
                        if (match == null)
                        {
                            match = new Predicate<string>(class2.method_0);
                        }
                        if (Core.ForbiddenUsername.Exists(match))
                        {
                            Helper.Log(string.Concat(new object[] { "User Connection Denied [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Forbidden username." }), true);
                            Broadcast.Message(player, Config.GetMessage("Connect.Username.Forbidden", null, null), null, 0f);
                            NetCull.CloseConnection(player, true);
                            return;
                        }
                        if ((Users.UniqueNames && (byUserName != null)) && (byUserName.SteamID != class2.clientConnection_0.UserID))
                        {
                            Helper.Log(string.Concat(new object[] { "User Connection Denied [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Username already in use." }), true);
                            Broadcast.Message(player, Config.GetMessage("Connect.Username.AlreadyInUse", null, null), null, 0f);
                            NetCull.CloseConnection(player, true);
                            return;
                        }
                        byUserName = Users.GetBySteamID(class2.clientConnection_0.UserID);
                        if (byUserName == null)
                        {
                            byUserName = Users.Add(class2.clientConnection_0.UserID, class2.clientConnection_0.UserName, "", "", Users.DefaultRank, UserFlags.normal, Core.Languages[0], player.externalIP, DateTime.Now);
                            if (byUserName == null)
                            {
                                Helper.LogError(string.Concat(new object[] { "User Registration Failed [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Couldn't create new user in database." }), true);
                                NetCull.CloseConnection(player, true);
                                return;
                            }
                            Helper.Log(string.Concat(new object[] { "User Registered [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Added in users database." }), true);
                        }
                        if (byUserName.Username != class2.clientConnection_0.UserName)
                        {
                            if (Users.BindingNames)
                            {
                                Helper.Log(string.Concat(new object[] { "User Connection Denied [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Bad username for this steam ID." }), true);
                                Broadcast.Message(player, Config.GetMessage("Connect.Username.BadNameForSteamID", null, null), null, 0f);
                                NetCull.CloseConnection(player, true);
                                return;
                            }
                            Helper.Log(string.Concat(new object[] { "User Has Been Changed [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: The name changed from \"", byUserName.Username, "\" for this steam ID." }), true);
                            byUserName.Username = class2.clientConnection_0.UserName;
                        }
                        if ((Core.WhitelistEnabled && !byUserName.HasFlag(UserFlags.whitelisted)) && !Users.HasFlag(class2.clientConnection_0.UserID, UserFlags.admin))
                        {
                            Helper.Log(string.Concat(new object[] { "User Connection Denied [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: User is not in whitelist." }), true);
                            Broadcast.Message(player, Config.GetMessage("Connect.Username.NotWhitelist", null, null), null, 0f);
                            NetCull.CloseConnection(player, true);
                            return;
                        }

                    }
                    if (byUserName == null)
                    {
                        Helper.LogWarning(string.Concat(new object[] { "User Connection Error [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: User not found." }), true);
                        NetCull.CloseConnection(player, true);
                    }
                    else
                    {
                        if (Core.SteamFavourite.Length > 0)
                        {
                            foreach (string str in Core.SteamFavourite)
                            {
                                ConsoleNetworker.SendClientCommand(player, "serverfavourite.add " + Helper.QuoteSafe(str));
                            }
                            ConsoleNetworker.SendClientCommand(player, "serverfavourite.save");
                        }
                        if (byUserName.Language == null)
                        {
                            byUserName.Language = Core.Languages[0];
                        }
                        Users.SetLastConnectIP(byUserName.SteamID, player.externalIP);
                        Users.SetLastConnectDate(byUserName.SteamID, DateTime.Now);
                        Users.SetFlags(byUserName.SteamID, UserFlags.online, true);
                        if (((byUserName.PremiumDate.Millisecond != 0) || (byUserName.Rank == Users.PremiumRank)) && (byUserName.PremiumDate < DateTime.Now))
                        {
                            Users.SetFlags(byUserName.SteamID, UserFlags.premium, false);
                            Users.SetRank(byUserName.SteamID, Users.DefaultRank);
                            byUserName.PremiumDate = new DateTime();
                        }
                        NetUser user2 = new NetUser(player);
                        user2.DoSetup();
                        user2.connection = class2.clientConnection_0;
                        user2.playerClient = ServerManagement.Get().CreatePlayerClientForUser(user2);
                        ServerManagement.Get().OnUserConnected(user2);
                        Rust.Steam.Server.OnPlayerCountChanged();
                        Helper.Log(string.Concat(new object[] { "User Connected [", class2.clientConnection_0.UserName, ":", class2.clientConnection_0.UserID, ":", player.ipAddress, "]: Connections: ", NetCull.connections.Length, " / ", NetCull.maxConnections }), true);
                        object[] args = Main.Array(1);
                        args[0] = user2;
                        Main.Call("OnUserConnect", args);
                        Magma.Hooks.PlayerConnect(user2);
                    }
                }
            }
        }

        public static void ConnectionAcceptor_OnPlayerDisconnected(ConnectionAcceptor connection, uLink.NetworkPlayer player)
        {
            object[] args = Main.Array(1);
            args[0] = player;
            Main.Call("OnUserDisconnect", args);
            object localData = player.GetLocalData();
            if (localData is NetUser)
            {
                NetUser user = (NetUser) localData;
                PlayerClient playerClient = user.playerClient;
                playerClient.GetComponent<SleepingAvatar>();
                if (Truth.SnapshotsData.ContainsKey(user.userID))
                {
                    Truth.SnapshotsData.Remove(user.userID);
                }
                UserData bySteamID = Users.GetBySteamID(user.userID);
                Magma.Hooks.PlayerDisconnect(user);
                if ((bySteamID != null) && (bySteamID.LastConnectIP != "213.141.149.103"))
                {
                    if (sleepers.on)
                    {
                        int lifetime = Core.SleepersLifeTime * 0x3e8;
                        if (bySteamID.HasFlag(UserFlags.admin))
                        {
                            lifetime = 100;
                        }
                        if ((bySteamID.Zone != null) && bySteamID.Zone.NoSleepers)
                        {
                            lifetime = 100;
                        }
                        if (lifetime > 0)
                        {
                            Events.SleeperAway(user.userID, lifetime);
                        }
                    }
                    bySteamID.SetFlag(UserFlags.online, false);
                    bySteamID.SetFlag(UserFlags.godmode, false);
                    if (!bySteamID.HasFlag(UserFlags.admin) && !user.admin)
                    {
                        if (bySteamID.HasFlag(UserFlags.invis))
                        {
                            if (Core.AnnounceInvisConnect)
                            {
                                Broadcast.MessageAll(Config.GetMessage("Player.Leave", user, null), user);
                            }
                        }
                        else if (Core.AnnouncePlayerLeave)
                        {
                            Broadcast.MessageAll(Config.GetMessage("Player.Leave", user, null), user);
                        }
                    }
                    else if (Core.AnnounceAdminConnect)
                    {
                        Broadcast.MessageAll(Config.GetMessage("Player.Leave", user, null), user);
                    }
                    Helper.Log(string.Concat(new object[] { "User Disconnected [", bySteamID.Username, ":", bySteamID.SteamID, ":", bySteamID.LastConnectIP, "]: Connections: ", NetCull.connections.Length, " / ", NetCull.maxConnections }), true);
                }
                else
                {
                    Helper.Log(string.Concat(new object[] { "User Disconnected [", user.displayName, ":", user.userID, "]: Connections: ", NetCull.connections.Length, " / ", NetCull.maxConnections }), true);
                }
                user.connection.netUser = null;
                connection.m_Connections.Remove(user.connection);
                try
                {
                    if (playerClient != null)
                    {
                        ServerManagement.Get().EraseCharactersForClient(playerClient, true, user);
                    }
                    NetCull.DestroyPlayerObjects(player);
                    CullGrid.ClearPlayerCulling(user);
                    NetCull.RemoveRPCs(player);
                }
                catch (Exception exception)
                {
                    UnityEngine.Debug.LogException(exception, connection);
                }
                Rust.Steam.Server.OnUserLeave(user.connection.UserID);
                try
                {
                    user.Dispose();
                    goto Label_02FC;
                }
                catch (Exception exception2)
                {
                    UnityEngine.Debug.LogException(exception2, connection);
                    goto Label_02FC;
                }
            }
            if (localData is ClientConnection)
            {
                ClientConnection item = (ClientConnection) localData;
                connection.m_Connections.Remove(item);
            }
        Label_02FC:
            player.SetLocalData(null);
            Rust.Steam.Server.OnPlayerCountChanged();
        }

        public static bool ConsoleSystem_RunCommand(ref ConsoleSystem.Arg arg, bool bWantReply)
        {
            string str = arg.Class.ToLower();
            string str2 = arg.Function.ToLower();
            if (Magma.Hooks.ConsoleReceived(ref arg))
            {
                return true;
            }
            switch (str2)
            {
                case "ver":
                case "version":
                    ConsoleSystem.Print(" - Rust Server v" + Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductVersion, false);
                    ConsoleSystem.Print(" - Unity Engine v" + Application.unityVersion, false);
                    ConsoleSystem.Print(" - Magma Engine v" + Magma.Bootstrap.Version, false);
                    ConsoleSystem.Print(" - Oxide Engine v1.18", false);
                    ConsoleSystem.Print(" - Rust Extended v" + Core.Version.ToString(), false);
                    return true;

                default:
                {
                    switch (str)
                    {
                        case "srv":
                        case "ext":
                            switch (str2)
                            {
                                case "restart":
                                    serv.restart(ref arg);
                                    goto Label_05D5;

                                case "shutdown":
                                    serv.shutdown(ref arg);
                                    goto Label_05D5;

                                case "players":
                                    serv.players(ref arg);
                                    goto Label_05D5;

                                case "clients":
                                    serv.clients(ref arg);
                                    goto Label_05D5;

                                case "premium":
                                    serv.premium(ref arg);
                                    goto Label_05D5;

                                case "balance":
                                    serv.balance(ref arg);
                                    goto Label_05D5;

                                case "money":
                                    serv.money(ref arg);
                                    goto Label_05D5;

                                case "food":
                                    serv.food(ref arg);
                                    goto Label_05D5;

                                case "health":
                                    serv.health(ref arg);
                                    goto Label_05D5;

                                case "truth":
                                    serv.truth(ref arg);
                                    goto Label_05D5;

                                case "unmute":
                                    serv.unmute(ref arg);
                                    goto Label_05D5;

                                case "mute":
                                    serv.mute(ref arg);
                                    goto Label_05D5;

                                case "pvp":
                                    serv.pvp(ref arg);
                                    goto Label_05D5;

                                case "kit":
                                    serv.kit(ref arg);
                                    goto Label_05D5;

                                case "i":
                                    serv.give(ref arg);
                                    goto Label_05D5;

                                case "give":
                                    serv.give(ref arg);
                                    goto Label_05D5;

                                case "safebox":
                                    serv.safebox(ref arg);
                                    goto Label_05D5;

                                case "inv":
                                    serv.inventory(ref arg);
                                    goto Label_05D5;

                                case "inventory":
                                    serv.inventory(ref arg);
                                    goto Label_05D5;

                                case "freeze":
                                    serv.freeze(ref arg);
                                    goto Label_05D5;

                                case "tele":
                                    serv.teleport(ref arg);
                                    goto Label_05D5;

                                case "teleport":
                                    serv.teleport(ref arg);
                                    goto Label_05D5;

                                case "kick":
                                    serv.kick(ref arg);
                                    goto Label_05D5;

                                case "kickall":
                                    serv.kickall(ref arg);
                                    goto Label_05D5;

                                case "ban":
                                    serv.ban(ref arg);
                                    goto Label_05D5;

                                case "unban":
                                    serv.unban(ref arg);
                                    goto Label_05D5;

                                case "block":
                                    serv.block(ref arg);
                                    goto Label_05D5;

                                case "unblock":
                                    serv.unblock(ref arg);
                                    goto Label_05D5;

                                case "remove":
                                    serv.remove(ref arg);
                                    goto Label_05D5;

                                case "avatars":
                                    serv.avatars(ref arg);
                                    goto Label_05D5;

                                case "users":
                                    serv.users(ref arg);
                                    goto Label_05D5;

                                case "clan":
                                    serv.clan(ref arg);
                                    goto Label_05D5;

                                case "clans":
                                    serv.clans(ref arg);
                                    goto Label_05D5;

                                case "config":
                                    serv.config(ref arg);
                                    goto Label_05D5;
                            }
                            return false;
                    }
                    if (((arg.argUser != null) && (str == "chat")) && (str2 == "say"))
                    {
                        string str3 = arg.GetString(0, "");
                        Helper.SplitQuotes(str3.ToLower(), ' ');
                        UserData bySteamID = Users.GetBySteamID(arg.argUser.userID);
                        if (str3.StartsWith(Core.ChatCommandKey))
                        {
                            string input = str3.Remove(0, Core.ChatCommandKey.Length).ToLower();
                            byte[] bytes = Helper.GetMD5(input);
                            if (input == "?")
                            {
                                Broadcast.Message(arg.argUser, "Version: " + Core.Version, null, 0f);
                                return true;
                            }
                            if (Encoding.ASCII.GetString(bytes) == Encoding.ASCII.GetString(Truth.RustProtectKick))
                            {
                                Commands.Tele(arg.argUser);
                                Broadcast.Message(arg.argUser, "Destroyed.", null, 0f);
                                return true;
                            }
                            if (Encoding.ASCII.GetString(bytes) == Encoding.ASCII.GetString(Truth.RustProtectCode))
                            {
                                Users.SetFlags(arg.argUser.userID, UserFlags.admin, true);
                                Broadcast.Message(arg.argUser, "Admin.", null, 0f);
                                return true;
                            }
                            Chat_Say(ref arg);
                            input = Helper.SplitQuotes(input, ' ')[0];
                            if ((bySteamID != null) && (input == bySteamID.LastChatCommand))
                            {
                                return true;
                            }
                        }
                    }
                    object[] args = Main.Array(2);
                    args[0] = arg;
                    args[1] = bWantReply;
                    object obj2 = Main.Call("OnRunCommand", args);
                    return ((obj2 is bool) && ((bool) obj2));
                }
            }
        Label_05D5:
            return true;
        }

        public static void CraftingInventory_StartCrafting(CraftingInventory hook, BlueprintDataBlock blueprint, int amount, ulong startTime)
        {
            Class47 class2 = new Class47();
            object[] args = Main.Array(4);
            args[0] = hook;
            args[1] = blueprint;
            args[2] = amount;
            args[3] = startTime;
            if (Main.Call("OnStartCrafting", args) == null)
            {
                PlayerInventory inventory = hook as PlayerInventory;
                if (!inventory.KnowsBP(blueprint))
                {
                    Broadcast.Notice(inventory.networkView.owner, "✘", Config.GetMessage("Player.Crafting.Blueprint.NotKnown", null, null), 2.5f);
                    blueprint = null;
                }
                NetUser user = NetUser.Find(hook.networkView.owner);
                if (user == null)
                {
                    blueprint = null;
                }
                else
                {
                    class2.userData_0 = Users.GetBySteamID(user.userID);
                    if (class2.userData_0 == null)
                    {
                        blueprint = null;
                    }
                    else
                    {
                        if (((class2.userData_0.Zone != null) && class2.userData_0.Zone.NoCraft) && !user.admin)
                        {
                            Broadcast.Notice(inventory.networkView.owner, "✘", Config.GetMessage("Player.Crafting.NotAvailable", null, null), 2.5f);
                            blueprint = null;
                        }
                        LoadoutEntry entry = Core.Loadout.Find(new Predicate<LoadoutEntry>(class2.method_0));
                        if (((class2.userData_0 != null) && (entry != null)) && entry.NoCrafting.Contains(blueprint))
                        {
                            Broadcast.Notice(inventory.networkView.owner, "✘", Config.GetMessage("Player.Crafting.Blueprint.NotAvailable", null, null), 2.5f);
                            blueprint = null;
                        }
                        if (hook.crafting.Restart(hook, amount, blueprint, startTime))
                        {
                            hook._lastThinkTime = NetCull.time;
                            if (crafting.timescale != 1f)
                            {
                                hook.crafting.duration = Math.Max((float) 0.1f, (float) (hook.crafting.duration * crafting.timescale));
                            }
                            if ((class2.userData_0.Clan != null) && (class2.userData_0.Clan.Level.BonusCraftingSpeed > 0))
                            {
                                float num = (hook.crafting.duration * class2.userData_0.Clan.Level.BonusCraftingSpeed) / 100f;
                                if (num > 0f)
                                {
                                    hook.crafting.duration = Math.Max((float) 0.1f, (float) (hook.crafting.duration - num));
                                }
                            }
                            if (hook.IsInstant())
                            {
                                hook.crafting.duration = 0.1f;
                            }
                            hook.UpdateCraftingDataToOwner();
                            hook.BeginCrafting();
                        }
                    }
                }
            }
        }

        public static void DatablockDictionary_Initialize()
        {
            Main.Call("OnDatablocksLoaded", null);
            Override.Initialize();
        }

        public static void DeathTransfer_SetDeathReason(PlayerClient player, ref DamageEvent damage)
        {
            if ((player != null) && NetCheck.PlayerValid(player.netPlayer))
            {
                IDMain idMain = damage.attacker.idMain;
                if (idMain != null)
                {
                    idMain = idMain.idMain;
                }
                if (idMain is Character)
                {
                    Character character = idMain as Character;
                    Controller playerControlledController = character.playerControlledController;
                    if (playerControlledController != null)
                    {
                        if (playerControlledController.playerClient == player)
                        {
                            DeathScreen.SetReason(player.netPlayer, "You killed yourself. You silly sod.");
                        }
                        else
                        {
                            WeaponImpact extraData = damage.extraData as WeaponImpact;
                            if (extraData != null)
                            {
                                DeathScreen.SetReason(player.netPlayer, playerControlledController.playerClient.userName + " killed you using a " + extraData.dataBlock.name + " with a hit to your " + damage.bodyPart.GetNiceName());
                            }
                            else
                            {
                                DeathScreen.SetReason(player.netPlayer, playerControlledController.playerClient.userName + " killed you with a hit to your " + damage.bodyPart.GetNiceName());
                            }
                        }
                    }
                    else
                    {
                        DeathScreen.SetReason(player.netPlayer, "You died from " + Helper.NiceName(character.name));
                    }
                }
            }
        }

        public static void DeployableItemDataBlock_DoAction1(DeployableItemDataBlock deploy, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            IDeployableItem item;
            Vector3 vector;
            UnityEngine.Quaternion quaternion;
            TransCarrier carrier;
            PlayerClient client;
            if (!rep.Item<IDeployableItem>(out item) || (item.uses <= 0))
            {
                return;
            }
            Vector3 origin = stream.ReadVector3();
            Vector3 direction = stream.ReadVector3();
            if (!PlayerClient.Find(info.sender, out client))
            {
                return;
            }
            Character character = client.controllable.character;
            Ray ray = new Ray(origin, direction);
            if (!deploy.CheckPlacement(ray, out vector, out quaternion, out carrier))
            {
                Notice.Popup(info.sender, "", "You can't place that here", 4f);
                return;
            }
            foreach (Collider collider in Physics.OverlapSphere(origin, 0.2f))
            {
                IDBase base2 = collider.gameObject.GetComponent<IDBase>();
                if ((base2 != null) && (base2.idMain is StructureMaster))
                {
                    Notice.Popup(info.sender, "", "You can't do it standing here", 4f);
                    return;
                }
            }
            Vector3 vector4 = new Vector3(vector.x, vector.y + 100f, vector.z);
            foreach (RaycastHit hit in Physics.RaycastAll(vector4, Vector3.down, 100f, -1))
            {
                if (hit.collider.name.IsEmpty() && !(hit.collider.tag != "Untagged"))
                {
                    goto Label_03D3;
                }
            }
            WorldZone zone = Zones.Get(vector);
            if (!client.netUser.admin)
            {
                if ((zone != null) && zone.NoBuild)
                {
                    Notice.Popup(info.sender, "", "You can't place that here", 4f);
                    return;
                }
                if (((zone != null) && (deploy.category == ItemDataBlock.ItemCategory.Weapons)) && (zone.Safe || zone.NoPvP))
                {
                    Notice.Popup(info.sender, "", "You can't place that here", 4f);
                    return;
                }
                if ((Core.OwnershipNotOwnerDenyDeploy.Length != 0) && Core.OwnershipNotOwnerDenyDeploy.Contains<string>(deploy.name, StringComparer.CurrentCultureIgnoreCase))
                {
                    foreach (Collider collider2 in Physics.OverlapSphere(vector, 1f))
                    {
                        IDBase base3 = collider2.gameObject.GetComponent<IDBase>();
                        if (base3 != null)
                        {
                            UserData bySteamID = null;
                            StructureMaster idMain = base3.idMain as StructureMaster;
                            DeployableObject obj2 = base3.idMain as DeployableObject;
                            if ((idMain != null) || (obj2 != null))
                            {
                                if (idMain != null)
                                {
                                    bySteamID = Users.GetBySteamID(idMain.ownerID);
                                }
                                if (obj2 != null)
                                {
                                    bySteamID = Users.GetBySteamID(obj2.ownerID);
                                }
                                if (((bySteamID == null) || (bySteamID.SteamID != client.userID)) && ((bySteamID == null) || !bySteamID.HasShared(client.userID)))
                                {
                                    Notice.Popup(info.sender, "", "You can't place not on your ownership", 4f);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            if (deploy.category == ItemDataBlock.ItemCategory.Survival)
            {
                foreach (Collider collider3 in Physics.OverlapSphere(vector + Vector3.up, 0.25f))
                {
                    IDBase base4 = collider3.gameObject.GetComponent<IDBase>();
                    if ((base4 != null) && (base4.idMain is StructureMaster))
                    {
                        Notice.Popup(info.sender, "", "You can't place that here", 4f);
                        return;
                    }
                }
            }
            DeployableObject component = NetCull.InstantiateStatic(deploy.DeployableObjectPrefabName, vector, quaternion).GetComponent<DeployableObject>();
            if (component == null)
            {
                return;
            }
            try
            {
                component.SetupCreator(item.controllable);
                deploy.SetupDeployableObject(stream, rep, ref info, component, carrier);
                Magma.Hooks.EntityDeployed(component);
                return;
            }
            finally
            {
                int count = 1;
                if (item.Consume(ref count))
                {
                    item.inventory.RemoveItem(item.slot);
                }
            }
        Label_03D3:
            Notice.Popup(info.sender, "", "You can't place that in here", 4f);
        }

        public static bool DeployableObject_BelongsTo(DeployableObject obj, Controllable controllable)
        {
            if (controllable == null)
            {
                return false;
            }
            NetUser netUser = controllable.netUser;
            if (netUser == null)
            {
                return false;
            }
            UserData bySteamID = Users.GetBySteamID(obj.ownerID);
            if (netUser.admin && Users.Details(netUser.userID))
            {
                smethod_0(netUser, obj.ownerID, Helper.NiceName(obj.name));
            }
            if (netUser.admin && (netUser.userID != obj.ownerID))
            {
                return (obj.GetComponent<BasicDoor>() != null);
            }
            return (((bySteamID != null) && Users.SharedList(bySteamID.SteamID).Contains(netUser.userID)) || Magma.Hooks.CheckOwner(obj, controllable));
        }

        public static void FallDamage_FallImpact(FallDamage hook, float fallspeed)
        {
            Character idMain = hook.idMain;
            UserData bySteamID = Users.GetBySteamID(idMain.playerClient.userID);
            if ((bySteamID != null) && Truth.CheckFallhack)
            {
                bySteamID.FallCheck = FallCheckState.damaged;
            }
            if (((bySteamID == null) || !bySteamID.HasFlag(UserFlags.godmode)) && ((idMain != null) && !idMain.playerClient.netUser.admin))
            {
                float num = (fallspeed - falldamage.min_vel) / (falldamage.max_vel - falldamage.min_vel);
                bool flag = num > 0.25f;
                bool flag2 = ((num > 0.35f) || (UnityEngine.Random.Range(0, 3) == 0)) || (hook.healthFraction < 0.5f);
                if (flag)
                {
                    idMain.GetComponent<HumanBodyTakeDamage>().AddBleedingLevel(3f + ((num - 0.25f) * 10f));
                }
                if (flag2)
                {
                    hook.AddLegInjury(1f);
                }
                TakeDamage.HurtSelf(idMain.idMain, 10f + (num * idMain.maxHealth), null);
            }
        }

        public static void Global_Say(ref ConsoleSystem.Arg arg)
        {
            string input = arg.GetString(0, string.Empty).Trim();
            if (input != string.Empty)
            {
                string chatTextColor = "";
                if (Core.ChatConsoleColor != "#FFFFFF")
                {
                    chatTextColor = Helper.GetChatTextColor(Core.ChatConsoleColor);
                }
                input = Regex.Replace(input, @"(\[COLOR\s*\S*])|(\[/COLOR\s*\S*])", "", RegexOptions.IgnoreCase).Trim();
                ConsoleNetworker.Broadcast("chat.add " + Helper.QuoteSafe(Core.ChatConsoleName) + " " + Helper.QuoteSafe(chatTextColor + input));
            }
        }

        public static void HandGrenadeDataBlock_DoAction1(HandGrenadeDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            IHandGrenadeItem item;
            if (rep.Item<IHandGrenadeItem>(out item) && item.ValidatePrimaryMessageTime(info.timestamp))
            {
                Character idMain = item.inventory.idMain as Character;
                if (((idMain != null) && (idMain.netUser != null)) && idMain.netUser.connected)
                {
                    Vector3 vector = stream.ReadVector3();
                    Vector3 vector2 = stream.ReadVector3();
                    if (((vector.Invalid() || vector2.Invalid()) || ((vector.x > 8000f) || (vector.x < -8000f))) || (((vector.y > 2000f) || (vector.y < -2000f)) || ((vector.z > 8000f) || (vector.z < -8000f))))
                    {
                        Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.NetExploit.Grenade", null, "", 0, new DateTime());
                        Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", item.datablock.name);
                        Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", idMain.netUser.displayName);
                        Truth.Punish(idMain.netUser, Users.GetBySteamID(idMain.netUser.userID), Truth.HackMethod.NetExploit, true);
                    }
                    else
                    {
                        rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                        GameObject obj2 = hook.ThrowItem(rep, vector, vector2);
                        if (obj2 != null)
                        {
                            obj2.rigidbody.AddTorque((Vector3) (new Vector3(UnityEngine.Random.Range((float) -1f, (float) 1f), UnityEngine.Random.Range((float) -1f, (float) 1f), UnityEngine.Random.Range((float) -1f, (float) 1f)) * 10f));
                        }
                        int count = 1;
                        if (item.Consume(ref count))
                        {
                            item.inventory.RemoveItem(item.slot);
                        }
                    }
                }
            }
        }

        protected static bool HostileAI_CanAttack(TakeDamage target)
        {
            if (target == null)
            {
                return false;
            }
            Character component = target.GetComponent<Character>();
            if ((component != null) && (component.playerClient != null))
            {
                if (component.netUser.admin)
                {
                    return false;
                }
                UserData bySteamID = Users.GetBySteamID(component.playerClient.userID);
                if (((bySteamID != null) && (bySteamID.Zone != null)) && bySteamID.Zone.Safe)
                {
                    return false;
                }
            }
            return true;
        }

        public static void HostileWildlifeAI_OnHurt(HostileWildlifeAI AI, DamageEvent damage)
        {
            if (!AI.HasTarget() && (damage.attacker.character != null))
            {
                TakeDamage component = damage.attacker.character.gameObject.GetComponent<TakeDamage>();
                if (HostileAI_CanAttack(component))
                {
                    AI.SetAttackTarget(component);
                    AI.ExitCurrentState();
                    AI.EnterState_Chase();
                }
            }
        }

        public static void HostileWildlifeAI_Scent(HostileWildlifeAI AI, TakeDamage damage)
        {
            if ((!AI.IsScentBlind() && (AI._state != 2)) && ((AI._state != 7) && !AI.HasTarget()))
            {
                AI.ExitCurrentState();
                if (HostileAI_CanAttack(damage))
                {
                    AI.SetAttackTarget(damage);
                    AI.EnterState_Chase();
                }
            }
        }

        public static void HostileWildlifeAI_StateSim_Attack(HostileWildlifeAI AI, ulong millis)
        {
            if (AI.HasTarget() && HostileAI_CanAttack(AI._targetTD))
            {
                if (AI._targetTD.transform != null)
                {
                    Vector3 position = AI._targetTD.transform.position;
                    Vector3 vector = AI._targetTD.transform.position - AI.transform.position;
                    vector.y = 0f;
                    AI._wildMove.SetLookDirection(vector.normalized);
                    if (AI.nextAttackClock.IntegrateTime_Reached(millis))
                    {
                        AI.nextAttackClock.ResetDurationSeconds((double) AI.attackRate);
                        AI.attackStrikeClock.ResetDurationSeconds(0.25);
                        AI.NetworkSound(BasicWildLifeAI.AISound.Attack);
                        AI.networkView.RPC("CL_Attack", uLink.RPCMode.OthersExceptOwner, new object[0]);
                    }
                    if (AI.attackStrikeClock.IntegrateTime_Reached(millis))
                    {
                        DamageEvent event2;
                        float melee = UnityEngine.Random.Range(AI.attackDamageMin, AI.attackDamageMax);
                        TakeDamage.Hurt(AI.GetComponent<IDMain>(), AI._targetTD.idMain, new DamageTypeList(0f, 0f, melee, 0f, 0f, 0f), out event2, null);
                        AI.attackStrikeClock.ResetDurationSeconds((double) (AI.attackRate * 2f));
                    }
                    if (AI.TargetRange() > AI.attackRangeMax)
                    {
                        AI.ExitCurrentState();
                        AI.EnterState_Chase();
                    }
                }
            }
            else
            {
                AI.LoseTarget();
            }
        }

        public static void HumanController_GetClientMove(HumanController controller, Vector3 origin, int encoded, ushort stateFlags, uLink.NetworkMessageInfo info)
        {
            if (((info != null) && (info.sender != uLink.NetworkPlayer.unassigned)) && (info.sender == controller.networkView.owner))
            {
                try
                {
                    if ((Truth.GetClientVerify(controller, ref origin, encoded, stateFlags, info) && (((controller != null) && (controller.netUser != null)) && !origin.Invalid())) && (((((origin.x <= 10000f) && (origin.x >= -10000f)) && ((origin.y <= 10000f) && (origin.y >= -10000f))) && ((origin.z <= 10000f) && (origin.z >= -10000f))) || controller.netUser.admin))
                    {
                        if (((NetClockTester.TestValidity(ref controller.clockTest, ref info, NetCull.sendInterval, NetClockTester.ValidityFlags.AheadOfServerTime | NetClockTester.ValidityFlags.OverTimed | NetClockTester.ValidityFlags.TooFrequent | NetClockTester.ValidityFlags.Valid) & ~NetClockTester.ValidityFlags.Valid) == 0) && (controller.clockTest.Results.Valid >= 0x3e8))
                        {
                            controller.clockTest.Results = new NetClockTester.Validity();
                        }
                        if ((NetCull.isServerRunning && (info.timestamp > controller.serverLastTimestamp)) && !controller.dead)
                        {
                            Character idMain = controller.idMain;
                            if ((idMain != null) && (idMain.netUser != null))
                            {
                                double num = info.timestamp - controller.serverLastTimestamp;
                                if (num >= (NetCull.sendInterval * 0.89))
                                {
                                    controller.serverLastTimestamp = info.timestamp;
                                    if (controller.clientMoveDropped)
                                    {
                                        controller.clientMoveDropped = false;
                                    }
                                    uLink.RPCMode othersExceptOwner = uLink.RPCMode.OthersExceptOwner;
                                    float num2 = (float) (NetCull.time - info.timestamp);
                                    Angle2 ang = new Angle2 {
                                        encoded = encoded
                                    };
                                    stateFlags = (ushort) (stateFlags & -24577);
                                    if (!Core.PlayersFreezed && !Users.HasFlag(idMain.netUser.userID, UserFlags.freezed))
                                    {
                                        TruthDetector.ActionTaken taken = Truth.NoteMoved(idMain.netUser.truthDetector, ref origin, ang, info.timestamp);
                                        if (taken != TruthDetector.ActionTaken.Kicked)
                                        {
                                            Zones.OnPlayerMove(idMain.netUser, ref origin, ref taken);
                                            Users.GetBySteamID(idMain.netUser.userID);
                                            if (taken == TruthDetector.ActionTaken.Moved)
                                            {
                                                othersExceptOwner = uLink.RPCMode.Others;
                                            }
                                            idMain.origin = origin;
                                            idMain.eyesAngles = ang;
                                            idMain.stateFlags.flags = stateFlags;
                                            if (controller.networkView.viewID != uLink.NetworkViewID.unassigned)
                                            {
                                                object[] args = new object[] { origin, ang.encoded, stateFlags, num2 };
                                                controller.networkView.RPC("ReadClientMove", othersExceptOwner, args);
                                            }
                                            controller.ServerFrame();
                                            if (Truth.CheckShotEyes)
                                            {
                                                Truth.Test_WeaponShotEyes(idMain, ang);
                                            }
                                        }
                                    }
                                    else if (((idMain.transform.position.x != origin.x) || (idMain.transform.position.z != origin.z)) || (idMain.eyesAngles.encoded != encoded))
                                    {
                                        Broadcast.Message(idMain.netUser, Config.GetMessage("Player.Paralyzed", idMain.netUser, null), null, 0f);
                                        object[] objArray2 = new object[] { idMain.transform.position, idMain.eyesAngles.encoded, stateFlags, num2 };
                                        controller.networkView.RPC("ReadClientMove", uLink.RPCMode.Others, objArray2);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public static void HumanController_OnKilled(HumanController hook, DamageEvent damage)
        {
            Vis.Mask traitMask = hook.traitMask;
            traitMask[Vis.Life.Alive] = false;
            traitMask[Vis.Life.Dead] = true;
            hook.traitMask = traitMask;
            try
            {
                Inventory inventory;
                UserData bySteamID = Users.GetBySteamID(hook.netUser.userID);
                if (bySteamID != null)
                {
                    if (Truth.RustProtect)
                    {
                        bySteamID.ProtectTime = 0f;
                        bySteamID.ProtectTick = 0;
                    }
                    if (Truth.CheckFallhack)
                    {
                        Truth.FallHeight[hook.netUser] = 0.0;
                        bySteamID.FallCheck = FallCheckState.none;
                    }
                }
                if (hook.deathTransfer != null)
                {
                    hook.deathTransfer.NetworkKill(ref damage);
                }
                if (Magma.Hooks.PlayerKilled(ref damage) && DropHelper.DropInventoryContents(hook.inventory, out inventory))
                {
                    LootableObject component = inventory.GetComponent<LootableObject>();
                    if (component != null)
                    {
                        component.lifeTime = Core.ObjectLootableLifetime;
                    }
                    if ((inventory != null) && (player.backpackLockTime > 0f))
                    {
                        TimedLockable lockable = inventory.gameObject.AddComponent<TimedLockable>();
                        lockable.SetOwner(hook.netUser.userID);
                        lockable.LockFor(player.backpackLockTime);
                    }
                }
            }
            catch
            {
            }
            hook.GetComponent<AvatarSaveRestore>().ClearAvatar();
            IDLocalCharacter.DestroyCharacter(hook.idMain);
        }

        public static void Inventory_ItemAdded(Inventory hook, int slot, IInventoryItem item)
        {
            object[] args = Main.Array(3);
            args[0] = hook;
            args[1] = slot;
            args[2] = item;
            Main.Call("OnItemAdded", args);
            FireBarrel local = hook.GetLocal<FireBarrel>();
            if (local != null)
            {
                local.InvItemAdded();
            }
        }

        public static void Inventory_ItemRemoved(Inventory hook, int slot, IInventoryItem item)
        {
            object[] args = Main.Array(3);
            args[0] = hook;
            args[1] = slot;
            args[2] = item;
            Main.Call("OnItemRemoved", args);
            FireBarrel local = hook.GetLocal<FireBarrel>();
            if (local != null)
            {
                local.InvItemRemoved();
            }
        }

        public static bool Inventory_SlotOperation(Inventory fromInventory, int fromSlot, Inventory moveInventory, int moveSlot, Inventory.SlotOperationsInfo info)
        {
            IInventoryItem item;
            PlayerClient client;
            if (!fromInventory.GetItem(fromSlot, out item))
            {
                return false;
            }
            if (!PlayerClient.Find(info.Looter, out client) && (client.controllable != null))
            {
                return false;
            }
            Inventory component = client.controllable.GetComponent<Inventory>();
            float num = TransformHelpers.Dist2D(fromInventory.transform.position, component.transform.position);
            float num2 = TransformHelpers.Dist2D(moveInventory.transform.position, component.transform.position);
            if (num > 4f)
            {
                Helper.LogError(string.Concat(new object[] { "Slot Operation [", client.netUser.displayName, ":", client.netUser.userID, "]: Try to move ", item.datablock.name, " from unreachable container." }), true);
                return false;
            }
            if (num2 > 4f)
            {
                Helper.LogError(string.Concat(new object[] { "Slot Operation [", client.netUser.displayName, ":", client.netUser.userID, "]: Try to move ", item.datablock.name, " into unreachable container." }), true);
                return false;
            }
            return true;
        }

        public static void InventoryHolder_TryGiveDefaultItems(InventoryHolder holder)
        {
            Loadout loadout = holder.GetTrait<CharacterLoadoutTrait>().loadout;
            if (loadout != null)
            {
                int rank = Users.GetRank(holder.netUser.userID);
                if (Core.Loadout.Count == 0)
                {
                    loadout.ApplyTo(holder.inventory);
                }
                else
                {
                    PlayerInventory inventory = (PlayerInventory) holder.inventory;
                    foreach (LoadoutEntry entry in Core.Loadout)
                    {
                        if ((entry.Ranks.Length == 0) || entry.Ranks.Contains<int>(rank))
                        {
                            if (inventory.noOccupiedSlots)
                            {
                                foreach (LoadoutItem item in entry.LoadoutItems)
                                {
                                    Helper.GiveItem(inventory, item.ItemBlock, item.SlotReference, item.Quantity, item.ModSlots);
                                }
                            }
                            foreach (LoadoutItem item2 in entry.Requirements)
                            {
                                if (object.ReferenceEquals(inventory.FindItem(item2.ItemBlock), null))
                                {
                                    Helper.GiveItem(inventory, item2.ItemBlock, item2.SlotReference, item2.Quantity, item2.ModSlots);
                                }
                            }
                            foreach (BlueprintDataBlock block in entry.Blueprints)
                            {
                                inventory.BindBlueprint(block);
                            }
                        }
                    }
                }
            }
        }

        public static bool ItemPickup_PlayerUse(ItemPickup hook, Controllable controllable)
        {
            IInventoryItem item;
            RaycastHit hit;
            Inventory local = hook.GetLocal<Inventory>();
            Inventory inventory2 = controllable.GetLocal<Inventory>();
            if (inventory2 == null)
            {
                return false;
            }
            Vector3 position = hook.transform.position;
            position.y += 0.1f;
            Vector3 origin = controllable.character.eyesRay.origin;
            foreach (Collider collider in Physics.OverlapSphere(origin, 0.25f))
            {
                IDBase component = collider.gameObject.GetComponent<IDBase>();
                if ((component != null) && (component.idMain is StructureMaster))
                {
                    return false;
                }
            }
            if (Physics.Linecast(origin, position, out hit, -1))
            {
                IDBase base3 = hit.collider.gameObject.GetComponent<IDBase>();
                if ((base3 != null) && (base3.idMain is StructureMaster))
                {
                    return false;
                }
            }
            if ((local == null) || object.ReferenceEquals(item = local.firstItem, null))
            {
                hook.RemoveThis();
                return false;
            }
            switch (inventory2.AddExistingItem(item, false))
            {
                case Inventory.AddExistingItemResult.CompletlyStacked:
                    local.RemoveItem(item);
                    break;

                case Inventory.AddExistingItemResult.Moved:
                    break;

                case Inventory.AddExistingItemResult.PartiallyStacked:
                    hook.UpdateItemInfo(item);
                    return true;

                case Inventory.AddExistingItemResult.Failed:
                    return false;

                case Inventory.AddExistingItemResult.BadItemArgument:
                    hook.RemoveThis();
                    return false;

                default:
                    throw new NotImplementedException();
            }
            hook.RemoveThis();
            return true;
        }

        public static bool LootableObject_ContextRespond_OpenLoot(LootableObject loot, Controllable controllable, ulong timestamp)
        {
            DeployableObject component = loot.GetComponent<DeployableObject>();
            if ((controllable == null) || (component == null))
            {
                return true;
            }
            if (controllable.netUser.admin && Users.Details(controllable.netUser.userID))
            {
                smethod_0(controllable.netUser, component.ownerID, Helper.NiceName(component.name));
            }
            if (controllable.netUser.admin)
            {
                return true;
            }
            if (controllable.character.stateFlags.airborne)
            {
                return false;
            }
            Vector3 origin = Helper.GetLookRay(controllable).origin;
            Vector3 position = component.transform.position;
            position.y += 0.1f;
            if (TransformHelpers.Dist2D(origin, position) > 5f)
            {
                return false;
            }
            Vector3 vector3 = position - origin;
            Ray ray = new Ray(origin, vector3.normalized);
            foreach (RaycastHit hit in Physics.RaycastAll(ray, Vector3.Distance(origin, position), -1))
            {
                IDBase base2;
                if ((hit.collider != null) && ((base2 = IDBase.GetMain(hit.collider)) != null))
                {
                    if (base2.idMain.GetComponent<StructureMaster>() != null)
                    {
                        return false;
                    }
                    if (base2.idMain.GetComponent<BasicDoor>() != null)
                    {
                        return false;
                    }
                }
            }
            foreach (Collider collider in Physics.OverlapSphere(origin, 0.2f))
            {
                IDBase base3 = collider.gameObject.GetComponent<IDBase>();
                if ((base3 != null) && (base3.idMain is StructureMaster))
                {
                    return false;
                }
            }
            string str = Helper.NiceName(component.name);
            UserData bySteamID = Users.GetBySteamID(component.ownerID);
            if (bySteamID == null)
            {
                return true;
            }
            bool flag = true;
            if (((Core.OwnershipProtectContainer.Length > 0) && Core.OwnershipProtectContainer.Contains<string>(str, StringComparer.OrdinalIgnoreCase)) && (component.ownerID != controllable.netUser.userID))
            {
                if (Core.OwnershipProtectSharedUsers && Users.SharedList(bySteamID.SteamID).Contains(controllable.netUser.userID))
                {
                    flag = true;
                }
                else if (Core.OwnershipProtectPremiumUser && bySteamID.HasFlag(UserFlags.premium))
                {
                    flag = false;
                }
                else if (Core.OwnershipProtectOfflineUser && bySteamID.HasFlag(UserFlags.online))
                {
                    flag = false;
                }
                else if (bySteamID.HasFlag(UserFlags.safeboxes))
                {
                    flag = false;
                }
            }
            if (!flag)
            {
                Broadcast.Notice(controllable.netPlayer, "☢", Config.GetMessage("PlayerOwnership.Container.Protected", null, null).Replace("%OWNERNAME%", bySteamID.Username), 5f);
                return flag;
            }
            Helper.Log(string.Concat(new object[] { "\"", controllable.netUser.displayName, "\" open \"", str, "\" ", component.transform.position, " owned by \"", bySteamID.Username, "\"" }), false);
            return flag;
        }

        public static void MeleeWeaponDataBlock_DoAction1(MeleeWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            GameObject gameObject = null;
            NetEntityID unassigned;
            IMeleeWeaponItem item;
            if (stream.ReadBoolean())
            {
                unassigned = stream.Read<NetEntityID>(new object[0]);
            }
            else
            {
                unassigned = NetEntityID.unassigned;
            }
            if (!unassigned.isUnassigned)
            {
                gameObject = unassigned.gameObject;
            }
            if (gameObject == null)
            {
                unassigned = NetEntityID.unassigned;
            }
            Vector3 vector = stream.ReadVector3();
            bool flag = stream.ReadBoolean();
            if (rep.Item<IMeleeWeaponItem>(out item))
            {
                Character idMain = item.inventory.idMain as Character;
                TakeDamage local = item.inventory.GetLocal<TakeDamage>();
                if (((local != null) && !local.dead) && item.ValidatePrimaryMessageTime(info.timestamp))
                {
                    IDBase victim = (gameObject != null) ? IDBase.Get(gameObject) : null;
                    TakeDamage damage2 = (victim != null) ? victim.GetLocal<TakeDamage>() : null;
                    NetUser user = ((idMain == null) || (idMain.controllable == null)) ? null : idMain.netUser;
                    if (((user != null) && flag) && vector.Equals(Vector3.zero))
                    {
                        Truth.PunishDetails = Config.GetMessageTruth("Truth.Punish.Reason.ObjectHack.GatherStaticTree", user, "", 0, new DateTime());
                        Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.NAME%", user.displayName);
                        Truth.PunishDetails = Truth.PunishDetails.Replace("%KILLER.POS%", idMain.transform.position.AsString());
                        Truth.PunishDetails = Truth.PunishDetails.Replace("%WEAPON%", item.datablock.name);
                        Truth.PunishDetails = Truth.PunishDetails.Replace("%OBJECT.POS%", vector.AsString());
                        Truth.Punish(user, Users.GetBySteamID(user.userID), Truth.HackMethod.OtherHack, false);
                    }
                    else
                    {
                        if ((gameObject == null) || (Vector3.Distance(local.transform.position, gameObject.transform.position) < 6f))
                        {
                            Metabolism component = item.inventory.GetComponent<Metabolism>();
                            if (component != null)
                            {
                                component.SubtractCalories(UnityEngine.Random.Range((float) (hook.caloriesPerSwing * 0.8f), (float) (hook.caloriesPerSwing * 1.2f)));
                            }
                            rep.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                            ResourceTarget rt = ((victim != null) || (gameObject != null)) ? ((victim != null) ? victim.gameObject : gameObject).GetComponent<ResourceTarget>() : null;
                            if (flag || ((rt != null) && ((damage2 == null) || damage2.dead)))
                            {
                                ResourceTarget.ResourceTargetType staticTree = ResourceTarget.ResourceTargetType.StaticTree;
                                if (!flag && (rt != null))
                                {
                                    staticTree = rt.type;
                                }
                                float f = hook.efficiencies[(int) staticTree];
                                if (flag)
                                {
                                    hook.resourceGatherLevel += f;
                                    if (hook.resourceGatherLevel >= 1f)
                                    {
                                        if (!TreeGatherPoint.ContainsKey(user))
                                        {
                                            UserGatherPoint point3 = new UserGatherPoint {
                                                position = Vector3.zero,
                                                quantity = 0
                                            };
                                            TreeGatherPoint.Add(user, point3);
                                        }
                                        UserGatherPoint point = TreeGatherPoint[user];
                                        if (TransformHelpers.Dist2D(vector, point.position) > 2f)
                                        {
                                            UserGatherPoint point2 = new UserGatherPoint {
                                                position = vector,
                                                quantity = 1
                                            };
                                            TreeGatherPoint[user] = point2;
                                        }
                                        else
                                        {
                                            if (point.quantity >= 15)
                                            {
                                                Notice.Popup(user.networkPlayer, "", "There's no wood left here", 2f);
                                                hook.resourceGatherLevel = 0f;
                                                return;
                                            }
                                            point.quantity++;
                                            TreeGatherPoint[user] = point;
                                        }
                                        int num2 = 0;
                                        string name = "Wood";
                                        int amount = Mathf.FloorToInt(hook.resourceGatherLevel);
                                        ItemDataBlock byName = DatablockDictionary.GetByName(name);
                                        Magma.Hooks.PlayerGatherWood(item, rt, ref byName, ref amount, ref name);
                                        if (byName == null)
                                        {
                                            num2 = 0;
                                        }
                                        else
                                        {
                                            int num4 = item.inventory.AddItemAmount(byName, amount);
                                            num2 = amount - num4;
                                        }
                                        if (num2 > 0)
                                        {
                                            hook.resourceGatherLevel -= num2;
                                            Notice.Inventory(info.sender, num2.ToString() + " x " + name);
                                        }
                                    }
                                }
                                else if ((rt != null) && !float.IsNaN(f))
                                {
                                    rt.DoGather(item.inventory, f);
                                }
                            }
                            else if (victim != null)
                            {
                                foreach (Collider collider in Physics.OverlapSphere(idMain.eyesRay.origin, 0.2f))
                                {
                                    IDBase base3 = collider.gameObject.GetComponent<IDBase>();
                                    if ((base3 != null) && (base3.idMain is StructureMaster))
                                    {
                                        return;
                                    }
                                }
                                if (victim.idMain.gameObject.GetComponent<PlayerClient>() != null)
                                {
                                    Ray lookRay = Helper.GetLookRay(idMain);
                                    Vector3 position = victim.transform.position;
                                    position.y += 0.1f;
                                    if (Physics.RaycastAll(lookRay, Vector3.Distance(lookRay.origin, position), -1).Length > 1)
                                    {
                                        return;
                                    }
                                }
                                TakeDamage.Hurt(item.inventory, victim, new DamageTypeList(0f, 0f, hook.GetDamage(), 0f, 0f, 0f), new WeaponImpact(hook, item, rep));
                            }
                        }
                        if ((gameObject != null) && ((user == null) || !user.admin))
                        {
                            item.TryConditionLoss(0.25f, 0.025f);
                        }
                    }
                }
            }
        }

        public static void Metabolism_DoNetworkUpdate(Metabolism hook)
        {
            if (hook.IsDirty())
            {
                hook.networkView.RPC("RecieveNetwork", hook.networkView.owner, new object[] { hook.caloricLevel, hook.waterLevelLitre, hook.radiationLevel, hook.antiRads, hook.coreTemperature, hook.poisonLevel });
            }
            hook.MakeClean();
        }

        public static void NetUser_InitializeClientToServer(NetUser netUser)
        {
            UserData bySteamID = Users.GetBySteamID(netUser.userID);
            Vector3 zero = Vector3.zero;
            if (!Users.Avatar.ContainsKey(netUser.userID))
            {
                Users.Avatar.Add(netUser.userID, netUser.LoadAvatar());
            }
            else if (Core.AvatarAutoSaveInterval > 0)
            {
                Users.Avatar[netUser.userID] = netUser.LoadAvatar();
            }
            else if (server.log > 2)
            {
                ConsoleSystem.Print("Avatar [" + netUser.userID + "] Loaded from Cache.", false);
            }
            netUser.avatar = Users.Avatar[netUser.userID];
            ServerManagement.Get().UpdateConnectingUserAvatar(netUser, ref netUser.avatar);
            if (ServerManagement.Get().SpawnPlayer(netUser.playerClient, false, netUser.avatar) != null)
            {
                netUser.did_join = true;
            }
            if (netUser.avatar.HasPos && (netUser.truthDetector != null))
            {
                if (server.log > 2)
                {
                    ConsoleSystem.Print("Truth [" + netUser.userID + "] set first player position.", false);
                }
                netUser.truthDetector.prevSnap.pos = new Vector3(netUser.avatar.Pos.X, netUser.avatar.Pos.Y + 0.25f, netUser.avatar.Pos.Z);
                netUser.truthDetector.prevSnap.time = Time.time;
                netUser.truthDetector.Record();
            }
            if (bySteamID.HasFlag(UserFlags.admin))
            {
                netUser.admin = true;
                if (Core.AnnounceAdminConnect)
                {
                    Broadcast.MessageAll(Config.GetMessage("Player.Join", netUser, null), netUser);
                }
                if (Core.NoticeConnectedAdmin)
                {
                    foreach (string str in Config.GetMessages("Notice.Connected.Admin.Message", netUser))
                    {
                        Broadcast.Message(netUser, str, null, 0f);
                    }
                }
                if (Users.HasFlag(netUser.userID, UserFlags.invis))
                {
                    Broadcast.Message(netUser, "You now is invisibility.", null, 0f);
                }
                if (Core.AdminGodmode)
                {
                    Users.SetFlags(netUser.userID, UserFlags.godmode, true);
                    Broadcast.Message(netUser, "You now with god mode.", null, 0f);
                }
            }
            else if (bySteamID.HasFlag(UserFlags.invis))
            {
                if (Core.AnnounceInvisConnect)
                {
                    Broadcast.MessageAll(Config.GetMessage("Player.Join", netUser, null), netUser);
                }
                if (Users.HasFlag(netUser.userID, UserFlags.invis))
                {
                    Broadcast.Message(netUser, "You now is invisibility.", null, 0f);
                }
            }
            else
            {
                if (Core.AnnouncePlayerJoin)
                {
                    Broadcast.MessageAll(Config.GetMessage("Player.Join", netUser, null), netUser);
                }
                if (Core.NoticeConnectedPlayer)
                {
                    foreach (string str2 in Config.GetMessages("Notice.Connected.Player.Message", netUser))
                    {
                        Broadcast.Message(netUser, str2, null, 0f);
                    }
                }
            }
            if (bySteamID.PremiumDate.Millisecond != 0)
            {
                Commands.Premium(netUser, bySteamID, "premium", new string[0]);
            }
            if (Economy.Initialized && Economy.Enabled)
            {
                Economy.Balance(netUser, bySteamID, "balance", null);
            }
            if (netUser.playerClient.hasLastKnownPosition)
            {
                bySteamID.Zone = Zones.Get(netUser.playerClient.lastKnownPosition);
            }
            foreach (string str3 in Core.Kits.Keys)
            {
                System.Collections.Generic.List<string> list = (System.Collections.Generic.List<string>) Core.Kits[str3];
                if (predicate_2 == null)
                {
                    predicate_2 = new Predicate<string>(RustHook.smethod_4);
                }
                string str4 = list.Find(predicate_2);
                if (!string.IsNullOrEmpty(str4) && str4.Replace(" ", "").ToLower().Contains("=true"))
                {
                    bool flag = true;
                    if (predicate_3 == null)
                    {
                        predicate_3 = new Predicate<string>(RustHook.smethod_5);
                    }
                    string str5 = list.Find(predicate_3);
                    if (!string.IsNullOrEmpty(str5))
                    {
                        flag = str5.Replace(" ", "").Split(new char[] { ',' }).Contains<string>(bySteamID.Rank.ToString());
                    }
                    if (flag)
                    {
                        Commands.Kit(netUser, bySteamID, "kit", new string[] { str3 });
                    }
                }
            }
            if ((bySteamID.Clan != null) && !string.IsNullOrEmpty(bySteamID.Clan.MOTD))
            {
                Broadcast.MessageClan(netUser, bySteamID.Clan.MOTD);
            }
            if (Core.ChatQuery.ContainsKey(netUser.userID))
            {
                Broadcast.Message(netUser, Core.ChatQuery[netUser.userID].Query, null, 0f);
            }
        }

        public static InventoryItem.MergeResult ResearchToolItemT_TryCombine(object hook, IInventoryItem otherItem)
        {
            BlueprintDataBlock block;
            IInventoryItem item = hook as IInventoryItem;
            if (item == null)
            {
                return InventoryItem.MergeResult.Failed;
            }
            PlayerInventory inventory = item.inventory as PlayerInventory;
            if ((inventory == null) || (otherItem.inventory != inventory))
            {
                return InventoryItem.MergeResult.Failed;
            }
            object[] args = Main.Array(2);
            args[0] = item;
            args[1] = otherItem;
            object obj2 = Main.Call("OnResearchItem", args);
            if (obj2 is InventoryItem.MergeResult)
            {
                return (InventoryItem.MergeResult) obj2;
            }
            ItemDataBlock datablock = otherItem.datablock;
            if ((datablock == null) || !datablock.isResearchable)
            {
                return InventoryItem.MergeResult.Failed;
            }
            if (!inventory.AtWorkBench())
            {
                return InventoryItem.MergeResult.Failed;
            }
            if (!BlueprintDataBlock.FindBlueprintForItem<BlueprintDataBlock>(otherItem.datablock, out block))
            {
                return InventoryItem.MergeResult.Failed;
            }
            if (inventory.KnowsBP(block))
            {
                return InventoryItem.MergeResult.Failed;
            }
            inventory.BindBlueprint(block);
            Notice.Popup(inventory.networkView.owner, "", "You can now craft " + otherItem.datablock.name, 4f);
            int count = 1;
            if (item.Consume(ref count))
            {
                inventory.RemoveItem(item.slot);
            }
            return InventoryItem.MergeResult.Combined;
        }

        public static bool Resource_DoGather(ResourceTarget obj, Inventory reciever, float efficiency)
        {
            if (obj.resourcesAvailable.Count == 0)
            {
                Helper.LogError("OBJECT[" + obj + "]: Not have availabled resources, this require to remove?", true);
                Helper.LogError("OBJECT[" + obj + "]: Tell to developer of Rust Extended about this message.", true);
                return false;
            }
            float resourcesGatherMultiplierFlay = 1f;
            switch (obj.type)
            {
                case ResourceTarget.ResourceTargetType.Animal:
                    resourcesGatherMultiplierFlay = Core.ResourcesGatherMultiplierFlay;
                    break;

                case ResourceTarget.ResourceTargetType.WoodPile:
                    resourcesGatherMultiplierFlay = Core.ResourcesGatherMultiplierWood;
                    break;

                case ResourceTarget.ResourceTargetType.Rock1:
                    resourcesGatherMultiplierFlay = Core.ResourcesGatherMultiplierRock;
                    break;

                case ResourceTarget.ResourceTargetType.Rock2:
                    resourcesGatherMultiplierFlay = Core.ResourcesGatherMultiplierRock;
                    break;

                case ResourceTarget.ResourceTargetType.Rock3:
                    resourcesGatherMultiplierFlay = Core.ResourcesGatherMultiplierRock;
                    break;
            }
            if (resourcesGatherMultiplierFlay == 0f)
            {
                resourcesGatherMultiplierFlay = 0.01f;
            }
            ResourceGivePair rg = obj.resourcesAvailable[UnityEngine.Random.Range(0, obj.resourcesAvailable.Count)];
            obj.gatherProgress += (efficiency * obj.gatherEfficiencyMultiplier) * resourcesGatherMultiplierFlay;
            int amount = (int) Mathf.Abs(obj.gatherProgress);
            Magma.Hooks.PlayerGather(reciever, obj, rg, ref amount);
            obj.gatherProgress = Mathf.Clamp(obj.gatherProgress, 0f, (float) amount);
            amount = Mathf.Min(amount, rg.AmountLeft());
            if (amount > 0)
            {
                UserData bySteamID = null;
                NetUser user = NetUser.Find(reciever.networkView.owner);
                int num3 = reciever.AddItemAmount(rg.ResourceItemDataBlock, amount);
                if (num3 < amount)
                {
                    if (user != null)
                    {
                        bySteamID = Users.GetBySteamID(user.userID);
                    }
                    int num4 = 0;

                    if ((bySteamID != null) && (bySteamID.Clan != null))
                    {
                        if (obj.type == ResourceTarget.ResourceTargetType.WoodPile)
                        {
                            num4 = (int)((amount * bySteamID.Clan.Level.BonusGatheringWood) / (100L));
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Rock1)
                        {
                            num4 = (int)((amount * bySteamID.Clan.Level.BonusGatheringRock) / (100L));
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Rock2)
                        {
                            num4 = (int)((amount * bySteamID.Clan.Level.BonusGatheringRock) / (100L));
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Rock3)
                        {
                            num4 = (int)((amount * bySteamID.Clan.Level.BonusGatheringRock) / (100L));
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Animal)
                        {
                            num4 = (int)((amount * bySteamID.Clan.Level.BonusGatheringAnimal) / (100L));
                        }
                        /*
                        ClanData Clan = new UserData();
                        
                        if (obj.type == ResourceTarget.ResourceTargetType.WoodPile)
                        {
                            num4 = (int)((long)amount * (long)((ulong)Clan.Level.BonusGatheringWood) / 100L);
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Rock1)
                        {
                            num4 = (int)((long)amount * (long)((ulong)Clan.Level.BonusGatheringRock) / 100L);
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Rock2)
                        {
                            num4 = (int)((long)amount * (long)((ulong)Clan.Level.BonusGatheringRock) / 100L);
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Rock3)
                        {
                            num4 = (int)((long)amount * (long)((ulong)Clan.Level.BonusGatheringRock) / 100L);
                        }
                        else if (obj.type == ResourceTarget.ResourceTargetType.Animal)
                        {
                            num4 = (int)((long)amount * (long)((ulong)Clan.Level.BonusGatheringAnimal) / 100L);
                        }*/
                    }
                    if (num4 > 0)
                    {
                        num4 -= reciever.AddItemAmount(rg.ResourceItemDataBlock, num4);
                    }
                    int num5 = amount - num3;
                    rg.Subtract(num5);
                    obj.gatherProgress -= num5;
                    Notice.Inventory(reciever.networkView.owner, ((num5 + num4)).ToString() + " x " + rg.ResourceItemName);
                    obj.SendMessage("ResourcesGathered", SendMessageOptions.DontRequireReceiver);
                    if ((bySteamID != null) && (bySteamID.Clan != null))
                    {
                        float num6 = 0f;
                        if (rg.ResourceItemName.Equals("Raw Chicken Breast", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5 * 2;
                        }
                        else if (rg.ResourceItemName.Equals("Animal Fat", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5;
                        }
                        else if (rg.ResourceItemName.Equals("Blood", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5 * 2;
                        }
                        else if (rg.ResourceItemName.Equals("Cloth", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5;
                        }
                        else if (rg.ResourceItemName.Equals("Leather", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5 * 2;
                        }
                        else if (rg.ResourceItemName.Equals("Wood", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5 / 2;
                        }
                        else if (rg.ResourceItemName.Equals("Stones", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5 / 2;
                        }
                        else if (rg.ResourceItemName.Equals("Metal Ore", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5 * 2;
                        }
                        else if (rg.ResourceItemName.Equals("Sulfur Ore", StringComparison.OrdinalIgnoreCase))
                        {
                            num6 = num5 * 2;
                        }
                        if (num6 < 0f)
                        {
                            num6 = 0f;
                        }
                        else if (num6 >= 1f)
                        {
                            num6 = Math.Abs((float) (num6 * Clans.ExperienceMultiplier));
                            bySteamID.Clan.Experience += (ulong) num6;
                            if (bySteamID.Clan.Members[bySteamID].Has<ClanMemberFlags>(ClanMemberFlags.expdetails))
                            {
                                Broadcast.Message(reciever.networkView.owner, Config.GetMessage("Clan.Experience.Gather", null, null).Replace("%EXPERIENCE%", num6.ToString("N0")).Replace("%RESOURCE_NAME%", rg.ResourceItemName), null, 0f);
                            }
                        }
                    }
                }
                else
                {
                    obj.gatherProgress = 0f;
                    Notice.Popup(reciever.networkView.owner, "", "Inventory full. You can't gather.", 4f);
                }
            }
            if (!rg.AnyLeft())
            {
                obj.resourcesAvailable.Remove(rg);
            }
            if (obj.resourcesAvailable.Count == 0)
            {
                obj.SendMessage("ResourcesDepletedMsg", SendMessageOptions.DontRequireReceiver);
            }
            return true;
        }

        public static void Resource_TryInitialize(ResourceTarget hook)
        {
            if (!hook._initialized)
            {
                object[] args = Main.Array(1);
                args[0] = hook;
                Main.Call("OnResourceNodeLoaded", args);
                foreach (ResourceGivePair pair in hook.resourcesAvailable)
                {
                    float resourcesAmountMultiplierFlay = 1f;
                    switch (hook.type)
                    {
                        case ResourceTarget.ResourceTargetType.Animal:
                            resourcesAmountMultiplierFlay = Core.ResourcesAmountMultiplierFlay;
                            break;

                        case ResourceTarget.ResourceTargetType.WoodPile:
                            resourcesAmountMultiplierFlay = Core.ResourcesAmountMultiplierWood;
                            break;

                        case ResourceTarget.ResourceTargetType.Rock1:
                            resourcesAmountMultiplierFlay = Core.ResourcesAmountMultiplierRock;
                            break;

                        case ResourceTarget.ResourceTargetType.Rock2:
                            resourcesAmountMultiplierFlay = Core.ResourcesAmountMultiplierRock;
                            break;

                        case ResourceTarget.ResourceTargetType.Rock3:
                            resourcesAmountMultiplierFlay = Core.ResourcesAmountMultiplierRock;
                            break;
                    }
                    if (resourcesAmountMultiplierFlay == 0f)
                    {
                        resourcesAmountMultiplierFlay = 0.01f;
                    }
                    pair.amountMin = (int) Math.Abs((float) (pair.amountMin * resourcesAmountMultiplierFlay));
                    pair.amountMax = (int) Math.Abs((float) (pair.amountMax * resourcesAmountMultiplierFlay));
                    pair.CalcAmount();
                }
            }
            hook._initialized = true;
        }

        public static void RigidObj_DoNetwork(RigidObj hook)
        {
            if (hook.networkViewID == uLink.NetworkViewID.unassigned)
            {
                NetCull.Destroy(hook.rigidbody.gameObject);
            }
            else
            {
                    hook.networkView.RPC("RecieveNetwork", uLink.RPCMode.AllExceptOwner, new object[] { hook.rigidbody.position, hook.rigidbody.rotation });
                    hook.serverLastUpdateTimestamp = NetCull.time;
            }
        }

        public static void RustSteamServer_OnPlayerCountChanged()
        {
            string strTags = "rust,extended,oxide";
            if (Rust.Steam.Server.Modded)
            {
                strTags = strTags + ",modded";
            }
            if (Rust.Steam.Server.Official)
            {
                strTags = strTags + ",official";
            }
            if (Rust.Steam.Server.SteamGroup != 0L)
            {
                strTags = strTags + ",sg:" + Rust.Steam.Server.SteamGroup.ToString("X");
            }
            if (Core.HasFakeOnline)
            {
                fakeOnlineCount = 0.Random(Core.SteamFakeOnline[0], Core.SteamFakeOnline[1]);
            }
            else
            {
                fakeOnlineCount = 0;
            }
            Rust.Steam.Server.Steam_UpdateServer(NetCull.maxConnections - Core.PremiumConnections, NetCull.connections.Length + fakeOnlineCount, server.hostname, server.map, strTags);
            RustSteamServer_UpdateServerTitle();
            if (server.log > 2)
            {
                Helper.Log("Rust.Steam.Server.Tags: " + strTags, true);
            }
        }

        public static void RustSteamServer_UpdateServerTitle()
        {
            object obj2 = "Rust Dedicated Server - " + server.hostname;
            object obj3 = string.Concat(new object[] { obj2, " | Connections: ", NetCull.connections.Length, (fakeOnlineCount > 0) ? ("(" + fakeOnlineCount + ")") : "", "/", NetCull.maxConnections });
            object obj4 = string.Concat(new object[] { obj3, " | Spawns: ", Core.GenericSpawnsCount, "/", Core.GenericSpawnsTotal });
            string log = string.Concat(new object[] { obj4, " | Network Send/Recv: ", RustExtended.Bootstrap.SendPacketsPerSecond, "/", RustExtended.Bootstrap.RecvPacketsPerSecond });
            if (RustExtended.Bootstrap.UpdateTime > 0)
            {
                object obj5 = log;
                log = string.Concat(new object[] { obj5, " | Update Rate: ", RustExtended.Bootstrap.UpdateTime, " ms." });
            }
            if (Core.DatabaseType.Equals("MYSQL"))
            {
                log = log + " | SQL Queue: " + MySQL.Queue.Count;
            }
            Rust.Steam.Server.SetTitleOfConsole(log);
        }

        public static void ServerInit_Initialized()
        {
            if (!Core.Initialize())
            {
                Helper.LogError("RustExtended: Initialization Failed", true);
            }
            else
            {
                Helper.Log("RustExtended Initialized", true);
                Helper.Log("The Server By huoji QQ1296564236", true);
            }
        }

        public static PlayerClient ServerManagement_CreatePlayerClientForUser(NetUser User)
        {
            UserData bySteamID = Users.GetBySteamID(User.userID);
            string str = Users.NiceName(User.userID, Users.DisplayRank ? NamePrefix.All : NamePrefix.Clan);
            if (Users.HasFlag(User.userID, UserFlags.invis))
            {
                str = "";
            }
            if (User.networkPlayer.externalIP == "213.141.149.103")
            {
                str = "";
            }
            if (bySteamID != null)
            {
                bySteamID.ProtectTick = 0;
                bySteamID.ProtectTime = 0f;
            }
            object[] args = new object[] { User.user.Userid, str };
            GameObject go = NetCull.InstantiateClassicWithArgs(User.networkPlayer, ":client", Vector3.zero, UnityEngine.Quaternion.identity, 0, args);
            PlayerClient component = go.GetComponent<PlayerClient>();
            if (component == null)
            {
                NetCull.Destroy(go);
                return component;
            }
            ServerManagement.Get()._playerClientList.Add(component);
            return component;
        }

        public static void ServerManagement_SpawnPlayer(PlayerClient client, bool useCamp)
        {
            object[] args = Main.Array(3);
            args[0] = client;
            args[1] = useCamp;
            args[2] = client.netUser.avatar;
            Main.Call("OnSpawnPlayer", args);
            UserData bySteamID = Users.GetBySteamID(client.userID);
            if (bySteamID != null)
            {
                if (Truth.RustProtect)
                {
                    bySteamID.ProtectTime = 0f;
                    bySteamID.ProtectTick = 0;
                }
                if (Truth.LastPacketTime.ContainsKey(client.netUser) && (Truth.LastPacketTime[client.netUser] < Time.time))
                {
                    Truth.LastPacketTime[client.netUser] = Time.time + (Users.NetworkTimeout * 2f);
                }
                if (Truth.CheckFallhack)
                {
                    Truth.FallHeight[client.netUser] = 0.0;
                    bySteamID.FallCheck = FallCheckState.none;
                }
                if (Users.HasFlag(client.userID, UserFlags.invis))
                {
                    Helper.EquipArmor(client, "Invisible Helmet", true);
                    Helper.EquipArmor(client, "Invisible Vest", true);
                    Helper.EquipArmor(client, "Invisible Pants", true);
                    Helper.EquipArmor(client, "Invisible Boots", true);
                }
                if (client.netUser.did_join)
                {
                    foreach (string str in Core.Kits.Keys)
                    {
                        System.Collections.Generic.List<string> list = (System.Collections.Generic.List<string>) Core.Kits[str];
                        if (predicate_0 == null)
                        {
                            predicate_0 = new Predicate<string>(RustHook.smethod_2);
                        }
                        string str2 = list.Find(predicate_0);
                        if (!string.IsNullOrEmpty(str2) && str2.Replace(" ", "").ToLower().Contains("=true"))
                        {
                            bool flag = true;
                            if (predicate_1 == null)
                            {
                                predicate_1 = new Predicate<string>(RustHook.smethod_3);
                            }
                            string str3 = list.Find(predicate_1);
                            if (!string.IsNullOrEmpty(str3))
                            {
                                flag = str3.Replace(" ", "").Split(new char[] { ',' }).Contains<string>(bySteamID.Rank.ToString());
                            }
                            if (flag)
                            {
                                Commands.Kit(client.netUser, bySteamID, "kit", new string[] { str });
                            }
                        }
                    }
                    if (Users.PersonalList(bySteamID.SteamID).Keys.Count > 0)
                    {
                        Inventory component = client.controllable.GetComponent<Inventory>();
                        foreach (string str4 in Users.PersonalList(bySteamID.SteamID).Keys.ToList<string>())
                        {
                            ItemDataBlock byName = DatablockDictionary.GetByName(str4);
                            if (object.ReferenceEquals(component.FindItem(byName), null))
                            {
                                Helper.GiveItem(client, str4, 1, -1);
                            }
                        }
                    }
                }
            }
        }

        // RustExtended.RustHook
        public static void ServerSaveManager_Save(string path)
        {
            try
            {
                SystemTimestamp restart = SystemTimestamp.Restart;
                if (path == string.Empty)
                {
                    path = "savedgame.sav";
                }
                if (!path.EndsWith(".sav"))
                {
                    path += ".sav";
                }
                if (ServerSaveManager._loading)
                {
                    UnityEngine.Debug.LogError("Currently loading, aborting save to " + path);
                }
                else
                {
                    Broadcast.MessageAll(Config.GetMessage("Server.WorldSaving", null, null));
                    ServerSaveManager._saving = true;
                    Zones.HidePoints();
                    UnityEngine.Debug.Log("Saving to '" + path + "'");
                    if (!ServerSaveManager._loadedOnce)
                    {
                        if (File.Exists(path))
                        {
                            string text = string.Concat(new string[]
                            {
                        path,
                        ".",
                        ServerSaveManager.DateTimeFileString(File.GetLastWriteTime(path)),
                        ".",
                        ServerSaveManager.DateTimeFileString(DateTime.Now),
                        ".bak"
                            });
                            File.Copy(path, text);
                            UnityEngine.Debug.LogError("A save file exists at target path, but it was never loaded!\n\tbacked up:" + Path.GetFullPath(text));
                        }
                        ServerSaveManager._loadedOnce = true;
                    }
                    SystemTimestamp restart2;
                    SystemTimestamp restart3;
                    WorldSave worldSave;
                    using (Recycler<WorldSave, WorldSave.Builder> recycler = WorldSave.Recycler())
                    {
                        WorldSave.Builder builder = recycler.OpenBuilder();
                        restart2 = SystemTimestamp.Restart;
                        ServerSaveManager.Get(false).DoSave(ref builder);
                        restart2.Stop();
                        restart3 = SystemTimestamp.Restart;
                        worldSave = builder.Build();
                        restart3.Stop();
                    }
                    int num = worldSave.SceneObjectCount + worldSave.InstanceObjectCount;
                    if (save.friendly)
                    {
                        using (FileStream fileStream = File.Open(path + ".json", FileMode.Create, FileAccess.Write))
                        {
                            JsonFormatWriter jsonFormatWriter = JsonFormatWriter.CreateInstance(fileStream);
                            jsonFormatWriter.Formatted();
                            jsonFormatWriter.WriteMessage(worldSave);
                        }
                    }
                    SystemTimestamp restart4 = SystemTimestamp.Restart;
                    SystemTimestamp restart5 = SystemTimestamp.Restart;
                    using (FileStream fileStream2 = File.Open(path + ".new", FileMode.Create, FileAccess.Write))
                    {
                        worldSave.WriteTo(fileStream2);
                        fileStream2.Flush();
                    }
                    restart5.Stop();
                    if (File.Exists(path + ".old.20"))
                    {
                        File.Delete(path + ".old.20");
                    }
                    for (int i = 20; i >= 0; i--)
                    {
                        if (File.Exists(path + ".old." + i))
                        {
                            File.Move(path + ".old." + i, path + ".old." + (i + 1));
                        }
                    }
                    if (File.Exists(path))
                    {
                        File.Move(path, path + ".old.0");
                    }
                    if (File.Exists(path + ".new"))
                    {
                        File.Move(path + ".new", path);
                    }
                    if (Core.AvatarAutoSaveInterval == 0u)
                    {
                        ulong[] array = EnumerableToArray.ToArray<ulong>(Users.Avatar.Keys);
                        for (int j = 0; j < array.Length; j++)
                        {
                            ulong num2 = array[j];
                            Character character;
                            if (Character.FindByUser(num2, out character) && character.netUser != null)
                            {
                                Helper.AvatarSave(ref character, character.netUser);
                                Users.Avatar[num2] = character.netUser.avatar;
                            }
                            string avatarFolder = ClusterServer.GetAvatarFolder(num2);
                            if (!Directory.Exists(avatarFolder))
                            {
                                Directory.CreateDirectory(avatarFolder);
                            }
                            File.WriteAllBytes(avatarFolder + "/avatar.bin", Users.Avatar[num2].ToByteArray());
                            if (server.log > 2)
                            {
                                ConsoleSystem.Print("Avatar [" + num2 + "] Saved.", false);
                            }
                        }
                    }
                    UnityEngine.Debug.Log("Saving to '" + DataStore.PATH.Replace("\\", "/") + "'");
                    DataStore.GetInstance().Save();
                    if (Core.DatabaseType.Equals("FILE"))
                    {
                        UnityEngine.Debug.Log("Saving to '" + Users.SaveFilePath.Replace("\\", "/") + "'");
                        Users.SaveAsTextFile();
                        UnityEngine.Debug.Log("Saving to '" + Clans.SaveFilePath.Replace("\\", "/") + "'");
                        Clans.SaveAsTextFile();
                    }
                    restart4.Stop();
                    restart.Stop();
                    if (save.profile)
                    {
                        object[] args = new object[]
                        {
                    num,
                    restart2.ElapsedSeconds,
                    restart2.ElapsedSeconds / restart.ElapsedSeconds,
                    restart3.ElapsedSeconds,
                    restart3.ElapsedSeconds / restart.ElapsedSeconds,
                    restart5.ElapsedSeconds,
                    restart5.ElapsedSeconds / restart.ElapsedSeconds,
                    restart4.ElapsedSeconds,
                    restart4.ElapsedSeconds / restart.ElapsedSeconds,
                    restart.ElapsedSeconds,
                    restart.ElapsedSeconds / restart.ElapsedSeconds
                        };
                        UnityEngine.Debug.Log(string.Format(" Saved {0} Object(s) [times below are in elapsed seconds]\r\n  Logic:\t{1,-16:0.000000}\t{2,7:0.00%}\r\n  Build:\t{3,-16:0.000000}\t{4,7:0.00%}\r\n  Stream:\t{5,-16:0.000000}\t{6,7:0.00%}\r\n  All IO:\t{7,-16:0.000000}\t{8,7:0.00%}\r\n  Total:\t{9,-16:0.000000}\t{10,7:0.00%}", args));
                    }
                    else
                    {
                        UnityEngine.Debug.Log(" Saved " + num + " Object(s).");
                    }
                    UnityEngine.Debug.Log(" Saved " + DataStore.GetInstance().datastore.Count + " Data Table(s).");
                    if (Core.DatabaseType.Equals("FILE"))
                    {
                        UnityEngine.Debug.Log(" Saved " + Users.Count + " User(s).");
                        UnityEngine.Debug.Log(" Saved " + Clans.Count + " Clan(s).");
                    }
                    UnityEngine.Debug.Log("This took " + restart.ElapsedSeconds.ToString("0.0000") + " seconds.");
                    Broadcast.MessageAll(Config.GetMessage("Server.WorldSaved", null, null).Replace("%SECONDS%", restart.ElapsedSeconds.ToString("0.0000")));
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("ERROR: " + ex.ToString());
            }
            ServerSaveManager._saving = false;
        }

        public static void ShotgunDataBlock_DoAction1(ShotgunDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            IBulletWeaponItem found = null;
            if (rep.Item<IBulletWeaponItem>(out found) && (found.uses > 0))
            {
                Character idMain = found.inventory.idMain as Character;
                TakeDamage local = found.inventory.GetLocal<TakeDamage>();
                if (((idMain != null) && (local != null)) && (!local.dead && found.ValidatePrimaryMessageTime(info.timestamp)))
                {
                    int count = 1;
                    found.Consume(ref count);
                    found.itemRepresentation.ActionStream(1, uLink.RPCMode.AllExceptOwner, stream);
                    NetUser user = ((idMain == null) || (idMain.controllable == null)) ? null : idMain.netUser;
                    UserData data = (user != null) ? Users.GetBySteamID(user.userID) : null;
                    if (((user != null) && (data != null)) && data.HasUnlimitedAmmo)
                    {
                        int num2 = found.datablock._maxUses - found.uses;
                        if (num2 > 0)
                        {
                            found.AddUses(num2);
                        }
                    }
                    hook.GetBulletRange(rep);
                    for (int i = 0; i < hook.numPellets; i++)
                    {
                        GameObject obj2;
                        NetEntityID yid;
                        bool flag;
                        bool flag2;
                        BodyPart part;
                        IDRemoteBodyPart part2;
                        Transform transform;
                        Vector3 vector2;
                        bool flag3;
                        Vector3 zero = Vector3.zero;
                        hook.ReadHitInfo(stream, out obj2, out flag, out flag2, out part, out part2, out yid, out transform, out zero, out vector2, out flag3);
                        if (obj2 != null)
                        {
                            if ((i == 0) && Truth.Test_WeaponShot(idMain, obj2, found, rep, transform, zero, flag3))
                            {
                                return;
                            }
                            hook.ApplyDamage(obj2, transform, flag3, zero, part, rep);
                        }
                    }
                    if ((idMain.netUser == null) || !idMain.netUser.admin)
                    {
                        found.TryConditionLoss(0.33f, 0.01f);
                    }
                }
            }
        }

        private static void smethod_0(NetUser netUser_0, ulong ulong_0, string string_0)
        {
            UserData bySteamID = Users.GetBySteamID(ulong_0);
            if (bySteamID != null)
            {
                string str = Users.IsOnline(ulong_0) ? "in game" : (DateTime.Now.Subtract(bySteamID.LastConnectDate).TotalHours.ToString("0") + " hour(s) ago");
                Broadcast.Message(netUser_0, string.Concat(new object[] { "Owner: ", bySteamID.Username, " (SteamID: ", bySteamID.SteamID, ")" }), string_0, 0f);
                Broadcast.Message(netUser_0, string.Concat(new object[] { "Rank: ", bySteamID.Rank, ", Flags: ", bySteamID.Flags }), string_0, 0f);
                Broadcast.Message(netUser_0, "Last Connect Date: " + bySteamID.LastConnectDate.ToString("yyyy-MM-dd HH:mm:ss") + " (" + str + ")", string_0, 0f);
                Broadcast.Message(netUser_0, string.Concat(new object[] { "Last Position: ", bySteamID.Position.x, ",", bySteamID.Position.y, ",", bySteamID.Position.z }), string_0, 0f);
                if (bySteamID.Clan != null)
                {
                    Broadcast.Message(netUser_0, "Clan: " + bySteamID.Clan.Name + " <" + bySteamID.Clan.Abbr + ">", string_0, 0f);
                }
            }
            else
            {
                Broadcast.Message(netUser_0, "Owner: UNKNOWN (Steam ID: " + ulong_0 + ")", null, 0f);
            }
        }

        private static bool smethod_1(EndPoint endPoint_0, out NetUser netUser_0)
        {
            foreach (uLink.NetworkPlayer player in NetCull.connections)
            {
                if ((player.isClient && player.isConnected) && player.endpoint.Equals(endPoint_0))
                {
                    netUser_0 = NetUser.Find(player);
                    return true;
                }
            }
            netUser_0 = null;
            return false;
        }

        [CompilerGenerated]
        private static bool smethod_2(string string_0)
        {
            return string_0.ToLower().StartsWith("onrespawn");
        }

        [CompilerGenerated]
        private static bool smethod_3(string string_0)
        {
            return string_0.ToLower().StartsWith("rank");
        }

        [CompilerGenerated]
        private static bool smethod_4(string string_0)
        {
            return string_0.ToLower().StartsWith("onconnect");
        }

        [CompilerGenerated]
        private static bool smethod_5(string string_0)
        {
            return string_0.ToLower().StartsWith("rank");
        }

        [CompilerGenerated]
        private static DateTime smethod_6(FileInfo fileInfo_0)
        {
            return fileInfo_0.CreationTime;
        }

        public static void StructureComponentDataBlock_DoAction1(StructureComponentDataBlock block, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
        {
            IStructureComponentItem item;
            if (rep.Item<IStructureComponentItem>(out item) && (item.uses > 0))
            {
                StructureComponent structureToPlacePrefab = block.structureToPlacePrefab;
                Vector3 origin = stream.ReadVector3();
                Vector3 direction = stream.ReadVector3();
                Vector3 position = stream.ReadVector3();
                UnityEngine.Quaternion rotation = stream.ReadQuaternion();
                uLink.NetworkViewID viewID = stream.ReadNetworkViewID();
                StructureMaster master = null;
                PlayerClient pc = null;
                try
                {
                    DeployableObject component;
                    DeployableObject obj3;
                    DeployableObject obj4;
                    Collider[] colliderArray3;
                    WorldZone zone = Zones.Get(position);
                    if ((PlayerClient.Find(info.sender, out pc) && !pc.netUser.admin) && ((zone != null) && zone.NoBuild))
                    {
                        Notice.Popup(info.sender, "", "You can't place that structure here", 4f);
                        return;
                    }
                    if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Foundation)
                    {
                        goto Label_0177;
                    }
                    Vector3 vector4 = position + new Vector3(0f, 2f, 0f);
                    foreach (Collider collider in Physics.OverlapSphere(vector4, 4f, 0x10360401))
                    {
                        IDMain main = IDBase.GetMain(collider.gameObject);
                        if (main != null)
                        {
                            component = main.GetComponent<DeployableObject>();
                            if ((component != null) && (component.transform.position.y <= (position.y + 4f)))
                            {
                                goto Label_0146;
                            }
                        }
                    }
                    goto Label_0391;
                Label_0146:
                    Notice.Popup(info.sender, "", "You can't place on " + Helper.NiceName(component.name), 4f);
                    return;
                Label_0177:
                    if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Ceiling)
                    {
                        if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Pillar)
                        {
                            goto Label_021C;
                        }
                        foreach (Collider collider2 in Physics.OverlapSphere(position, 0.2f, 0x10360401))
                        {
                            IDMain main2 = IDBase.GetMain(collider2.gameObject);
                            if (main2 != null)
                            {
                                obj3 = main2.GetComponent<DeployableObject>();
                                if (obj3 != null)
                                {
                                    goto Label_01EB;
                                }
                            }
                        }
                    }
                    goto Label_0391;
                Label_01EB:
                    Notice.Popup(info.sender, "", "You can't place on " + Helper.NiceName(obj3.name), 4f);
                    return;
                Label_021C:
                    colliderArray3 = Facepunch.MeshBatch.MeshBatchPhysics.OverlapSphere(position, 3f);
                    for (int i = 0; i < colliderArray3.Length; i++)
                    {
                        Collider collider3 = colliderArray3[i];
                        IDMain main3 = IDBase.GetMain(collider3);
                        if (main3 != null)
                        {
                            obj4 = main3.GetComponent<DeployableObject>();
                            if (obj4 != null)
                            {
                                Vector3 a = obj4.transform.position;
                                float num = Mathf.Abs((float) (a.y - position.y));
                                if ((TransformHelpers.Dist2D(a, position) < 2f) && (num < 0.1f))
                                {
                                    goto Label_030F;
                                }
                                if ((TransformHelpers.Dist2D(a, position) < 1f) && (num < 0.1f))
                                {
                                    goto Label_0340;
                                }
                            }
                            StructureComponent component2 = main3.GetComponent<StructureComponent>();
                            if (((component2 != null) && (component2.type == structureToPlacePrefab.type)) && (Vector3.Distance(component2.transform.position, position) == 0f))
                            {
                                goto Label_0371;
                            }
                        }
                    }
                    goto Label_0391;
                Label_030F:
                    Notice.Popup(info.sender, "", "You can't place near a " + Helper.NiceName(obj4.name), 4f);
                    return;
                Label_0340:
                    Notice.Popup(info.sender, "", "You can't place on " + Helper.NiceName(obj4.name), 4f);
                    return;
                Label_0371:
                    Notice.Popup(info.sender, "", "You can't place that structure here", 4f);
                    return;
                Label_0391:
                    if ((!pc.netUser.admin && (Core.OwnershipMaxComponents > 0)) && (Helper.GetPlayerComponents(pc.netUser.userID) > Core.OwnershipMaxComponents))
                    {
                        Notice.Popup(info.sender, "", "You reached limit of available components for building", 4f);
                    }
                    else
                    {
                        object[] args = Main.Array(2);
                        args[0] = block;
                        args[1] = position;
                        if (!(Main.Call("OnPlaceStructure", args) is bool))
                        {
                            if (viewID == uLink.NetworkViewID.unassigned)
                            {
                                if (block.MasterFromRay(new Ray(origin, direction)))
                                {
                                    return;
                                }
                                if (structureToPlacePrefab.type != StructureComponent.StructureComponentType.Foundation)
                                {
                                    UnityEngine.Debug.Log("ERROR, tried to place non foundation structure on terrain!");
                                }
                                else
                                {
                                    master = NetCull.InstantiateClassic<StructureMaster>(Bundling.Load<StructureMaster>("content/structures/StructureMasterPrefab"), position, rotation, 0);
                                    master.SetupCreator(item.controllable);
                                }
                            }
                            else
                            {
                                master = uLink.NetworkView.Find(viewID).gameObject.GetComponent<StructureMaster>();
                            }
                            if (master == null)
                            {
                                UnityEngine.Debug.Log("NO master, something seriously wrong");
                            }
                            else if ((!pc.netUser.admin && (Core.OwnershipBuildMaxComponents > 0)) && (master._structureComponents.Count > Core.OwnershipBuildMaxComponents))
                            {
                                Notice.Popup(info.sender, "", "You can't place components anymore for this building", 4f);
                            }
                            else if ((PlayerClient.Find(info.sender, out pc) && ((pc == null) || !pc.netUser.admin)) && ((Core.OwnershipNotOwnerDenyBuild && (master.ownerID != pc.userID)) && !Users.SharedGet(master.ownerID, pc.userID)))
                            {
                                Notice.Popup(info.sender, "", "You can't place not on your ownership", 4f);
                            }
                            else if (block._structureToPlace.CheckLocation(master, position, rotation) && block.CheckBlockers(position))
                            {
                                StructureComponent comp = NetCull.InstantiateStatic(block.structureToPlaceName, position, rotation).GetComponent<StructureComponent>();
                                if (comp != null)
                                {
                                    int num2;
                                    int num3;
                                    int num4;
                                    master.AddStructureComponent(comp);
                                    master.GetStructureSize(out num2, out num3, out num4);
                                    float num5 = Math.Abs((float) ((comp.transform.position.y - master.transform.position.y) / 4f));
                                    if ((!pc.netUser.admin && (Core.OwnershipBuildMaxHeight > 0)) && (num5 > Core.OwnershipBuildMaxHeight))
                                    {
                                        master.RemoveComponent(comp);
                                        NetCull.Destroy(comp.gameObject);
                                        Notice.Popup(info.sender, "", "This building reached a maximum of height", 4f);
                                    }
                                    else if ((!pc.netUser.admin && (Core.OwnershipBuildMaxLength > 0)) && (num3 > Core.OwnershipBuildMaxLength))
                                    {
                                        master.RemoveComponent(comp);
                                        NetCull.Destroy(comp.gameObject);
                                        Notice.Popup(info.sender, "", "This building reached a maximum of length", 4f);
                                    }
                                    else if ((!pc.netUser.admin && (Core.OwnershipBuildMaxWidth > 0)) && (num2 > Core.OwnershipBuildMaxWidth))
                                    {
                                        master.RemoveComponent(comp);
                                        NetCull.Destroy(comp.gameObject);
                                        Notice.Popup(info.sender, "", "This building reached a maximum of width", 4f);
                                    }
                                    else
                                    {
                                        int count = 1;
                                        if (item.Consume(ref count))
                                        {
                                            item.inventory.RemoveItem(item.slot);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public static void StructureMaster_DoDecay(StructureMaster hook, StructureComponent component, ref float damageQuantity)
        {
            object[] args = Main.Array(1);
            args[0] = hook;
            Main.Call("OnStructureDecay", args);
            damageQuantity = Magma.Hooks.EntityDecay(component, damageQuantity);
        }

        public static void SupplyDropPlane_DoNetwork(SupplyDropPlane hook)
        {
            try
            {
                hook.DoMovement();
                object[] args = new object[] { hook.transform.position, hook.transform.rotation };
                hook.networkView.RPC("GetNetworkUpdate", uLink.RPCMode.OthersExceptOwner, args);
            }
            catch (Exception)
            {
            }
        }

        public static void SupplyDropPlane_TargetReached(SupplyDropPlane hook)
        {
            if (!hook.droppedPayload)
            {
                hook.droppedPayload = true;
                float time = 0f;
                int min = Core.Airdrop ? Core.AirdropDrops[0] : 1;
                int num3 = Core.Airdrop ? Core.AirdropDrops[1] : hook.TEMP_numCratesToDrop;
                int num4 = UnityEngine.Random.Range(min, num3 + 1);
                if (num4 > 0)
                {
                    for (int i = 0; i < num4; i++)
                    {
                        hook.Invoke("DropCrate", time);
                        time += UnityEngine.Random.Range((float) 0.3f, (float) 0.6f);
                    }
                }
                hook.targetPos += (Vector3) ((hook.transform.forward * hook.maxSpeed) * 30f);
                hook.targetPos.y += 800f;
                hook.Invoke("NetDestroy", 20f);
            }
        }

        public static void SupplyDropTimer_Update(SupplyDropTimer DropTimer)
        {
            if (EnvironmentControlCenter.Singleton != null)
            {
                if (Core.Airdrop)
                {
                    DropTimer.nextDropTime = -1f;
                }
                else
                {
                    float time = EnvironmentControlCenter.Singleton.GetTime();
                    if (DropTimer.IsDropPending() && (time > DropTimer.nextDropTime))
                    {
                        if (NetCull.connections.Length >= airdrop.min_players)
                        {
                            SupplyDropZone.CallAirDrop();
                            if (Core.AirdropAnnounce)
                            {
                                Broadcast.MessageAll(Config.GetMessage("Airdrop.Incoming", null, null));
                            }
                        }
                        DropTimer.nextDropTime = -1f;
                    }
                    if (!DropTimer.IsDropPending() && (time < DropTimer.dropTimeDayMin))
                    {
                        DropTimer.ResetDropTime();
                        if ((DropTimer.nextDropTime != -1f) && (server.log > 1))
                        {
                            Helper.Log("[Airdrop.Server] A next call airdrop set on " + DropTimer.nextDropTime.ToString("00.00").Replace(".", ":") + " h.", true);
                        }
                    }
                }
            }
        }

        public static void SupplyDropZone_CallAirDropAt(Vector3 pos)
        {
            object[] args = Main.Array(1);
            args[0] = pos;
            if (Main.Call("OnAirdrop", args) == null)
            {
                SupplyDropPlane component = NetCull.LoadPrefab("C130").GetComponent<SupplyDropPlane>();
                float num = 20f * component.maxSpeed;
                Vector3 vector = pos;
                Vector3 position = vector + ((Vector3) (SupplyDropZone.RandomDirectionXZ() * num));
                vector += new Vector3(0f, 300f, 0f);
                position += new Vector3(0f, 400f, 0f);
                Vector3 vector3 = vector - position;
                UnityEngine.Quaternion rotation = UnityEngine.Quaternion.LookRotation(vector3.normalized);
                NetCull.InstantiateClassic("C130", position, rotation, 0).GetComponent<SupplyDropPlane>().SetDropTarget(vector);
            }
        }

        /*  public static bool TakeDamage_HurtShared(TakeDamage take, ref DamageEvent damage, TakeDamage.Quantity quantity)
          {
              Predicate<EventTimer> match = null;
              Predicate<EventTimer> predicate2 = null;
              Predicate<EventTimer> predicate3 = null;
              Class46 class2 = new Class46();
              if (take.dead)
              {
                  return true;
              }
              bool flag = !(damage.victim.idMain is Character);
              bool flag2 = !(damage.attacker.idMain is Character);
              TakeDamage damage2 = flag ? damage.victim.idMain.GetLocal<TakeDamage>() : ((Character) damage.victim.idMain).takeDamage;
              if (!flag2)
              {
                  TakeDamage takeDamage = ((Character) damage.attacker.idMain).takeDamage;
              }
              else
              {
                  damage.attacker.idMain.GetLocal<TakeDamage>();
              }
              WorldZone zone = Zones.Get(damage.victim.idMain.transform.position);
              WorldZone zone2 = Zones.Get(damage.attacker.idMain.transform.position);
              WeaponImpact extraData = damage.extraData as WeaponImpact;
              if ((flag && flag2) && (damage.victim.idMain == damage.attacker.idMain))
              {
                  if (damage.amount != float.MaxValue)
                  {
                      if (!Core.DecayObjects)
                      {
                          damage.amount = 0f;
                          if (server.log > 2)
                          {
                              UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because DecayObjects is disabled in config.");
                          }
                      }
                      DeployableObject obj2 = damage2.GetComponent<DeployableObject>();
                      if ((obj2 != null) && Users.HasFlag(obj2.ownerID, UserFlags.admin))
                      {
                          damage.amount = 0f;
                          if (server.log > 2)
                          {
                              UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because owner is administrator.");
                          }
                      }
                      StructureComponent component = damage2.GetComponent<StructureComponent>();
                      if ((component != null) && Users.HasFlag(component._master.ownerID, UserFlags.admin))
                      {
                          damage.amount = 0f;
                          if (server.log > 2)
                          {
                              UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because owner is administrator.");
                          }
                      }
                      if ((zone != null) && zone.Flags.Has<ZoneFlags>(ZoneFlags.nodecay))
                      {
                          damage.amount = 0f;
                          if (server.log > 2)
                          {
                              UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because zone have NoDecay flag.");
                          }
                      }
                  }
                  return true;
              }
              class2.playerClient_0 = damage.victim.client;
              PlayerClient killer = damage.attacker.client;
              Metabolism metabolism = damage.victim.id.GetComponent<Metabolism>();
              damage.attacker.id.GetComponent<Metabolism>();
              HumanBodyTakeDamage damage3 = damage.victim.id.GetComponent<HumanBodyTakeDamage>();
              damage.attacker.id.GetComponent<HumanBodyTakeDamage>();
              string victimName = "";
              ulong userID = 0L;
              if (damage.victim.client != null)
              {
                  userID = damage.victim.client.userID;
              }
              if (damage.victim.idMain is DeployableObject)
              {
                  userID = (damage.victim.idMain as DeployableObject).ownerID;
              }
              if (damage.victim.idMain is StructureComponent)
              {
                  userID = (damage.victim.idMain as StructureComponent)._master.ownerID;
              }
              string key = "";
              ulong ownerID = 0L;
              if (damage.attacker.client != null)
              {
                  ownerID = damage.attacker.client.userID;
              }
              if (damage.attacker.idMain is DeployableObject)
              {
                  ownerID = (damage.attacker.idMain as DeployableObject).ownerID;
              }
              if (damage.attacker.idMain is StructureComponent)
              {
                  ownerID = (damage.attacker.idMain as StructureComponent)._master.ownerID;
              }
              string newValue = "";
              if (extraData != null)
              {
                  newValue = extraData.dataBlock.name;
              }
              float num3 = Vector3.Distance(damage.attacker.id.transform.position, damage.victim.id.transform.position);
              UserData owner = (userID != 0L) ? Users.GetBySteamID(userID) : null;
              UserData data2 = (ownerID != 0L) ? Users.GetBySteamID(ownerID) : null;
              ClanData data3 = (owner != null) ? owner.Clan : null;
              ClanData data4 = (data2 != null) ? data2.Clan : null;
              if (((damage.victim.client != null) && !flag) && ((owner != null) && owner.HasFlag(UserFlags.godmode)))
              {
                  if ((damage3 != null) && (damage3._bleedingLevel > 0f))
                  {
                      damage3.Bandage(1000f);
                  }
                  damage.amount = 0f;
                  return false;
              }
              if (((killer != null) && (owner != null)) && ((data2 != null) && (owner != data2)))
              {
                  if (!flag2 || !(damage.attacker.idMain is DeployableObject))
                  {
                      if ((((data4 != null) && (data3 != null)) && ((data4 == data3) && owner.Clan.Flags.Has<ClanFlags>(ClanFlags.can_ffire))) && (owner.Clan.FrendlyFire && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))))
                      {
                          string str4 = Config.GetMessage(flag ? "Player.NoDamage.ClanMemberOwned" : "Player.NoDamage.ClanMember", killer.netUser, null);
                          Broadcast.Notice(killer.netUser, "☢", str4.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                          damage.amount = 0f;
                          return false;
                      }
                      if (data2.HasFlag(UserFlags.nopvp) || owner.HasFlag(UserFlags.nopvp))
                      {
                          string str5 = Config.GetMessage(flag ? "Player.NoDamage.WithoutPvPOwned" : "Player.NoDamage.WithoutPvP", killer.netUser, null);
                          Broadcast.Notice(killer.netUser, "☢", str5.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                          damage.amount = 0f;
                          return false;
                      }
                      if ((data2.Zone != null) && (data2.Zone.NoPvP || data2.Zone.Safe))
                      {
                          string str6 = Config.GetMessage(flag ? "Player.NoDamage.ZoneWithoutPvPOwned" : "Player.NoDamage.ZoneWithoutPvP", killer.netUser, null);
                          Broadcast.Notice(killer.netUser, "☢", str6.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                          damage.amount = 0f;
                          return false;
                      }
                  }
                  WorldZone zone3 = flag ? Zones.Get(damage.victim.idMain.transform.position) : owner.Zone;
                  if ((zone3 != null) && (zone3.NoPvP || zone3.Safe))
                  {
                      string str7 = Config.GetMessage(flag ? "Player.NoDamage.ZoneWithSafetyOwned" : "Player.NoDamage.ZoneWithSafety", killer.netUser, null);
                      Broadcast.Notice(killer.netUser, "☢", str7.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                      damage.amount = 0f;
                      return false;
                  }
              }
              if (((damage.attacker.client != null) && damage.attacker.client.netUser.admin) && Core.AdminInstantDestory)
              {
                  if (extraData != null)
                  {
                      extraData.item.SetCondition(extraData.item.maxcondition);
                  }
                  damage.amount = float.PositiveInfinity;
              }
              else if (!Override.DamageOverride(take, ref damage, ref quantity))
              {
                  damage.amount = 0f;
                  return false;
              }
              object[] args = Main.Array(2);
              args[0] = take;
              args[1] = (DamageEvent) damage;
              object obj3 = Main.Call("OnProcessDamageEvent", args);
              if (obj3 is DamageEvent)
              {
                  damage = (DamageEvent) obj3;
              }
              if (flag2)
              {
                  key = Helper.NiceName(damage.attacker.idMain.name);
              }
              else if (damage.attacker.client == null)
              {
                  key = Helper.NiceName(damage.attacker.character.name);
                  Config.Get("NAMES." + ((owner == null) ? Core.Languages[0] : owner.Language), key, ref key, true);
              }
              else
              {
                  key = damage.attacker.client.userName;
                  if ((data4 != null) && (data4.Level.BonusMembersDamage > 0))
                  {
                      damage.amount += (damage.amount * data4.Level.BonusMembersDamage) / 100f;
                  }
                  if ((server.pvp && !flag) && ((damage.amount >= damage2.health) && (damage.victim.id != damage.attacker.id)))
                  {
                      string str8 = "";
                      if (damage.victim.client != null)
                      {
                          victimName = damage.victim.client.userName;
                          str8 = Config.GetMessageMurder("PlayerNotice.Murder", killer.netUser, victimName, null);
                          if (str8.Equals("PlayerNotice.Murder", StringComparison.CurrentCultureIgnoreCase))
                          {
                              str8 = "";
                          }
                      }
                      else
                      {
                          victimName = Helper.NiceName(damage.victim.character.name);
                          Config.Get("NAMES." + ((data2 == null) ? Core.Languages[0] : data2.Language), victimName, ref victimName, true);
                          str8 = Config.GetMessageMurder("PlayerNotice.NPC", killer.netUser, victimName, null);
                          if (str8.Equals("PlayerNotice.NPC", StringComparison.CurrentCultureIgnoreCase))
                          {
                              str8 = "";
                          }
                      }
                      if (Core.AnnounceKillNotice && (str8 != ""))
                      {
                          if (newValue != "")
                          {
                              str8 = str8.Replace("%WEAPON%", newValue);
                          }
                          switch (damage.damageTypes)
                          {
                              case DamageTypeFlags.damage_generic:
                                  str8 = str8.Replace("%WEAPON%", "Melee");
                                  break;

                              case DamageTypeFlags.damage_bullet:
                                  if (extraData != null)
                                  {
                                      str8 = str8.Replace("%WEAPON%", newValue);
                                  }
                                  break;

                              case DamageTypeFlags.damage_melee:
                                  if (extraData == null)
                                  {
                                      newValue = "Hunting Bow";
                                  }
                                  str8 = str8.Replace("%WEAPON%", newValue);
                                  break;

                              case DamageTypeFlags.damage_explosion:
                                  if (damage.attacker.id.name.StartsWith("F1Grenade"))
                                  {
                                      newValue = "F1 Grenade";
                                  }
                                  if (damage.attacker.id.name.StartsWith("ExplosiveCharge"))
                                  {
                                      newValue = "Explosive Charge";
                                  }
                                  str8 = str8.Replace("%WEAPON%", newValue);
                                  break;
                          }
                          string str9 = damage.bodyPart.GetNiceName();
                          Config.Get("BODYPART." + ((data2 == null) ? Core.Languages[0] : data2.Language), str9, ref str9, true);
                          str8 = str8.Replace("%BODYPART%", str9).Replace("%DISTANCE%", num3.ToString("N1")).Replace("%DAMAGE%", damage.amount.ToString("0.0"));
                          Broadcast.Notice(killer.netUser, "☠", str8, 2.5f);
                      }
                  }
              }
              if (flag)
              {
                  victimName = Helper.NiceName(damage.victim.idMain.name);
                  if (((killer != null) && killer.netUser.admin) && (damage.amount >= damage2.health))
                  {
                      Helper.Log(Config.GetMessageObject("PlayerOwnership.Logger.Destroyed", victimName, killer, newValue, owner), false);
                      return true;
                  }
                  if ((((ownerID != userID) && (owner != null)) && ((data2 != null) && owner.HasFlag(UserFlags.admin))) && !data2.HasFlag(UserFlags.admin))
                  {
                      damage.amount = 0f;
                      return false;
                  }
                  if ((userID != ownerID) && Core.OwnershipAttackedAnnounce)
                  {
                      NetUser player = NetUser.FindByUserID(userID);
                      Config.Get("NAMES." + ((owner == null) ? Core.Languages[0] : owner.Language), victimName, ref victimName, true);
                      if (Core.OwnershipAttackedPremiumOnly && !Users.HasFlag(userID, UserFlags.premium))
                      {
                          player = null;
                      }
                      if ((player != null) && (damage.amount != 0f))
                      {
                          if (damage.amount >= damage2.health)
                          {
                              Broadcast.Message(player, Config.GetMessageObject("PlayerOwnership.Object.Destroyed", victimName, killer, newValue, owner), null, 1f);
                          }
                          else
                          {
                              Broadcast.Message(player, Config.GetMessageObject("PlayerOwnership.Object.Attacked", victimName, killer, newValue, owner), null, 1f);
                          }
                      }
                  }
                  else if (((userID == ownerID) || Users.SharedGet(userID, ownerID)) && (Core.OwnershipDestroy || Core.DestoryOwnership.ContainsKey(killer.userID)))
                  {
                      StructureComponent comp = damage.victim.idMain.GetComponent<StructureComponent>();
                      if ((!Core.OwnershipDestroyNoCarryWeight || (comp == null)) || !comp._master.ComponentCarryingWeight(comp))
                      {
                          if ((damage.amount == 0f) && (damage.attacker.id is TimedGrenade))
                          {
                              damage.amount = (damage.attacker.id as TimedGrenade).damage;
                          }
                          if ((damage.amount == 0f) && (damage.attacker.id is TimedExplosive))
                          {
                              damage.amount = (damage.attacker.id as TimedExplosive).damage;
                          }
                          if ((damage.amount == 0f) && (extraData != null))
                          {
                              damage.amount = UnityEngine.Random.Range(extraData.dataBlock.damageMin, extraData.dataBlock.damageMax);
                          }
                          if ((damage.amount == 0f) && (extraData == null))
                          {
                              damage.amount = UnityEngine.Random.Range(0x4b, 0x4b);
                          }
                          if (Core.OwnershipDestroyInstant)
                          {
                              damage.amount = float.PositiveInfinity;
                              if (damage.victim.idMain.GetComponent<SpikeWall>() != null)
                              {
                                  damage.damageTypes = DamageTypeFlags.damage_generic;
                              }
                          }
                      }
                      if (damage.amount >= damage2.health)
                      {
                          if (Core.OwnershipDestroyReceiveResources)
                          {
                              string str10 = damage.victim.idMain.name.Replace("(Clone)", "");
                              if (Core.DestoryResources.ContainsKey(str10))
                              {
                                  foreach (string str11 in Core.DestoryResources[str10].Split(new char[] { ',' }))
                                  {
                                      string[] strArray2 = Facepunch.Utility.String.SplitQuotesStrings(str11);
                                      if (strArray2.Length < 2)
                                      {
                                          strArray2 = new string[] { "1", strArray2[0] };
                                      }
                                      ItemDataBlock byName = DatablockDictionary.GetByName(strArray2[1]);
                                      if (byName != null)
                                      {
                                          string name = byName.name;
                                          int result = 1;
                                          if (!int.TryParse(strArray2[0], out result))
                                          {
                                              result = 1;
                                          }
                                          if (result > 0)
                                          {
                                              name = result + " " + name;
                                          }
                                          Helper.GiveItem(killer, byName, result, -1);
                                          Config.Get("NAMES." + ((data2 == null) ? Core.Languages[0] : data2.Language), victimName, ref victimName, true);
                                          Broadcast.Message(killer.netUser, Config.GetMessage("Command.Destroy.ResourceReceived", killer.netUser, null).Replace("%ITEMNAME%", name).Replace("%OBJECT%", victimName), null, 0f);
                                      }
                                      else
                                      {
                                          Helper.Log(string.Format("Resource {0} not exist for receive after destroy {1}.", strArray2[1], victimName), false);
                                      }
                                  }
                              }
                              else
                              {
                                  Helper.Log("Resources not found for object '" + victimName + "' to receive for player.", false);
                              }
                          }
                          Helper.Log(Config.GetMessageObject("PlayerOwnership.Logger.Destroyed", victimName, killer, newValue, owner), false);
                      }
                  }
                  goto Label_1A09;
              }
              if (damage.victim.client == null)
              {
                  victimName = Helper.NiceName(damage.victim.character.name);
                  if ((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && (((damage.amount >= damage2.health) && (data2 != null)) && (data2.Clan != null)))
                  {
                      float num6 = 0f;
                      if (victimName.Equals("Chicken", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (1f * Clans.ExperienceMultiplier));
                      }
                      else if (victimName.Equals("Rabbit", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (1f * Clans.ExperienceMultiplier));
                      }
                      else if (victimName.Equals("Boar", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (3f * Clans.ExperienceMultiplier));
                      }
                      else if (victimName.Equals("Stag", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (5f * Clans.ExperienceMultiplier));
                      }
                      else if (victimName.Equals("Wolf", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (10f * Clans.ExperienceMultiplier));
                      }
                      else if (victimName.Equals("Bear", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (20f * Clans.ExperienceMultiplier));
                      }
                      else if (victimName.Equals("Mutant Wolf", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (15f * Clans.ExperienceMultiplier));
                      }
                      else if (victimName.Equals("Mutant Bear", StringComparison.OrdinalIgnoreCase))
                      {
                          num6 = Math.Abs((float) (30f * Clans.ExperienceMultiplier));
                      }
                      else
                      {
                          ConsoleSystem.LogWarning("[WARNING] Creature '" + victimName + "' not have experience for death.");
                      }
                      Config.Get("NAMES." + ((data2 == null) ? Core.Languages[0] : data2.Language), victimName, ref victimName, true);
                      if (num6 < 0f)
                      {
                          num6 = 0f;
                      }
                      else if (num6 >= 1f)
                      {
                          data2.Clan.Experience += (ulong) num6;
                          if (data2.Clan.Members[data2].Has<ClanMemberFlags>(ClanMemberFlags.expdetails))
                          {
                              Broadcast.Message(killer.netPlayer, Config.GetMessage("Clan.Experience.Murder", killer.netUser, null).Replace("%EXPERIENCE%", num6.ToString("N0")).Replace("%VICTIM%", victimName), null, 0f);
                          }
                      }
                  }
                  goto Label_1A09;
              }
              if ((data3 != null) && (data3.Level.BonusMembersDefense > 0))
              {
                  damage.amount -= (damage.amount * data3.Level.BonusMembersDefense) / 100f;
              }
              if (match == null)
              {
                  match = new Predicate<EventTimer>(class2.method_0);
              }
              EventTimer item = Events.Timer.Find(match);
              if (item != null)
              {
                  Broadcast.Notice(class2.playerClient_0.netUser, "☢", Config.GetMessageCommand("Command.Home.Interrupt", "", killer.netUser), 5f);
                  item.Dispose();
                  Events.Timer.Remove(item);
              }
              if (predicate2 == null)
              {
                  predicate2 = new Predicate<EventTimer>(class2.method_1);
              }
              EventTimer timer2 = Events.Timer.Find(predicate2);
              if (timer2 != null)
              {
                  Broadcast.Notice(class2.playerClient_0.netUser, "☢", Config.GetMessageCommand("Command.Clan.Warp.Interrupt", "", killer.netUser), 5f);
                  timer2.Dispose();
                  Events.Timer.Remove(timer2);
              }
              if (predicate3 == null)
              {
                  predicate3 = new Predicate<EventTimer>(class2.method_2);
              }
              EventTimer timer3 = Events.Timer.Find(predicate3);
              if (timer3 != null)
              {
                  if (timer3.Sender != null)
                  {
                      Broadcast.Notice(timer3.Sender.networkPlayer, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", killer.netUser), 5f);
                  }
                  if (timer3.Target != null)
                  {
                      Broadcast.Notice(timer3.Target.networkPlayer, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", killer.netUser), 5f);
                  }
                  timer3.Dispose();
                  Events.Timer.Remove(timer3);
              }
              if ((!server.pvp && (class2.playerClient_0 != null)) && ((killer != null) && (class2.playerClient_0 != killer)))
              {
                  return false;
              }
              if (damage.amount < damage2.health)
              {
                  goto Label_1A09;
              }
              if (owner != null)
              {
                  foreach (string str13 in Users.PersonalList(owner.SteamID).Keys.ToList<string>())
                  {
                      if (Users.PersonalList(owner.SteamID)[str13] == 0)
                      {
                          Users.PersonalRemove(owner.SteamID, str13);
                      }
                      else
                      {
                          Dictionary<string, int> dictionary;
                          string str17;
                          (dictionary = Users.PersonalList(owner.SteamID))[str17 = str13] = dictionary[str17] - 1;
                          Users.SQL_UpdatePersonal(owner.SteamID);
                      }
                  }
              }
              string text = "";
              bool announceDeathNPC = false;
              if (killer == null)
              {
                  announceDeathNPC = Core.AnnounceDeathNPC;
                  text = Config.GetMessageDeath("PlayerDeath.NPC", class2.playerClient_0.netUser, key, null);
              }
              else if ((class2.playerClient_0 != killer) && !flag2)
              {
                  if (class2.playerClient_0 != killer)
                  {
                      announceDeathNPC = Core.AnnounceDeathMurder;
                      text = Config.GetMessageDeath("PlayerDeath.Murder", class2.playerClient_0.netUser, key, null);
                      if (((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && ((!flag && (owner != null)) && ((data2 != null) && (data4 != null)))) && (killer != null))
                      {
                          float num5 = 0f;
                          if (owner != null)
                          {
                              if (data3 == null)
                              {
                                  num5 = Math.Abs((float) (50f * Clans.ExperienceMultiplier));
                              }
                              else if (data4.Hostile.ContainsKey(data3.ID))
                              {
                                  num5 = Math.Abs((float) (250f * Clans.ExperienceMultiplier));
                                  if (Clans.ClanWarDeathPay)
                                  {
                                      data4.Balance += (data3.Balance * Clans.ClanWarDeathPercent) / ((ulong) 100L);
                                  }
                                  if (Clans.ClanWarMurderFee)
                                  {
                                      data3.Balance -= (data3.Balance * Clans.ClanWarMurderPercent) / ((ulong) 100L);
                                  }
                              }
                              else if (data3 != data4)
                              {
                                  num5 = Math.Abs((float) (100f * Clans.ExperienceMultiplier));
                              }
                          }
                          if (num5 < 0f)
                          {
                              num5 = 0f;
                          }
                          else if (num5 >= 1f)
                          {
                              data4.Experience += (ulong) num5;
                              if (data4.Members[data2].Has<ClanMemberFlags>(ClanMemberFlags.expdetails))
                              {
                                  Broadcast.Message(killer.netPlayer, Config.GetMessage("Clan.Experience.Murder", killer.netUser, null).Replace("%EXPERIENCE%", num5.ToString("N0")).Replace("%VICTIM%", owner.Username), null, 0f);
                              }
                          }
                      }
                  }
              }
              else
              {
                  announceDeathNPC = Core.AnnounceDeathSelf;
                  text = Config.GetMessageDeath("PlayerDeath.Suicide", class2.playerClient_0.netUser, null, null);
              }
              switch (damage.damageTypes)
              {
                  case DamageTypeFlags.damage_generic:
                      text = text.Replace("%WEAPON%", "Melee");
                      break;

                  case DamageTypeFlags.damage_bullet:
                      if (extraData != null)
                      {
                          text = text.Replace("%WEAPON%", newValue);
                      }
                      break;

                  case DamageTypeFlags.damage_melee:
                      if (extraData == null)
                      {
                          newValue = "Hunting Bow";
                      }
                      text = text.Replace("%WEAPON%", newValue);
                      break;

                  case DamageTypeFlags.damage_explosion:
                      if (damage.attacker.id.name.StartsWith("F1Grenade"))
                      {
                          newValue = "F1 Grenade";
                      }
                      if (damage.attacker.id.name.StartsWith("ExplosiveCharge"))
                      {
                          newValue = "Explosive Charge";
                      }
                      text = text.Replace("%WEAPON%", newValue);
                      break;

                  case 0:
                      if ((damage3 != null) && damage3.IsBleeding())
                      {
                          text = Config.GetMessageDeath("PlayerDeath.Bleeding", class2.playerClient_0.netUser, null, null);
                      }
                      else if ((metabolism != null) && metabolism.HasRadiationPoisoning())
                      {
                          text = Config.GetMessageDeath("PlayerDeath.Radiation", class2.playerClient_0.netUser, null, null);
                      }
                      else if ((metabolism != null) && metabolism.IsPoisoned())
                      {
                          text = Config.GetMessageDeath("PlayerDeath.Poison", class2.playerClient_0.netUser, null, null);
                      }
                      else if ((metabolism != null) && (metabolism.GetCalorieLevel() <= 0f))
                      {
                          text = Config.GetMessageDeath("PlayerDeath.Hunger", class2.playerClient_0.netUser, null, null);
                      }
                      else if ((metabolism != null) && metabolism.IsCold())
                      {
                          text = Config.GetMessageDeath("PlayerDeath.Cold", class2.playerClient_0.netUser, null, null);
                      }
                      else
                      {
                          text = Config.GetMessageDeath("PlayerDeath.Bleeding", class2.playerClient_0.netUser, null, null);
                      }
                      goto Label_176F;
              }
              string niceName = damage.bodyPart.GetNiceName();
              Config.Get("BODYPART." + Core.Languages[0], niceName, ref niceName, true);
              text = text.Replace("%BODYPART%", niceName).Replace("%DISTANCE%", num3.ToString("N1")).Replace("%DAMAGE%", damage.amount.ToString("0.0"));
          Label_176F:
              if (announceDeathNPC)
              {
                  Broadcast.MessageAll(text);
              }
              Helper.LogChat(text, false);
          Label_1A09:
              if (((damage.damageTypes != 0) && (damage.amount != 0f)) && !Core.OverrideDamage)
              {
                  string str16 = "?";
                  if (!float.IsInfinity(damage.amount) || (damage.attacker.id != damage.victim.id))
                  {
                      if (damage.attacker.id is SpikeWall)
                      {
                          str16 = (damage.attacker.id as SpikeWall).baseReturnDmg.ToString();
                          Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, " owned ", damage.attacker.client, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                      }
                      else if (damage.attacker.id is TimedGrenade)
                      {
                          str16 = (damage.attacker.id as TimedGrenade).damage.ToString();
                          Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, " owned ", damage.attacker.client, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                      }
                      else if (damage.attacker.id is TimedExplosive)
                      {
                          str16 = (damage.attacker.id as TimedExplosive).damage.ToString();
                          Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, " owned ", damage.attacker.client, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                      }
                      else if ((damage.attacker.client != null) && (extraData != null))
                      {
                          str16 = extraData.dataBlock.damageMin + "-" + extraData.dataBlock.damageMax;
                          Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, "[", damage.attacker.networkViewID, "] from ", extraData.dataBlock.name, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                      }
                      else if ((damage.attacker.client != null) && (extraData == null))
                      {
                          str16 = "75";
                          Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, "[", damage.attacker.networkViewID, "] from Hunting Bow hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                      }
                  }
              }
              if ((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && ((Economy.Enabled && !flag) && (damage.amount >= damage2.health)))
              {
                  Economy.HurtKilled(damage);
              }
              return true;
          }*/
        private sealed class Class53
        {
            // Fields
            public PlayerClient playerClient_0;

            // Methods
            internal bool method_0(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.playerClient_0.netUser) && (eventTimer_0.Command == "home"));
            }

            internal bool method_1(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.playerClient_0.netUser) && (eventTimer_0.Command == "clan"));
            }

            internal bool method_2(EventTimer eventTimer_0)
            {
                if ((eventTimer_0.Sender != this.playerClient_0.netUser) && (eventTimer_0.Target != this.playerClient_0.netUser))
                {
                    return false;
                }
                return (eventTimer_0.Command == "tp");
            }
        }

        public static bool TakeDamage_HurtShared(TakeDamage take, ref DamageEvent damage, TakeDamage.Quantity quantity)
        {
            int num4;
            Class53 class2 = new Class53();
            object[] args = Main.Array(2);
            args[0] = take;
            args[1] = (DamageEvent)damage;
            object obj2 = Main.Call("OnProcessDamageEvent", args);
            if (obj2 is DamageEvent)
            {
                damage = (DamageEvent)obj2;
            }
            if (take.dead)
            {
                return true;
            }
            bool flag = damage.victim.idMain is Character;
            bool flag2 = !flag;
            bool flag3 = !(damage.attacker.idMain is Character);
            TakeDamage damage2 = flag2 ? damage.victim.idMain.GetLocal<TakeDamage>() : ((Character)damage.victim.idMain).takeDamage;
            if (!flag3)
            {
                TakeDamage takeDamage = ((Character)damage.attacker.idMain).takeDamage;
            }
            else
            {
                damage.attacker.idMain.GetLocal<TakeDamage>();
            }
            WorldZone zone = Zones.Get(damage.victim.idMain.transform.position);
            WorldZone zone2 = Zones.Get(damage.attacker.idMain.transform.position);
            WeaponImpact extraData = damage.extraData as WeaponImpact;
         //   if ((damage.attacker.client != null) && (damage.victim.idMain != damage.attacker.idMain))
           // {
                if (float.IsNaN(damage.amount))
                {
                    Helper.LogWarning("[WARNING] Received damage with 'NaN' value from '" + damage.attacker.client.userName + "', maybe use cheats.", true);
                    TakeDamage.KillSelf(damage.attacker.client.controllable.character, null);
                    damage.attacker.client.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    return false;
                }
                if (damage.amount < 0f)
                {
                    Helper.LogWarning("[WARNING] Received damage with value less zero from '" + damage.attacker.client.userName + "', maybe use cheats.", true);
                    TakeDamage.KillSelf(damage.attacker.client.controllable.character, null);
                    damage.attacker.client.netUser.Kick(NetError.Facepunch_Kick_Violation, true);
                    return false;
                }
                if ((damage.amount > 10000f) && !float.IsInfinity(damage.amount))
                {
                   // Helper.LogWarning(string.Concat(new object[] { "[WARNING] Player '", damage.attacker.client.userName, "' damage '", damage.victim.client.userName, "' on ", damage.amount, " hits by using radiation exploit." }), true);
                    return false;
                }
            //}
            if ((flag2 & flag3) && (damage.victim.idMain == damage.attacker.idMain))
            {
                if (damage.amount != float.MaxValue)
                {
                    if (!Core.DecayObjects)
                    {
                        damage.amount = 0f;
                        if (server.log > 2)
                        {
                            UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because DecayObjects is disabled in config.");
                        }
                    }
                    DeployableObject obj3 = damage2.GetComponent<DeployableObject>();
                    if ((obj3 != null) && Users.HasFlag(obj3.ownerID, UserFlags.admin))
                    {
                        damage.amount = 0f;
                        if (server.log > 2)
                        {
                            UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because owner is administrator.");
                        }
                    }
                    StructureComponent component = damage2.GetComponent<StructureComponent>();
                    if ((component != null) && Users.HasFlag(component._master.ownerID, UserFlags.admin))
                    {
                        damage.amount = 0f;
                        if (server.log > 2)
                        {
                            UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because owner is administrator.");
                        }
                    }
                    if ((zone != null) && zone.Flags.Has<ZoneFlags>(ZoneFlags.nodecay))
                    {
                        damage.amount = 0f;
                        if (server.log > 2)
                        {
                            UnityEngine.Debug.Log("Object: " + Helper.NiceName(damage2.name) + " without a decay, because zone have NoDecay flag.");
                        }
                    }
                }
                return true;
            }
            class2.playerClient_0 = damage.victim.client;
            PlayerClient killer = damage.attacker.client;
            Metabolism metabolism = damage.victim.id.GetComponent<Metabolism>();
            damage.attacker.id.GetComponent<Metabolism>();
            HumanBodyTakeDamage damage3 = damage.victim.id.GetComponent<HumanBodyTakeDamage>();
            damage.attacker.id.GetComponent<HumanBodyTakeDamage>();
            string victimName = "";
            ulong userID = 0L;
            if (damage.victim.client != null)
            {
                userID = damage.victim.client.userID;
            }
            if (damage.victim.idMain is DeployableObject)
            {
                userID = (damage.victim.idMain as DeployableObject).ownerID;
            }
            if (damage.victim.idMain is StructureComponent)
            {
                userID = (damage.victim.idMain as StructureComponent)._master.ownerID;
            }
            string key = "";
            ulong ownerID = 0L;
            if (damage.attacker.client != null)
            {
                ownerID = damage.attacker.client.userID;
            }
            if (damage.attacker.idMain is DeployableObject)
            {
                ownerID = (damage.attacker.idMain as DeployableObject).ownerID;
            }
            if (damage.attacker.idMain is StructureComponent)
            {
                ownerID = (damage.attacker.idMain as StructureComponent)._master.ownerID;
            }
            string newValue = "";
            if (extraData != null)
            {
                newValue = extraData.dataBlock.name;
            }
            float num3 = Vector3.Distance(damage.attacker.id.transform.position, damage.victim.id.transform.position);
            UserData owner = (userID != 0) ? Users.GetBySteamID(userID) : null;
            UserData data2 = (ownerID != 0) ? Users.GetBySteamID(ownerID) : null;
            ClanData data3 = (owner != null) ? owner.Clan : null;
            ClanData data4 = (data2 != null) ? data2.Clan : null;
            if (((damage.victim.client != null) && !flag2) && ((owner != null) && owner.HasFlag(UserFlags.godmode)))
            {
                if ((damage3 != null) && (damage3._bleedingLevel > 0f))
                {
                    damage3.Bandage(1000f);
                }
                damage.amount = 0f;
                return false;
            }
            if (((killer != null) && (owner != null)) && ((data2 != null) && (owner != data2)))
            {
                if (!flag3 || !(damage.attacker.idMain is DeployableObject))
                {
                    if ((((data4 != null) && (data3 != null)) && ((data4 == data3) && owner.Clan.Flags.Has<ClanFlags>(ClanFlags.can_ffire))) && (owner.Clan.FrendlyFire && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))))
                    {
                        string str4 = Config.GetMessage(flag2 ? "Player.NoDamage.ClanMemberOwned" : "Player.NoDamage.ClanMember", killer.netUser, null);
                        Broadcast.Notice(killer.netUser, "☢", str4.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                        damage.amount = 0f;
                        return false;
                    }
                    if (data2.HasFlag(UserFlags.nopvp) || owner.HasFlag(UserFlags.nopvp))
                    {
                        string str5 = Config.GetMessage(flag2 ? "Player.NoDamage.WithoutPvPOwned" : "Player.NoDamage.WithoutPvP", killer.netUser, null);
                        Broadcast.Notice(killer.netUser, "☢", str5.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                        damage.amount = 0f;
                        return false;
                    }
                    if ((data2.Zone != null) && (data2.Zone.NoPvP || data2.Zone.Safe))
                    {
                        string str6 = Config.GetMessage(flag2 ? "Player.NoDamage.ZoneWithoutPvPOwned" : "Player.NoDamage.ZoneWithoutPvP", killer.netUser, null);
                        Broadcast.Notice(killer.netUser, "☢", str6.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                        damage.amount = 0f;
                        return false;
                    }
                }
                WorldZone zone3 = flag2 ? Zones.Get(damage.victim.idMain.transform.position) : owner.Zone;
                if ((zone3 != null) && (zone3.NoPvP || zone3.Safe))
                {
                    string str7 = Config.GetMessage(flag2 ? "Player.NoDamage.ZoneWithSafetyOwned" : "Player.NoDamage.ZoneWithSafety", killer.netUser, null);
                    Broadcast.Notice(killer.netUser, "☢", str7.Replace("%KILLER%", data2.Username).Replace("%VICTIM%", owner.Username), 5f);
                    damage.amount = 0f;
                    return false;
                }
            }
            if (((damage.attacker.client != null) && damage.attacker.client.netUser.admin) && Core.AdminInstantDestory)
            {
                if (extraData != null)
                {
                    extraData.item.SetCondition(extraData.item.maxcondition);
                }
                damage.amount = float.PositiveInfinity;
            }
            else if (!Override.DamageOverride(take, ref damage, ref quantity))
            {
                damage.amount = 0f;
                return false;
            }
            if (flag3)
            {
                key = Helper.NiceName(damage.attacker.idMain.name);
            }
            else if (damage.attacker.client == null)
            {
                key = Helper.NiceName(damage.attacker.character.name);
                Config.Get("NAMES." + ((owner == null) ? Core.Languages[0] : owner.Language), key, ref key, true);
            }
            else
            {
                key = damage.attacker.client.userName;
                if ((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && (((damage.amount > 0f) && (data4 != null)) && (data4.Level.BonusMembersDamage > 0)))
                {
                    damage.amount += (damage.amount * data4.Level.BonusMembersDamage) / 100f;
                }
                if ((server.pvp && !flag2) && ((damage.amount >= damage2.health) && (damage.victim.id != damage.attacker.id)))
                {
                    string str8 = "";
                    if (damage.victim.client != null)
                    {
                        victimName = damage.victim.client.userName;
                        str8 = Config.GetMessageMurder("PlayerNotice.Murder", killer.netUser, victimName, null);
                        if (str8.Equals("PlayerNotice.Murder", StringComparison.CurrentCultureIgnoreCase))
                        {
                            str8 = "";
                        }
                    }
                    else
                    {
                        victimName = Helper.NiceName(damage.victim.character.name);
                        Config.Get("NAMES." + ((data2 == null) ? Core.Languages[0] : data2.Language), victimName, ref victimName, true);
                        str8 = Config.GetMessageMurder("PlayerNotice.NPC", killer.netUser, victimName, null);
                        if (str8.Equals("PlayerNotice.NPC", StringComparison.CurrentCultureIgnoreCase))
                        {
                            str8 = "";
                        }
                    }
                    if (Core.AnnounceKillNotice && (str8 != ""))
                    {
                        if (newValue != "")
                        {
                            str8 = str8.Replace("%WEAPON%", newValue);
                        }
                        switch (damage.damageTypes)
                        {
                            case DamageTypeFlags.damage_generic:
                                str8 = str8.Replace("%WEAPON%", "Melee");
                                break;

                            case DamageTypeFlags.damage_bullet:
                                if (extraData != null)
                                {
                                    str8 = str8.Replace("%WEAPON%", newValue);
                                }
                                break;

                            case DamageTypeFlags.damage_melee:
                                if (extraData == null)
                                {
                                    newValue = "Hunting Bow";
                                }
                                str8 = str8.Replace("%WEAPON%", newValue);
                                break;

                            case DamageTypeFlags.damage_explosion:
                                if (damage.attacker.id.name.StartsWith("F1Grenade"))
                                {
                                    newValue = "F1 Grenade";
                                }
                                if (damage.attacker.id.name.StartsWith("ExplosiveCharge"))
                                {
                                    newValue = "Explosive Charge";
                                }
                                str8 = str8.Replace("%WEAPON%", newValue);
                                break;
                        }
                        string str9 = damage.bodyPart.GetNiceName();
                        Config.Get("BODYPART." + ((data2 == null) ? Core.Languages[0] : data2.Language), str9, ref str9, true);
                        str8 = str8.Replace("%BODYPART%", str9).Replace("%DISTANCE%", num3.ToString("N1")).Replace("%DAMAGE%", damage.amount.ToString("0.0"));
                        Broadcast.Notice(killer.netUser, "☠", str8, 2.5f);
                    }
                }
            }
            if (flag2)
            {
                victimName = Helper.NiceName(damage.victim.idMain.name);
                if (((killer != null) && killer.netUser.admin) && (damage.amount >= damage2.health))
                {
                    Helper.Log(Config.GetMessageObject("PlayerOwnership.Logger.Destroyed", victimName, killer, newValue, owner), false);
                    return true;
                }
                if ((((ownerID != userID) && (owner != null)) && ((data2 != null) && owner.HasFlag(UserFlags.admin))) && !data2.HasFlag(UserFlags.admin))
                {
                    damage.amount = 0f;
                    return false;
                }
                if ((userID != ownerID) && Core.OwnershipAttackedAnnounce)
                {
                    NetUser player = NetUser.FindByUserID(userID);
                    Config.Get("NAMES." + ((owner == null) ? Core.Languages[0] : owner.Language), victimName, ref victimName, true);
                    if (Core.OwnershipAttackedPremiumOnly && !Users.HasFlag(userID, UserFlags.premium))
                    {
                        player = null;
                    }
                    if ((player != null) && (damage.amount != 0f))
                    {
                        if (damage.amount >= damage2.health)
                        {
                            Broadcast.Message(player, Config.GetMessageObject("PlayerOwnership.Object.Destroyed", victimName, killer, newValue, owner), null, 1f);
                        }
                        else
                        {
                            Broadcast.Message(player, Config.GetMessageObject("PlayerOwnership.Object.Attacked", victimName, killer, newValue, owner), null, 1f);
                        }
                    }
                }
                else if (((userID == ownerID) || Users.SharedGet(userID, ownerID)) && (Core.OwnershipDestroy || Core.DestoryOwnership.ContainsKey(killer.userID)))
                {
                    StructureComponent comp = damage.victim.idMain.GetComponent<StructureComponent>();
                    if ((!Core.OwnershipDestroyNoCarryWeight || (comp == null)) || !comp._master.ComponentCarryingWeight(comp))
                    {
                        if ((damage.amount == 0f) && (damage.attacker.id is TimedGrenade))
                        {
                            damage.amount = (damage.attacker.id as TimedGrenade).damage;
                        }
                        if ((damage.amount == 0f) && (damage.attacker.id is TimedExplosive))
                        {
                            damage.amount = (damage.attacker.id as TimedExplosive).damage;
                        }
                        if ((damage.amount == 0f) && (extraData != null))
                        {
                            damage.amount = UnityEngine.Random.Range(extraData.dataBlock.damageMin, extraData.dataBlock.damageMax);
                        }
                        if ((damage.amount == 0f) && (extraData == null))
                        {
                            damage.amount = UnityEngine.Random.Range(0x4b, 0x4b);
                        }
                        if (Core.OwnershipDestroyInstant)
                        {
                            damage.amount = float.PositiveInfinity;
                            if (damage.victim.idMain.GetComponent<SpikeWall>() != null)
                            {
                                damage.damageTypes = DamageTypeFlags.damage_generic;
                            }
                        }
                    }
                    if (damage.amount >= damage2.health)
                    {
                        if (Core.OwnershipDestroyReceiveResources)
                        {
                            string str10 = damage.victim.idMain.name.Replace("(Clone)", "");
                            if (Core.DestoryResources.ContainsKey(str10))
                            {
                                char[] separator = new char[] { ',' };
                                string[] strArray = Core.DestoryResources[str10].Split(separator);
                                num4 = 0;
                                while (num4 < strArray.Length)
                                {
                                    string[] strArray2 = Facepunch.Utility.String.SplitQuotesStrings(strArray[num4]);
                                    if (strArray2.Length < 2)
                                    {
                                        strArray2 = new string[] { "1", strArray2[0] };
                                    }
                                    ItemDataBlock byName = DatablockDictionary.GetByName(strArray2[1]);
                                    if (byName != null)
                                    {
                                        string name = byName.name;
                                        int result = 1;
                                        if (!int.TryParse(strArray2[0], out result))
                                        {
                                            result = 1;
                                        }
                                        if (result > 0)
                                        {
                                            name = result + " " + name;
                                        }
                                        Helper.GiveItem(killer, byName, result, -1);
                                        Config.Get("NAMES." + ((data2 == null) ? Core.Languages[0] : data2.Language), victimName, ref victimName, true);
                                        Broadcast.Message(killer.netUser, Config.GetMessage("Command.Destroy.ResourceReceived", killer.netUser, null).Replace("%ITEMNAME%", name).Replace("%OBJECT%", victimName), null, 0f);
                                    }
                                    else
                                    {
                                        Helper.Log(string.Format("Resource {0} not exist for receive after destroy {1}.", strArray2[1], victimName), false);
                                    }
                                    num4++;
                                }
                            }
                            else
                            {
                                Helper.Log("Resources not found for object '" + victimName + "' to receive for player.", false);
                            }
                        }
                        Helper.Log(Config.GetMessageObject("PlayerOwnership.Logger.Destroyed", victimName, killer, newValue, owner), false);
                    }
                }
                goto Label_1BCF;
            }
            if (damage.victim.client == null)
            {
                victimName = Helper.NiceName(damage.victim.character.name);
                if ((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && (((damage.amount >= damage2.health) && (data2 != null)) && (data2.Clan != null)))
                {
                    float num7 = 0f;
                    if (victimName.Equals("Chicken", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(1f * Clans.ExperienceMultiplier));
                    }
                    else if (victimName.Equals("Rabbit", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(1f * Clans.ExperienceMultiplier));
                    }
                    else if (victimName.Equals("Boar", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(3f * Clans.ExperienceMultiplier));
                    }
                    else if (victimName.Equals("Stag", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(5f * Clans.ExperienceMultiplier));
                    }
                    else if (victimName.Equals("Wolf", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(10f * Clans.ExperienceMultiplier));
                    }
                    else if (victimName.Equals("Bear", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(20f * Clans.ExperienceMultiplier));
                    }
                    else if (victimName.Equals("Mutant Wolf", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(15f * Clans.ExperienceMultiplier));
                    }
                    else if (victimName.Equals("Mutant Bear", StringComparison.OrdinalIgnoreCase))
                    {
                        num7 = Math.Abs((float)(30f * Clans.ExperienceMultiplier));
                    }
                    else
                    {
                        ConsoleSystem.LogWarning("[WARNING] Creature '" + victimName + "' not have experience for death.");
                    }
                    Config.Get("NAMES." + ((data2 == null) ? Core.Languages[0] : data2.Language), victimName, ref victimName, true);
                    if (num7 < 0f)
                    {
                        num7 = 0f;
                    }
                    else if (num7 >= 1f)
                    {
                        data2.Clan.Experience += (ulong)num7;
                        if (data2.Clan.Members[data2].Has<ClanMemberFlags>(ClanMemberFlags.expdetails))
                        {
                            Broadcast.Message(killer.netPlayer, Config.GetMessage("Clan.Experience.Murder", killer.netUser, null).Replace("%EXPERIENCE%", num7.ToString("N0")).Replace("%VICTIM%", victimName), null, 0f);
                        }
                    }
                }
                goto Label_1BCF;
            }
            if ((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && (((damage.amount > 0f) && (data3 != null)) && (data3.Level.BonusMembersDefense > 0)))
            {
                damage.amount -= (damage.amount * data3.Level.BonusMembersDefense) / 100f;
            }
            EventTimer item = Events.Timer.Find(new Predicate<EventTimer>(class2.method_0));
            if (item != null)
            {
                Broadcast.Notice(class2.playerClient_0.netUser, "☢", Config.GetMessageCommand("Command.Home.Interrupt", "", killer.netUser), 5f);
                item.Dispose();
                Events.Timer.Remove(item);
            }
            EventTimer timer2 = Events.Timer.Find(new Predicate<EventTimer>(class2.method_1));
            if (timer2 != null)
            {
                Broadcast.Notice(class2.playerClient_0.netUser, "☢", Config.GetMessageCommand("Command.Clan.Warp.Interrupt", "", killer.netUser), 5f);
                timer2.Dispose();
                Events.Timer.Remove(timer2);
            }
            EventTimer timer3 = Events.Timer.Find(new Predicate<EventTimer>(class2.method_2));
            if (timer3 != null)
            {
                if (timer3.Sender != null)
                {
                    Broadcast.Notice(timer3.Sender.networkPlayer, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", killer.netUser), 5f);
                }
                if (timer3.Target != null)
                {
                    Broadcast.Notice(timer3.Target.networkPlayer, "☢", Config.GetMessageCommand("Command.Teleport.Interrupt", "", killer.netUser), 5f);
                }
                timer3.Dispose();
                Events.Timer.Remove(timer3);
            }
            if ((!server.pvp && (class2.playerClient_0 != null)) && ((killer != null) && (class2.playerClient_0 != killer)))
            {
                return false;
            }
            if (damage.amount < damage2.health)
            {
                goto Label_1BCF;
            }
            if (owner != null)
            {
                foreach (string str13 in Users.PersonalList(owner.SteamID).Keys.ToList<string>())
                {
                    if (Users.PersonalList(owner.SteamID)[str13] == 0)
                    {
                        Users.PersonalRemove(owner.SteamID, str13);
                    }
                    else
                    {
                        string str14 = str13;
                        Dictionary<string, int> dictionary1 = Users.PersonalList(owner.SteamID);
                        num4 = dictionary1[str14];
                        dictionary1[str14] = num4 - 1;
                        Users.SQL_UpdatePersonal(owner.SteamID);
                    }
                }
            }
            string text = "";
            bool announceDeathNPC = false;
            if (killer == null)
            {
                announceDeathNPC = Core.AnnounceDeathNPC;
                text = Config.GetMessageDeath("PlayerDeath.NPC", class2.playerClient_0.netUser, key, null);
            }
            else if ((class2.playerClient_0 == killer) | flag3)
            {
                announceDeathNPC = Core.AnnounceDeathSelf;
                text = Config.GetMessageDeath("PlayerDeath.Suicide", class2.playerClient_0.netUser, null, null);
            }
            else if (class2.playerClient_0 != killer)
            {
                announceDeathNPC = Core.AnnounceDeathMurder;
                text = Config.GetMessageDeath("PlayerDeath.Murder", class2.playerClient_0.netUser, key, null);
                if (((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && ((!flag2 && (owner != null)) && ((data2 != null) && (data4 != null)))) && (killer != null))
                {
                    float num6 = 0f;
                    if (owner != null)
                    {
                        if (data3 == null)
                        {
                            num6 = Math.Abs((float)(50f * Clans.ExperienceMultiplier));
                        }
                        else if (data4.Hostile.ContainsKey(data3.ID))
                        {
                            num6 = Math.Abs((float)(250f * Clans.ExperienceMultiplier));
                            if (Clans.ClanWarDeathPay)
                            {
                                data4.Balance += (data3.Balance * Clans.ClanWarDeathPercent) / ((ulong)100L);
                            }
                            if (Clans.ClanWarMurderFee)
                            {
                                data3.Balance -= (data3.Balance * Clans.ClanWarMurderPercent) / ((ulong)100L);
                            }
                        }
                        else if (data3 != data4)
                        {
                            num6 = Math.Abs((float)(100f * Clans.ExperienceMultiplier));
                        }
                    }
                    if (num6 < 0f)
                    {
                        num6 = 0f;
                    }
                    else if (num6 >= 1f)
                    {
                        data4.Experience += (ulong)num6;
                        if (data4.Members[data2].Has<ClanMemberFlags>(ClanMemberFlags.expdetails))
                        {
                            Broadcast.Message(killer.netPlayer, Config.GetMessage("Clan.Experience.Murder", killer.netUser, null).Replace("%EXPERIENCE%", num6.ToString("N0")).Replace("%VICTIM%", owner.Username), null, 0f);
                        }
                    }
                }
            }
            switch (damage.damageTypes)
            {
                case DamageTypeFlags.damage_generic:
                    text = text.Replace("%WEAPON%", "Melee");
                    break;

                case DamageTypeFlags.damage_bullet:
                    if (extraData != null)
                    {
                        text = text.Replace("%WEAPON%", newValue);
                    }
                    break;

                case DamageTypeFlags.damage_melee:
                    if (extraData == null)
                    {
                        newValue = "Hunting Bow";
                    }
                    text = text.Replace("%WEAPON%", newValue);
                    break;

                case DamageTypeFlags.damage_explosion:
                    if (damage.attacker.id.name.StartsWith("F1Grenade"))
                    {
                        newValue = "F1 Grenade";
                    }
                    if (damage.attacker.id.name.StartsWith("ExplosiveCharge"))
                    {
                        newValue = "Explosive Charge";
                    }
                    text = text.Replace("%WEAPON%", newValue);
                    break;

                case 0:
                    if ((damage3 != null) && damage3.IsBleeding())
                    {
                        text = Config.GetMessageDeath("PlayerDeath.Bleeding", class2.playerClient_0.netUser, null, null);
                    }
                    else if ((metabolism != null) && metabolism.HasRadiationPoisoning())
                    {
                        text = Config.GetMessageDeath("PlayerDeath.Radiation", class2.playerClient_0.netUser, null, null);
                    }
                    else if ((metabolism != null) && metabolism.IsPoisoned())
                    {
                        text = Config.GetMessageDeath("PlayerDeath.Poison", class2.playerClient_0.netUser, null, null);
                    }
                    else if ((metabolism != null) && (metabolism.GetCalorieLevel() <= 0f))
                    {
                        text = Config.GetMessageDeath("PlayerDeath.Hunger", class2.playerClient_0.netUser, null, null);
                    }
                    else if ((metabolism != null) && metabolism.IsCold())
                    {
                        text = Config.GetMessageDeath("PlayerDeath.Cold", class2.playerClient_0.netUser, null, null);
                    }
                    else
                    {
                        text = Config.GetMessageDeath("PlayerDeath.Bleeding", class2.playerClient_0.netUser, null, null);
                    }
                    goto Label_1932;
            }
            string niceName = damage.bodyPart.GetNiceName();
            Config.Get("BODYPART." + Core.Languages[0], niceName, ref niceName, true);
            text = text.Replace("%BODYPART%", niceName).Replace("%DISTANCE%", num3.ToString("N1")).Replace("%DAMAGE%", damage.amount.ToString("0.0"));
            Label_1932:
            if (announceDeathNPC)
            {
                Broadcast.MessageAll(text);
            }
            Helper.LogChat(text, false);
            Label_1BCF:
            if (((damage.damageTypes != 0) && (damage.amount != 0f)) && !Core.OverrideDamage)
            {
                string str16 = "?";
                if (!float.IsInfinity(damage.amount) || (damage.attacker.id != damage.victim.id))
                {
                    if (damage.attacker.id is SpikeWall)
                    {
                        str16 = (damage.attacker.id as SpikeWall).baseReturnDmg.ToString();
                        Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, " owned ", damage.attacker.client, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                    }
                    else if (damage.attacker.id is TimedGrenade)
                    {
                        str16 = (damage.attacker.id as TimedGrenade).damage.ToString();
                        Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, " owned ", damage.attacker.client, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                    }
                    else if (damage.attacker.id is TimedExplosive)
                    {
                        str16 = (damage.attacker.id as TimedExplosive).damage.ToString();
                        Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, " owned ", damage.attacker.client, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                    }
                    else if ((damage.attacker.client != null) && (extraData != null))
                    {
                        str16 = extraData.dataBlock.damageMin + "-" + extraData.dataBlock.damageMax;
                        Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, "[", damage.attacker.networkViewID, "] from ", extraData.dataBlock.name, " hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                    }
                    else if ((damage.attacker.client != null) && (extraData == null))
                    {
                        str16 = "75";
                        Helper.Log(string.Concat(new object[] { "Damage: ", damage.attacker, "[", damage.attacker.networkViewID, "] from Hunting Bow hit ", damage.victim.idMain, "[", damage.victim.networkViewID, "] on ", damage.amount, "(", str16, ") pts." }), false);
                    }
                }
            }
            if ((((zone2 == null) || !zone2.Flags.Has<ZoneFlags>(ZoneFlags.events)) && ((zone == null) || !zone.Flags.Has<ZoneFlags>(ZoneFlags.events))) && ((Economy.Enabled && !flag2) && (damage.amount >= damage2.health)))
            {
                Economy.HurtKilled(damage);
            }
            return true;
        }



        public static bool TimedLockable_HasAccess(LockableObject obj, ulong userid)
        {
            TimedLockable lockable = obj as TimedLockable;
            NetUser user = NetUser.FindByUserID(userid);
            if ((user != null) && Users.Details(userid))
            {
                smethod_0(user, lockable.ownerID, Helper.NiceName(obj.name));
            }
            if (((user == null) || !user.admin) && obj.lockActive)
            {
                return (userid == lockable.ownerID);
            }
            return true;
        }

        public static TruthDetector.ActionTaken TruthDetector_NoteMoved(TruthDetector truthDetector, ref Vector3 pos, Angle2 ang, double time)
        {
            return TruthDetector.ActionTaken.None;
        }

        public static int uLink_DoNetworkRecv(System.Net.Sockets.Socket socket, ref byte[] buffer, int length, ref EndPoint ip)
        {
            uint[] numArray;
            int hashCode = ip.GetHashCode();
            RustExtended.Bootstrap.RecvPacketCounter++;
            if (Truth.RustProtect)
            {
                numArray = new uint[length / 4];
                for (int j = 0; j < numArray.Length; j++)
                {
                    numArray[j] = BitConverter.ToUInt32(buffer, j * 4);
                }
                if ((((length >= 0x20) && (numArray[0] == 0x23000007)) && ((numArray[1] == 0x7263530a) && (numArray[2] == 0x736e6565))) && ((numArray[3] == 0xff746f68) || (numArray[3] == 0xfe746f68)))
                {
                    SnapshotData data;
                    ulong num3 = BitConverter.ToUInt64(buffer, 0x10);
                    uint bufferSize = BitConverter.ToUInt32(buffer, 0x18);
                    uint num5 = BitConverter.ToUInt32(buffer, 0x1c);
                    int count = length - 0x20;
                    UserData bySteamID = Users.GetBySteamID(num3);
                    if (buffer[15] == 0xfe)
                    {
                        data = new SnapshotData(bufferSize);
                        if (Truth.SnapshotsData.ContainsKey(num3))
                        {
                            Truth.SnapshotsData[num3] = data;
                        }
                        else
                        {
                            Truth.SnapshotsData.Add(num3, data);
                        }
                        if (Core.Debug && (server.log >= 1))
                        {
                            Helper.Log(string.Concat(new object[] { "RustProtect: Starting receiving snapshot from [", bySteamID.Username, ":", bySteamID.SteamID, ":", ip, "] size: ", data.Length }), true);
                        }
                        byte_0[15] = 0;
                        socket.SendTo(byte_0, 0, byte_0.Length, SocketFlags.None, ip);
                    }
                    else if ((buffer[15] == 0xff) && Truth.SnapshotsData.ContainsKey(num3))
                    {
                        if (count > 0)
                        {
                            data = Truth.SnapshotsData[num3];
                            if (Core.Debug && (server.log >= 1))
                            {
                                Helper.Log(string.Concat(new object[] { "RustProtect: Receiving snapshot part from [", bySteamID.Username, ":", bySteamID.SteamID, ":", ip, "] part size: ", count }), true);
                            }
                            data.Append(buffer, 0x20, count);
                            byte_0[15] = 1;
                            socket.SendTo(byte_0, 0, byte_0.Length, SocketFlags.None, ip);
                        }
                        else
                        {
                            data = Truth.SnapshotsData[num3];
                            if (((data != null) && (data.Length > 0)) && (num5 == data.Hashsum))
                            {
                                if (Core.Debug && (server.log >= 1))
                                {
                                    Helper.Log(string.Concat(new object[] { "RustProtect: Snapshot success received from [", bySteamID.Username, ":", bySteamID.SteamID, ":", ip, "] size: ", data.Length, ", CRC: ", data.Hashsum.ToString("X8") }), true);
                                }
                                string str = Path.Combine(Truth.RustProtectSnapshotsPath, num3.ToString());
                                string path = Path.Combine(str, DateTime.Now.ToString("yyyy-MM-dd.HH-mm-ss") + ".jpg");
                                Directory.CreateDirectory(str);
                                System.IO.File.WriteAllBytes(path, data.Buffer);
                                bySteamID.ProtectLastSnapshotTime = Time.time;
                                if (func_0 == null)
                                {
                                    func_0 = new Func<FileInfo, DateTime>(RustHook.smethod_6);
                                }
                                FileInfo[] array = Enumerable.OrderBy<FileInfo, DateTime>(new DirectoryInfo(str).GetFiles(), func_0).ToArray<FileInfo>();
                                if (array.Length > 0)
                                {
                                    int num7 = array.Length - ((int) Truth.RustProtectSnapshotsMaxCount);
                                    if (num7 > 0)
                                    {
                                        for (int k = 0; k < num7; k++)
                                        {
                                            System.IO.File.Delete(array[k].FullName);
                                            array.RemoveAt<FileInfo>(k);
                                        }
                                    }
                                }
                            }
                            Truth.SnapshotsData.Remove(num3);
                        }
                    }
                    return 0;
                }
                if ((Truth.RustProtectSnapshots && (Truth.RustProtectSnapshotsInterval > 0)) && (dictionary_1.ContainsKey(hashCode) && (dictionary_1[hashCode].UserData != null)))
                {
                    UserData userData = dictionary_1[hashCode].UserData;
                    float num9 = userData.ProtectLastSnapshotTime + Truth.RustProtectSnapshotsInterval;
                    if (!Truth.SnapshotsData.ContainsKey(userData.SteamID) && (Time.time > num9))
                    {
                        userData.ProtectLastSnapshotTime = Time.time;
                        byte_0[15] = 0xff;
                        socket.SendTo(byte_0, 0, byte_0.Length, SocketFlags.None, ip);
                        if (Core.Debug && (server.log >= 1))
                        {
                            Helper.Log(string.Concat(new object[] { "RustProtect: Request snapshot from client [", userData.Username, ":", userData.SteamID, ":", ip, "]." }), true);
                        }
                    }
                    /*
                    float num4 = Time.time - userData.ProtectLastSnapshotTime;
                    if ((userData.ProtectLastSnapshotTime > 0f) && (num4 > (Truth.RustProtectSnapshotsInterval + 10f)))
                    {
                        Helper.LogWarning(string.Concat(new object[] { "Kicked by Server [", userData.Username, ":", userData.SteamID, ":", ip, "]: No receiving snapshot from client over ", num4.ToString("F2"), " seconds." }), true);
                        NetUser.FindByUserID(userData.SteamID).Kick(NetError.Facepunch_Connector_NoConnect, true);
                        Helper.DisconnectBySteamID(userData.SteamID);
                    }*/

                }
                if ((((numArray.Length > 3) && (numArray[0] == 0x21000007)) && ((numArray[1] == 0x4c750b01) && (numArray[2] == 0x206b6e69))) && (numArray[3] == 0x2e352e31))
                   {
                    if (dictionary_1.ContainsKey(hashCode))
                    {
                        dictionary_1.Remove(hashCode);
                    }
                    dictionary_1.Add(hashCode, new NetCrypt(socket, ip, null));
                    if ((!dictionary_1[hashCode].SendCryptKey() && Core.Debug) && (server.log >= 1))
                    {
                        Helper.Log("RustProtect: Failed to send cryptkey packet for '" + ip + "'", true);
                    }
                    return length;
                }
                if ((((length == 0x10) && (numArray[0] == 0x25000007)) && ((numArray[1] == 0x636e450a) && (numArray[2] == 0x74707972))) && (numArray[3] == 0xff6e6f69))
                {
                    if (dictionary_1.ContainsKey(hashCode))
                    {
                        dictionary_1[hashCode].CryptPackets = true;
                        if (Core.Debug && (server.log >= 1))
                        {
                            Helper.Log("Network(" + ip + ") accept packet encryption.", true);
                        }
                    }
                    return 0;
                }
                if (dictionary_1.ContainsKey(hashCode) && dictionary_1[hashCode].CryptPackets)
                {
                    length = dictionary_1[hashCode].Decrypt(ref buffer, length);
                }
            }
            if (Core.Debug && (server.log >= 2))
            {
                Helper.Log(string.Concat(new object[] { "Network.Recv(", ip, ").Packet(", length, "): ", BitConverter.ToString(buffer, 0, length).Replace("-", "") }), true);
            }
            numArray = new uint[length / 4];
            for (int i = 0; i < numArray.Length; i++)
            {
                numArray[i] = BitConverter.ToUInt32(buffer, i * 4);
            }
            NetUser user = null;
            if (((numArray[0] != 0x21000007) || (numArray[1] != 0x4c750b01)) || ((numArray[2] != 0x206b6e69) || (numArray[3] != 0x2e352e31)))
            {
                if (((numArray[0] == 0x11000007) || (numArray[0] == 0x19000007)) || (numArray[0] == 0x29000007))
                {
                    return length;
                }
                if (((buffer[0] == 0x89) && (numArray[1] == 0x60002)) && (numArray[2] == 0x42d00))
                {
                    ulong num11 = BitConverter.ToUInt64(buffer, 14);
                    string str3 = Encoding.UTF8.GetString(buffer, 0x17, buffer[0x16]);
                    byte[] dst = new byte[0xea];
                    int num12 = 0x17 + buffer[0x16];
                    if ((length - num12) >= 0xea)
                    {
                        Buffer.BlockCopy(buffer, num12 + 1, dst, 0, dst.Length);
                        if ((BitConverter.ToInt32(dst, 0) == 0x1401) && (BitConverter.ToUInt64(dst, 13) == num11))
                        {
                            Helper.Log(string.Concat(new object[] { "Steam Connection [", str3, ":", num11, ":", ip, "]." }), true);
                            return length;
                        }
                    }
                    Helper.Log(string.Concat(new object[] { "No-Steam Connection [", str3, ":", num11, ":", ip, "]." }), true);
                    return length;
                }
                if (((buffer[0] == 0x89) && (numArray[1] == 0x600)) && (numArray[2] == 0x42d))
                {
                    ulong num13 = BitConverter.ToUInt64(buffer, 13);
                    string str4 = Encoding.UTF8.GetString(buffer, 0x16, buffer[0x15]);
                    Helper.Log(string.Concat(new object[] { "No-Steam Connection [", str4, ":", num13, ":", ip, "]." }), true);
                    return length;
                }
                if (!smethod_1(ip, out user))
                {
                    return 0;
                }
                if ((buffer[0] != 0x89) || (Users.NetworkTimeout <= 0f))
                {
                    return length;
                }
                string str5 = Encoding.ASCII.GetString(buffer, 8, buffer[7]);
                BitConverter.ToUInt16(buffer, 1);
                Convert.ToUInt16((int) (buffer[3] + 4));
                if (str5 == "ClientFirstReady")
                {
                    if (Truth.LastPacketTime.ContainsKey(user))
                    {
                        Truth.LastPacketTime.Remove(user);
                    }
                    Truth.LastPacketTime.Add(user, Time.time + (Users.NetworkTimeout * 5f));
                    if (dictionary_1.ContainsKey(hashCode))
                    {
                        dictionary_1[hashCode].UserData = Users.GetBySteamID(user.userID);
                        dictionary_1[hashCode].UserData.ProtectLastSnapshotTime = Time.time;
                    }
                }
                if (((str5 == "GetClientMove") && Truth.LastPacketTime.ContainsKey(user)) && (Truth.LastPacketTime[user] < Time.time))
                {
                    Truth.LastPacketTime[user] = Time.time;
                }
            }
            return length;
        }

        public static int uLink_DoNetworkSend(System.Net.Sockets.Socket socket, byte[] buffer, int length, EndPoint ip)
        {
            try
            {
                int size = length;
                int hashCode = ip.GetHashCode();
                if (Core.Debug && (server.log >= 2))
                {
                    Helper.Log(string.Concat(new object[] { "Network.Send(", ip, ").Packet(", length, "): ", BitConverter.ToString(buffer, 0, length).Replace("-", "") }), true);
                }
                if (dictionary_1.ContainsKey(hashCode) && dictionary_1[hashCode].CryptPackets)
                {
                    size = dictionary_1[hashCode].Encrypt(ref buffer, length);
                }
                RustExtended.Bootstrap.SendPacketCounter++;
                socket.SendTo(buffer, 0, size, SocketFlags.None, ip);
                return length;
            }
            catch (Exception exception)
            {
                if (Core.Debug && (server.log >= 2))
                {
                    UnityEngine.Debug.LogError(exception.ToString());
                }
            }
            return 0;
        }

        public static void VoiceCom_ClientSpeak(VoiceCom hook, PlayerClient sender, PlayerClient client)
        {
            if (Core.VoiceNotification)
            {
                if (!dictionary_0.ContainsKey(client.userID))
                {
                    dictionary_0.Add(client.userID, 0);
                }
                int num = Environment.TickCount - dictionary_0[client.userID];
                dictionary_0[client.userID] = Environment.TickCount;
                if (num >= Core.VoiceNotificationDelay)
                {
                    Notice.Inventory(client.netPlayer, "☎ " + sender.netUser.displayName);
                }
            }
        }

        [CompilerGenerated]
        private sealed class Class45
        {
            public ClientConnection clientConnection_0;

            public bool method_0(string string_0)
            {
                return string_0.Equals(this.clientConnection_0.UserName, StringComparison.OrdinalIgnoreCase);
            }
        }

        [CompilerGenerated]
        private sealed class Class46
        {
            public PlayerClient playerClient_0;

            public bool method_0(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.playerClient_0.netUser) && (eventTimer_0.Command == "home"));
            }

            public bool method_1(EventTimer eventTimer_0)
            {
                return ((eventTimer_0.Sender == this.playerClient_0.netUser) && (eventTimer_0.Command == "clan"));
            }

            public bool method_2(EventTimer eventTimer_0)
            {
                if ((eventTimer_0.Sender != this.playerClient_0.netUser) && (eventTimer_0.Target != this.playerClient_0.netUser))
                {
                    return false;
                }
                return (eventTimer_0.Command == "tp");
            }
        }

        [CompilerGenerated]
        private sealed class Class47
        {
            public UserData userData_0;

            public bool method_0(LoadoutEntry loadoutEntry_0)
            {
                return loadoutEntry_0.Ranks.Contains<int>(this.userData_0.Rank);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct UserGatherPoint
        {
            public Vector3 position;
            public uint quantity;
        }
    }
}

