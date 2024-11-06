using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour
{
    public Level Level = new();

    public Inventory Inventory => GetComponent<Inventory>();

    [Inject] private readonly ViewManager _viewManager;
    [Inject] private readonly GameManager _gameManager;

    public PlayerStats PlayerStats { get; private set; }

    public int Coins { get; private set; }
    //public Equipment Equipment;

    private void Awake()
    {
        Inventory.Initialize();
    }

    private void Start()
    {
        SetBaseEquipment();
    }

    private void OnEnable()
    {
        EventManager.AddListener((InventoryUpdateEvent e) => EventManager.Broadcast(Events.DataUpdateEvent));
        EventManager.AddListener((LevelChangeEvent e) => EventManager.Broadcast(Events.DataUpdateEvent));
    }

    private void OnDisable()
    {
        EventManager.RemoveListener((InventoryUpdateEvent e) => EventManager.Broadcast(Events.DataUpdateEvent));
        EventManager.RemoveListener((LevelChangeEvent e) => EventManager.Broadcast(Events.DataUpdateEvent));
    }

    public bool ItemEquipped(InventoryItem<Item> item)
    {
        if (item.Item == Inventory.EquippedShoes?.Item || item.Item == Inventory.EquippedPants?.Item || item.Item == Inventory.EquippedOuterwear?.Item || item.Item == Inventory.EquippedRing?.Item)
            return true;
        else return false;
    }

    private void UpdatePlayerStats()
    {
        PlayerStats = new PlayerStats
        (
            Level.GetBaseStreamCapacity(),
            Level.GetBaseHP(),
            Inventory
        );
        StatsChangeEvent evt = new();
        evt.Stats = PlayerStats;
        EventManager.Broadcast(evt);
        _gameManager.UpdatePlayerStats(PlayerStats);
    }

    private void SetBaseEquipment()
    {
        Inventory.SetDefaultEquipment();
        UpdatePlayerStats();
    }

    public void Equip<T>(InventoryItem<T> item) where T : Item
    {
        if (item.Item is not IEquippable) return;
        item.Initialize();
        switch (item.Item)
        {
            case ShoesItem shoes:
                Inventory.EquippedShoes.Item = shoes;
                Inventory.EquippedShoes.Level = item.Level;
                Inventory.EquippedShoes.Amount = item.Amount;
                break;
            case PantsItem pants:
                Inventory.EquippedPants.Item = pants;
                Inventory.EquippedPants.Level = item.Level;
                Inventory.EquippedPants.Amount = item.Amount;
                break;
            case OuterwearItem outer:
                Inventory.EquippedOuterwear.Item = outer;
                Inventory.EquippedOuterwear.Level = item.Level;
                Inventory.EquippedOuterwear.Amount = item.Amount;
                break;
            case RingItem ring:
                Inventory.EquippedRing.Item = ring;
                Inventory.EquippedRing.Level = item.Level;
                Inventory.EquippedRing.Amount = item.Amount;
                break;
        }
        UpdatePlayerStats();
        EventManager.Broadcast(Events.DataUpdateEvent);
        //DisplayItem(item);
    }

    //private void DisplayItem<T>(InventoryItem<T> item) where T : Item
    //{
    //    _viewManager.GetView<InventoryView>().CharacterCustomizeTab.DisplayEquippedItem(item);
    //}
}

[Serializable]
public class PlayerStats
{
    public int StreamCapacity { get; private set; }
    public int HP { get; private set; }
    public int EvasionChance { get; private set; }
    public float StreamRegen { get; private set; }
    public int BarrierDurability { get; private set; }
    public AttackType AttackType { get; private set; }
    public int Damage { get; private set; }
    public int UltimateDamage { get; private set; }
    public float AttackRate { get; private set; }
    public int AttackStreamCost { get; private set; }

    private readonly Inventory _inv;

    public PlayerStats(int baseStream, int baseHP, Inventory inv)
    {
        StreamCapacity = baseStream + (inv.EquippedShoes?.Item?.StreamCapacityBonus ?? 0) + (inv.EquippedPants?.Item?.StreamCapacityBonus ?? 0) + (inv.EquippedOuterwear?.Item?.StreamCapacityBonus ?? 0);
        HP = baseHP + (inv.EquippedShoes?.Item?.MaxHPBonus ?? 0) + (inv.EquippedPants?.Item?.MaxHPBonus ?? 0) + (inv.EquippedOuterwear?.Item?.MaxHPBonus ?? 0);
        EvasionChance = inv.EquippedShoes?.Item?.EvasionChance ?? 0;
        StreamRegen = inv.EquippedPants?.Item?.StreamRegenBonus ?? 0;
        BarrierDurability = inv.EquippedOuterwear?.Item?.BarrierDurability ?? 0;
        AttackType = inv.EquippedRing?.Item?.AttackType ?? AttackType.Explosion;
        Damage = inv.EquippedRing?.Item?.Damage ?? 0;
        UltimateDamage = inv.EquippedRing?.Item?.UltimateDamage ?? 0;
        AttackRate = inv.EquippedRing?.Item?.AttackRate ?? 0;
        AttackStreamCost = inv.EquippedRing?.Item?.AttackStreamCost ?? 0;
        _inv = inv;
    }

    public int StreamCapacityWithItem(ClothingItem item)
    {
        Debug.Log(item.name);
        return item switch
        {
            ShoesItem shoes => StreamCapacity - (_inv.EquippedShoes?.Item?.StreamCapacityBonus ?? 0) + shoes.StreamCapacityBonus,
            PantsItem pants => StreamCapacity - (_inv.EquippedPants?.Item?.StreamCapacityBonus ?? 0) + pants.StreamCapacityBonus,
            OuterwearItem outer => StreamCapacity - (_inv.EquippedOuterwear?.Item?.StreamCapacityBonus ?? 0) + outer.StreamCapacityBonus,
            _ => -1,
        };
    }

    public int HPWithItem(ClothingItem item)
    {
        return item switch
        {
            ShoesItem shoes => HP - (_inv.EquippedShoes?.Item?.MaxHPBonus ?? 0) + shoes.MaxHPBonus,
            PantsItem pants => HP - (_inv.EquippedPants?.Item?.MaxHPBonus ?? 0) + pants.MaxHPBonus,
            OuterwearItem outer => HP - (_inv.EquippedOuterwear?.Item?.MaxHPBonus ?? 0) + outer.MaxHPBonus,
            _ => -1,
        };
    }
}
