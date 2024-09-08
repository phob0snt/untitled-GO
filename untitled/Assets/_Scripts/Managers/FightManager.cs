using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class FightManager : MonoBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly IEnemyFactory _enemyFactory;
    [Inject] private readonly Player _player;

    [HideInInspector] public UnityEvent OnSceneReady = new();
    [HideInInspector] public UnityEvent OnAttackPerformed = new();
    [HideInInspector] public UnityEvent OnBarrierSet = new();

    public Enemy CurrentEnemy { get; private set; }

    public BattleData CurrentBattleData => _gameManager.CurrentBattleData;

    private void OnEnable()
    {
        _gameManager.OnFightSceneLoaded.AddListener(ConfigureScene);
    }

    private void OnDisable()
    {
        _gameManager.OnFightSceneLoaded.RemoveListener(ConfigureScene);
    }

    private void ConfigureScene()
    {
        CurrentEnemy = _enemyFactory.CreateEnemy(CurrentBattleData.EnemyData);

        //switch (CurrentBattleData.EnemyData.Type)
        //{
        //    case EnemyType.Base:
        //        break;
        //}
    }

    private void PerformAttack()
    {
        if (_player.TryToAttack())
        {
            CurrentEnemy.ApplyDamage(_gameManager.PlayerStats.Damage);
            OnAttackPerformed.Invoke();
            Debug.Log("ATTACK");
        }
    }
}
