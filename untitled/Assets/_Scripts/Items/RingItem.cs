using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/RingItem")]
public class RingItem : Item, IEquippable, IUpgradable
{
    public int UpgradeLevel { get; protected set; }
    public override ItemType Type => ItemType.Ring;
    public int Damage => _damage;
    [SerializeField, Min(0)] int _damage;
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
