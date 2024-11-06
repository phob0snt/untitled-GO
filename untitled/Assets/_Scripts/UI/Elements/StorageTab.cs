using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StorageTab : UIElement
{
    [Inject] private readonly ItemUIPool _itemUIPool;
    [Inject] private readonly Character _character;

    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private Image _activeTabIndicator;


    [SerializeField] private ContentRectAutoExpand _rectExpand;


    public override void Initialize()
    {
        EventManager.AddListener<InventoryUpdateEvent>(UpdateItemsDisplay);
    }

    private void OnEnable()
    {
        _activeTabIndicator.color = new Color32(53, 137, 195, 255);
    }

    private void OnDisable()
    {
        _activeTabIndicator.color = Color.white;
    }

    private void UpdateItemsDisplay(InventoryUpdateEvent e)
    {
        _itemUIPool.ClearAllItems(_contentLabel);
        foreach (var item in e.Items)
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
