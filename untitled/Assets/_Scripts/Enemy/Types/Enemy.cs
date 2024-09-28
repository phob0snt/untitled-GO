using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public abstract class Enemy : MonoBehaviour
{
    [Inject] private readonly FightManager _fightManager;
    public int HP { get; private set; }

    private EnemyData _enemyData;
    protected abstract void Attack();

    public virtual void Initialize(EnemyData data)
    {
        _enemyData = data;
        HP = _enemyData.HP;
        _fightManager.OnSceneReady.Invoke();
    }

    public virtual void ApplyDamage(int damage)
    {
        if (damage <= 0) return;
        if (HP - damage <= 0)
        {
            HP = 0;
            Die();
        }
        else
            HP -= damage;
        _fightManager.OnEnemyHPChanged.Invoke(HP);
        Debug.Log("Enemy Damaged " + HP);
    }

    protected virtual void Die()
    {
        Debug.Log("DEAD!!!!!");
    }
}
