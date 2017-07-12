using System;
using System.Collections.Generic;
using UnityEngine;

namespace RustExtended
{
	internal class Magma
	{
		public static ItemDataBlock[] ItemsLoaded(List<ItemDataBlock> items, Dictionary<string, int> stringDB, Dictionary<int, int> idDB)
		{
			return (ItemDataBlock[])Method.Invoke("Magma.Hooks.ItemsLoaded", new object[]
			{
				items,
				stringDB,
				idDB
			}).AsObject;
		}

		public static Dictionary<string, LootSpawnList> TablesLoaded(Dictionary<string, LootSpawnList> lists)
		{
			return (Dictionary<string, LootSpawnList>)Method.Invoke("Magma.Hooks.TablesLoaded", new object[]
			{
				lists
			}).AsObject;
		}

		public static void EntityHurt(object entity, ref DamageEvent e)
		{
			Method.Invoke("Magma.Hooks.EntityHurt", new object[]
			{
				entity,
				e
			});
		}

		public static void NPCHurt(ref DamageEvent e)
		{
			Method.Invoke("Magma.Hooks.NPCHurt", new object[]
			{
				e
			});
		}

		public static void NPCKilled(ref DamageEvent e)
		{
			Method.Invoke("Magma.Hooks.NPCKilled", new object[]
			{
				e
			});
		}

		public static void PlayerHurt(ref DamageEvent e)
		{
			Method.Invoke("Magma.Hooks.PlayerHurt", new object[]
			{
				e
			});
		}

		public static void PlayerSpawned(PlayerClient pc, Vector3 pos, bool camp)
		{
			Method.Invoke("Magma.Hooks.PlayerSpawned", new object[]
			{
				pc,
				pos,
				camp
			});
		}

		public static Vector3 PlayerSpawning(PlayerClient pc, Vector3 pos, bool camp)
		{
			return (Vector3)Method.Invoke("Magma.Hooks.PlayerSpawning", new object[]
			{
				pc,
				pos,
				camp
			}).AsObject;
		}
	}
}
