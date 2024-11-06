using UnityEngine;

[CreateAssetMenu(menuName = "Items/OuterwearItem")]
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
    protected override void CalculateStats()
    {
        base.CalculateStats();
        _currentBarrierDurability = BarrierWithLevel(UpgradeLevel);
    }

    public int BarrierWithLevel(int level)
    {
        return (int)(_baseBarrierDurability * Mathf.Pow(1.05f, level - 1));
    }

    public override void Upgrade()
    {
        base.Upgrade();
        UpgradeLevel++;
        CalculateStats();
    }
}
