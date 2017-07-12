namespace RustExtended
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    public class ShopItem
    {
        public int BuyPrice;
        public int Index;
        public ItemDataBlock itemData;
        public string Name;
        public int Quantity;
        public int SellPrice;
        public int Slots;

        public ShopItem(int item_index, string item_name, int sell_price, int buy_price, [Optional, DefaultParameterValue(1)] int quantity, [Optional, DefaultParameterValue(-1)] int slots, [Optional, DefaultParameterValue(null)] ItemDataBlock item_data)
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

