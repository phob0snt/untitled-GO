using UnityEngine;
using Zenject;

[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour
{
    public Inventory Inventory => GetComponent<Inventory>();

    [Inject] private ViewManager _viewManager;
    [Inject] private ProgressManager _progressManager;
    [Inject] private GameManager _gameManager;

    public Progress Progress { get; private set; }

    private PlayerStats _playerStats;
    
    private Item _hairSlot;
    private ShoesItem _shoesSlot;
    private PantsItem _pantsSlot;
    private OuterwearItem _outerwearSlot;
    private RingItem _ringSlot;

    private void Awake()
    {
        UpdatePlayerStats();
    }

    public bool CheckItemEquipment(Item item)
    {
        if (item == _shoesSlot || item == _pantsSlot || item == _outerwearSlot || item == _ringSlot)
            return true;
        else return false;
    }

    private void UpdatePlayerStats()
    {
        _playerStats = new PlayerStats
        (
            300 + (_shoesSlot?.StreamCapacityBonus ?? 0) + (_pantsSlot?.StreamCapacityBonus ?? 0) + (_outerwearSlot?.StreamCapacityBonus ?? 0),
            1200 + ((_shoesSlot?.MaxHPBonus ?? 0)) + (_pantsSlot?.MaxHPBonus ?? 0) + (_outerwearSlot?.MaxHPBonus ?? 0),
            0 + (_shoesSlot?.EvasionChance ?? 0),
            0 + (_pantsSlot?.StreamRegenBonus ?? 0),
            0 + (_outerwearSlot?.BarrierDurability ?? 0),
            0 + (_ringSlot?.Damage ?? 0),
            0 + (_ringSlot?.AttackRate ?? 0),
            0 + (_ringSlot?.AttackStreamCost ?? 0)
        );
        _viewManager.GetView<InventoryView>().CharacterCustomizeTab.UpdatePlayerStats(_playerStats);
        _gameManager.UpdatePlayerStats(_playerStats);
    }

    public void Equip(Item item)
    {
        if (item is not IEquippable) return;
        switch (item.Type)
        {
            case ItemType.Shoes:
                _shoesSlot = item as ShoesItem;
                break;
            case ItemType.Pants:
                _pantsSlot = item as PantsItem;
                break;
            case ItemType.Outerwear:
                _outerwearSlot = item as OuterwearItem;
                break;
            case ItemType.Ring:
                _ringSlot = item as RingItem;
                break;
        }
        UpdatePlayerStats();
        DisplayItem(item);
    }

    private void DisplayItem(Item item)
    {
        _viewManager.GetView<InventoryView>().CharacterCustomizeTab.DisplayEquippedItem(item);
    }
}

public struct PlayerStats
{
    public int StreamCapacity { get; private set; }
    public int HP { get; private set; }
    public float StreamRegen { get; private set; }
    public int EvasionChance { get; private set; }
    public int BarrierDurability { get; private set; }
    public int Damage { get; private set; }
    public float AttackRate { get; private set; }
    public float AttackStreamCost { get; private set; }

    public PlayerStats(int streamCapacity, int hp, int evasionChance, float streamRegen, int barrierDurability, int damage, float attackRate, float attackStreamCost)
    {
        StreamCapacity = streamCapacity;
        HP = hp;
        EvasionChance = evasionChance;
        StreamRegen = streamRegen;
        BarrierDurability = barrierDurability;
        Damage = damage;
        AttackRate = attackRate;
        AttackStreamCost = attackStreamCost;
    }
}
