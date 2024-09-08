using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/OuterWearItem")]
public class OuterwearItem : ClothingItem
{
    public override ItemType Type => ItemType.Outerwear;
    public ClothSet ClothSet => _clothset;
    [SerializeField] private ClothSet _clothset;
    public int BarrierDurability => _currentBarrierDurability;
    private int _currentBarrierDurability;
    [SerializeField, Min(1)] private int _baseBarrierDurability;

    public override void Initialize(int level, int amount)
    {
        base.Initialize(level, amount);
        CalculateStats();
    }
    private void CalculateStats()
    {
        _currentMaxHPBonus = (int)(_baseMaxHPBonus * Mathf.Pow(1.05f, UpgradeLevel - 1));
        _currentStreamCapacityBonus = (int)(_baseStreamCapacityBonus * Mathf.Pow(1.05f, UpgradeLevel - 1));
        _currentBarrierDurability = (int)(_baseBarrierDurability * Mathf.Pow(1.05f, UpgradeLevel - 1));
    }

    public override void Upgrade()
    {
        base.Upgrade();
        UpgradeLevel++;
        CalculateStats();
    }
}
