using Facepunch.MeshBatch;
using Facepunch.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RustExtended
{
	public class Override
	{
		public static string OverridePath = "";

		private static List<string> list_0 = new List<string>();

		[CompilerGenerated]
		private static string string_0;

		[CompilerGenerated]
		private static string string_1;

		[CompilerGenerated]
		private static string string_2;

		[CompilerGenerated]
		private static int int_0;

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static bool bool_1;

		[CompilerGenerated]
		private static int int_1;

		[CompilerGenerated]
		private static bool bool_2;

		[CompilerGenerated]
		private static bool bool_3;

		[CompilerGenerated]
		private static int int_2;

		[CompilerGenerated]
		private static bool bool_4;

		[CompilerGenerated]
		private static bool bool_5;

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

		private static Dictionary<string, int> asas;

		public static string LootsFile
		{
			get;
			private set;
		}

		public static string ItemsFile
		{
			get;
			private set;
		}

		public static string SpawnsFile
		{
			get;
			private set;
		}

		public static int LootsCount
		{
			get;
			private set;
		}

		public static bool LootsInitialized
		{
			get;
			private set;
		}

		public static bool LootsFileCreated
		{
			get;
			private set;
		}

		public static int ItemsCount
		{
			get;
			private set;
		}

		public static bool ItemsInitialized
		{
			get;
			private set;
		}

		public static bool ItemsFileCreated
		{
			get;
			private set;
		}

		public static int SpawnsCount
		{
			get;
			private set;
		}

		public static bool SpawnsInitialized
		{
			get;
			private set;
		}

		public static bool SpawnsFileCreated
		{
			get;
			private set;
		}

		public static void Initialize()
		{
			Override.OverridePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Core.SavePath), "cfg\\RustOverride");
			if (!Directory.Exists(Override.OverridePath))
			{
				Directory.CreateDirectory(Override.OverridePath);
			}
			Override.LootsFile = Path.Combine(Override.OverridePath, "LootsList.cfg");
			Override.ItemsFile = Path.Combine(Override.OverridePath, "ItemsList.cfg");
			Override.SpawnsFile = Path.Combine(Override.OverridePath, "SpawnList.cfg");
			Override.LootsInitialized = false;
			Override.LootsFileCreated = false;
			Override.LootsCount = 0;
			Override.ItemsInitialized = false;
			Override.ItemsFileCreated = false;
			Override.ItemsCount = 0;
			Override.SpawnsInitialized = false;
			Override.SpawnsFileCreated = false;
			Override.SpawnsCount = 0;
			Override.list_0 = DatablockDictionary._lootSpawnLists.Keys.ToList<string>();
			if (!File.Exists(Override.LootsFile))
			{
				Override.LootsFileCreated = Override.LootSaveFile();
			}
			else if (Core.OverrideLoots)
			{
				Override.LootsInitialized = Override.smethod_0();
			}
			Override.LootsCount = DatablockDictionary._lootSpawnLists.Count;
			if (!File.Exists(Override.ItemsFile))
			{
				Override.ItemsFileCreated = Override.smethod_1();
			}
			else if (Core.OverrideItems)
			{
				Override.ItemsInitialized = Override.smethod_2();
			}
			Override.InitializeSpawnsFile();
			string path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Core.SavePath), "cfg\\rust_items.txt");
			if (!File.Exists(path))
			{
				string text = "";
				ItemDataBlock[] all = DatablockDictionary.All;
				for (int i = 0; i < all.Length; i++)
				{
					ItemDataBlock itemDataBlock = all[i];
					text = text + itemDataBlock.name + Environment.NewLine;
				}
				File.WriteAllText(path, text);
			}
		}

		public static bool LootSaveFile()
		{
			using (StreamWriter streamWriter = File.CreateText(Override.LootsFile))
			{
				foreach (string current in Override.list_0)
				{
					LootSpawnList lootSpawnList = DatablockDictionary._lootSpawnLists[current];
					streamWriter.WriteLine("[" + lootSpawnList.name + "]");
					streamWriter.WriteLine(string.Concat(new object[]
					{
						"PackagesToSpawn=",
						lootSpawnList.minPackagesToSpawn,
						",",
						lootSpawnList.maxPackagesToSpawn
					}));
					streamWriter.WriteLine("SpawnOneOfEach=" + lootSpawnList.spawnOneOfEach.ToString());
					streamWriter.WriteLine("NoDuplicates=" + lootSpawnList.noDuplicates.ToString());
					streamWriter.WriteLine("// Type   Weight\tList/Item\t\tMin\tMax");
					LootSpawnList.LootWeightedEntry[] lootPackages = lootSpawnList.LootPackages;
					for (int i = 0; i < lootPackages.Length; i++)
					{
						LootSpawnList.LootWeightedEntry lootWeightedEntry = lootPackages[i];
						if (!(lootWeightedEntry.obj == null))
						{
							if (lootWeightedEntry.obj is ItemDataBlock)
							{
								streamWriter.Write("PackageItem=");
							}
							else
							{
								streamWriter.Write("PackageList=");
							}
							streamWriter.Write(lootWeightedEntry.weight + "\t");
							streamWriter.Write(lootWeightedEntry.obj.name + new string('\t', 4 - lootWeightedEntry.obj.name.Length / 8));
							streamWriter.Write(lootWeightedEntry.amountMin + "\t" + lootWeightedEntry.amountMax);
							streamWriter.WriteLine();
						}
					}
					streamWriter.WriteLine();
				}
			}
			return true;
		}

		private static bool smethod_0()
		{
			List<string> list = File.ReadAllLines(Override.LootsFile).ToList<string>();
			List<string> list2 = list;
			if (Override.predicate_0 == null)
			{
				Override.predicate_0 = new Predicate<string>(Override.smethod_6);
			}
			bool result;
			if (!list2.Exists(Override.predicate_0))
			{
				ConsoleSystem.PrintError("ERROR: Spawn list for \"AILootList\" not found in \"lootslist.cfg\".", false);
				result = false;
			}
			else
			{
				List<string> list3 = list;
				if (Override.predicate_1 == null)
				{
					Override.predicate_1 = new Predicate<string>(Override.smethod_7);
				}
				if (!list3.Exists(Override.predicate_1))
				{
					ConsoleSystem.PrintError("ERROR: Spawn list for \"AmmoSpawnList\" not found in \"lootslist.cfg\".", false);
					result = false;
				}
				else
				{
					List<string> list4 = list;
					if (Override.predicate_2 == null)
					{
						Override.predicate_2 = new Predicate<string>(Override.smethod_8);
					}
					if (!list4.Exists(Override.predicate_2))
					{
						ConsoleSystem.PrintError("ERROR: Spawn list for \"JunkSpawnList\" not found in \"lootslist.cfg\".", false);
						result = false;
					}
					else
					{
						List<string> list5 = list;
						if (Override.predicate_3 == null)
						{
							Override.predicate_3 = new Predicate<string>(Override.smethod_9);
						}
						if (!list5.Exists(Override.predicate_3))
						{
							ConsoleSystem.PrintError("ERROR: Spawn list for \"MedicalSpawnList\" not found in \"lootslist.cfg\".", false);
							result = false;
						}
						else
						{
							List<string> list6 = list;
							if (Override.predicate_4 == null)
							{
								Override.predicate_4 = new Predicate<string>(Override.smethod_10);
							}
							if (!list6.Exists(Override.predicate_4))
							{
								ConsoleSystem.PrintError("ERROR: Spawn list for \"WeaponSpawnList\" not found in \"lootslist.cfg\".", false);
								result = false;
							}
							else
							{
								List<string> list7 = list;
								if (Override.predicate_5 == null)
								{
									Override.predicate_5 = new Predicate<string>(Override.smethod_11);
								}
								if (!list7.Exists(Override.predicate_5))
								{
									ConsoleSystem.PrintError("ERROR: Spawn list for \"SupplyDropSpawnListMaster\" not found in \"lootslist.cfg\".", false);
									result = false;
								}
								else
								{
									DatablockDictionary._lootSpawnLists.Clear();
									Dictionary<string, LootSpawnList> dictionary = new Dictionary<string, LootSpawnList>();
									foreach (string current in list)
									{
										string text = current.Trim();
										if (!string.IsNullOrEmpty(text) && !text.StartsWith("//"))
										{
											if (text.Contains("//"))
											{
												text = text.Split(new string[]
												{
													"//"
												}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
											}
											if (!string.IsNullOrEmpty(text) && text.StartsWith("[") && text.EndsWith("]"))
											{
												text = text.Substring(1, text.Length - 2);
												dictionary[text] = ScriptableObject.CreateInstance<LootSpawnList>();
											}
										}
									}
									LootSpawnList lootSpawnList = null;
									List<LootSpawnList.LootWeightedEntry> list8 = null;
									foreach (string current2 in list)
									{
										string text2 = current2.Trim();
										if (!string.IsNullOrEmpty(text2) && !text2.StartsWith("//"))
										{
											if (text2.Contains("//"))
											{
												text2 = text2.Split(new string[]
												{
													"//"
												}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
											}
											if (!string.IsNullOrEmpty(text2))
											{
												if (text2.StartsWith("[") && text2.EndsWith("]"))
												{
													string text3 = text2.Substring(1, text2.Length - 2);
													lootSpawnList = dictionary[text3];
													lootSpawnList.name = text3;
													DatablockDictionary._lootSpawnLists.Add(lootSpawnList.name, lootSpawnList);
													list8 = new List<LootSpawnList.LootWeightedEntry>();
												}
												else if (text2.Contains("=") && lootSpawnList != null)
												{
													string[] array = text2.Split(new char[]
													{
														'='
													});
													string a;
													if (array.Length >= 2 && (a = array[0].ToUpper()) != null)
													{
														if (!(a == "PACKAGESTOSPAWN"))
														{
															if (!(a == "SPAWNONEOFEACH"))
															{
																if (!(a == "NODUPLICATES"))
																{
																	if (!(a == "PACKAGELIST"))
																	{
																		if (a == "PACKAGEITEM")
																		{
																			array = array[1].Split(new string[]
																			{
																				"\t"
																			}, StringSplitOptions.RemoveEmptyEntries);
																			LootSpawnList.LootWeightedEntry lootWeightedEntry = new LootSpawnList.LootWeightedEntry();
																			lootWeightedEntry.obj = DatablockDictionary.GetByName(array[1]);
																			if (lootWeightedEntry.obj == null)
																			{
																				ConsoleSystem.LogError(string.Format("Package {0} has a reference to an item named {1}, but it not exist.", lootSpawnList.name, array[1]));
																			}
																			else
																			{
																				float.TryParse(array[0], out lootWeightedEntry.weight);
																				int.TryParse(array[2], out lootWeightedEntry.amountMin);
																				int.TryParse(array[3], out lootWeightedEntry.amountMax);
																				list8.Add(lootWeightedEntry);
																				lootSpawnList.LootPackages = list8.ToArray();
																			}
																		}
																	}
																	else
																	{
																		array = array[1].Split(new string[]
																		{
																			"\t"
																		}, StringSplitOptions.RemoveEmptyEntries);
																		LootSpawnList.LootWeightedEntry lootWeightedEntry = new LootSpawnList.LootWeightedEntry();
																		if (!dictionary.ContainsKey(array[1]))
																		{
																			ConsoleSystem.LogError(string.Format("Package {0} has a reference to an spawn list named {1}, but it not exist.", lootSpawnList.name, array[1]));
																		}
																		else
																		{
																			lootWeightedEntry.obj = dictionary[array[1]];
																			float.TryParse(array[0], out lootWeightedEntry.weight);
																			int.TryParse(array[2], out lootWeightedEntry.amountMin);
																			int.TryParse(array[3], out lootWeightedEntry.amountMax);
																			list8.Add(lootWeightedEntry);
																			lootSpawnList.LootPackages = list8.ToArray();
																		}
																	}
																}
																else
																{
																	bool.TryParse(array[1], out lootSpawnList.noDuplicates);
																}
															}
															else
															{
																bool.TryParse(array[1], out lootSpawnList.spawnOneOfEach);
															}
														}
														else
														{
															if (array[1].Contains(","))
															{
																array = array[1].Split(new char[]
																{
																	','
																});
															}
															else
															{
																array = new string[]
																{
																	array[1],
																	array[1]
																};
															}
															int.TryParse(array[0], out lootSpawnList.minPackagesToSpawn);
															int.TryParse(array[1], out lootSpawnList.maxPackagesToSpawn);
														}
													}
												}
											}
										}
									}
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		private static bool smethod_1()
		{
			using (StreamWriter streamWriter = File.CreateText(Override.ItemsFile))
			{
				streamWriter.WriteLine("// - Help of item properties -");
				streamWriter.WriteLine("// SlotFlags = Flags [Belt|Chest|Cooked|Debris|Equip|Feet|FuelBasic|Head|Legs|Raw|Safe|Storage]");
				streamWriter.WriteLine("// TransientMode = Flags [None|Untransferable|DoesNotSave|Full]");
				streamWriter.WriteLine("// Changed properties change only on server side. But it work.");
				streamWriter.WriteLine("");
				ItemDataBlock[] all = DatablockDictionary.All;
				for (int i = 0; i < all.Length; i++)
				{
					ItemDataBlock itemDataBlock = all[i];
					streamWriter.WriteLine("[" + itemDataBlock.name + "]");
					streamWriter.WriteLine("Description=" + itemDataBlock.GetItemDescription());
					streamWriter.WriteLine("IsRepairable=" + itemDataBlock.isRepairable);
					streamWriter.WriteLine("IsRecycleable=" + itemDataBlock.isRecycleable);
					streamWriter.WriteLine("IsResearchable=" + itemDataBlock.isResearchable);
					streamWriter.WriteLine("IsSplittable=" + itemDataBlock._splittable);
					streamWriter.WriteLine("TransientMode=" + itemDataBlock.transientMode);
					streamWriter.WriteLine("LoseDurability=" + itemDataBlock.doesLoseCondition);
					streamWriter.WriteLine("MaxDurability=" + itemDataBlock._maxCondition);
					BlueprintDataBlock blueprintDataBlock;
					if (Override.smethod_3(itemDataBlock, out blueprintDataBlock))
					{
						string text = "";
						BlueprintDataBlock.IngredientEntry[] ingredients = blueprintDataBlock.ingredients;
						for (int j = 0; j < ingredients.Length; j++)
						{
							BlueprintDataBlock.IngredientEntry ingredientEntry = ingredients[j];
							object obj = text;
							text = string.Concat(new object[]
							{
								obj,
								ingredientEntry.amount,
								" \"",
								ingredientEntry.Ingredient.name,
								"\", "
							});
						}
						if (text.Length > 0)
						{
							text = text.Substring(0, text.Length - 2);
						}
						streamWriter.WriteLine("Crafting.Ingredients=" + text);
						streamWriter.WriteLine("Crafting.RequireWorkbench=" + blueprintDataBlock.RequireWorkbench);
						streamWriter.WriteLine("Crafting.Duration=" + blueprintDataBlock.craftingDuration);
					}
					streamWriter.WriteLine("MinUses=" + itemDataBlock._minUsesForDisplay);
					streamWriter.WriteLine("MaxUses=" + itemDataBlock._maxUses);
					streamWriter.WriteLine();
				}
			}
			return true;
		}

		private static bool smethod_2()
		{
			ItemDataBlock itemDataBlock = null;
			BlueprintDataBlock blueprintDataBlock = null;
			List<string> list = File.ReadAllLines(Override.ItemsFile).ToList<string>();
			foreach (string current in list)
			{
				string text = current.Trim();
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("//"))
				{
					if (text.Contains("//"))
					{
						text = text.Split(new string[]
						{
							"//"
						}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
					}
					if (!string.IsNullOrEmpty(text))
					{
						if (text.StartsWith("[") && text.EndsWith("]"))
						{
							string text2 = text.Substring(1, text.Length - 2);
							itemDataBlock = DatablockDictionary.GetByName(text2);
							if (itemDataBlock != null)
							{
								Override.ItemsCount++;
							}
							else
							{
								ConsoleSystem.LogError(string.Format("Item named of {0} not exist in dictionary.", text2));
							}
						}
						else if (text.Contains("=") && itemDataBlock != null)
						{
							string[] array = text.Split(new char[]
							{
								'='
							});
							string key;
							if (array.Length >= 2 && (key = array[0].ToUpper()) != null)
							{
								if (Override.asas == null)
								{
									Override.asas = new Dictionary<string, int>(13)
									{
										{
											"DESCRIPTION",
											0
										},
										{
											"ISREPAIRABLE",
											1
										},
										{
											"ISRECYCLEABLE",
											2
										},
										{
											"ISRESEARCHABLE",
											3
										},
										{
											"ISSPLITTABLE",
											4
										},
										{
											"TRANSIENTMODE",
											5
										},
										{
											"LOSEDURABILITY",
											6
										},
										{
											"MAXDURABILITY",
											7
										},
										{
											"CRAFTING.INGREDIENTS",
											8
										},
										{
											"CRAFTING.REQUIREWORKBENCH",
											9
										},
										{
											"CRAFTING.DURATION",
											10
										},
										{
											"MINUSES",
											11
										},
										{
											"MAXUSES",
											12
										}
									};
								}
								int num;
								if (Override.asas.TryGetValue(key, out num))
								{
									switch (num)
									{
									case 0:
										itemDataBlock.itemDescriptionOverride = array[1];
										break;
									case 1:
										itemDataBlock.isRepairable = bool.Parse(array[1]);
										break;
									case 2:
										itemDataBlock.isRecycleable = bool.Parse(array[1]);
										break;
									case 3:
										itemDataBlock.isResearchable = bool.Parse(array[1]);
										break;
									case 4:
										itemDataBlock._splittable = bool.Parse(array[1]);
										break;
									case 5:
										if (array[1].IndexOf("full", StringComparison.OrdinalIgnoreCase) >= 0)
										{
											itemDataBlock.transientMode = ItemDataBlock.TransientMode.Full;
										}
										if (array[1].IndexOf("doesnotsave", StringComparison.OrdinalIgnoreCase) >= 0)
										{
											itemDataBlock.transientMode = ItemDataBlock.TransientMode.DoesNotSave;
										}
										if (array[1].IndexOf("untransferable", StringComparison.OrdinalIgnoreCase) >= 0)
										{
											itemDataBlock.transientMode = ItemDataBlock.TransientMode.Untransferable;
										}
										if (array[1].IndexOf("none", StringComparison.OrdinalIgnoreCase) >= 0)
										{
											itemDataBlock.transientMode = ItemDataBlock.TransientMode.None;
										}
										break;
									case 6:
										itemDataBlock.doesLoseCondition = bool.Parse(array[1]);
										break;
									case 7:
										itemDataBlock._maxCondition = float.Parse(array[1]);
										break;
									case 8:
										if (Override.smethod_3(itemDataBlock, out blueprintDataBlock))
										{
											string[] array2 = array[1].Split(new char[]
											{
												','
											});
											List<BlueprintDataBlock.IngredientEntry> list2 = new List<BlueprintDataBlock.IngredientEntry>();
											string[] array3 = array2;
											for (int i = 0; i < array3.Length; i++)
											{
												string input = array3[i];
												string[] array4 = Facepunch.Utility.String.SplitQuotesStrings(input);
												if (array4.Length < 2)
												{
													array4 = new string[]
													{
														"1",
														array4[0]
													};
												}
												ItemDataBlock byName = DatablockDictionary.GetByName(array4[1]);
												if (byName != null)
												{
													list2.Add(new BlueprintDataBlock.IngredientEntry
													{
														amount = int.Parse(array4[0]),
														Ingredient = byName
													});
												}
												else
												{
													ConsoleSystem.LogError(string.Format("Blueprint ingredient {0} not exist for item {1}.", array4[1], itemDataBlock.name));
												}
											}
											blueprintDataBlock.ingredients = list2.ToArray();
										}
										else
										{
											ConsoleSystem.LogError(string.Format("Blueprint for item {1} not exist.", itemDataBlock.name));
										}
										break;
									case 9:
										if (Override.smethod_3(itemDataBlock, out blueprintDataBlock))
										{
											blueprintDataBlock.RequireWorkbench = bool.Parse(array[1]);
										}
										else
										{
											ConsoleSystem.LogError(string.Format("Blueprint for item {1} not exist.", itemDataBlock.name));
										}
										break;
									case 10:
										if (Override.smethod_3(itemDataBlock, out blueprintDataBlock))
										{
											blueprintDataBlock.craftingDuration = float.Parse(array[1]);
										}
										else
										{
											ConsoleSystem.LogError(string.Format("Blueprint for item {1} not exist.", itemDataBlock.name));
										}
										break;
									case 11:
										itemDataBlock._minUsesForDisplay = int.Parse(array[1]);
										break;
									case 12:
										itemDataBlock._maxUses = int.Parse(array[1]);
										break;
									}
								}
							}
						}
					}
				}
			}
			return true;
		}

		private static bool smethod_3(ItemDataBlock itemDataBlock_0, out BlueprintDataBlock blueprintDataBlock_0)
		{
			ItemDataBlock[] all = DatablockDictionary.All;
			bool result;
			for (int i = 0; i < all.Length; i++)
			{
				ItemDataBlock itemDataBlock = all[i];
				BlueprintDataBlock blueprintDataBlock = itemDataBlock as BlueprintDataBlock;
				if (blueprintDataBlock != null && blueprintDataBlock.resultItem == itemDataBlock_0)
				{
					blueprintDataBlock_0 = blueprintDataBlock;
					result = true;
					return result;
				}
			}
			blueprintDataBlock_0 = null;
			result = false;
			return result;
		}

		public static void InitializeSpawnsFile()
		{
			Override.SpawnsInitialized = false;
			Override.SpawnsFileCreated = false;
			Override.SpawnsCount = 0;
			if (!File.Exists(Override.SpawnsFile))
			{
				Override.SpawnsFileCreated = Override.smethod_4();
			}
			else if (Core.OverrideSpawns)
			{
				Override.SpawnsInitialized = Override.smethod_5();
			}
		}

		private static bool smethod_4()
		{
			GenericSpawner[] array = UnityEngine.Object.FindObjectsOfType<GenericSpawner>();
			using (StreamWriter streamWriter = File.CreateText(Override.SpawnsFile))
			{
				GenericSpawner[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					GenericSpawner genericSpawner = array2[i];
					streamWriter.WriteLine("[" + genericSpawner.name + "]");
					streamWriter.WriteLine("RADIUS=" + genericSpawner.radius);
					streamWriter.WriteLine("THINKDELAY=" + genericSpawner.thinkDelay);
					streamWriter.WriteLine("POSITION=" + genericSpawner.transform.position.AsString());
					streamWriter.WriteLine("//\tPrefabName\tMaxPopulation\tSpawnPerTick\tUseNavmeshSample\tForceStatic");
					foreach (GenericSpawnerSpawnList.GenericSpawnInstance current in genericSpawner._spawnList)
					{
						streamWriter.Write("SPAWN=" + current.prefabName);
						streamWriter.Write(",\t\t" + current.targetPopulation);
						streamWriter.Write(",\t" + current.numToSpawnPerTick);
						streamWriter.Write(",\t" + current.useNavmeshSample);
						streamWriter.Write(",\t" + current.forceStaticInstantiate);
						streamWriter.WriteLine();
					}
					streamWriter.WriteLine();
					UnityEngine.Object.Destroy(genericSpawner);
				}
			}
			return true;
		}

		private static bool smethod_5()
		{
			GenericSpawner[] array = UnityEngine.Object.FindObjectsOfType<GenericSpawner>();
			for (int i = 0; i < array.Length; i++)
			{
				GenericSpawner obj = array[i];
				UnityEngine.Object.Destroy(obj);
			}
			List<string> list = File.ReadAllLines(Override.SpawnsFile).ToList<string>();
			bool result;
			if (list.Count == 0)
			{
				result = false;
			}
			else
			{
				GenericSpawner genericSpawner = null;
				foreach (string current in list)
				{
					string text = current.Trim();
					if (!string.IsNullOrEmpty(text) && !text.StartsWith("//"))
					{
						if (text.Contains("//"))
						{
							text = text.Split(new string[]
							{
								"//"
							}, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
						}
						if (!string.IsNullOrEmpty(text))
						{
							if (text.StartsWith("[") && text.EndsWith("]"))
							{
								text.Substring(1, text.Length - 2);
								genericSpawner = null;
							}
							else if (text.Contains("=") && genericSpawner != null)
							{
								string[] array2 = text.Replace(" ", "").Split(new char[]
								{
									'='
								});
								string a;
								if (array2.Length >= 2 && (a = array2[0].ToUpper()) != null)
								{
									if (!(a == "RADIUS"))
									{
										if (!(a == "THINKDELAY"))
										{
											if (!(a == "POSITION"))
											{
												if (a == "SPAWN")
												{
													array2 = array2[1].Replace(" ", "").Split(new char[]
													{
														','
													});
													GenericSpawnerSpawnList.GenericSpawnInstance genericSpawnInstance = new GenericSpawnerSpawnList.GenericSpawnInstance();
													if (array2.Length > 0)
													{
														genericSpawnInstance.prefabName = array2[0];
													}
													if (array2.Length > 1 && int.TryParse(array2[1], out genericSpawnInstance.targetPopulation))
													{
														genericSpawnInstance.targetPopulation = 1;
													}
													if (array2.Length > 2 && int.TryParse(array2[2], out genericSpawnInstance.numToSpawnPerTick))
													{
														genericSpawnInstance.numToSpawnPerTick = 1;
													}
													if (genericSpawnInstance.numToSpawnPerTick > genericSpawnInstance.targetPopulation)
													{
														genericSpawnInstance.numToSpawnPerTick = genericSpawnInstance.targetPopulation;
													}
													if (array2.Length > 3 && bool.TryParse(array2[3], out genericSpawnInstance.useNavmeshSample))
													{
														genericSpawnInstance.useNavmeshSample = false;
													}
													if (array2.Length > 4 && bool.TryParse(array2[4], out genericSpawnInstance.forceStaticInstantiate))
													{
														genericSpawnInstance.forceStaticInstantiate = false;
													}
													genericSpawner._spawnList.Add(genericSpawnInstance);
													Override.SpawnsCount++;
												}
											}
											else
											{
												array2 = array2[1].Replace(" ", "").Split(new char[]
												{
													','
												});
												if (array2.Length < 3)
												{
													UnityEngine.Object.Destroy(genericSpawner);
													genericSpawner = null;
												}
												else
												{
													float x = 0f;
													if (!float.TryParse(array2[0], out x))
													{
														UnityEngine.Object.Destroy(genericSpawner);
														genericSpawner = null;
													}
													else
													{
														float y = 0f;
														if (!float.TryParse(array2[1], out y))
														{
															UnityEngine.Object.Destroy(genericSpawner);
															genericSpawner = null;
														}
														else
														{
															float z = 0f;
															if (!float.TryParse(array2[2], out z))
															{
																UnityEngine.Object.Destroy(genericSpawner);
																genericSpawner = null;
															}
															else
															{
																genericSpawner.transform.position = new Vector3(x, y, z);
															}
														}
													}
												}
											}
										}
										else if (!float.TryParse(array2[1], out genericSpawner.thinkDelay))
										{
											genericSpawner.thinkDelay = 60f;
										}
									}
									else if (!float.TryParse(array2[1], out genericSpawner.radius))
									{
										genericSpawner.radius = 50f;
									}
								}
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		public static bool DamageOverride(TakeDamage take, ref DamageEvent damage, ref TakeDamage.Quantity quantity)
		{
			bool result;
			if (damage.attacker.idMain == damage.victim.idMain)
			{
				result = true;
			}
			else if (Core.OverrideDamage && !float.IsInfinity(damage.amount))
			{
				if (damage.victim.id.GetComponent<Character>() == null && damage.attacker.client != null)
				{
					ulong num = damage.victim.client ? damage.victim.client.userID : 0uL;
					if (damage.victim.idMain is DeployableObject)
					{
						num = (damage.victim.idMain as DeployableObject).ownerID;
					}
					if (damage.victim.idMain is StructureComponent)
					{
						num = (damage.victim.idMain as StructureComponent)._master.ownerID;
					}
					ulong num2 = damage.attacker.client ? damage.attacker.client.userID : 0uL;
					if (damage.attacker.idMain is DeployableObject)
					{
						num2 = (damage.attacker.idMain as DeployableObject).ownerID;
					}
					if (damage.attacker.idMain is StructureComponent)
					{
						num2 = (damage.attacker.idMain as StructureComponent)._master.ownerID;
					}
					if ((num == num2 || Users.SharedGet(num, num2)) && (Core.OwnershipDestroy || Core.DestoryOwnership.ContainsKey(damage.attacker.client.userID)))
					{
						Config.Get("OVERRIDE.DAMAGE", damage.attacker.idMain.name.Replace("(Clone)", "") + ".DAMAGE", ref damage.amount, true);
						result = true;
						return result;
					}
				}
				bool flag = true;
				if (damage.attacker.client && damage.attacker.idMain is Character)
				{
					WeaponImpact weaponImpact = damage.extraData as WeaponImpact;
					string text = (weaponImpact != null) ? weaponImpact.dataBlock.name : "Hunting Bow";
					string text2 = text.Replace(" ", "") + ".DAMAGE";
					string key = text2 + "." + damage.victim.idMain.name.Replace("(Clone)", "");
					string text3 = text2 + ".HEADSHOT";
					if (Config.Get("OVERRIDE.DAMAGE", key, ref flag, true) && !flag)
					{
						result = false;
						return result;
					}
					float[] array = (weaponImpact != null) ? new float[]
					{
						weaponImpact.dataBlock.damageMin,
						weaponImpact.dataBlock.damageMax
					} : new float[]
					{
						75f,
						75f
					};
					Config.Get("OVERRIDE.DAMAGE", ((int)damage.bodyPart == 9) ? text3 : text2, ref array, true);
					damage.amount = UnityEngine.Random.Range(Math.Min(array[0], array[1]), Math.Max(array[0], array[1]));
					if (weaponImpact != null && damage.extraData is BulletWeaponDataBlock)
					{
						damage.amount *= ((damage.extraData as BulletWeaponDataBlock).IsSilenced(weaponImpact.itemRep) ? 0.8f : 1f);
					}
					if (weaponImpact != null && damage.extraData is BulletWeaponImpact)
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
						Override.ApplyDamageTypeList(take, ref damage, quantity.DamageTypeList);
					}
					Helper.Log(string.Concat(new object[]
					{
						"Damage: ",
						damage.attacker.idMain,
						"[",
						damage.attacker.networkViewID,
						"] from ",
						text,
						" hit ",
						damage.victim.idMain,
						"[",
						damage.victim.networkViewID,
						"] on ",
						damage.amount,
						"(",
						array[0],
						"-",
						array[1],
						") pts."
					}), false);
				}
				else if (!(damage.attacker.idMain is Character))
				{
					float num3 = 0f;
					float num4 = 0f;
					if (damage.attacker.id is TimedGrenade)
					{
						num3 = (damage.attacker.id as TimedGrenade).damage;
						num4 = (damage.attacker.id as TimedGrenade).explosionRadius;
					}
					if (damage.attacker.id is TimedExplosive)
					{
						num3 = (damage.attacker.id as TimedExplosive).damage;
						num4 = (damage.attacker.id as TimedExplosive).explosionRadius;
					}
					if (damage.attacker.id is SpikeWall)
					{
						num3 = (damage.attacker.id as SpikeWall).baseReturnDmg;
						num4 = 0f;
					}
					if (num3 > 0f)
					{
						string text4 = damage.attacker.idMain.name.Replace("(Clone)", "") + ".DAMAGE";
						string key2 = text4 + "." + damage.victim.idMain.name.Replace("(Clone)", "");
						if (Config.Get("OVERRIDE.DAMAGE", key2, ref flag, true) && !flag)
						{
							result = false;
							return result;
						}
						Config.Get("OVERRIDE.DAMAGE", text4, ref num3, true);
						if (num4 > 0f)
						{
							Vector3 center = damage.victim.idMain.collider.bounds.center;
							Vector3 center2 = damage.attacker.idMain.collider.bounds.center;
							Vector3 vector = center - center2;
							float num5 = Vector3.Distance(center2, center);
							RaycastHit raycastHit;
							bool flag2;
							MeshBatchInstance meshBatchInstance;
                            if (MeshBatchPhysics.Raycast(center2, vector, out raycastHit, num5, 271975425, out flag2, out meshBatchInstance))
							{
								IDMain iDMain = flag2 ? meshBatchInstance.idMain : IDBase.GetMain(raycastHit.collider);
								GameObject x = (iDMain != null) ? iDMain.gameObject : raycastHit.collider.gameObject;
								if (x != damage.victim.idMain.gameObject)
								{
									damage.amount = (1f - Mathf.Clamp01(num5 / num4)) * num3;
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
							Override.ApplyDamageTypeList(take, ref damage, quantity.DamageTypeList);
						}
					}
					Helper.Log(string.Concat(new object[]
					{
						"Damage: ",
						damage.attacker.idMain,
						"[",
						damage.attacker.networkViewID,
						"] owned ",
						damage.attacker.client,
						" hit ",
						damage.victim.idMain,
						"[",
						damage.victim.networkViewID,
						"] on ",
						damage.amount,
						"(",
						num3,
						") pts."
					}), false);
				}
				result = true;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public static void ApplyDamageTypeList(TakeDamage takeDamage, ref DamageEvent damage, DamageTypeList damageType)
		{
			ProtectionTakeDamage protectionTakeDamage = takeDamage as ProtectionTakeDamage;
			string str = damage.victim.idMain.name.Replace("(Clone)", "");
			for (int i = 0; i < 6; i++)
			{
				DamageTypeFlags damageTypeFlags = (DamageTypeFlags)(1 << i);
				float num = (protectionTakeDamage != null) ? protectionTakeDamage.GetArmorValue(i) : 0f;
				string key = str + "." + damageTypeFlags.ToString().Replace("damage_", "");
				if (!(damage.victim.idMain is Character))
				{
					Config.Get("OVERRIDE.ARMOR", key, ref num, true);
				}
				if (num > 0f && damageType[i] > 0f)
				{
					int index;
					damageType[index = i] = damageType[index] * Mathf.Clamp01(1f - num / 200f);
				}
				if (!Mathf.Approximately(damageType[i], 0f))
				{
					damage.damageTypes |= damageTypeFlags;
					damage.amount += damageType[i];
				}
			}
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
	}
}
