using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ItemsUpgradeTab : Tab
{
    [Inject] private readonly Character _character;
    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _itemNameField;
    [SerializeField] private TMP_Text _itemStatsField;

    [SerializeField] private ContentRectAutoExpand _rectExpand;
    public override void Initialize()
    {
        foreach (var item in _character.Inventory.Items)
        {
            if (item is IUpgradable)
                DisplayItem(item);
        }
    }

    private void DisplayItem(Item item)
    {
        ItemUI itemUI = Instantiate(_itemUIPrefab, _contentLabel).GetComponent<ItemUI>();
        itemUI.SetItem(item);
        itemUI.OnClick.AddListener(() => SelectItem(item));
        _rectExpand.UpdateRectSize();
    }

    private void SelectItem(Item item)
    {
        _itemImage.sprite = item.Icon;
        _itemNameField.text = item.Name;
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
    }
}
