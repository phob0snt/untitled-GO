using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public static UnityEvent OnInventoryUpdate = new();
    public List<InventoryItem<Item>> Items => _items;
    [SerializeField] private List<InventoryItem<Item>> _items;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        foreach (var item in _items)
        {
            item.Initialize();
        }
    }

    public void AddItem(InventoryItem<Item> item)
    {
        _items.Add(item);
        OnInventoryUpdate.Invoke();
    }

    public void TryToUpgradeItem(IUpgradable item)
    {
        if (item.UpgradeLevel >= IUpgradable.MAX_UPGRADE_LVL) return;
        Debug.Log(item.UpgradeRequirements);
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
        OnInventoryUpdate.Invoke();
    }

    public void RemoveItem(InventoryItem<Item> item)
    {
        _items.Remove(item);
    }

    public void Clear()
    {
        _items.Clear();
    }
}
