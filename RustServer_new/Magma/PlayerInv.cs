using System;

namespace Magma
{
	public class PlayerInv
	{
		private PlayerItem[] _armorItems;

		private PlayerItem[] _barItems;

		private Inventory _inv;

		private PlayerItem[] _items;

		private Player player;

		public PlayerItem[] ArmorItems
		{
			get
			{
				return this._armorItems;
			}
			set
			{
				this._armorItems = value;
			}
		}

		public PlayerItem[] BarItems
		{
			get
			{
				return this._barItems;
			}
			set
			{
				this._barItems = value;
			}
		}

		public int FreeSlots
		{
			get
			{
				return this.GetFreeSlots();
			}
		}

		public Inventory InternalInventory
		{
			get
			{
				return this._inv;
			}
			set
			{
				this._inv = value;
			}
		}

		public PlayerItem[] Items
		{
			get
			{
				return this._items;
			}
			set
			{
				this._items = value;
			}
		}

		public PlayerInv(Player player)
		{
			this.player = player;
			this._inv = player.PlayerClient.controllable.GetComponent<Inventory>();
			this.InitItems();
		}

		public void AddItem(string name)
		{
			this.AddItem(name, 1);
		}

		public void AddItem(string name, int amount)
		{
			string[] args = new string[]
			{
				name,
				amount.ToString()
			};
			ConsoleSystem.Arg arg = new ConsoleSystem.Arg("")
			{
				Args = args
			};
			arg.SetUser(this.player.PlayerClient.netUser);
			inv.give(ref arg);
		}

		public void AddItemTo(string name, int slot)
		{
			this.AddItemTo(name, slot, 1);
		}

		public void AddItemTo(string name, int slot, int amount)
		{
			ItemDataBlock byName = DatablockDictionary.GetByName(name);
			if (byName != null)
			{
				Inventory.Slot.Kind value = Inventory.Slot.Kind.Default;
				if (slot > 29 && slot < 36)
				{
					value = Inventory.Slot.Kind.Belt;
				}
				else if (slot >= 36 && slot < 40)
				{
					value = Inventory.Slot.Kind.Armor;
				}
				this._inv.AddItemSomehow(byName, new Inventory.Slot.Kind?(value), slot, amount);
			}
		}

		public void Clear()
		{
			PlayerItem[] items = this.Items;
			for (int i = 0; i < items.Length; i++)
			{
				PlayerItem playerItem = items[i];
				this._inv.RemoveItem(playerItem.Item);
			}
			PlayerItem[] barItems = this.BarItems;
			for (int j = 0; j < barItems.Length; j++)
			{
				PlayerItem playerItem2 = barItems[j];
				this._inv.RemoveItem(playerItem2.Item);
			}
		}

		public void ClearAll()
		{
			this._inv.Clear();
		}

		public void ClearArmor()
		{
			PlayerItem[] armorItems = this.ArmorItems;
			for (int i = 0; i < armorItems.Length; i++)
			{
				PlayerItem playerItem = armorItems[i];
				this._inv.RemoveItem(playerItem.Item);
			}
		}

		public void ClearBar()
		{
			PlayerItem[] barItems = this.BarItems;
			for (int i = 0; i < barItems.Length; i++)
			{
				PlayerItem playerItem = barItems[i];
				this._inv.RemoveItem(playerItem.Item);
			}
		}

		public void DropAll()
		{
			DropHelper.DropInventoryContents(this.InternalInventory);
		}

		public void DropItem(PlayerItem pi)
		{
			DropHelper.DropItem(this.InternalInventory, pi.Slot);
		}

		public void DropItem(int slot)
		{
			DropHelper.DropItem(this.InternalInventory, slot);
		}

		private int GetFreeSlots()
		{
			int num = 0;
			for (int i = 0; i < this._inv.slotCount - 4; i++)
			{
				if (this._inv.IsSlotFree(i))
				{
					num++;
				}
			}
			return num + 1;
		}

		public bool HasItem(string name)
		{
			return this.HasItem(name, 1);
		}

		public bool HasItem(string name, int number)
		{
			int num = 0;
			PlayerItem[] items = this.Items;
			bool result;
			for (int i = 0; i < items.Length; i++)
			{
				PlayerItem playerItem = items[i];
				if (playerItem.Name == name)
				{
					if (playerItem.UsesLeft >= number)
					{
						bool flag = true;
						result = flag;
						return result;
					}
					num += playerItem.UsesLeft;
				}
			}
			PlayerItem[] barItems = this.BarItems;
			for (int j = 0; j < barItems.Length; j++)
			{
				PlayerItem playerItem2 = barItems[j];
				if (playerItem2.Name == name)
				{
					if (playerItem2.UsesLeft >= number)
					{
						bool flag = true;
						result = flag;
						return result;
					}
					num += playerItem2.UsesLeft;
				}
			}
			PlayerItem[] armorItems = this.ArmorItems;
			for (int k = 0; k < armorItems.Length; k++)
			{
				PlayerItem playerItem3 = armorItems[k];
				if (playerItem3.Name == name)
				{
					if (playerItem3.UsesLeft >= number)
					{
						bool flag = true;
						result = flag;
						return result;
					}
					num += playerItem3.UsesLeft;
				}
			}
			result = (num >= number);
			return result;
		}

		private void InitItems()
		{
			this.Items = new PlayerItem[30];
			this.ArmorItems = new PlayerItem[4];
			this.BarItems = new PlayerItem[6];
			for (int i = 0; i < this._inv.slotCount; i++)
			{
				if (i < 30)
				{
					this.Items[i] = new PlayerItem(ref this._inv, i);
				}
				else if (i < 36)
				{
					this.BarItems[i - 30] = new PlayerItem(ref this._inv, i);
				}
				else if (i < 40)
				{
					this.ArmorItems[i - 36] = new PlayerItem(ref this._inv, i);
				}
			}
		}

		public void MoveItem(int s1, int s2)
		{
			this._inv.MoveItemAtSlotToEmptySlot(this._inv, s1, s2);
		}

		public void RemoveItem(PlayerItem pi)
		{
			PlayerItem[] items = this.Items;
			for (int i = 0; i < items.Length; i++)
			{
				PlayerItem playerItem = items[i];
				if (playerItem == pi)
				{
					this._inv.RemoveItem(pi.Item);
					return;
				}
			}
			PlayerItem[] armorItems = this.ArmorItems;
			for (int j = 0; j < armorItems.Length; j++)
			{
				PlayerItem playerItem2 = armorItems[j];
				if (playerItem2 == pi)
				{
					this._inv.RemoveItem(pi.Item);
					return;
				}
			}
			PlayerItem[] barItems = this.BarItems;
			for (int k = 0; k < barItems.Length; k++)
			{
				PlayerItem playerItem3 = barItems[k];
				if (playerItem3 == pi)
				{
					this._inv.RemoveItem(pi.Item);
					break;
				}
			}
		}

		public void RemoveItem(int slot)
		{
			this._inv.RemoveItem(slot);
		}

		public void RemoveItem(string name, int number)
		{
			int num = number;
			PlayerItem[] items = this.Items;
			for (int i = 0; i < items.Length; i++)
			{
				PlayerItem playerItem = items[i];
				if (playerItem.Name == name)
				{
					if (playerItem.UsesLeft <= num)
					{
						num -= playerItem.UsesLeft;
						if (num < 0)
						{
							num = 0;
						}
						this._inv.RemoveItem(playerItem.Slot);
						if (num != 0)
						{
							goto IL_1FC;
						}
					}
					else
					{
						playerItem.Consume(num);
						num = 0;
					}
					if (num != 0)
					{
						PlayerItem[] armorItems = this.ArmorItems;
						for (int j = 0; j < armorItems.Length; j++)
						{
							PlayerItem playerItem2 = armorItems[j];
							if (playerItem2.Name == name)
							{
								if (playerItem2.UsesLeft <= num)
								{
									num -= playerItem2.UsesLeft;
									if (num < 0)
									{
										num = 0;
									}
									this._inv.RemoveItem(playerItem2.Slot);
									if (num != 0)
									{
										goto IL_1E0;
									}
								}
								else
								{
									playerItem2.Consume(num);
									num = 0;
								}
								if (num != 0)
								{
									PlayerItem[] barItems = this.BarItems;
									for (int k = 0; k < barItems.Length; k++)
									{
										PlayerItem playerItem3 = barItems[k];
										if (playerItem3.Name == name)
										{
											if (playerItem3.UsesLeft > num)
											{
												playerItem3.Consume(num);
												break;
											}
											num -= playerItem3.UsesLeft;
											if (num < 0)
											{
												num = 0;
											}
											this._inv.RemoveItem(playerItem3.Slot);
											if (num == 0)
											{
												break;
											}
										}
									}
									break;
								}
								break;
							}
							IL_1E0:;
						}
					}
					return;
				}
				IL_1FC:;
			}
		}

		public void RemoveItemAll(string name)
		{
			this.RemoveItem(name, 99999);
		}
	}
}
