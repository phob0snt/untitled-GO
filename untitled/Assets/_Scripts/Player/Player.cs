using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    [Inject] private ViewManager _viewManager;
    [Inject] private GameManager _gameManager;
    private PlayerStats _playerStats;

    private void Awake()
    {
        _playerStats = _gameManager.PlayerStats;
    }

    private void OnEnable()
    {
        _viewManager.GetView<FightView>().OnPressAttack.AddListener(Attack);
        _viewManager.GetView<FightView>().OnPressBarrier.AddListener(SetBarrier);
    }

    private void Attack()
    {

    }

    private void SetBarrier()
    {

    }
}
