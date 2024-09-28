using UnityEngine;
using Zenject;

public class LocalBattle : LocalMapObject
{
    [SerializeField] private BattleData _battleData;
    [Inject] private readonly GameManager _gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Character>() != null)
            _gameManager.LoadFightScene(_battleData);
    }
}
