using System;

[Serializable]
public class Equipment
{
    public InventoryItem<OuterwearItem> Outerwear;
    public InventoryItem<PantsItem> Pants;
    public InventoryItem<ShoesItem> Shoes;
    public InventoryItem<RingItem> Ring;

    public Equipment()
    {
        Outerwear = new InventoryItem<OuterwearItem>();
        Pants = new InventoryItem<PantsItem>();
        Shoes = new InventoryItem<ShoesItem>();
        Ring = new InventoryItem<RingItem>();
    }
}
