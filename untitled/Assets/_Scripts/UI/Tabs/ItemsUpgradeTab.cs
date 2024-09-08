using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemsUpgradeTab : Tab
{
    [Inject] private readonly Character _character;
    [Inject] private readonly ItemUIPool _itemUIPool;

    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _itemNameField;
    [SerializeField] private TMP_Text _itemStatsField;

    [SerializeField] private Button _upgradeButton;

    private Item _selectedItem;

    [SerializeField] private ContentRectAutoExpand _rectExpand;
    public override void Initialize()
    {
        RefreshItems();
    }

    private void OnEnable()
    {
        RefreshItems();
        _upgradeButton.onClick.AddListener(UpgradeItem);
        Inventory.OnInventoryUpdate.AddListener(RefreshItems);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(UpgradeItem);
        Inventory.OnInventoryUpdate.RemoveListener(RefreshItems);
    }

    private void RefreshItems()
    {
        Debug.Log("ItemsUpdated");
        _itemUIPool.ClearAllItems(_contentLabel);
        foreach (var item in _character.Inventory.Items)
        {
            if (item.Item is IUpgradable)
                DisplayItemIcon(item.Item);
        }
        DisplayItemStats(_selectedItem);
    }

    private void DisplayItemIcon(Item item)
    {
        ItemUI itemUI = _itemUIPool.GetItemUI(_contentLabel);
        itemUI.SetItem(item);
        itemUI.OnClick.AddListener(() => SelectItem(item));
        _rectExpand.UpdateRectSize();
    }

    private void UpgradeItem()
    {
        if (_selectedItem == null) return;
        _character.Inventory.TryToUpgradeItem(_selectedItem as IUpgradable);
    }

    private void SelectItem(Item item)
    {
        _selectedItem = item;
        _itemImage.sprite = item.Icon;
        _itemNameField.text = item.Name;
        DisplayItemStats(item);
    }

    private void DisplayItemStats(Item item)
    {
        if (item == null)
        {
            Debug.Log("pizda");
            return;
        }
        switch (item.Type)
        {
            case ItemType.Shoes:
                ShoesItem shoes = item as ShoesItem;
                _itemStatsField.text = $"Ï: {shoes.StreamCapacityBonus}\n" +
                                       $"Ç: {shoes.MaxHPBonus}\n" +
                                       $"Ó: {shoes.EvasionChance}%";
                break;
            case ItemType.Pants:
                PantsItem pants = item as PantsItem;
                _itemStatsField.text = $"Ï: {pants.StreamCapacityBonus}\n" +
                                       $"Ç: {pants.MaxHPBonus}\n" +
                                       $"Â: {pants.StreamRegenBonus}";
                break;
            case ItemType.Outerwear:
                OuterwearItem outerwear = item as OuterwearItem;
                _itemStatsField.text = $"Ï: {outerwear.StreamCapacityBonus}\n" +
                                       $"Ç: {outerwear.MaxHPBonus}\n" +
                                       $"Á: {outerwear.BarrierDurability}";
                break;
            case ItemType.Ring:
                RingItem ring = item as RingItem;
                _itemStatsField.text = $"DM: {ring.Damage}\n" +
                                       $"AR: {ring.AttackRate}\n" +
                                       $"AC: {ring.AttackStreamCost}";
                break;
        }
        Debug.Log($"Displayed {item}");
    }
}
