using UnityEngine;

[CreateAssetMenu(menuName = "Battle/BattleData")]
public class BattleData : ScriptableObject
{
    public string Name => _name;
    [SerializeField] private string _name;
    public EnemyData EnemyData => _enemyData;
    [SerializeField] private EnemyData _enemyData;
}
