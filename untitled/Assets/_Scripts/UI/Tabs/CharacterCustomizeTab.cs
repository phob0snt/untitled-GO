using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterCustomizeTab : Tab
{
    [Inject] private readonly Character _character;
    [Inject] private readonly ItemUIPool _itemUIPool;

    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private Button _equipItem;
    private PlayerStats _prevStats;
    private InventoryItem<Item> _selectedItem;

    [SerializeField] private TMP_Text _streamCapacity;
    [SerializeField] private TMP_Text _hp;
    [SerializeField] private TMP_Text _streamRegen;
    [SerializeField] private TMP_Text _evasionChance;
    [SerializeField] private TMP_Text _barrierDurability;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private TMP_Text _attackRate;
    [SerializeField] private TMP_Text _attackStreamCost;

    [SerializeField] private ContentRectAutoExpand _rectExpand;
    public override void Initialize()
    {
        UpdateItemsDisplay();
    }

    private void OnEnable()
    {
        UpdateItemsDisplay();
        _equipItem.onClick.AddListener(EquipItem);
        Inventory.OnInventoryUpdate.AddListener(UpdateItemsDisplay);
    }

    private void OnDisable()
    {
        _equipItem.onClick.RemoveListener(EquipItem);
        Inventory.OnInventoryUpdate.RemoveListener(UpdateItemsDisplay);
    }

    private void UpdateItemsDisplay()
    {
        _itemUIPool.ClearAllItems(_contentLabel);
        foreach (var item in _character.Inventory.Items)
        {
            if (item.Item is IEquippable)
                DisplayStorageItem(item);
        }
    }

    private void DisplayStorageItem(InventoryItem<Item> item)
    {
        ItemUI itemUI = _itemUIPool.GetItemUI(_contentLabel);
        itemUI.SetItem(item.Item);
        itemUI.OnClick.AddListener(() => SelectItem(item));
        _rectExpand.UpdateRectSize();
    }

    private void SelectItem(InventoryItem<Item> item)
    {
        _selectedItem = item;
        UpdatePlayerStats(_prevStats);
        if (_character.ItemEquipped(item)) return;
        switch (item.Item)
        {
            case ShoesItem shoes:
                _streamCapacity.text += $" + {shoes.StreamCapacityBonus}";
                _hp.text += $" + {shoes.MaxHPBonus}";
                _evasionChance.text += $" + {shoes.EvasionChance} %";
                break;
            case PantsItem pants:
                _streamCapacity.text += $" + {pants.StreamCapacityBonus}";
                _hp.text += $" + {pants.MaxHPBonus}";
                _streamRegen.text += $" + {pants.StreamRegenBonus}";
                break;
            case OuterwearItem outerwear:
                _streamCapacity.text += $" + {outerwear.StreamCapacityBonus}";
                _hp.text += $" + {outerwear.MaxHPBonus}";
                _barrierDurability.text += $" + {outerwear.BarrierDurability}";
                break;
            case RingItem ring:
                _damage.text += $" + {ring.Damage}";
                _attackRate.text += $" + {ring.AttackRate}";
                _attackStreamCost.text += $" + {ring.AttackStreamCost}";
                break;
        }
    }

    private void EquipItem()
    {
        if (_selectedItem != null )
        {
            Debug.Log("Equipping");
            _character.Equip(_selectedItem);
        }
    }

    public void DisplayEquippedItem(InventoryItem<Item> item)
    {
        // отображение предметов одежды
    }

    public void UpdatePlayerStats(PlayerStats playerStats)
    {
        _prevStats = playerStats;
        _streamCapacity.text = "П: " + playerStats.StreamCapacity.ToString();
        _hp.text = "З: " + playerStats.HP.ToString();
        _evasionChance.text = "У: " + playerStats.EvasionChance.ToString() + " %";
        _streamRegen.text = "В: " + playerStats.StreamRegen.ToString();
        _barrierDurability.text = "Б: " + playerStats.BarrierDurability.ToString();
        _damage.text = "DM: " + playerStats.Damage.ToString();
        _attackRate.text = "AR: " + playerStats.AttackRate.ToString();
        _attackStreamCost.text = "AC: " + playerStats.AttackStreamCost.ToString();
    }
}
