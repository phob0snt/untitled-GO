using UnityEngine;
using Zenject;

public class StorageTab : Tab
{
    [Inject] private readonly ItemUIPool _itemUIPool;
    [Inject] private readonly Character _character;

    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;

    [SerializeField] private ContentRectAutoExpand _rectExpand;

    public override void Initialize()
    {
        UpdateItemsDisplay();
    }

    private void OnEnable()
    {
        UpdateItemsDisplay();
        Inventory.OnInventoryUpdate.AddListener(UpdateItemsDisplay);
    }

    private void OnDisable()
    {
        Inventory.OnInventoryUpdate.RemoveListener(UpdateItemsDisplay);
    }

    private void UpdateItemsDisplay()
    {
        _itemUIPool.ClearAllItems(_contentLabel);
        foreach (var item in _character.Inventory.Items)
        {
            DisplayStorageItem(item.Item);
        }
    }

    private void DisplayStorageItem(Item item)
    {
        ItemUI itemUI = _itemUIPool.GetItemUI(_contentLabel);
        itemUI.SetItem(item);
        _rectExpand.UpdateRectSize();
    }
}
