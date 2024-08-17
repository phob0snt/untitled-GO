using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class FightManager : MonoBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly IEnemyFactory _enemyFactory;
    [HideInInspector] public UnityEvent OnSceneReady = new();

    private Enemy _currentEnemy;

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
        _currentEnemy = _enemyFactory.CreateEnemy(CurrentBattleData.EnemyData);

        //switch (CurrentBattleData.EnemyData.Type)
        //{
        //    case EnemyType.Base:
        //        break;
        //}
    }
}
