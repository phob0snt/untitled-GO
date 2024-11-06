using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterCustomizeTab : UIElement
{
    [Inject] private readonly Character _character;
    [Inject] private readonly ItemUIPool _itemUIPool;

    [SerializeField] private CharacterDisplayLabel _charDisplay;
    [SerializeField] private Image _activeTabIndicator;

    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private Button _equipItem;

    private InventoryItem<Item> _selectedItem;


    [SerializeField] private ContentRectAutoExpand _rectExpand;
    public override void Initialize()
    {
        _charDisplay.Initialize();
        EventManager.AddListener<InventoryUpdateEvent>(UpdateItemsDisplay);
    }

    private void OnEnable()
    {
        _equipItem.onClick.AddListener(EquipItem);
        _activeTabIndicator.color = new Color32(53, 137, 195, 255);
    }

    private void OnDisable()
    {
        _equipItem.onClick.RemoveListener(EquipItem);
        _activeTabIndicator.color = Color.white;
    }

    private void UpdateItemsDisplay(InventoryUpdateEvent evt)
    {
        _itemUIPool.ClearAllItems(_contentLabel);
        foreach (var item in evt.Items)
        {
            if (item.Item is IEquippable)
                DisplayItem(item);
        }
        _rectExpand.UpdateRectSize();
    }

    private void DisplayItem(InventoryItem<Item> item)
    {
        ItemUI itemUI = _itemUIPool.GetItemUI(_contentLabel);
        itemUI.SetItem(item.Item);
        itemUI.OnClick.AddListener(() => SelectItem(item));
    }

    private void SelectItem(InventoryItem<Item> item)
    {
        _selectedItem = item;
        if (_character.ItemEquipped(item))
            _charDisplay.ClearAdditionalStats();
        else
            _charDisplay.DisplaySelectedItem(item.Item);
    }

    private void EquipItem()
    {
        if (_selectedItem != null )
        {
            Debug.Log("Equipping");
            _character.Equip(_selectedItem);
        }
    }

    public void DisplayEquippedItem<T>(InventoryItem<T> item) where T : Item
    {
        // отображение предметов одежды
    }
}
