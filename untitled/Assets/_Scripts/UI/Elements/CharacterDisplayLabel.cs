using TMPro;
using UnityEngine;
using Zenject;

public class CharacterDisplayLabel : UIElement
{
    [Inject] private readonly Character _character;


    [SerializeField] private TMP_Text _streamCapacity;
    [SerializeField] private TMP_Text _hp;
    [SerializeField] private TMP_Text _streamRegen;
    [SerializeField] private TMP_Text _evasionChance;
    [SerializeField] private TMP_Text _barrierDurability;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private TMP_Text _attackRate;
    [SerializeField] private TMP_Text _attackStreamCost;

    private PlayerStats _prevStats;

    public override void Initialize()
    {
        EventManager.AddListener<StatsChangeEvent>(DisplayPlayerStats);
    }

    public void DisplaySelectedItem(Item item)
    {
        ClearAdditionalStats();
        if (item is ClothingItem cloth)
        {
            _streamCapacity.text += $" => {_character.PlayerStats.StreamCapacityWithItem(cloth)}";
            _hp.text += $" => {_character.PlayerStats.HPWithItem(cloth)}";
            switch (item)
            {
                case ShoesItem shoes:
                    _evasionChance.text += $" => {shoes.EvasionChance} %";
                    break;
                case PantsItem pants:
                    _streamRegen.text += $" => {pants.StreamRegenBonus}";
                    break;
                case OuterwearItem outerwear:
                    _barrierDurability.text += $" => {outerwear.BarrierDurability}";
                    break;
            }
        }
        else if (item is RingItem ring)
        {
            _damage.text += $" => {ring.Damage}";
            _attackRate.text += $" => {ring.AttackRate}";
            _attackStreamCost.text += $" => {ring.AttackStreamCost}";
        }
    }

    public void ClearAdditionalStats() => DisplayPlayerStats(new StatsChangeEvent() { Stats = _prevStats});

    private void DisplayPlayerStats(StatsChangeEvent e)
    {
        _prevStats = e.Stats;

        _streamCapacity.text = "Ï: " + e.Stats.StreamCapacity.ToString();
        _hp.text = "Ç: " + e.Stats.HP.ToString();
        _evasionChance.text = "Ó: " + e.Stats.EvasionChance.ToString() + " %";
        _streamRegen.text = "Â: " + e.Stats.StreamRegen.ToString();
        _barrierDurability.text = "Á: " + e.Stats.BarrierDurability.ToString();
        _damage.text = "DM: " + e.Stats.Damage.ToString();
        _attackRate.text = "AR: " + e.Stats.AttackRate.ToString();
        _attackStreamCost.text = "AC: " + e.Stats.AttackStreamCost.ToString();
    }
}
