using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/PantsItem")]
public class PantsItem : ClothingItem
{
    public override ItemType Type => ItemType.Pants;
    public ClothSet ClothSet => _clothset;
    [SerializeField] private ClothSet _clothset;
    public float StreamRegenBonus => _currentStreamRegenBonus;
    private float _currentStreamRegenBonus;
    [SerializeField, Min(0)] private float _baseStreamRegenBonus;

    public override void Initialize(int level, int amount)
    {
        base.Initialize(level, amount);
        CalculateStats();
    }

    protected override void CalculateStats()
    {
        base.CalculateStats();
        _currentStreamRegenBonus = RegenWithLevel(UpgradeLevel);
    }

    public float RegenWithLevel(int level)
    {
        return (float)Math.Round(_baseStreamRegenBonus * Mathf.Pow(1.05f, level - 1), 1);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        UpgradeLevel++;
        CalculateStats();
    }
}
