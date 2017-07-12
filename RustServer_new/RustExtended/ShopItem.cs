using System;

namespace RustExtended
{
	[Serializable]
	public class ShopItem
	{
		public int Index;

		public string Name;

		public int SellPrice;

		public int BuyPrice;

		public int Quantity;

		public int Slots;

		public ItemDataBlock itemData;

		public ShopItem(int item_index, string item_name, int sell_price, int buy_price, int quantity = 1, int slots = -1, ItemDataBlock item_data = null)
		{
			this.Index = item_index;
			this.Name = item_name;
			this.SellPrice = sell_price;
			this.BuyPrice = buy_price;
			this.Quantity = quantity;
			this.Slots = slots;
			this.itemData = item_data;
		}
	}
}
