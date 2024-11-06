using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemsUpgradeTab : UIElement
{
    [Inject] private readonly Character _character;
    [Inject] private readonly ItemUIPool _itemUIPool;

    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _itemNameField;
    [SerializeField] private TMP_Text _itemStatsField;

    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Image _activeTabIndicator;


    private Item _selectedItem;


    [SerializeField] private ContentRectAutoExpand _rectExpand;
    public override void Initialize()
    {
        EventManager.AddListener<InventoryUpdateEvent>(RefreshItems);
    }

    private void OnEnable()
    {
        _upgradeButton.onClick.AddListener(UpgradeItem);
        _activeTabIndicator.color = new Color32(53, 137, 195, 255);
    }

    private void OnDisable()
    {
        _upgradeButton.onClick.RemoveListener(UpgradeItem);
        _activeTabIndicator.color = Color.white;
    }

    private void RefreshItems(InventoryUpdateEvent e)
    {
        _itemUIPool.ClearAllItems(_contentLabel);
        foreach (var item in e.Items)
        {
            if (item.Item is IUpgradable)
                DisplayItemIcon(item.Item);
        }
        if (_selectedItem != null)
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

        static string FormatStat(string statName, float currentValue, float nextValue, bool isMaxLevel)
        {
            return isMaxLevel ? $"{statName}: {currentValue}" : $"{statName}: {currentValue} => {nextValue}";
        }

        switch (item)
        {
            case ShoesItem shoes:
                bool shoesMaxLevel = shoes.UpgradeLevel >= IUpgradable.MAX_UPGRADE_LVL;
                _itemStatsField.text = FormatStat("Ï", shoes.StreamCapacityBonus, shoes.StreamWithLevel(shoes.UpgradeLevel + 1), shoesMaxLevel) + "\n" +
                                       FormatStat("Ç", shoes.MaxHPBonus, shoes.HPWithLevel(shoes.UpgradeLevel + 1), shoesMaxLevel) + "\n" +
                                       FormatStat("Ó", shoes.EvasionChance, shoes.EvasionWithLevel(shoes.UpgradeLevel + 1), shoesMaxLevel) + "%";
                break;
            case PantsItem pants:
                bool pantsMaxLevel = pants.UpgradeLevel >= IUpgradable.MAX_UPGRADE_LVL;
                _itemStatsField.text = FormatStat("Ï", pants.StreamCapacityBonus, pants.StreamWithLevel(pants.UpgradeLevel + 1), pantsMaxLevel) + "\n" +
                                       FormatStat("Ç", pants.MaxHPBonus, pants.HPWithLevel(pants.UpgradeLevel + 1), pantsMaxLevel) + "\n" +
                                       FormatStat("Â", pants.StreamRegenBonus, pants.RegenWithLevel(pants.UpgradeLevel + 1), pantsMaxLevel);
                break;
            case OuterwearItem outerwear:
                bool outerMaxLevel = outerwear.UpgradeLevel >= IUpgradable.MAX_UPGRADE_LVL;
                _itemStatsField.text = FormatStat("Ï", outerwear.StreamCapacityBonus, outerwear.StreamWithLevel(outerwear.UpgradeLevel + 1), outerMaxLevel) + "\n" +
                                       FormatStat("Ç", outerwear.MaxHPBonus, outerwear.HPWithLevel(outerwear.UpgradeLevel + 1), outerMaxLevel) + "\n" +
                                       FormatStat("Á", outerwear.BarrierDurability, outerwear.BarrierWithLevel(outerwear.UpgradeLevel + 1), outerMaxLevel);
                break;
            case RingItem ring:
                _itemStatsField.text = $"DM: {ring.Damage}\n" +
                                       $"AR: {ring.AttackRate}\n" +
                                       $"AC: {ring.AttackStreamCost}";
                break;
        }
        Debug.Log($"Displayed {item}");
    }
}
