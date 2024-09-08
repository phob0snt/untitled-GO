using UnityEngine;

[CreateAssetMenu(menuName = "Items/ShoesItem")]
public class ShoesItem : ClothingItem
{
    public override ItemType Type => ItemType.Shoes;
    public ClothSet ClothSet => _clothset;
    [SerializeField] private ClothSet _clothset;
    public int EvasionChance => _currentEvasionChance;
    private int _currentEvasionChance;
    [SerializeField, Min(0)] private int _baseEvasionChance;

    public override void Initialize(int level, int amount)
    {
        base.Initialize(level, amount);
        CalculateStats();
    }

    private void CalculateStats()
    {
        _currentMaxHPBonus = (int)(_baseMaxHPBonus * Mathf.Pow(1.05f, UpgradeLevel - 1));
        _currentStreamCapacityBonus = (int)(_baseStreamCapacityBonus * Mathf.Pow(1.05f, UpgradeLevel - 1));
        _currentEvasionChance = (int)(_baseEvasionChance * Mathf.Pow(1.05f, UpgradeLevel - 1));
    }

    public override void Upgrade()
    {
        base.Upgrade();
        UpgradeLevel++;
        CalculateStats();
    }
}
