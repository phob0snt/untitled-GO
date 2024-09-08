using System.Collections.Generic;
using UnityEngine;

public abstract class ClothingItem : Item, IUpgradable, IEquippable
{
    public int UpgradeLevel { get; protected set; }
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
    public virtual void Upgrade()
    {
        Debug.Log($"{this} улучшено!!!");
    }
}
