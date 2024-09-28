using UnityEngine;
using Zenject;

[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour
{
    public Inventory Inventory => GetComponent<Inventory>();
    public Level Level = new();

    [Inject] private ViewManager _viewManager;
    [Inject] private GameManager _gameManager;

    private PlayerStats _playerStats;

    public int Coins { get; private set; }
    public Equipment Equipment { get; private set; }

    private void Awake()
    {
        Equipment ??= new();
        UpdatePlayerStats();
    }

    public void LoadLevel()
    {

    }

    public bool ItemEquipped(InventoryItem<Item> item)
    {
        if (item.Item == Equipment.Shoes?.Item || item.Item == Equipment.Pants?.Item || item.Item == Equipment.Outerwear?.Item || item.Item == Equipment.Ring?.Item)
            return true;
        else return false;
    }

    private void UpdatePlayerStats()
    {
        _playerStats = new PlayerStats
        (
            300 + (Equipment.Shoes?.Item?.StreamCapacityBonus ?? 0) + (Equipment.Pants?.Item?.StreamCapacityBonus ?? 0) + (Equipment.Outerwear?.Item?.StreamCapacityBonus ?? 0),
            1200 + (Equipment.Shoes?.Item?.MaxHPBonus ?? 0) + (Equipment.Pants?.Item?.MaxHPBonus ?? 0) + (Equipment.Outerwear?.Item?.MaxHPBonus ?? 0),
            0 + (Equipment.Shoes?.Item?.EvasionChance ?? 0),
            0 + (Equipment.Pants?.Item?.StreamRegenBonus ?? 0),
            0 + (Equipment.Outerwear?.Item?.BarrierDurability ?? 0),
            Equipment.Ring?.Item?.AttackType ?? AttackType.Explosion,
            0 + (Equipment.Ring?.Item?.Damage ?? 0),
            0 + (Equipment.Ring?.Item?.UltimateDamage ?? 0),
            0 + (Equipment.Ring?.Item?.AttackRate ?? 0),
            0 + (Equipment.Ring?.Item?.AttackStreamCost ?? 0)
        );

        _viewManager.GetView<InventoryView>().CharacterCustomizeTab.UpdatePlayerStats(_playerStats);
        _gameManager.UpdatePlayerStats(_playerStats);
    }

    public void Equip(InventoryItem<Item> item)
    {
        if (item.Item is not IEquippable) return;
        Debug.Log("is equippable");
        switch (item.Item)
        {
            case ShoesItem shoes:
                Equipment.Shoes.Item = shoes;
                Equipment.Shoes.Level = item.Level;
                Equipment.Shoes.Amount = item.Amount;
                break;
            case PantsItem pants:
                Equipment.Pants.Item = pants;
                Equipment.Pants.Level = item.Level;
                Equipment.Pants.Amount = item.Amount;
                break;
            case OuterwearItem outer:
                Equipment.Outerwear.Item = outer;
                Equipment.Outerwear.Level = item.Level;
                Equipment.Outerwear.Amount = item.Amount;
                break;
            case RingItem ring:
                Equipment.Ring.Item = ring;
                Equipment.Ring.Level = item.Level;
                Equipment.Ring.Amount = item.Amount;
                break;
        }
        UpdatePlayerStats();
        DisplayItem(item);
    }

    public void SetEquipment(Equipment equipment)
    {
        Equipment = equipment;
    }

    private void DisplayItem(InventoryItem<Item> item)
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
    public AttackType AttackType { get; private set; }
    public int Damage { get; private set; }
    public int UltimateDamage { get; private set; }
    public float AttackRate { get; private set; }
    public int AttackStreamCost { get; private set; }

    public PlayerStats(int streamCapacity, int hp, int evasionChance, float streamRegen, int barrierDurability, AttackType attack, int damage, int ultimateDamage, float attackRate, int attackStreamCost)
    {
        StreamCapacity = streamCapacity;
        HP = hp;
        EvasionChance = evasionChance;
        StreamRegen = streamRegen;
        BarrierDurability = barrierDurability;
        AttackType = attack;
        Damage = damage;
        UltimateDamage = ultimateDamage;
        AttackRate = attackRate;
        AttackStreamCost = attackStreamCost;
    }
}
