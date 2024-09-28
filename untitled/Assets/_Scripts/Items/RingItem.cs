using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/RingItem")]
public class RingItem : Item, IEquippable, IUpgradable
{
    public int UpgradeLevel { get; protected set; }
    public override ItemType Type => ItemType.Ring;
    public AttackType AttackType => _attackType;
    [SerializeField] private AttackType _attackType;
    public int Damage => _damage;
    [SerializeField, Min(0)] int _damage;
    public int UltimateDamage => _ultimateDamage;
    [SerializeField, Min(0)] int _ultimateDamage;
    public float AttackRate => _attackRate;
    [SerializeField, Min(0.1f)] float _attackRate;
    public int AttackStreamCost => _attackStreamCost;
    [SerializeField, Min(0)] int _attackStreamCost;
    public List<UpgradeRequirements> UpgradeRequirements => _upgradeRequirements;

    [SerializeField] private List<UpgradeRequirements> _upgradeRequirements;

    public void Upgrade()
    {
    }
}

public enum AttackType
{
    Explosion,
    Heatwave
}
