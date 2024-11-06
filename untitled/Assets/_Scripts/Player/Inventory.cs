using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem<Item>> Items => _items;
    [SerializeField] private List<InventoryItem<Item>> _items;

    public InventoryItem<ShoesItem> EquippedShoes { get; private set; } = new();
    public InventoryItem<PantsItem> EquippedPants { get; private set; } = new();
    public InventoryItem<OuterwearItem> EquippedOuterwear { get; private set; } = new();
    public InventoryItem<RingItem> EquippedRing { get; private set; } = new();


    public void Initialize()
    {
        foreach (var item in Items)
        {
            item.Initialize();
        }

        SetDefaultEquipment();

        InventoryUpdateEvent evt = new();
        evt.Items = Items;
        EventManager.Broadcast(evt);
    }

    public void AddItem(InventoryItem<Item> item)
    {
        _items.Add(item);
        item.Initialize();
        InventoryUpdateEvent evt = new();
        evt.Items = Items;
        EventManager.Broadcast(evt);
    }

    public void LoadInventory(List<InventoryItem<Item>> items)
    {
        _items = items;
        Initialize();
    }

    public void TryToEquipItem(InventoryItem<Item> item)
    {
        if (item.Item is not IEquippable) return;
        
    }

    public void TryToUpgradeItem(IUpgradable item)
    {
        if (item.UpgradeLevel >= IUpgradable.MAX_UPGRADE_LVL) return;
        UpgradeRequirements updradeRequirements = item.UpgradeRequirements.FirstOrDefault(x => x.Level == item.UpgradeLevel);
        if (updradeRequirements == null) return;
        if (!HasEnoughResources(updradeRequirements)) return;

        foreach (var component in updradeRequirements.RequiredComponents)
        {
            for (int i = 0; i < component.Amount; i++)
                RemoveItem(component.UpgradeComponent);
        }

        item.Upgrade();
        _items.Find(x => x.Item == item as Item).Level++;
        InventoryUpdateEvent evt = new();
        evt.Items = Items;
        EventManager.Broadcast(evt);
    }

    private bool HasEnoughResources(UpgradeRequirements requirements)
    {
        foreach (var component in requirements.RequiredComponents)
        {
            int componentsAmount = _items.Find(x => x.Item == component.UpgradeComponent).Amount;
            if (componentsAmount < component.Amount) return false;
        }
        return true;
    }
    public void RemoveItem(Item item)
    {
        InventoryItem<Item> invItem = _items.Find(x => x.Item == item);
        if (invItem == null) return;
        if (invItem.Amount > 1)
            invItem.Amount--;
        else
            _items.Remove(_items.Find(x => x.Item == item));
        InventoryUpdateEvent evt = new();
        evt.Items = Items;
        EventManager.Broadcast(evt);
    }

    public void RemoveItem(InventoryItem<Item> item)
    {
        _items.Remove(item);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public void SetDefaultEquipment()
    {
        //InventoryItem<ShoesItem> shoes = new();
        //shoes.Item = Items.Find(x => x.Item.name == "StarterShoes").Item as ShoesItem;
        //InventoryItem<PantsItem> pants = new();
        //pants.Item = Items.Find(x => x.Item.name == "StarterPants").Item as PantsItem;
        //InventoryItem<OuterwearItem> outer = new();
        //outer.Item = Items.Find(x => x.Item.name == "StarterOuterwear").Item as OuterwearItem;
        //InventoryItem<RingItem> ring = new();
        //ring.Item = Items.Find(x => x.Item.name == "StarterRing").Item as RingItem;

        //GetComponent<Character>().Equip(shoes);
        //GetComponent<Character>().Equip(pants);
        //GetComponent<Character>().Equip(outer);
        //GetComponent<Character>().Equip(ring);

        EquippedShoes.Item = Items.Find(x => x.Item.name == "StarterShoes").Item as ShoesItem;
        EquippedPants.Item = Items.Find(x => x.Item.name == "StarterPants").Item as PantsItem;
        EquippedOuterwear.Item = Items.Find(x => x.Item.name == "StarterOuterwear").Item as OuterwearItem;
        EquippedRing.Item = Items.Find(x => x.Item.name == "StarterRing").Item as RingItem;

    }
}
