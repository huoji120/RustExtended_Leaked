namespace RustExtended
{
    using System;
    using System.Collections.Generic;

    public class LoadoutEntry
    {
        public List<BlueprintDataBlock> Blueprints = new List<BlueprintDataBlock>();
        public List<LoadoutItem> LoadoutItems = new List<LoadoutItem>();
        public string Name;
        public List<BlueprintDataBlock> NoCrafting = new List<BlueprintDataBlock>();
        public int[] Ranks;
        public List<LoadoutItem> Requirements = new List<LoadoutItem>();
    }
}

