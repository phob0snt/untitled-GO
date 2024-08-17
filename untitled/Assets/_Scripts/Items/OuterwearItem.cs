using UnityEngine;

[CreateAssetMenu(menuName = "Items/OuterWearItemData")]
public class OuterwearItem : Item, IEquippable, IUpgradable
{
    public ClothSet ClothSet => _clothset;
    [SerializeField] private ClothSet _clothset;
    public int StreamCapacityBonus => _streamCapacityBonus;
    [SerializeField, Min(0)] private int _streamCapacityBonus;
    public int MaxHPBonus => _maxHPBonus;
    [SerializeField, Min(0)] private int _maxHPBonus;
    public int BarrierDurability => _barrierDurability;
    [SerializeField, Min(1)] private int _barrierDurability;

    public void Upgrade()
    {
    }
}
