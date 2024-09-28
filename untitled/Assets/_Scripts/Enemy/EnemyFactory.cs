using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyFactory : MonoBehaviour, IEnemyFactory
{
    [Inject] private DiContainer _container;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    private GameObject _currentPrefab;

    public Enemy CreateEnemy(EnemyData enemyData)
    {
        switch (enemyData.Type)
        {
            case EnemyType.Base:
                _currentPrefab = _enemyPrefabs.Find(x => x.TryGetComponent<BaseEnemy>(out _));
                break;
        }
        if (_currentPrefab == null)
        {
            throw new ArgumentException($"No prefab found for enemy type {enemyData.Type}");
        }

        Enemy enemy = _container.InstantiatePrefabForComponent<Enemy>(_currentPrefab, new Vector3(0, 5, 19), Quaternion.identity, transform.root);
        enemy.Initialize(enemyData);

        return enemy;
    }
}
