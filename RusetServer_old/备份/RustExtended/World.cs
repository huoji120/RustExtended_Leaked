namespace RustExtended
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using uLink;
    using UnityEngine;

    public class World
    {
        [CompilerGenerated]
        private static bool bool_0;
        private static string string_0 = "prefabs.txt";
        [CompilerGenerated]
        private static string string_1;

        public static void Initialize()
        {
            FilePath = Path.Combine(Core.SavePath, string_0);
            Initialized = true;
        }

        public static bool LookAtPosition(PlayerClient player, out Vector3 position, [Optional, DefaultParameterValue(100f)] float maxDistance)
        {
            RaycastHit hit;
            position = new Vector3(0f, 0f, 0f);
            Character idMain = player.controllable.idMain;
            if ((idMain != null) && Physics.Raycast(idMain.eyesRay, out hit, maxDistance, -1))
            {
                Vector3 vector;
                TransformHelpers.GetGroundInfo(hit.point, 100f, out position, out vector);
                return true;
            }
            return false;
        }

        public static void Prefabs()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            foreach (ItemDataBlock block in DatablockDictionary.All)
            {
                if (block is DeployableItemDataBlock)
                {
                    DeployableItemDataBlock block2 = block as DeployableItemDataBlock;
                    File.AppendAllText(FilePath, block2.ObjectToPlace.name + "=" + block2.DeployableObjectPrefabName + "\n");
                }
                if (block is StructureComponentDataBlock)
                {
                    StructureComponentDataBlock block3 = block as StructureComponentDataBlock;
                    File.AppendAllText(FilePath, block3.structureToPlacePrefab.name + "=" + block3.structureToPlaceName + "\n");
                }
            }
        }

        public static GameObject Spawn(string prefab, Vector3 position)
        {
            return Spawn(prefab, position, 1);
        }

        public static GameObject Spawn(string prefab, Vector3 position, int amount)
        {
            return Spawn(prefab, position, Quaternion.identity, amount);
        }

        public static GameObject Spawn(string prefab, float x, float y, float z)
        {
            return Spawn(prefab, x, y, z, 1);
        }

        public static GameObject Spawn(string prefab, Vector3 position, Quaternion rotation, int count)
        {
            GameObject obj2 = null;
            for (int i = 0; i < count; i++)
            {
                if (prefab == ":player_soldier")
                {
                    obj2 = NetCull.InstantiateDynamic(uLink.NetworkPlayer.server, prefab, position, rotation);
                }
                else if (prefab.Contains("C130"))
                {
                    obj2 = NetCull.InstantiateClassic(prefab, position, rotation, 0);
                }
                else
                {
                    obj2 = NetCull.InstantiateStatic(prefab, position, rotation);
                    obj2.GetComponent<StructureComponent>();
                    DeployableObject component = obj2.GetComponent<DeployableObject>();
                    if (component != null)
                    {
                        component.ownerID = 0L;
                        component.creatorID = 0L;
                        component.CacheCreator();
                        component.CreatorSet();
                    }
                }
            }
            return obj2;
        }

        public static GameObject Spawn(string prefab, float x, float y, float z, int count)
        {
            return Spawn(prefab, new Vector3(x, y, z), Quaternion.identity, count);
        }

        public static GameObject Spawn(string prefab, float x, float y, float z, Quaternion rot)
        {
            return Spawn(prefab, x, y, z, rot, 1);
        }

        public static GameObject Spawn(string prefab, float x, float y, float z, Quaternion rot, int count)
        {
            return Spawn(prefab, new Vector3(x, y, z), rot, count);
        }

        public static GameObject SpawnAtPlayer(string prefab, PlayerClient player)
        {
            Vector3 vector;
            Vector3 vector2;
            IDMain idMain = player.controllable.idMain;
            Transform transform = idMain.transform;
            idMain.transform.GetGroundInfo(out vector, out vector2);
            Quaternion rotation = TransformHelpers.LookRotationForcedUp(player.transform.forward, vector2);
            return Spawn(prefab, vector, rotation, 1);
        }

        public static GameObject SpawnAtPlayer(string prefab, PlayerClient player, int count)
        {
            Vector3 vector;
            Vector3 vector2;
            IDMain idMain = player.controllable.idMain;
            Transform transform = idMain.transform;
            idMain.transform.GetGroundInfo(out vector, out vector2);
            Quaternion rotation = TransformHelpers.LookRotationForcedUp(player.transform.forward, vector2);
            return Spawn(prefab, vector, rotation, count);
        }

        public static string FilePath
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

