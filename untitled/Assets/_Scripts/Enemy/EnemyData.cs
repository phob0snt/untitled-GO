using UnityEngine;

public enum EnemyType
{
    Base,
}
[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public int HP => _hp;
    [SerializeField] private int _hp;
    public EnemyType Type => _type;
    [SerializeField] private EnemyType _type;
    public int Damage => _damage;
    [SerializeField] private int _damage;
}
