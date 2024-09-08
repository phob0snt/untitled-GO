using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

public class DataManager : MonoBehaviour
{
    [Inject] private readonly Character _character;
    [SerializeField] private List<Item> _itemsRefs;

    private string _savePath => Application.persistentDataPath + "/inventory.json";

    private void OnDisable()
    {
        SaveData();   
    }

    private void Awake()
    {
        LoadData();
        Debug.Log(_savePath);
    }

    public void SaveData()
    {
        InventoryData inventoryData = new InventoryData();
        InventoryItem<OuterwearItem> outer = _character.Equipment.Outerwear;
        InventoryItem<PantsItem> pants = _character.Equipment.Pants;
        InventoryItem<ShoesItem> shoes = _character.Equipment.Shoes;
        InventoryItem<RingItem> ring = _character.Equipment.Ring;

        inventoryData.Outerwear = new InventoryItemData { ID = outer.Item.name, Level = outer.Level, Amount = outer.Amount };
        inventoryData.Pants = new InventoryItemData { ID = pants.Item.name, Level = pants.Level, Amount = pants.Amount };
        inventoryData.Shoes = new InventoryItemData { ID = shoes.Item.name, Level = shoes.Level, Amount = shoes.Amount };
        inventoryData.Ring = new InventoryItemData { ID = ring.Item.name, Level = ring.Level, Amount = ring.Amount };


        foreach (var item in _character.Inventory.Items)
        {
            inventoryData.Items.Add(new InventoryItemData
            {
                ID = item.Item.name,
                Level = item.Level,
                Amount = item.Amount
            });
        }

        string json = JsonUtility.ToJson(inventoryData);
        File.WriteAllText(_savePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);
            _character.Inventory.Clear();

            Equipment equipment = new Equipment();

            OuterwearItem outer = _itemsRefs.Find(x => x.name == inventoryData.Outerwear.ID) as OuterwearItem;
            PantsItem pants = _itemsRefs.Find(x => x.name == inventoryData.Pants.ID) as PantsItem;
            ShoesItem shoes = _itemsRefs.Find(x => x.name == inventoryData.Shoes.ID) as ShoesItem;
            RingItem ring = _itemsRefs.Find(x => x.name == inventoryData.Ring.ID) as RingItem;

            equipment.Outerwear = new InventoryItem<OuterwearItem>
            {
                Item = outer,
                Level = inventoryData.Outerwear.Level, Amount = inventoryData.Outerwear.Amount
            };

            equipment.Pants = new InventoryItem<PantsItem>
            {
                Item = pants,
                Level = inventoryData.Pants.Level, Amount = inventoryData.Pants.Amount
            };

            equipment.Shoes = new InventoryItem<ShoesItem>
            {
                Item = shoes,
                Level = inventoryData.Shoes.Level, Amount = inventoryData.Shoes.Amount
            };

            equipment.Ring = new InventoryItem<RingItem>
            {
                Item = ring,
                Level = inventoryData.Ring.Level, Amount = inventoryData.Ring.Amount
            };

            _character.SetEquipment(equipment);

            foreach (var itemData in inventoryData.Items)
            {
                Item item = _itemsRefs.Find(x => x.name == itemData.ID);

                if (item != null)
                {
                    InventoryItem<Item> newItem = new InventoryItem<Item>
                    {
                        Item = item,
                        Level = itemData.Level,
                        Amount = itemData.Amount
                    };
                    _character.Inventory.AddItem(newItem);
                }
            }
        }
    }
}
