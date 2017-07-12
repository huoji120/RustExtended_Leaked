using Facepunch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RustExtended
{
	public class Spawns : Facepunch.MonoBehaviour
	{
		public class SpawnerGeneric
		{
			public class SpawnInstance
			{
				public List<GameObject> Spawned = new List<GameObject>();

				public string PrefabName = string.Empty;

				public bool UseNavmeshSample = true;

				public bool StaticInstantiate;

				public int NumToSpawnPerTick = 1;

				public int TargetPopulation;

				public int GetActiveCount
				{
					get
					{
						return this.Spawned.Count;
					}
				}
			}

			private static float float_0;

			private float float_1;

			[CompilerGenerated]
			private bool bool_0;

			public List<Spawns.SpawnerGeneric.SpawnInstance> SpawnList;

			public bool InitialSpawn;

			public float SpawnRadius = 40f;

			public float ThinkDelay = 60f;

			public Vector3 Position;

			public bool Initialized
			{
				get;
				private set;
			}

			public SpawnerGeneric()
			{
				this.InitialSpawn = false;
				this.SpawnList = new List<Spawns.SpawnerGeneric.SpawnInstance>();
			}

			public void Initialize()
			{
				if (!this.InitialSpawn)
				{
					this.SpawnThink();
					this.float_1 = Time.time + Spawns.SpawnerGeneric.float_0;
					Spawns.SpawnerGeneric.float_0 += UnityEngine.Random.Range(1f, 2f);
					this.Initialized = true;
					this.InitialSpawn = true;
				}
			}

			public void SpawnThink()
			{
				if (Time.time > this.float_1)
				{
					this.float_1 += this.ThinkDelay;
					foreach (Spawns.SpawnerGeneric.SpawnInstance current in this.SpawnList)
					{
						int num = current.TargetPopulation - current.Spawned.Count;
						if (num > 0)
						{
							num = (this.InitialSpawn ? UnityEngine.Random.Range(1, Mathf.Min(num, current.NumToSpawnPerTick) + 1) : current.TargetPopulation);
							if (num > 0)
							{
								for (int i = 0; i < num; i++)
								{
									this.SpawnGeneric(current);
								}
							}
						}
					}
				}
			}

			public GameObject SpawnGeneric(Spawns.SpawnerGeneric.SpawnInstance instance)
			{
				Vector3 startPos = this.Position + UnityEngine.Random.insideUnitSphere * this.SpawnRadius;
				startPos.y = this.Position.y;
				Quaternion quaternion = Quaternion.Euler(new Vector3(0f, (float)UnityEngine.Random.Range(0, 360), 0f));
				Vector3 vector;
				if (instance.UseNavmeshSample && TransformHelpers.GetGroundInfoNavMesh(startPos, out vector, 15f, -1))
				{
					startPos = vector;
				}
				Vector3 up;
				GameObject result;
				if (TransformHelpers.GetGroundInfoTerrainOnly(startPos, 300f, out vector, out up))
				{
					vector.y += 0.05f;
					if (instance.PrefabName == "BoxLoot")
					{
						vector.y += 0.35f;
					}
					quaternion = TransformHelpers.LookRotationForcedUp(quaternion * Vector3.forward, up);
					GameObject gameObject;
					if (!instance.StaticInstantiate && !instance.PrefabName.StartsWith(";"))
					{
						gameObject = NetCull.InstantiateDynamic(instance.PrefabName, vector, quaternion);
					}
					else
					{
						gameObject = NetCull.InstantiateStatic(instance.PrefabName, vector, quaternion);
					}
					if (gameObject != null)
					{
						instance.Spawned.Add(gameObject);
					}
					result = gameObject;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		public class SpawnerLootable
		{
			public struct LootableParams
			{
				public float Weight;

				public float LifeTime;

				public float LootCycle;

				public LootableParams(float weight, float lifetime, float lootcycle)
				{
					this.Weight = weight;
					this.LifeTime = lifetime;
					this.LootCycle = lootcycle;
				}
			}

			private float float_0;

			internal Vector3 vector3_0;

			public GameObject SpawnedObject;

			[CompilerGenerated]
			private bool bool_0;

			public Dictionary<string, Spawns.SpawnerLootable.LootableParams> List;

			public bool SpawnOnStart = true;

			public float SpawnTimeMin = 5f;

			public float SpawnTimeMax = 10f;

			public bool Initialized
			{
				get;
				private set;
			}

			public SpawnerLootable()
			{
				this.List = new Dictionary<string, Spawns.SpawnerLootable.LootableParams>();
			}

			public void Initialize()
			{
				if (this.SpawnOnStart)
				{
					this.SpawnThink();
				}
				else
				{
					this.float_0 = Time.time + this.SpawnTimeMax * 60f;
				}
				this.Initialized = true;
			}

			private static GameObject smethod_183(Spawns.SpawnerLootable spawnerLootable_0)
			{
				GameObject result;
				if (spawnerLootable_0.SpawnedObject == null && spawnerLootable_0.vector3_0 != Vector3.zero && spawnerLootable_0.List.Count > 0)
				{
					string text = spawnerLootable_0.method_0();
					if (text != null)
					{
						Vector3 vector;
						Vector3 vector2;
						TransformHelpers.GetGroundInfo(spawnerLootable_0.vector3_0, out vector, out vector2);
						if (text == "BoxLoot")
						{
							vector.y += 0.35f;
						}
						Quaternion quaternion = Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f);
						GameObject gameObject = NetCull.InstantiateStatic(text, vector, quaternion);
						LootableObject component = gameObject.GetComponent<LootableObject>();
						if (component != null)
						{
							component.LootCycle = spawnerLootable_0.List[text].LootCycle;
							component.lifeTime = spawnerLootable_0.List[text].LifeTime;
							component.ResetInvokes();
						}
						result = gameObject;
						return result;
					}
				}
				result = spawnerLootable_0.SpawnedObject;
				return result;
			}

			public void SetPostition(Vector3 position)
			{
				this.vector3_0 = position;
			}

			public void AddLoot(string prefab, float weight, float lifetime, float lootcycle)
			{
				this.List.Add(prefab, new Spawns.SpawnerLootable.LootableParams(weight, lifetime, lootcycle));
			}

			public void SpawnThink()
			{
				if (this.vector3_0 != Vector3.zero && Time.time > this.float_0)
				{
					float num = UnityEngine.Random.Range(this.SpawnTimeMin, this.SpawnTimeMax);
					this.float_0 = Time.time + num * 60f;
					this.SpawnedObject = Spawns.SpawnerLootable.smethod_183(this);
				}
			}

			internal string method_0()
			{
				float num = 0f;
				foreach (Spawns.SpawnerLootable.LootableParams current in this.List.Values)
				{
					num += current.Weight;
				}
				string result;
				if (num > 0f)
				{
					float num2 = UnityEngine.Random.Range(0f, num);
					foreach (string current2 in this.List.Keys)
					{
						num2 -= this.List[current2].Weight;
						if (num2 <= 0f)
						{
							result = current2;
							return result;
						}
					}
					result = this.List.Keys.Last<string>();
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		[CompilerGenerated]
		[Serializable]
		private sealed class c
		{
			public static readonly Spawns.c g = new Spawns.c();

			public static Predicate<GameObject> g__54_0;

			internal bool method_0(GameObject gameObject_0)
			{
				return gameObject_0 == null;
			}
		}

		[CompilerGenerated]
		private static Spawns spawns_0;

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static GenericSpawner[] genericSpawner_0;

		[CompilerGenerated]
		private static LootableObjectSpawner[] lootableObjectSpawner_0;

		[CompilerGenerated]
		private static List<Spawns.SpawnerGeneric> list_0;

		[CompilerGenerated]
		private static List<Spawns.SpawnerLootable> list_1;

		[CompilerGenerated]
		private static string string_0;

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static int int_0;

		[CompilerGenerated]
		private static int int_1;

		[CompilerGenerated]
		private static int int_2;

		[CompilerGenerated]
		private static int int_3;

		public static Spawns Singleton
		{
			get;
			private set;
		}

		public static bool Initialized
		{
			get;
			private set;
		}

		public static GenericSpawner[] Generic
		{
			get;
			private set;
		}

		public static LootableObjectSpawner[] Lootable
		{
			get;
			private set;
		}

		public static List<Spawns.SpawnerGeneric> GenericSpawners
		{
			get;
			private set;
		}

		public static List<Spawns.SpawnerLootable> LootableSpawners
		{
			get;
			private set;
		}

		public static string OverridePath
		{
			get;
			private set;
		}

		public static string OverrideFile
		{
			get;
			private set;
		}

		public static int SpawnsCount
		{
			get;
			private set;
		}

		public static int SpawnsTotal
		{
			get;
			private set;
		}

		public static int TotalGeneric
		{
			get;
			private set;
		}

		public static int TotalLootable
		{
			get;
			private set;
		}

		public void Awake()
		{
			Spawns.Singleton = this;
			Spawns.GenericSpawners = new List<Spawns.SpawnerGeneric>();
			Spawns.LootableSpawners = new List<Spawns.SpawnerLootable>();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnServerLoad()
		{
			Spawns.OverridePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Core.SavePath), "cfg\\RustOverride");
			if (!Directory.Exists(Spawns.OverridePath))
			{
				Directory.CreateDirectory(Spawns.OverridePath);
			}
			Spawns.OverrideFile = Path.Combine(Override.OverridePath, "SpawnsList.cfg");
			if (Core.OverrideSpawns && File.Exists(Spawns.OverrideFile))
			{
				GenericSpawner[] array = UnityEngine.Object.FindObjectsOfType<GenericSpawner>();
				for (int i = 0; i < array.Length; i++)
				{
					UnityEngine.Object.DestroyImmediate(array[i]);
				}
				LootableObjectSpawner[] array2 = UnityEngine.Object.FindObjectsOfType<LootableObjectSpawner>();
				for (int i = 0; i < array2.Length; i++)
				{
					UnityEngine.Object.DestroyImmediate(array2[i].gameObject);
				}
			}
			Spawns.Generic = UnityEngine.Object.FindObjectsOfType<GenericSpawner>();
			Spawns.Lootable = UnityEngine.Object.FindObjectsOfType<LootableObjectSpawner>();
			Spawns.Initialized = false;
		}

		public bool Initialize()
		{
			bool result;
			if (NetCull.isServerRunning && !Spawns.Initialized)
			{
				if (Core.OverrideSpawns && File.Exists(Spawns.OverrideFile))
				{
					Spawns.smethod_96();
					base.InvokeRepeating("UpdateSpawners", 0f, NetCull.sendIntervalF);
				}
				else if (!File.Exists(Spawns.OverrideFile))
				{
					Spawns.smethod_0();
				}
				Spawns.TotalGeneric = Spawns.Generic.Length + Spawns.GenericSpawners.Count;
				Spawns.TotalLootable = Spawns.Lootable.Length + Spawns.LootableSpawners.Count;
				base.InvokeRepeating("CalculateSpawners", 0f, 1f);
				result = (Spawns.Initialized = true);
			}
			else
			{
				result = Spawns.Initialized;
			}
			return result;
		}

		public static void smethod_96()
		{
			using (StreamReader streamReader = File.OpenText(Spawns.OverrideFile))
			{
				string a = string.Empty;
				Spawns.SpawnerGeneric spawnerGeneric = null;
				Spawns.SpawnerLootable spawnerLootable = null;
				string[] separator = new string[]
				{
					"//"
				};
				while (!streamReader.EndOfStream)
				{
					string text = streamReader.ReadLine().Trim();
					if (!string.IsNullOrEmpty(text) && text.Contains("//"))
					{
						text = text.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
						text = text.Trim();
					}
					if (!string.IsNullOrEmpty(text))
					{
						if (text.IndexOf('[') < text.IndexOf(']'))
						{
							a = text.Substring(1, text.Length - 2).ToLower();
							if (spawnerGeneric != null && !spawnerGeneric.Initialized)
							{
								spawnerGeneric.Initialize();
								Spawns.GenericSpawners.Add(spawnerGeneric);
							}
							if (spawnerLootable != null && !spawnerLootable.Initialized)
							{
								spawnerLootable.Initialize();
								Spawns.LootableSpawners.Add(spawnerLootable);
							}
							if (a == "generic")
							{
								spawnerGeneric = new Spawns.SpawnerGeneric();
							}
							if (a == "lootable")
							{
								spawnerLootable = new Spawns.SpawnerLootable();
							}
						}
						else
						{
							string[] array = text.Split(new char[]
							{
								'='
							});
							if (array.Length > 1)
							{
								string a2 = array[0].Trim().ToLower();
								array = array[1].RemoveChars(new char[]
								{
									' ',
									'\t'
								}).Split(new char[]
								{
									','
								});
								if (a == "lootable" && spawnerLootable != null)
								{
									if (!(a2 == "position"))
									{
										if (!(a2 == "spawntimemin"))
										{
											if (!(a2 == "spawntimemax"))
											{
												if (!(a2 == "spawnonstart"))
												{
													if (a2 == "spawnobject" && array.Length == 4)
													{
														string prefab = array[0];
														float weight = float.Parse(array[1]);
														float lifetime = float.Parse(array[2]);
														float lootcycle = float.Parse(array[3]);
														spawnerLootable.AddLoot(prefab, weight, lifetime, lootcycle);
													}
												}
												else if (array.Length == 1)
												{
													spawnerLootable.SpawnOnStart = bool.Parse(array[0]);
												}
											}
											else if (array.Length == 1)
											{
												spawnerLootable.SpawnTimeMax = float.Parse(array[0]);
											}
										}
										else if (array.Length == 1)
										{
											spawnerLootable.SpawnTimeMin = float.Parse(array[0]);
										}
									}
									else if (array.Length == 3)
									{
										float x = float.Parse(array[0]);
										float y = float.Parse(array[1]);
										float z = float.Parse(array[2]);
										spawnerLootable.SetPostition(new Vector3(x, y, z));
									}
								}
								else if (a == "generic" && spawnerGeneric != null)
								{
									if (!(a2 == "position"))
									{
										if (!(a2 == "spawnradius"))
										{
											if (!(a2 == "updatedelay"))
											{
												if (a2 == "spawnobject" && array.Length == 5)
												{
													Spawns.SpawnerGeneric.SpawnInstance spawnInstance = new Spawns.SpawnerGeneric.SpawnInstance();
													spawnInstance.PrefabName = array[0];
													spawnInstance.TargetPopulation = int.Parse(array[1]);
													spawnInstance.NumToSpawnPerTick = int.Parse(array[2]);
													spawnInstance.StaticInstantiate = bool.Parse(array[3]);
													spawnInstance.UseNavmeshSample = bool.Parse(array[4]);
													spawnerGeneric.SpawnList.Add(spawnInstance);
												}
											}
											else if (array.Length == 1)
											{
												spawnerGeneric.ThinkDelay = float.Parse(array[0]);
											}
										}
										else if (array.Length == 1)
										{
											spawnerGeneric.SpawnRadius = float.Parse(array[0]);
										}
									}
									else if (array.Length == 3)
									{
										float x2 = float.Parse(array[0]);
										float y2 = float.Parse(array[1]);
										float z2 = float.Parse(array[2]);
										spawnerGeneric.Position = new Vector3(x2, y2, z2);
									}
								}
							}
						}
					}
				}
				if (spawnerGeneric != null && !spawnerGeneric.Initialized)
				{
					spawnerGeneric.Initialize();
					Spawns.GenericSpawners.Add(spawnerGeneric);
				}
				if (spawnerLootable != null && !spawnerLootable.Initialized)
				{
					spawnerLootable.Initialize();
					Spawns.LootableSpawners.Add(spawnerLootable);
				}
			}
		}

		private void UpdateSpawners()
		{
			if (NetCull.isServerRunning && Core.Initialized && Spawns.Initialized)
			{
				using (List<Spawns.SpawnerGeneric>.Enumerator enumerator = Spawns.GenericSpawners.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.SpawnThink();
					}
				}
				using (List<Spawns.SpawnerLootable>.Enumerator enumerator2 = Spawns.LootableSpawners.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.SpawnThink();
					}
				}
			}
		}

		private void CalculateSpawners()
		{
			if (Core.Initialized && NetCull.isServerRunning)
			{
				Spawns.SpawnsTotal = 0;
				Spawns.SpawnsCount = 0;
				if (Spawns.Generic != null && Spawns.Generic.Length != 0)
				{
					GenericSpawner[] generic = Spawns.Generic;
					for (int i = 0; i < generic.Length; i++)
					{
						foreach (GenericSpawnerSpawnList.GenericSpawnInstance current in generic[i]._spawnList)
						{
							Spawns.SpawnsCount += current.spawned.Count;
							Spawns.SpawnsTotal += current.targetPopulation;
						}
					}
				}
				if (Spawns.Lootable != null && Spawns.Lootable.Length != 0)
				{
					LootableObjectSpawner[] lootable = Spawns.Lootable;
					for (int i = 0; i < lootable.Length; i++)
					{
						if (lootable[i] != null)
						{
							Spawns.SpawnsCount++;
							Spawns.SpawnsTotal++;
						}
					}
				}
				if (Spawns.GenericSpawners.Count > 0)
				{
					using (List<Spawns.SpawnerGeneric>.Enumerator enumerator2 = Spawns.GenericSpawners.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							foreach (Spawns.SpawnerGeneric.SpawnInstance current2 in enumerator2.Current.SpawnList)
							{
								List<GameObject> spawned = current2.Spawned;
								Predicate<GameObject> match;
								if ((match = Spawns.c.g__54_0) == null)
								{
									match = (Spawns.c.g__54_0 = new Predicate<GameObject>(Spawns.c.g.method_0));
								}
								spawned.RemoveAll(match);
								Spawns.SpawnsCount += current2.GetActiveCount;
								Spawns.SpawnsTotal += current2.TargetPopulation;
							}
						}
					}
				}
				if (Spawns.LootableSpawners.Count > 0)
				{
					using (List<Spawns.SpawnerLootable>.Enumerator enumerator4 = Spawns.LootableSpawners.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							if (enumerator4.Current.SpawnedObject != null)
							{
								Spawns.SpawnsCount++;
							}
						}
					}
					Spawns.SpawnsTotal += Spawns.LootableSpawners.Count;
				}
			}
		}

		private static void smethod_0()
		{
			using (StreamWriter streamWriter = File.CreateText(Spawns.OverrideFile))
			{
				streamWriter.WriteLine("// Format & Help:");
				streamWriter.WriteLine("//  Lootable->Position - Position of spawn for create lootable object");
				streamWriter.WriteLine("//  Lootable->SpawnTimeMin - Minimum time in seconds to re-spawn a lootable object");
				streamWriter.WriteLine("//  Lootable->SpawnTimeMax - Maximum time in seconds to re-spawn a lootable object");
				streamWriter.WriteLine("//  Lootable->SpawnOnStart - Enable/disable for spawn after server start");
				streamWriter.WriteLine("//  Lootable->SpawnObject = Name, Weight, LifeTime, LootCycle");
				streamWriter.WriteLine("//  Lootable->SpawnObject->Name - Name of object to spawn, can be only (BoxLoot, AmmoLootBox, MedicalLootBox, WeaponLootBox)");
				streamWriter.WriteLine("//  Lootable->SpawnObject->Weight - Weight as chance for creation, in relation to other objects in list");
				streamWriter.WriteLine("//  Lootable->SpawnObject->LifeTime - Time of life of loot, loot will be removed after the expiration of time");
				streamWriter.WriteLine("//  Lootable->SpawnObject->LootCycle - Changes loot after specified time, regular");
				streamWriter.WriteLine();
				streamWriter.WriteLine("//  Generic->Position - Position of generic spawner for create objects around him");
				streamWriter.WriteLine("//  Generic->SpawnRadius - Spawn radius around current position of generic spawner");
				streamWriter.WriteLine("//  Generic->UpdateDelay - Delay in seconds to update spawns around generic spawner");
				streamWriter.WriteLine("//  Generic->SpawnObject = Prefab, TargetPopulation, NumToSpawnPerTick, UseNavmeshSample");
				streamWriter.WriteLine("//  Generic->SpawnObject->Prefab - Prefab name of object to spawn");
				streamWriter.WriteLine("//  Generic->SpawnObject->TargetPopulation - Maximum count of objects to spawn around spawner");
				streamWriter.WriteLine("//  Generic->SpawnObject->NumToSpawnPerTick - Amount of objects to spawn per time (specified in UpdateDelay)");
				streamWriter.WriteLine("//  Generic->SpawnObject->StaticInstantiate - Mark as static instantiated (when object is not movable: boxes, resources and etc.)");
				streamWriter.WriteLine("//  Generic->SpawnObject->UseNavmeshSample - Using navmesh checks of position, where spawn will be created");
				LootableObjectSpawner[] lootable = Spawns.Lootable;
				for (int i = 0; i < lootable.Length; i++)
				{
					LootableObjectSpawner lootableObjectSpawner = lootable[i];
					streamWriter.WriteLine();
					streamWriter.WriteLine("[Lootable]");
					streamWriter.WriteLine("Position=" + lootableObjectSpawner.transform.position.AsString());
					streamWriter.WriteLine("SpawnTimeMin=" + lootableObjectSpawner.spawnTimeMin);
					streamWriter.WriteLine("SpawnTimeMax=" + lootableObjectSpawner.spawnTimeMax);
					streamWriter.WriteLine("SpawnOnStart=" + lootableObjectSpawner.spawnOnStart.ToString());
					LootableObjectSpawner.ChancePick[] lootableChances = lootableObjectSpawner._lootableChances;
					for (int j = 0; j < lootableChances.Length; j++)
					{
						LootableObjectSpawner.ChancePick chancePick = lootableChances[j];
						string text = new string('\t', 2 - (chancePick.obj.name.Length + 5) / 8 + 1);
						streamWriter.WriteLine(string.Concat(new object[]
						{
							"SpawnObject=",
							chancePick.obj.name,
							",",
							text,
							chancePick.weight,
							",\t",
							chancePick.obj.lifeTime,
							",\t",
							chancePick.obj.LootCycle
						}));
					}
				}
				GenericSpawner[] generic = Spawns.Generic;
				for (int i = 0; i < generic.Length; i++)
				{
					GenericSpawner genericSpawner = generic[i];
					streamWriter.WriteLine();
					streamWriter.WriteLine("[Generic]");
					streamWriter.WriteLine("Position=" + genericSpawner.transform.position.AsString());
					streamWriter.WriteLine("SpawnRadius=" + genericSpawner.radius);
					streamWriter.WriteLine("UpdateDelay=" + genericSpawner.thinkDelay);
					foreach (GenericSpawnerSpawnList.GenericSpawnInstance current in genericSpawner._spawnList)
					{
						string text2 = new string('\t', 2 - (current.prefabName.Length + 5) / 8 + 1);
						streamWriter.WriteLine(string.Concat(new object[]
						{
							"SpawnObject=",
							current.prefabName,
							",",
							text2,
							current.targetPopulation,
							",\t",
							current.numToSpawnPerTick,
							",\t",
							current.forceStaticInstantiate.ToString(),
							",\t",
							current.useNavmeshSample.ToString()
						}));
					}
				}
			}
		}
	}
}
