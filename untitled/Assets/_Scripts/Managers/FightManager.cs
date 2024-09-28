using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class FightManager : MonoBehaviour
{
    [Inject] private readonly GameManager _gameManager;
    [Inject] private readonly IEnemyFactory _enemyFactory;

    [HideInInspector] public UnityEvent OnSceneReady = new();
    [HideInInspector] public UnityEvent<int> OnEnemyHPChanged = new();
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
    }
}
