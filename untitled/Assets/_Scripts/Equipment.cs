using System;

[Serializable]
public class Equipment
{
    public InventoryItem<OuterwearItem> Outerwear;
    public InventoryItem<PantsItem> Pants;
    public InventoryItem<ShoesItem> Shoes;
    public InventoryItem<RingItem> Ring;
}
