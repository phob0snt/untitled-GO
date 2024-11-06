using System.Collections.Generic;
using UnityEngine;

public abstract class ClothingItem : Item, IUpgradable, IEquippable
{
    public int UpgradeLevel { get; protected set; } = 1;
    public int StreamCapacityBonus => _currentStreamCapacityBonus;
    protected int _currentStreamCapacityBonus;
    [SerializeField, Min(0)] protected int _baseStreamCapacityBonus;
    public int MaxHPBonus => _currentMaxHPBonus;
    protected int _currentMaxHPBonus;
    [SerializeField, Min(0)] protected int _baseMaxHPBonus;
    public List<UpgradeRequirements> UpgradeRequirements => _upgradeRequirements;

    [SerializeField] protected List<UpgradeRequirements> _upgradeRequirements;

    public override void Initialize(int level, int amount)
    {
        UpgradeLevel = level;
    }

    protected virtual void CalculateStats()
    {
        _currentMaxHPBonus = HPWithLevel(UpgradeLevel);
        _currentStreamCapacityBonus = StreamWithLevel(UpgradeLevel);
    }

    public int HPWithLevel(int level)
    {
        return (int)(_baseMaxHPBonus * Mathf.Pow(1.05f, level - 1));
    }

    public int StreamWithLevel(int level)
    {
        return (int)(_baseStreamCapacityBonus * Mathf.Pow(1.05f, level - 1));
    }
    public virtual void Upgrade()
    {
        Debug.Log($"{this} улучшено!!!");
    }
}
