using UnityEngine;

[CreateAssetMenu(menuName = "Items/PantsItemData")]
public class PantsItem : Item, IEquippable, IUpgradable
{
    public ClothSet ClothSet => _clothset;
    [SerializeField] private ClothSet _clothset;
    public int StreamCapacityBonus => _streamCapacityBonus;
    [SerializeField, Min(0)] private int _streamCapacityBonus;
    public int MaxHPBonus => _maxHPBonus;
    [SerializeField, Min(0)] private int _maxHPBonus;
    public float StreamRegenBonus => _streamRegenBonus;
    [SerializeField, Min(0)] private float _streamRegenBonus;

    public void Upgrade()
    {
    }
}
