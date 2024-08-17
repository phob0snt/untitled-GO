using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterCustomizeTab : Tab
{
    [Inject] private readonly Character _character;

    [SerializeField] private Transform _contentLabel;
    [SerializeField] private GameObject _itemUIPrefab;
    [SerializeField] private Button _equipItem;
    private PlayerStats _prevStats;
    private Item _selectedItem;

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
        foreach (var item in _character.Inventory.Items)
        {
            if (item is IEquippable)
                DisplayStorageItem(item);
        }
    }

    private void OnEnable()
    {
        _equipItem.onClick.AddListener(EquipItem);
    }

    private void DisplayStorageItem(Item item)
    {
        ItemUI itemUI = Instantiate(_itemUIPrefab, _contentLabel).GetComponent<ItemUI>();
        itemUI.SetItem(item);
        itemUI.OnClick.AddListener(() => SelectItem(item));
        _rectExpand.UpdateRectSize();
    }

    private void SelectItem(Item item)
    {
        _selectedItem = item;
        UpdatePlayerStats(_prevStats);
        if (_character.CheckItemEquipment(item)) return;
        switch (item.Type)
        {
            case ItemType.Shoes:
                ShoesItem shoes = item as ShoesItem;
                _streamCapacity.text += $" + {shoes.StreamCapacityBonus}";
                _hp.text += $" + {shoes.MaxHPBonus}";
                _evasionChance.text += $" + {shoes.EvasionChance} %";
                break;
            case ItemType.Pants:
                PantsItem pants = item as PantsItem;
                _streamCapacity.text += $" + {pants.StreamCapacityBonus}";
                _hp.text += $" + {pants.MaxHPBonus}";
                _streamRegen.text += $" + {pants.StreamRegenBonus}";
                break;
            case ItemType.Outerwear:
                OuterwearItem outerwear = item as OuterwearItem;
                _streamCapacity.text += $" + {outerwear.StreamCapacityBonus}";
                _hp.text += $" + {outerwear.MaxHPBonus}";
                _barrierDurability.text += $" + {outerwear.BarrierDurability}";
                break;
            case ItemType.Ring:
                RingItem ring = item as RingItem;
                _damage.text += $" + {ring.Damage}";
                _attackRate.text += $" + {ring.AttackRate}";
                _attackStreamCost.text += $" + {ring.AttackStreamCost}";
                break;
        }
    }

    private void EquipItem()
    {
        if (_selectedItem != null )
            _character.Equip(_selectedItem);
    }

    public void DisplayEquippedItem(Item item)
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
