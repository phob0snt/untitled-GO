using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public List<InventoryItemData> Items = new();
    public int Level;
    public int Coins;

    public InventoryItemData Outerwear;
    public InventoryItemData Pants;
    public InventoryItemData Shoes;
    public InventoryItemData Ring;
}
