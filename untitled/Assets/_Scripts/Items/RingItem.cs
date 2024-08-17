using UnityEngine;

[CreateAssetMenu(menuName = "Items/RingItemData")]
public class RingItem : Item, IEquippable, IUpgradable
{
    public int Damage => _damage;
    [SerializeField, Min(0)] int _damage;
    public float AttackRate => _attackRate;
    [SerializeField, Min(0.1f)] float _attackRate;
    public float AttackStreamCost => _attackStreamCost;
    [SerializeField, Min(0)] float _attackStreamCost;

    public void Upgrade()
    {
    }
}
