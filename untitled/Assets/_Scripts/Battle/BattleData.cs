using UnityEngine;

[CreateAssetMenu(menuName = "Battle/BattleData")]
public class BattleData : ScriptableObject
{
    public Reward Reward { get; set; }
    public EnemyData EnemyData { get; set;}
}
