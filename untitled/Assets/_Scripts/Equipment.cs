public class Equipment
{
    public InventoryItem<OuterwearItem> Outerwear { get; set; }

    public InventoryItem<PantsItem> Pants { get; set; }

    public InventoryItem<ShoesItem> Shoes { get; set; }

    public InventoryItem<RingItem> Ring { get; set; }



    public Equipment(InventoryItem<OuterwearItem> outerwear, InventoryItem<PantsItem> pants,
        InventoryItem<ShoesItem> shoes, InventoryItem<RingItem> ring)
    {
        Outerwear = outerwear;
        Pants = pants;
        Shoes = shoes;
        Ring = ring;
    }
}
