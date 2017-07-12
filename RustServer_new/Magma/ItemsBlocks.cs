using System;
using System.Collections.Generic;

namespace Magma
{
	public class ItemsBlocks : List<ItemDataBlock>
	{
		public ItemsBlocks(List<ItemDataBlock> items)
		{
			foreach (ItemDataBlock current in items)
			{
				base.Add(current);
			}
		}

		public ItemDataBlock Find(string str)
		{
			ItemDataBlock result;
			foreach (ItemDataBlock current in this)
			{
				if (current.name.ToLower() == str.ToLower())
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}
	}
}
