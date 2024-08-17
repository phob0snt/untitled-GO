using UnityEngine;

[CreateAssetMenu(menuName = "Items/ShoesItemData")]
public class ShoesItem : Item, IEquippable, IUpgradable
{
    public ClothSet ClothSet => _clothset;
    [SerializeField] private ClothSet _clothset;
    public int StreamCapacityBonus => _streamCapacityBonus;
    [SerializeField, Min(0)] private int _streamCapacityBonus;
    public int MaxHPBonus => _maxHPBonus;
    [SerializeField, Min(0)] private int _maxHPBonus;
    public int EvasionChance => _evasionChance;
    [SerializeField, Min(0)] private int _evasionChance;

    public void Upgrade()
    {

    }
}
