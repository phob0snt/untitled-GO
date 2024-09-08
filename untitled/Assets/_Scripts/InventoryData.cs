using System;
using System.Collections.Generic;

[Serializable]
public class InventoryData
{
    public List<InventoryItemData> Items = new();

    public InventoryItemData Outerwear;
    public InventoryItemData Pants;
    public InventoryItemData Shoes;
    public InventoryItemData Ring;
}
