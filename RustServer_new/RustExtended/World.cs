using System;
using System.IO;
using System.Runtime.CompilerServices;
using uLink;
using UnityEngine;

namespace RustExtended
{
	public class World
	{
		private static string string_0 = "prefabs.txt";

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static bool bool_0;

		public static string FilePath
		{
			get;
			private set;
		}

		public static bool Initialized
		{
			get;
			private set;
		}

		public static void Initialize()
		{
			World.FilePath = Path.Combine(Core.SavePath, World.string_0);
			World.Initialized = true;
		}

		public static void Prefabs()
		{
			if (File.Exists(World.FilePath))
			{
				File.Delete(World.FilePath);
			}
			ItemDataBlock[] all = DatablockDictionary.All;
			for (int i = 0; i < all.Length; i++)
			{
				ItemDataBlock itemDataBlock = all[i];
				if (itemDataBlock is DeployableItemDataBlock)
				{
					DeployableItemDataBlock deployableItemDataBlock = itemDataBlock as DeployableItemDataBlock;
					File.AppendAllText(World.FilePath, deployableItemDataBlock.ObjectToPlace.name + "=" + deployableItemDataBlock.DeployableObjectPrefabName + "\n");
				}
				if (itemDataBlock is StructureComponentDataBlock)
				{
					StructureComponentDataBlock structureComponentDataBlock = itemDataBlock as StructureComponentDataBlock;
					File.AppendAllText(World.FilePath, structureComponentDataBlock.structureToPlacePrefab.name + "=" + structureComponentDataBlock.structureToPlaceName + "\n");
				}
			}
		}

		public static bool LookAtPosition(PlayerClient player, out Vector3 position, float maxDistance = 100f)
		{
			position = new Vector3(0f, 0f, 0f);
			Character idMain = player.controllable.idMain;
			RaycastHit raycastHit;
			bool result;
			if (idMain != null && Physics.Raycast(idMain.eyesRay, out raycastHit, maxDistance, -1))
			{
				Vector3 vector;
				TransformHelpers.GetGroundInfo(raycastHit.point, 100f, out position, out vector);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static GameObject Spawn(string prefab, Vector3 position, Quaternion rotation, int count)
		{
			GameObject gameObject = null;
			for (int i = 0; i < count; i++)
			{
				if (prefab == ":player_soldier")
				{
					gameObject = NetCull.InstantiateDynamic(uLink.NetworkPlayer.server, prefab, position, rotation);
				}
				else if (prefab.Contains("C130"))
				{
					gameObject = NetCull.InstantiateClassic(prefab, position, rotation, 0);
				}
				else
				{
					gameObject = NetCull.InstantiateStatic(prefab, position, rotation);
					gameObject.GetComponent<StructureComponent>();
					DeployableObject component = gameObject.GetComponent<DeployableObject>();
					if (component != null)
					{
						component.ownerID = 0uL;
						component.creatorID = 0uL;
						component.CacheCreator();
						component.CreatorSet();
					}
				}
			}
			return gameObject;
		}

		public static GameObject Spawn(string prefab, Vector3 position)
		{
			return World.Spawn(prefab, position, 1);
		}

		public static GameObject Spawn(string prefab, Vector3 position, int amount)
		{
			return World.Spawn(prefab, position, Quaternion.identity, amount);
		}

		public static GameObject Spawn(string prefab, float x, float y, float z)
		{
			return World.Spawn(prefab, x, y, z, 1);
		}

		public static GameObject Spawn(string prefab, float x, float y, float z, int count)
		{
			return World.Spawn(prefab, new Vector3(x, y, z), Quaternion.identity, count);
		}

		public static GameObject Spawn(string prefab, float x, float y, float z, Quaternion rot)
		{
			return World.Spawn(prefab, x, y, z, rot, 1);
		}

		public static GameObject Spawn(string prefab, float x, float y, float z, Quaternion rot, int count)
		{
			return World.Spawn(prefab, new Vector3(x, y, z), rot, count);
		}

		public static GameObject SpawnAtPlayer(string prefab, PlayerClient player)
		{
			IDMain idMain = player.controllable.idMain;
			Transform transform = idMain.transform;
			Vector3 position;
			Vector3 up;
			idMain.transform.GetGroundInfo(out position, out up);
			Quaternion rotation = TransformHelpers.LookRotationForcedUp(player.transform.forward, up);
			return World.Spawn(prefab, position, rotation, 1);
		}

		public static GameObject SpawnAtPlayer(string prefab, PlayerClient player, int count)
		{
			IDMain idMain = player.controllable.idMain;
			Transform transform = idMain.transform;
			Vector3 position;
			Vector3 up;
			idMain.transform.GetGroundInfo(out position, out up);
			Quaternion rotation = TransformHelpers.LookRotationForcedUp(player.transform.forward, up);
			return World.Spawn(prefab, position, rotation, count);
		}
	}
}
