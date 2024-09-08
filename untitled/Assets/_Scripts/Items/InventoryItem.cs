using System;
using UnityEngine;

[Serializable]
public class InventoryItem<T> where T : Item
{
    public T Item;
    [Min(1)] public int Amount;
    [Range(1, IUpgradable.MAX_UPGRADE_LVL)] public int Level;

    public void Initialize()
    {
        Item.Initialize(Level, Amount);
    }
}
