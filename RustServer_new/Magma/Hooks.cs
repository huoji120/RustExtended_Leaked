using Magma.Events;
using RustExtended;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magma
{
	public class Hooks
	{
		public delegate void BlueprintUseHandlerDelagate(Player player, BPUseEvent ae);

		public delegate void ChatHandlerDelegate(Player player, ref ChatString text);

		public delegate void CommandHandlerDelegate(Player player, string text, string[] args);

		public delegate void ConnectionHandlerDelegate(Player player);

		public delegate void ConsoleHandlerDelegate(ref ConsoleSystem.Arg arg, bool external);

		public delegate void DisconnectionHandlerDelegate(Player player);

		public delegate void DoorOpenHandlerDelegate(Player p, DoorEvent de);

		public delegate void EntityDecayDelegate(DecayEvent de);

		public delegate void EntityDeployedDelegate(Player player, Entity e);

		public delegate void EntityHurtDelegate(HurtEvent he);

		public delegate void HurtHandlerDelegate(HurtEvent he);

		public delegate void ItemsDatablocksLoaded(ItemsBlocks items);

		public delegate void KillHandlerDelegate(DeathEvent de);

		public delegate void LootTablesLoaded(Dictionary<string, LootSpawnList> lists);

		public delegate void PlayerGatheringHandlerDelegate(Player player, GatherEvent ge);

		public delegate void PlayerSpawnHandlerDelegate(Player player, SpawnEvent se);

		public delegate void PluginInitHandlerDelegate();

		public delegate void ServerInitDelegate();

		public delegate void ServerShutdownDelegate();

		private static List<object> decayList = new List<object>();

		private static Hashtable talkerTimers = new Hashtable();

		public static event Hooks.BlueprintUseHandlerDelagate OnBlueprintUse;

		public static event Hooks.ChatHandlerDelegate OnChat;

		public static event Hooks.CommandHandlerDelegate OnCommand;

		public static event Hooks.ConsoleHandlerDelegate OnConsoleReceived;

		public static event Hooks.DoorOpenHandlerDelegate OnDoorUse;

		public static event Hooks.EntityDecayDelegate OnEntityDecay;

		public static event Hooks.EntityDeployedDelegate OnEntityDeployed;

		public static event Hooks.EntityHurtDelegate OnEntityHurt;

		public static event Hooks.ItemsDatablocksLoaded OnItemsLoaded;

		public static event Hooks.HurtHandlerDelegate OnNPCHurt;

		public static event Hooks.KillHandlerDelegate OnNPCKilled;

		public static event Hooks.ConnectionHandlerDelegate OnPlayerConnected;

		public static event Hooks.DisconnectionHandlerDelegate OnPlayerDisconnected;

		public static event Hooks.PlayerGatheringHandlerDelegate OnPlayerGathering;

		public static event Hooks.HurtHandlerDelegate OnPlayerHurt;

		public static event Hooks.KillHandlerDelegate OnPlayerKilled;

		public static event Hooks.PlayerSpawnHandlerDelegate OnPlayerSpawned;

		public static event Hooks.PlayerSpawnHandlerDelegate OnPlayerSpawning;

		public static event Hooks.PluginInitHandlerDelegate OnPluginInit;

		public static event Hooks.ServerInitDelegate OnServerInit;

		public static event Hooks.ServerShutdownDelegate OnServerShutdown;

		public static event Hooks.LootTablesLoaded OnTablesLoaded;

		public static bool BlueprintUse(IBlueprintItem item, BlueprintDataBlock bdb)
		{
			Player player = Player.FindByPlayerClient(item.controllable.playerClient);
			bool result;
			if (player != null)
			{
				BPUseEvent bPUseEvent = new BPUseEvent(bdb);
				if (Hooks.OnBlueprintUse != null)
				{
					Hooks.OnBlueprintUse(player, bPUseEvent);
				}
				if (!bPUseEvent.Cancel)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static bool ChatReceived(ref ConsoleSystem.Arg arg, ref string chatText)
		{
			if (Hooks.OnChat != null)
			{
				ChatString chatString = new ChatString(chatText);
				Hooks.OnChat(Player.FindByPlayerClient(arg.argUser.playerClient), ref chatString);
				chatText = chatString.NewText;
			}
			return chatText != "";
		}

		public static bool CheckOwner(DeployableObject obj, Controllable controllable)
		{
			DoorEvent doorEvent = new DoorEvent(new Entity(obj));
			if (obj.ownerID == controllable.playerClient.userID)
			{
				doorEvent.Open = true;
			}
			if (obj.GetComponent<BasicDoor>() != null && Hooks.OnDoorUse != null)
			{
				Hooks.OnDoorUse(Player.FindByPlayerClient(controllable.playerClient), doorEvent);
			}
			return doorEvent.Open;
		}

		public static bool ConsoleReceived(ref ConsoleSystem.Arg a)
		{
			bool result;
			if (a.argUser == null && a.Class == "magmaweb" && a.Function == "handshake")
			{
				a.ReplyWith("All Good !");
				result = true;
			}
			else
			{
				bool flag = a.argUser == null;
				if (Hooks.OnConsoleReceived != null)
				{
					Hooks.OnConsoleReceived(ref a, flag);
				}
				if (a.Class == "magma" && a.Function.ToLower() == "reload")
				{
					if (a.argUser != null && a.argUser.admin)
					{
						PluginEngine.GetPluginEngine().ReloadPlugins(Player.FindByPlayerClient(a.argUser.playerClient));
						a.ReplyWith("Magma: Reloaded");
					}
					else if (flag)
					{
						PluginEngine.GetPluginEngine().ReloadPlugins(null);
						a.ReplyWith("Magma: Reloaded");
					}
				}
				if (a.Reply != null && a.Reply != "")
				{
					a.ReplyWith(string.Concat(new string[]
					{
						"Magma: ",
						a.Class,
						".",
						a.Function,
						" was executed!"
					}));
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		public static float EntityDecay(object entity, float dmg)
		{
			DecayEvent decayEvent = new DecayEvent(new Entity(entity), ref dmg);
			if (Hooks.OnEntityDecay != null)
			{
				Hooks.OnEntityDecay(decayEvent);
			}
			if (Hooks.decayList.Contains(entity))
			{
				Hooks.decayList.Remove(entity);
			}
			Hooks.decayList.Add(entity);
			return decayEvent.DamageAmount;
		}

		public static void EntityDeployed(object entity)
		{
			Entity entity2 = new Entity(entity);
			Player creator = entity2.Creator;
			if (Hooks.OnEntityDeployed != null)
			{
				Hooks.OnEntityDeployed(creator, entity2);
			}
		}

		public static void EntityHurt(object entity, ref DamageEvent e)
		{
			try
			{
				HurtEvent hurtEvent = new HurtEvent(ref e, new Entity(entity));
				if (Hooks.decayList.Contains(entity))
				{
					hurtEvent.IsDecay = true;
				}
				if (hurtEvent.Entity.IsStructure() && !hurtEvent.IsDecay)
				{
					StructureComponent structureComponent = entity as StructureComponent;
					if (structureComponent.IsType(StructureComponent.StructureComponentType.Ceiling) || structureComponent.IsType(StructureComponent.StructureComponentType.Foundation) || structureComponent.IsType(StructureComponent.StructureComponentType.Pillar))
					{
						hurtEvent.DamageAmount = 0f;
					}
				}
				TakeDamage takeDamage = hurtEvent.Entity.GetTakeDamage();
				takeDamage.health += hurtEvent.DamageAmount;
				if (Hooks.OnEntityHurt != null)
				{
					Hooks.OnEntityHurt(hurtEvent);
				}
				Zone3D zone3D = Zone3D.GlobalContains(hurtEvent.Entity);
				if ((zone3D == null || !zone3D.Protected) && hurtEvent.Entity.GetTakeDamage().health - hurtEvent.DamageAmount > 0f)
				{
					TakeDamage takeDamage2 = hurtEvent.Entity.GetTakeDamage();
					takeDamage2.health -= hurtEvent.DamageAmount;
				}
			}
			catch (Exception ex)
			{
				Helper.LogError(ex.ToString(), true);
			}
		}

		public static void handleCommand(ref ConsoleSystem.Arg arg, string cmd, string[] args)
		{
			if (Hooks.OnCommand != null)
			{
				Hooks.OnCommand(Player.FindByPlayerClient(arg.argUser.playerClient), cmd, args);
			}
		}

		public static ItemDataBlock[] ItemsLoaded(List<ItemDataBlock> items, Dictionary<string, int> stringDB, Dictionary<int, int> idDB)
		{
			ItemsBlocks itemsBlocks = new ItemsBlocks(items);
			if (Hooks.OnItemsLoaded != null)
			{
				Hooks.OnItemsLoaded(itemsBlocks);
			}
			int num = 0;
			foreach (ItemDataBlock current in itemsBlocks)
			{
				stringDB.Add(current.name, num);
				idDB.Add(current.uniqueID, num);
				num++;
			}
			Server.GetServer().Items = itemsBlocks;
			return itemsBlocks.ToArray();
		}

		public static void NPCHurt(ref DamageEvent e)
		{
			try
			{
				HurtEvent hurtEvent = new HurtEvent(ref e);
				if ((hurtEvent.Victim as NPC).Health > 0f)
				{
					NPC nPC = hurtEvent.Victim as NPC;
					nPC.Health += hurtEvent.DamageAmount;
					if (Hooks.OnNPCHurt != null)
					{
						Hooks.OnNPCHurt(hurtEvent);
					}
					if ((hurtEvent.Victim as NPC).Health - hurtEvent.DamageAmount <= 0f)
					{
						(hurtEvent.Victim as NPC).Kill();
					}
					else
					{
						NPC nPC2 = hurtEvent.Victim as NPC;
						nPC2.Health -= hurtEvent.DamageAmount;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public static void NPCKilled(ref DamageEvent e)
		{
			try
			{
				DeathEvent de = new DeathEvent(ref e);
				if (Hooks.OnNPCKilled != null)
				{
					Hooks.OnNPCKilled(de);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public static bool PlayerConnect(NetUser user)
		{
			Player player = new Player(user.playerClient);
			Server.GetServer().Players.Add(player);
			bool connected = user.connected;
			if (Hooks.OnPlayerConnected != null)
			{
				Hooks.OnPlayerConnected(player);
			}
			return connected;
		}

		public static void PlayerDisconnect(NetUser user)
		{
			Player player = Player.FindByPlayerClient(user.playerClient);
			if (player != null)
			{
				Server.GetServer().Players.Remove(player);
			}
			if (Hooks.OnPlayerDisconnected != null)
			{
				Hooks.OnPlayerDisconnected(player);
			}
		}

		public static void PlayerGather(Inventory rec, ResourceTarget rt, ResourceGivePair rg, ref int amount)
		{
			Player player = Player.FindByNetworkPlayer(rec.networkView.owner);
			GatherEvent gatherEvent = new GatherEvent(rt, rg, amount);
			if (Hooks.OnPlayerGathering != null)
			{
				Hooks.OnPlayerGathering(player, gatherEvent);
			}
			amount = gatherEvent.Quantity;
			if (!gatherEvent.Override)
			{
				amount = Mathf.Min(amount, rg.AmountLeft());
			}
			rg._resourceItemDatablock = gatherEvent.Item;
			rg.ResourceItemName = gatherEvent.Item;
		}

		public static void PlayerGatherWood(IMeleeWeaponItem rec, ResourceTarget rt, ref ItemDataBlock db, ref int amount, ref string name)
		{
			Player player = Player.FindByNetworkPlayer(rec.inventory.networkView.owner);
			GatherEvent gatherEvent = new GatherEvent(rt, db, amount)
			{
				Item = "Wood"
			};
			if (Hooks.OnPlayerGathering != null)
			{
				Hooks.OnPlayerGathering(player, gatherEvent);
			}
			db = Server.GetServer().Items.Find(gatherEvent.Item);
			amount = gatherEvent.Quantity;
			name = gatherEvent.Item;
		}

		public static void PlayerHurt(ref DamageEvent e)
		{
			HurtEvent hurtEvent = new HurtEvent(ref e);
			if (!(hurtEvent.Attacker is NPC) && !(hurtEvent.Victim is NPC))
			{
				Player player = hurtEvent.Attacker as Player;
				Player player2 = hurtEvent.Victim as Player;
				Zone3D zone3D = Zone3D.GlobalContains(player);
				if (zone3D != null && !zone3D.PVP)
				{
					player.Message("You are in a PVP restricted area.");
					hurtEvent.DamageAmount = 0f;
					e = hurtEvent.DamageEvent;
					return;
				}
				zone3D = Zone3D.GlobalContains(player2);
				if (zone3D != null && !zone3D.PVP)
				{
					player.Message(player2.Name + " is in a PVP restricted area.");
					hurtEvent.DamageAmount = 0f;
					e = hurtEvent.DamageEvent;
					return;
				}
			}
			if (Hooks.OnPlayerHurt != null)
			{
				Hooks.OnPlayerHurt(hurtEvent);
			}
			e = hurtEvent.DamageEvent;
		}

		public static bool PlayerKilled(ref DamageEvent de)
		{
			bool result;
			try
			{
				DeathEvent deathEvent = new DeathEvent(ref de);
				if (Hooks.OnPlayerKilled != null)
				{
					Hooks.OnPlayerKilled(deathEvent);
				}
				result = deathEvent.DropItems;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				result = true;
			}
			return result;
		}

		public static void PlayerSpawned(PlayerClient pc, Vector3 pos, bool camp)
		{
			Player player = Player.FindByPlayerClient(pc);
			SpawnEvent se = new SpawnEvent(pos, camp);
			if (Hooks.OnPlayerSpawned != null && player != null)
			{
				Hooks.OnPlayerSpawned(player, se);
			}
		}

		public static Vector3 PlayerSpawning(PlayerClient pc, Vector3 pos, bool camp)
		{
			Player player = Player.FindByPlayerClient(pc);
			SpawnEvent spawnEvent = new SpawnEvent(pos, camp);
			if (Hooks.OnPlayerSpawning != null && player != null)
			{
				Hooks.OnPlayerSpawning(player, spawnEvent);
			}
			return new Vector3(spawnEvent.X, spawnEvent.Y, spawnEvent.Z);
		}

		public static void PluginInit()
		{
			if (Hooks.OnPluginInit != null)
			{
				Hooks.OnPluginInit();
			}
		}

		public static void ResetHooks()
		{
			Hooks.OnPluginInit = delegate
			{
			};
			Hooks.OnChat = delegate(Player param0, ref ChatString param1)
			{
			};
			Hooks.OnCommand = delegate(Player param0, string param1, string[] param2)
			{
			};
			Hooks.OnPlayerConnected = delegate(Player param0)
			{
			};
			Hooks.OnPlayerDisconnected = delegate(Player param0)
			{
			};
			Hooks.OnNPCKilled = delegate(DeathEvent param0)
			{
			};
			Hooks.OnNPCHurt = delegate(HurtEvent param0)
			{
			};
			Hooks.OnPlayerKilled = delegate(DeathEvent param0)
			{
			};
			Hooks.OnPlayerHurt = delegate(HurtEvent param0)
			{
			};
			Hooks.OnPlayerSpawned = delegate(Player param0, SpawnEvent param1)
			{
			};
			Hooks.OnPlayerSpawning = delegate(Player param0, SpawnEvent param1)
			{
			};
			Hooks.OnPlayerGathering = delegate(Player param0, GatherEvent param1)
			{
			};
			Hooks.OnEntityHurt = delegate(HurtEvent param0)
			{
			};
			Hooks.OnEntityDecay = delegate(DecayEvent param0)
			{
			};
			Hooks.OnEntityDeployed = delegate(Player param0, Entity param1)
			{
			};
			Hooks.OnConsoleReceived = delegate(ref ConsoleSystem.Arg param0, bool param1)
			{
			};
			Hooks.OnBlueprintUse = delegate(Player param0, BPUseEvent param1)
			{
			};
			Hooks.OnDoorUse = delegate(Player param0, DoorEvent param1)
			{
			};
			Hooks.OnTablesLoaded = delegate(Dictionary<string, LootSpawnList> param0)
			{
			};
			Hooks.OnItemsLoaded = delegate(ItemsBlocks param0)
			{
			};
			Hooks.OnServerInit = delegate
			{
			};
			Hooks.OnServerShutdown = delegate
			{
			};
			foreach (Player current in Server.GetServer().Players)
			{
				current.FixInventoryRef();
			}
		}

		public static void ServerShutdown()
		{
			if (Hooks.OnServerShutdown != null)
			{
				Hooks.OnServerShutdown();
			}
			DataStore.GetInstance().Save();
		}

		public static void ServerStarted()
		{
			DataStore.GetInstance().Load();
			if (Hooks.OnServerInit != null)
			{
				Hooks.OnServerInit();
			}
		}

		public static Dictionary<string, LootSpawnList> TablesLoaded(Dictionary<string, LootSpawnList> lists)
		{
			if (Hooks.OnTablesLoaded != null)
			{
				Hooks.OnTablesLoaded(lists);
			}
			return lists;
		}
	}
}
