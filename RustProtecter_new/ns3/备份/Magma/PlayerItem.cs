namespace Magma
{
    using System;

    public class PlayerItem
    {
        private Inventory internalInv;
        private int internalSlot;

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
            IInventoryItem item;
            this.internalInv.GetItem(this.internalSlot, out item);
            return item;
        }

        public bool IsEmpty()
        {
            return (this.Item == null);
        }

        public bool TryCombine(PlayerItem pi)
        {
            return ((!this.IsEmpty() && !pi.IsEmpty()) && (this.Item.TryCombine(pi.Item) != InventoryItem.MergeResult.Failed));
        }

        public bool TryStack(PlayerItem pi)
        {
            return ((!this.IsEmpty() && !pi.IsEmpty()) && (this.Item.TryStack(pi.Item) != InventoryItem.MergeResult.Failed));
        }

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
                if (!this.IsEmpty())
                {
                    return this.Item.datablock.name;
                }
                return null;
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
                if (!this.IsEmpty())
                {
                    return this.Item.slot;
                }
                return -1;
            }
        }

        public int UsesLeft
        {
            get
            {
                if (!this.IsEmpty())
                {
                    return this.Item.uses;
                }
                return -1;
            }
            set
            {
                this.Item.SetUses(value);
            }
        }
    }
}

