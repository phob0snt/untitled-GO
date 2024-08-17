using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> Items => _items;
    [SerializeField] private List<Item> _items;

    public void AddItem(Item item)
    {
    }

    public void RemoveItem(Item item)
    {
    }
}
