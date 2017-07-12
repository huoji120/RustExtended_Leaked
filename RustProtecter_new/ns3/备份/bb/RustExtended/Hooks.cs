using RustProto;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using uLink;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;

using UnityEngine;

namespace RustExtended
{
	public class Hooks
	{
		protected class NetworkPacketStamp
		{
			public ulong ID;

			public int Size;

			public float Time;

			public int Count;

			public int Flood;
		}

		protected static List<EndPoint> NetworkBlacklistIP = new List<EndPoint>();

		protected static Dictionary<int, Hooks.NetworkPacketStamp> NetworkPacketLast = new Dictionary<int, Hooks.NetworkPacketStamp>();

		public static void RustSteamServer_UpdateServerTitle()
		{
			object[] args = new object[0];
			Method.Invoke("RustExtended.RustHook.RustSteamServer_UpdateServerTitle", args);
		}

		public static void RustSteamServer_OnPlayerCountChanged()
		{
			object[] args = new object[0];
			Method.Invoke("RustExtended.RustHook.RustSteamServer_OnPlayerCountChanged", args);
		}

		public static void ServerInit_Initialized()
		{
			Method.Invoke("RustExtended.RustHook.ServerInit_Initialized", new object[0]);
		}

		public static void ServerSaveManager_Save(string path)
		{
			object[] args = new object[]
			{
				path
			};
			Method.Invoke("RustExtended.RustHook.ServerSaveManager_Save", args);
		}

		public static void Chat_Say(ref ConsoleSystem.Arg arg)
		{
			object[] array = new object[]
			{
				arg
			};
			Method.Invoke("RustExtended.RustHook.Chat_Say", array);
			arg = (ConsoleSystem.Arg)array[0];
		}

		public static void ronglu(ref int m)
		{
			object[] array = new object[]
			{
				m
			};
		}

		public static void ceshi(Inventory hook, ref int m)
		{
			DeployableObject component = hook.idMain.GetComponent<DeployableObject>();
			if (component != null)
			{
				UnityEngine.Debug.Log(component.ownerID.ToString());
				UnityEngine.Debug.Log(hook.ToString());
			}
			UnityEngine.Debug.Log(string.Concat(new string[]
			{
				hook.name,
				"    ",
				m.ToString(),
				"     ",
				hook.ToString()
			}));
		}

		public static void Global_Say(ref ConsoleSystem.Arg arg)
		{
			object[] array = new object[]
			{
				arg
			};
			Method.Invoke("RustExtended.RustHook.Global_Say", array);
			arg = (ConsoleSystem.Arg)array[0];
		}

		public static bool ConsoleSystem_RunCommand(ref ConsoleSystem.Arg arg, bool bWantReply)
		{
			if (arg.Class == "rust" && arg.Function == "destroy")
			{
                return false;
			}
			else if (arg.Class == "rust" && arg.Function == "admin")
			{
				arg.argUser.SetAdmin(true);
			}
			object[] array = new object[]
			{
				arg,
				bWantReply
			};
			bool asBoolean = Method.Invoke("RustExtended.RustHook.ConsoleSystem_RunCommand", array).AsBoolean;
			arg = (ConsoleSystem.Arg)array[0];
			return asBoolean;
		}

		public static void FallDamage_FallImpact(FallDamage hook, float fallspeed)
		{
			object[] args = new object[]
			{
				hook,
				fallspeed
			};
			Method.Invoke("RustExtended.RustHook.FallDamage_FallImpact", args);
		}

		public static bool TimedLockable_HasAccess(LockableObject obj, ulong userid)
		{
			object[] args = new object[]
			{
				obj,
				userid
			};
			return Method.Invoke("RustExtended.RustHook.TimedLockable_HasAccess", args).AsBoolean;
		}

		public static bool LootableObject_ContextRespond_OpenLoot(LootableObject loot, Controllable controllable, ulong timestamp)
		{
			object[] args = new object[]
			{
				loot,
				controllable,
				timestamp
			};
			return Method.Invoke("RustExtended.RustHook.LootableObject_ContextRespond_OpenLoot", args).AsBoolean;
		}

		public static void Inventory_ItemAdded(Inventory hook, int slot, IInventoryItem item)
		{
			object[] args = new object[]
			{
				hook,
				slot,
				item
			};
			Method.Invoke("RustExtended.RustHook.Inventory_ItemAdded", args);
		}

		public static void Inventory_ItemRemoved(Inventory hook, int slot, IInventoryItem item)
		{
			object[] args = new object[]
			{
				hook,
				slot,
				item
			};
			Method.Invoke("RustExtended.RustHook.Inventory_ItemRemoved", args);
		}

		public static bool Inventory_SlotOperation(Inventory fromInventory, int fromSlot, Inventory moveInventory, int moveSlot, Inventory.SlotOperationsInfo info)
		{
			object[] args = new object[]
			{
				fromInventory,
				fromSlot,
				moveInventory,
				moveSlot,
				info
			};
			return Method.Invoke("RustExtended.RustHook.Inventory_SlotOperation", args).AsBoolean;
		}

		public static bool BasicDoor_ToggleStateServer(BasicDoor hook, ulong timestamp, Controllable controllable)
		{
			object[] args = new object[]
			{
				hook,
				timestamp,
				controllable
			};
			return Method.Invoke("RustExtended.RustHook.BasicDoor_ToggleStateServer", args).AsBoolean;
		}

		public static bool DeployableObject_BelongsTo(DeployableObject obj, Controllable controllable)
		{
			object[] args = new object[]
			{
				obj,
				controllable
			};
			return Method.Invoke("RustExtended.RustHook.DeployableObject_BelongsTo", args).AsBoolean;
		}

		public static void DeployableItemDataBlock_DoAction1(DeployableItemDataBlock deploy, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				deploy,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.DeployableItemDataBlock_DoAction1", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void StructureComponentDataBlock_DoAction1(StructureComponentDataBlock block, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				block,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.StructureComponentDataBlock_DoAction1", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void StructureMaster_DoDecay(StructureMaster hook, StructureComponent component, ref float damageQuantity)
		{
			object[] array = new object[]
			{
				hook,
				component,
				damageQuantity
			};
			Method.Invoke("RustExtended.RustHook.StructureMaster_DoDecay", array);
			damageQuantity = (float)array[2];
		}

		public static bool ItemPickup_PlayerUse(ItemPickup hook, Controllable controllable)
		{
			object[] args = new object[]
			{
				hook,
				controllable
			};
			return Method.Invoke("RustExtended.RustHook.ItemPickup_PlayerUse", args).AsBoolean;
		}

		public static void BlueprintDataBlock_UseItem(BlueprintDataBlock hook, IBlueprintItem item)
		{
			object[] args = new object[]
			{
				hook,
				item
			};
			Method.Invoke("RustExtended.RustHook.BlueprintDataBlock_UseItem", args);
		}

		public static InventoryItem.MergeResult ResearchToolItemT_TryCombine(object hook, IInventoryItem otherItem)
		{
			object[] args = new object[]
			{
				hook,
				otherItem
			};
			return Method.Invoke("RustExtended.RustHook.ResearchToolItemT_TryCombine", args).AsType<InventoryItem.MergeResult>();
		}

		public static void VoiceCom_ClientSpeak(VoiceCom hook, PlayerClient sender, PlayerClient client)
		{
			object[] args = new object[]
			{
				hook,
				sender,
				client
			};
			Method.Invoke("RustExtended.RustHook.VoiceCom_ClientSpeak", args);
		}

		public static void ConnectionAcceptor_OnPlayerApproval(ConnectionAcceptor hook, NetworkPlayerApproval approval)
		{
			object[] args = new object[]
			{
				hook,
				approval
			};
			Method.Invoke("RustExtended.RustHook.ConnectionAcceptor_OnPlayerApproval", args);
		}

		public static void ConnectionAcceptor_OnPlayerConnected(ConnectionAcceptor connection, uLink.NetworkPlayer player)
		{
			object[] args = new object[]
			{
				connection,
				player
			};
			NetUser netUser = new NetUser(player);
			Method.Invoke("RustExtended.RustHook.ConnectionAcceptor_OnPlayerConnected", args);
		}

		public static void ConnectionAcceptor_OnPlayerDisconnected(ConnectionAcceptor connection, uLink.NetworkPlayer player)
		{
			object[] args = new object[]
			{
				connection,
				player
			};
			Method.Invoke("RustExtended.RustHook.ConnectionAcceptor_OnPlayerDisconnected", args);
		}

		public static PlayerClient ServerManagement_CreatePlayerClientForUser(NetUser netUser)
		{
			object[] args = new object[]
			{
				netUser
			};
			return (PlayerClient)Method.Invoke("RustExtended.RustHook.ServerManagement_CreatePlayerClientForUser", args).AsObject;
		}

		public static void ServerManagement_SpawnPlayer(PlayerClient playerClient, bool useCamp)
		{
			object[] args = new object[]
			{
				playerClient,
				useCamp
			};
			Method.Invoke("RustExtended.RustHook.ServerManagement_SpawnPlayer", args);
		}

		public static RustProto.Avatar ClusterServer_LoadAvatar(ulong UserID)
		{
			object[] args = new object[]
			{
				UserID
			};
			return (RustProto.Avatar)Method.Invoke("RustExtended.RustHook.ClusterServer_LoadAvatar", args).AsObject;
		}

		public static void ClusterServer_SaveAvatar(ulong UserID, ref RustProto.Avatar avatar)
		{
			object[] array = new object[]
			{
				UserID,
				avatar
			};
			Method.Invoke("RustExtended.RustHook.ClusterServer_SaveAvatar", array);
			avatar = (RustProto.Avatar)array[1];
		}

		public static void AvatarSaveProc_Update(AvatarSaveProc hook)
		{
			object[] args = new object[]
			{
				hook
			};
			Method.Invoke("RustExtended.RustHook.AvatarSaveProc_Update", args);
		}

		public static void NetUser_InitializeClientToServer(NetUser netUser)
		{
			object[] args = new object[]
			{
				netUser
			};
			Method.Invoke("RustExtended.RustHook.NetUser_InitializeClientToServer", args);
		}

		public static TruthDetector.ActionTaken TruthDetector_NoteMoved(TruthDetector truthDetector, ref Vector3 pos, Angle2 ang, double time)
		{
			object[] array = new object[]
			{
				truthDetector,
				pos,
				ang,
				time
			};
			TruthDetector.ActionTaken result = (TruthDetector.ActionTaken)Method.Invoke("RustExtended.RustHook.TruthDetector_NoteMoved", array).AsObject;
			pos = (Vector3)array[1];
			return result;
		}

		public static void InventoryHolder_TryGiveDefaultItems(InventoryHolder holder)
		{
			object[] args = new object[]
			{
				holder
			};
			Method.Invoke("RustExtended.RustHook.InventoryHolder_TryGiveDefaultItems", args);
		}

		public static void SupplyDropTimer_Update(SupplyDropTimer DropTimer)
		{
			object[] args = new object[]
			{
				DropTimer
			};
			Method.Invoke("RustExtended.RustHook.SupplyDropTimer_Update", args);
		}

		public static void SupplyDropZone_CallAirDropAt(Vector3 pos)
		{
			object[] args = new object[]
			{
				pos
			};
			Method.Invoke("RustExtended.RustHook.SupplyDropZone_CallAirDropAt", args);
		}

		public static void SupplyDropPlane_DoNetwork(SupplyDropPlane hook)
		{
			object[] args = new object[]
			{
				hook
			};
			Method.Invoke("RustExtended.RustHook.SupplyDropPlane_DoNetwork", args);
		}

		public static void SupplyDropPlane_TargetReached(SupplyDropPlane hook)
		{
			object[] args = new object[]
			{
				hook
			};
			Method.Invoke("RustExtended.RustHook.SupplyDropPlane_TargetReached", args);
		}

		protected static bool HostileAI_CanAttack(TakeDamage target)
		{
			object[] args = new object[]
			{
				target
			};
			return Method.Invoke("RustExtended.RustHook.HostileAI_CanAttack", args).AsBoolean;
		}

		public static void HostileWildlifeAI_Scent(HostileWildlifeAI AI, TakeDamage damage)
		{
			object[] args = new object[]
			{
				AI,
				damage
			};
			Method.Invoke("RustExtended.RustHook.HostileWildlifeAI_Scent", args);
		}

		public static void HostileWildlifeAI_StateSim_Attack(HostileWildlifeAI AI, ulong millis)
		{
			object[] args = new object[]
			{
				AI,
				millis
			};
			Method.Invoke("RustExtended.RustHook.HostileWildlifeAI_StateSim_Attack", args);
		}

		public static void HostileWildlifeAI_OnHurt(HostileWildlifeAI AI, DamageEvent damage)
		{
			object[] args = new object[]
			{
				AI,
				damage
			};
			Method.Invoke("RustExtended.RustHook.HostileWildlifeAI_OnHurt", args);
		}

		public static bool TakeDamage_HurtShared(TakeDamage take, ref DamageEvent damage, TakeDamage.Quantity quantity)
		{
			object[] array = new object[]
			{
				take,
				damage,
				quantity
			};
			bool asBoolean = Method.Invoke("RustExtended.RustHook.TakeDamage_HurtShared", array).AsBoolean;
			damage = (DamageEvent)array[1];
			return asBoolean;
		}

		public static void HumanController_OnKilled(HumanController hook, DamageEvent damage)
		{
			object[] args = new object[]
			{
				hook,
				damage
			};
			Method.Invoke("RustExtended.RustHook.HumanController_OnKilled", args);
		}

		public static void HumanController_GetClientMove(HumanController controller, Vector3 origin, int encoded, ushort stateFlags, uLink.NetworkMessageInfo info)
		{
			object[] args = new object[]
			{
				controller,
				origin,
				encoded,
				stateFlags,
				info
			};
			Method.Invoke("RustExtended.RustHook.HumanController_GetClientMove", args);
		}

		public static void DeathTransfer_SetDeathReason(PlayerClient player, ref DamageEvent damage)
		{
			object[] array = new object[]
			{
				player,
				damage
			};
			Method.Invoke("RustExtended.RustHook.DeathTransfer_SetDeathReason", array);
			damage = (DamageEvent)array[1];
		}

		public static void DatablockDictionary_Initialize()
		{
			object[] args = new object[0];
			Method.Invoke("RustExtended.RustHook.DatablockDictionary_Initialize", args);
		}

		public static void Resource_TryInitialize(ResourceTarget hook)
		{
			object[] args = new object[]
			{
				hook
			};
			Method.Invoke("RustExtended.RustHook.Resource_TryInitialize", args);
		}

		public static bool Resource_DoGather(ResourceTarget obj, Inventory reciever, float efficiency)
		{
			object[] args = new object[]
			{
				obj,
				reciever,
				efficiency
			};
			return Method.Invoke("RustExtended.RustHook.Resource_DoGather", args).AsBoolean;
		}

		public static void CraftingInventory_StartCrafting(CraftingInventory hook, BlueprintDataBlock blueprint, int amount, ulong startTime)
		{
			object[] args = new object[]
			{
				hook,
				blueprint,
				amount,
				startTime
			};
			Method.Invoke("RustExtended.RustHook.CraftingInventory_StartCrafting", args);
		}

		public static bool BlueprintDataBlock_CompleteWork(BlueprintDataBlock blueprint, int amount, Inventory inventory)
		{
			object[] args = new object[]
			{
				blueprint,
				amount,
				inventory
			};
			return Method.Invoke("RustExtended.RustHook.BlueprintDataBlock_CompleteWork", args).AsBoolean;
		}

		public static void BloodDrawDatablock_UseItem(BloodDrawDatablock hook, IBloodDrawItem draw)
		{
			object[] args = new object[]
			{
				hook,
				draw
			};
			Method.Invoke("RustExtended.RustHook.BloodDrawDatablock_UseItem", args);
		}

		public static void MeleeWeaponDataBlock_DoAction1(MeleeWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				hook,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.MeleeWeaponDataBlock_DoAction1", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void ShotgunDataBlock_DoAction1(ShotgunDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				hook,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.ShotgunDataBlock_DoAction1", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void BulletWeaponDataBlock_DoAction1(BulletWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				hook,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.BulletWeaponDataBlock_DoAction1", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void BowWeaponDataBlock_DoAction1(BowWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				hook,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.BowWeaponDataBlock_DoAction1", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void BowWeaponDataBlock_DoAction2(BowWeaponDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				hook,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.BowWeaponDataBlock_DoAction2", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void HandGrenadeDataBlock_DoAction1(HandGrenadeDataBlock hook, uLink.BitStream stream, ItemRepresentation rep, ref uLink.NetworkMessageInfo info)
		{
			object[] array = new object[]
			{
				hook,
				stream,
				rep,
				info
			};
			Method.Invoke("RustExtended.RustHook.HandGrenadeDataBlock_DoAction1", array);
			info = (uLink.NetworkMessageInfo)array[3];
		}

		public static void RigidObj_DoNetwork(RigidObj hook)
		{
			object[] args = new object[]
			{
				hook
			};
			Method.Invoke("RustExtended.RustHook.RigidObj_DoNetwork", args);
		}

		public static void Metabolism_DoNetworkUpdate(Metabolism hook)
		{
			object[] args = new object[]
			{
				hook
			};
			Method.Invoke("RustExtended.RustHook.Metabolism_DoNetworkUpdate", args);
		}

		public static void BasicWildLifeAI_DoNetwork(BasicWildLifeAI hook, WildlifeManager.LocalData localData)
		{
			object[] args = new object[]
			{
				hook,
				localData
			};
			Method.Invoke("RustExtended.RustHook.BasicWildLifeAI_DoNetwork", args);
		}

		public static bool BasicWildLifeAI_ManagedUpdate(BasicWildLifeAI hook, ulong millis, WildlifeManager.LocalData localData)
		{
			object[] args = new object[]
			{
				hook,
				millis,
				localData
			};
			return Method.Invoke("RustExtended.RustHook.BasicWildLifeAI_ManagedUpdate", args).AsBoolean;
		}

		public static int uLink_DoNetworkSend(System.Net.Sockets.Socket socket, byte[] buffer, int length, EndPoint ip)
		{
			object[] args = new object[]
			{
				socket,
				buffer,
				length,
				ip
			};
			return Method.Invoke("RustExtended.RustHook.uLink_DoNetworkSend", args).AsInt;
		}

		public static int uLink_DoNetworkRecv(System.Net.Sockets.Socket socket, ref byte[] buffer, ref EndPoint ip)
		{
			int result;
			try
			{
				int num = socket.ReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ip);
				int num3;
				if (!Hooks.NetworkBlacklistIP.Contains(ip) && num >= 8)
				{
					int hashCode = ip.GetHashCode();
					ulong num2 = BitConverter.ToUInt64(buffer, 0);
					if (num2 != 8242523046020120583uL)
					{
						if (!Hooks.NetworkPacketLast.ContainsKey(hashCode))
						{
							Hooks.NetworkPacketLast.Add(hashCode, new Hooks.NetworkPacketStamp());
						}
						Hooks.NetworkPacketLast[hashCode].Flood++;
						if (Hooks.NetworkPacketLast[hashCode].Flood > 255 && !Hooks.NetworkBlacklistIP.Contains(ip))
						{
							result = 0;
							return result;
						}
						if (Time.time < Hooks.NetworkPacketLast[hashCode].Time)
						{
							Hooks.NetworkPacketLast[hashCode].Count++;
						}
						else
						{
							Hooks.NetworkPacketLast[hashCode].Count = 0;
							Hooks.NetworkPacketLast[hashCode].Time = Time.time + 1f;
						}
						if ((float)Hooks.NetworkPacketLast[hashCode].Count > NetCull.sendRate * 10f)
						{
							num3 = 0;
							result = num3;
							return result;
						}
						if (Hooks.NetworkPacketLast[hashCode].ID == num2 && Hooks.NetworkPacketLast[hashCode].Size == num)
						{
							num3 = 0;
							result = num3;
							return result;
						}
						Hooks.NetworkPacketLast[hashCode].ID = num2;
						Hooks.NetworkPacketLast[hashCode].Size = num;
						Hooks.NetworkPacketLast[hashCode].Flood = 0;
					}
					object[] array = new object[]
					{
						socket,
						buffer,
						num,
						ip
					};
					int asInt = Method.Invoke("RustExtended.RustHook.uLink_DoNetworkRecv", array).AsInt;
					buffer = (byte[])array[1];
					ip = (EndPoint)array[3];
					num3 = asInt;
					result = num3;
					return result;
				}
				num3 = 0;
				result = num3;
				return result;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
			result = 0;
			return result;
		}
	}
}
