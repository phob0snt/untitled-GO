using UnityEngine;
using Zenject;

public class StorageTab : Tab
{
    [Inject] private readonly Character _character;
    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;

    [SerializeField] private ContentRectAutoExpand _rectExpand;

    public override void Initialize()
    {
        foreach(var item in _character.Inventory.Items)
        {
            DisplayItem(item);
        }
    }

    private void DisplayItem(Item item)
    {
        ItemUI itemUI = Instantiate(_itemUIPrefab, _contentLabel).GetComponent<ItemUI>();
        itemUI.SetItem(item);
        _rectExpand.UpdateRectSize();
    }
}
