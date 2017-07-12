using Facepunch;
using System;
using System.Collections.Generic;
using System.IO;
using uLink;
using UnityEngine;

namespace Magma
{
	public class World
	{
		private static World world;

		public float DayLength
		{
			get
			{
				return env.daylength;
			}
			set
			{
				env.daylength = value;
			}
		}

		public List<Entity> Entities
		{
			get
			{
				List<Entity> list = new List<Entity>();
				UnityEngine.Object[] array = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(StructureComponent));
				for (int i = 0; i < array.Length; i++)
				{
					StructureComponent obj = (StructureComponent)array[i];
					list.Add(new Entity(obj));
				}
				UnityEngine.Object[] array2 = UnityEngine.Resources.FindObjectsOfTypeAll(typeof(DeployableObject));
				for (int j = 0; j < array2.Length; j++)
				{
					DeployableObject obj2 = (DeployableObject)array2[j];
					list.Add(new Entity(obj2));
				}
				return list;
			}
		}

		public float NightLength
		{
			get
			{
				return env.nightlength;
			}
			set
			{
				env.nightlength = value;
			}
		}

		public float Time
		{
			get
			{
				return EnvironmentControlCenter.Singleton.GetTime();
			}
			set
			{
				EnvironmentControlCenter.Singleton.SetTime(value);
			}
		}

		public void Airdrop()
		{
			this.Airdrop(1);
		}

		public void Airdrop(int rep)
		{
			for (int i = 0; i < rep; i++)
			{
				SupplyDropZone.CallAirDrop();
			}
		}

		public void AirdropAt(float x, float y, float z)
		{
			this.AirdropAt(x, y, z, 1);
		}

		public void AirdropAt(float x, float y, float z, int rep)
		{
			for (int i = 0; i < rep; i++)
			{
				SupplyDropZone.CallAirDropAt(new Vector3(x, y, z));
			}
		}

		public void AirdropAtPlayer(Player p)
		{
			this.AirdropAtPlayer(p, 1);
		}

		public void AirdropAtPlayer(Player p, int rep)
		{
			for (int i = 0; i < rep; i++)
			{
				SupplyDropZone.CallAirDropAt(p.Location);
			}
		}

		public void Blocks()
		{
			ItemDataBlock[] all = DatablockDictionary.All;
			for (int i = 0; i < all.Length; i++)
			{
				ItemDataBlock itemDataBlock = all[i];
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Name : " + itemDataBlock.name + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "ID : " + itemDataBlock.uniqueID + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Flags : " + itemDataBlock._itemFlags.ToString() + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Max Condition : " + itemDataBlock._maxCondition + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Loose Condition : " + itemDataBlock.doesLoseCondition + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Max Uses : " + itemDataBlock._maxUses + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Mins Uses (Display) : " + itemDataBlock._minUsesForDisplay + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Spawn Uses Max : " + itemDataBlock._spawnUsesMax + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Spawn Uses Min : " + itemDataBlock._spawnUsesMin + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Splittable : " + itemDataBlock._splittable + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Category : " + itemDataBlock.category.ToString() + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Combinations :\n");
				ItemDataBlock.CombineRecipe[] combinations = itemDataBlock.Combinations;
				for (int j = 0; j < combinations.Length; j++)
				{
					ItemDataBlock.CombineRecipe combineRecipe = combinations[j];
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "\t" + combineRecipe.ToString() + "\n");
				}
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Icon : " + itemDataBlock.icon + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "IsRecycleable : " + itemDataBlock.isRecycleable + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "IsRepairable : " + itemDataBlock.isRepairable + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "IsResearchable : " + itemDataBlock.isResearchable + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Description : " + itemDataBlock.itemDescriptionOverride + "\n");
				if (itemDataBlock is BulletWeaponDataBlock)
				{
					BulletWeaponDataBlock bulletWeaponDataBlock = (BulletWeaponDataBlock)itemDataBlock;
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Min Damage : " + bulletWeaponDataBlock.damageMin + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Max Damage : " + bulletWeaponDataBlock.damageMax + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Ammo : " + bulletWeaponDataBlock.ammoType.ToString() + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Recoil Duration : " + bulletWeaponDataBlock.recoilDuration + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "RecoilPitch Min : " + bulletWeaponDataBlock.recoilPitchMin + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "RecoilPitch Max : " + bulletWeaponDataBlock.recoilPitchMax + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "RecoilYawn Min : " + bulletWeaponDataBlock.recoilYawMin + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "RecoilYawn Max : " + bulletWeaponDataBlock.recoilYawMax + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Bullet Range : " + bulletWeaponDataBlock.bulletRange + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Sway : " + bulletWeaponDataBlock.aimSway + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "SwaySpeed : " + bulletWeaponDataBlock.aimSwaySpeed + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Aim Sensitivity : " + bulletWeaponDataBlock.aimSensitivtyPercent + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "FireRate : " + bulletWeaponDataBlock.fireRate + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "FireRate Secondary : " + bulletWeaponDataBlock.fireRateSecondary + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Max Clip Ammo : " + bulletWeaponDataBlock.maxClipAmmo + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Reload Duration : " + bulletWeaponDataBlock.reloadDuration + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "Attachment Point : " + bulletWeaponDataBlock.attachmentPoint + "\n");
				}
				File.AppendAllText(Util.GetAbsoluteFilePath("BlocksData.txt"), "------------------------------------------------------------\n\n");
			}
		}

		public StructureMaster CreateSM(Player p)
		{
			return this.CreateSM(p, p.X, p.Y, p.Z, p.PlayerClient.transform.rotation);
		}

		public StructureMaster CreateSM(Player p, float x, float y, float z)
		{
			return this.CreateSM(p, x, y, z, Quaternion.identity);
		}

		public StructureMaster CreateSM(Player p, float x, float y, float z, Quaternion rot)
		{
			StructureMaster structureMaster = NetCull.InstantiateClassic<StructureMaster>(Bundling.Load<StructureMaster>("content/structures/StructureMasterPrefab"), new Vector3(x, y, z), rot, 0);
			structureMaster.SetupCreator(p.PlayerClient.controllable);
			return structureMaster;
		}

		public Zone3D CreateZone(string name)
		{
			return new Zone3D(name);
		}

		public float GetGround(float x, float z)
		{
			Vector3 origin = new Vector3(x, 2000f, z);
			Vector3 direction = new Vector3(0f, -1f, 0f);
			return Physics.RaycastAll(origin, direction)[0].point.y;
		}

		public static World GetWorld()
		{
			if (World.world == null)
			{
				World.world = new World();
			}
			return World.world;
		}

		public void Lists()
		{
			foreach (LootSpawnList current in DatablockDictionary._lootSpawnLists.Values)
			{
				File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Name : " + current.name + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Min Spawn : " + current.minPackagesToSpawn + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Max Spawn : " + current.maxPackagesToSpawn + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "NoDuplicate : " + current.noDuplicates + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "OneOfEach : " + current.spawnOneOfEach + "\n");
				File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Entries :\n");
				LootSpawnList.LootWeightedEntry[] lootPackages = current.LootPackages;
				for (int i = 0; i < lootPackages.Length; i++)
				{
					LootSpawnList.LootWeightedEntry lootWeightedEntry = lootPackages[i];
					File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Amount Min : " + lootWeightedEntry.amountMin + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Amount Max : " + lootWeightedEntry.amountMax + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Weight : " + lootWeightedEntry.weight + "\n");
					File.AppendAllText(Util.GetAbsoluteFilePath("Lists.txt"), "Object : " + lootWeightedEntry.obj.ToString() + "\n\n");
				}
			}
		}

		public void Prefabs()
		{
			ItemDataBlock[] all = DatablockDictionary.All;
			for (int i = 0; i < all.Length; i++)
			{
				ItemDataBlock itemDataBlock = all[i];
				if (itemDataBlock is DeployableItemDataBlock)
				{
					DeployableItemDataBlock deployableItemDataBlock = itemDataBlock as DeployableItemDataBlock;
					File.AppendAllText(Util.GetAbsoluteFilePath("Prefabs.txt"), string.Concat(new string[]
					{
						"[\"",
						deployableItemDataBlock.ObjectToPlace.name,
						"\", \"",
						deployableItemDataBlock.DeployableObjectPrefabName,
						"\"],\n"
					}));
				}
				else if (itemDataBlock is StructureComponentDataBlock)
				{
					StructureComponentDataBlock structureComponentDataBlock = itemDataBlock as StructureComponentDataBlock;
					File.AppendAllText(Util.GetAbsoluteFilePath("Prefabs.txt"), string.Concat(new string[]
					{
						"[\"",
						structureComponentDataBlock.structureToPlacePrefab.name,
						"\", \"",
						structureComponentDataBlock.structureToPlaceName,
						"\"],\n"
					}));
				}
			}
		}

		public object Spawn(string prefab, Vector3 location)
		{
			return this.Spawn(prefab, location, 1);
		}

		public object Spawn(string prefab, Vector3 location, int rep)
		{
			return this.Spawn(prefab, location, Quaternion.identity, rep);
		}

		public object Spawn(string prefab, float x, float y, float z)
		{
			return this.Spawn(prefab, x, y, z, 1);
		}

		private object Spawn(string prefab, Vector3 location, Quaternion rotation, int rep)
		{
			object result = null;
			for (int i = 0; i < rep; i++)
			{
				if (prefab == ":player_soldier")
				{
					result = NetCull.InstantiateDynamic(uLink.NetworkPlayer.server, prefab, location, rotation);
				}
				else if (prefab.Contains("C130"))
				{
					result = NetCull.InstantiateClassic(prefab, location, rotation, 0);
				}
				else
				{
					GameObject gameObject = NetCull.InstantiateStatic(prefab, location, rotation);
					result = gameObject;
					StructureComponent component = gameObject.GetComponent<StructureComponent>();
					if (component != null)
					{
						result = new Entity(component);
					}
					else
					{
						DeployableObject component2 = gameObject.GetComponent<DeployableObject>();
						if (component2 != null)
						{
							component2.ownerID = 0uL;
							component2.creatorID = 0uL;
							component2.CacheCreator();
							component2.CreatorSet();
							result = new Entity(component2);
						}
					}
				}
			}
			return result;
		}

		public object Spawn(string prefab, float x, float y, float z, int rep)
		{
			return this.Spawn(prefab, new Vector3(x, y, z), Quaternion.identity, rep);
		}

		public object Spawn(string prefab, float x, float y, float z, Quaternion rot)
		{
			return this.Spawn(prefab, x, y, z, rot, 1);
		}

		public object Spawn(string prefab, float x, float y, float z, Quaternion rot, int rep)
		{
			return this.Spawn(prefab, new Vector3(x, y, z), rot, rep);
		}

		public object SpawnAtPlayer(string prefab, Player p)
		{
			return this.Spawn(prefab, p.Location, p.PlayerClient.transform.rotation, 1);
		}

		public object SpawnAtPlayer(string prefab, Player p, int rep)
		{
			return this.Spawn(prefab, p.Location, p.PlayerClient.transform.rotation, rep);
		}
	}
}
