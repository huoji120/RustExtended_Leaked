using System;
using System.Collections.Generic;

namespace RustExtended
{
	public class LoadoutEntry
	{
		public string Name;

		public int[] Ranks;

		public List<LoadoutItem> LoadoutItems = new List<LoadoutItem>();

		public List<LoadoutItem> Requirements = new List<LoadoutItem>();

		public List<BlueprintDataBlock> Blueprints = new List<BlueprintDataBlock>();

		public List<BlueprintDataBlock> NoCrafting = new List<BlueprintDataBlock>();
	}
}
