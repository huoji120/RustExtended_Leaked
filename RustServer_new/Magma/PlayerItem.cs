using System;

namespace Magma
{
	public class PlayerItem
	{
		private Inventory internalInv;

		private int internalSlot;

		public IInventoryItem Item
		{
			get
			{
				return this.GetItemRef();
			}
			set
			{
				this.Item = value;
			}
		}

		public string Name
		{
			get
			{
				string result;
				if (!this.IsEmpty())
				{
					result = this.Item.datablock.name;
				}
				else
				{
					result = null;
				}
				return result;
			}
			set
			{
				this.Item.datablock.name = value;
			}
		}

		public int Quantity
		{
			get
			{
				return this.UsesLeft;
			}
			set
			{
				this.UsesLeft = value;
			}
		}

		public int Slot
		{
			get
			{
				int result;
				if (!this.IsEmpty())
				{
					result = this.Item.slot;
				}
				else
				{
					result = -1;
				}
				return result;
			}
		}

		public int UsesLeft
		{
			get
			{
				int result;
				if (!this.IsEmpty())
				{
					result = this.Item.uses;
				}
				else
				{
					result = -1;
				}
				return result;
			}
			set
			{
				this.Item.SetUses(value);
			}
		}

		public PlayerItem()
		{
		}

		public PlayerItem(ref Inventory inv, int slot)
		{
			this.internalInv = inv;
			this.internalSlot = slot;
		}

		public void Consume(int qty)
		{
			if (!this.IsEmpty())
			{
				this.Item.Consume(ref qty);
			}
		}

		public void Drop()
		{
			DropHelper.DropItem(this.internalInv, this.Slot);
		}

		private IInventoryItem GetItemRef()
		{
			IInventoryItem result;
			this.internalInv.GetItem(this.internalSlot, out result);
			return result;
		}

		public bool IsEmpty()
		{
			return this.Item == null;
		}

		public bool TryCombine(PlayerItem pi)
		{
			return !this.IsEmpty() && !pi.IsEmpty() && this.Item.TryCombine(pi.Item) != InventoryItem.MergeResult.Failed;
		}

		public bool TryStack(PlayerItem pi)
		{
			return !this.IsEmpty() && !pi.IsEmpty() && this.Item.TryStack(pi.Item) != InventoryItem.MergeResult.Failed;
		}
	}
}
