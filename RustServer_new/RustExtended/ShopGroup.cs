using System;

namespace RustExtended
{
	[Serializable]
	public class ShopGroup
	{
		public int Index;

		public string Name;

		public ShopGroup(string group_name, int group_index)
		{
			this.Name = group_name;
			this.Index = group_index;
		}
	}
}
