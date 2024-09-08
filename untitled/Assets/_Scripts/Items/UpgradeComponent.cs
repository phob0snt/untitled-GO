using UnityEngine;

public enum ItemRarity
{
    Common,
    Rare,
    HyperRare,
    Legendary
}

public enum ComponentType
{
    Cloth,
    Toolkit,
    StreamConductor,
    StreamGenerator,
    ForceFieldSensor,
    WarpChip
}

[CreateAssetMenu(menuName = "Items/UpgradeComponent")]
public class UpgradeComponent : Item
{
    public override ItemType Type => ItemType.UpgradeComponent;

    public ItemRarity Rarity => _rarity;
    [SerializeField] private ItemRarity _rarity;
    public ComponentType ComponentType => _componentType;
    [SerializeField] ComponentType _componentType;
}
