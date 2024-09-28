using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FightView : View
{
    [Inject] private readonly Player _player;
    [Inject] private readonly FightManager _fightManager;

    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _streamText;
    [SerializeField] private TMP_Text _barrierText;
    [SerializeField] private Image _enemyHPBar;
    [SerializeField] private Image _ultimateEnergyBar;


    public override void Init()
    {

    }

    private void OnEnable()
    {
        _player.OnHPChange.AddListener(ShowHP);
        _player.OnBarrierChange.AddListener(ShowBarrier);
        _player.OnStreamChange.AddListener(ShowStream);
        _player.OnEnegryChange.AddListener(ShowUltimateEnergy);
        _fightManager.OnEnemyHPChanged.AddListener(ShowEnemyHP);
    }

    private void OnDisable()
    {
        _player.OnHPChange.RemoveListener(ShowHP);
        _player.OnBarrierChange.RemoveListener(ShowBarrier);
        _player.OnStreamChange.RemoveListener(ShowStream);
        _player.OnEnegryChange.RemoveAllListeners();
        _fightManager.OnEnemyHPChanged.RemoveListener(ShowEnemyHP);
    }
    private void ShowHP(int hp)
    {
        _hpText.text = hp.ToString();
    }

    private void ShowStream(int stream)
    {
        _streamText.text = stream.ToString();
    }
    private void ShowBarrier(int barrier)
    {
        _barrierText.text = barrier.ToString();
    }

    private void ShowEnemyHP(int hp)
    {
        int enemyMaxHp = _fightManager.CurrentBattleData.EnemyData.HP;
        _enemyHPBar.fillAmount = (float)hp / (float)enemyMaxHp;
    }

    private void ShowUltimateEnergy(int energy)
    {
        _ultimateEnergyBar.fillAmount = (float)energy / (float)_player.EnergyForUltimate;
    }
}
