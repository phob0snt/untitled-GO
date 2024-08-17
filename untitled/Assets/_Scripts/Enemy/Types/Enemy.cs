using UnityEngine;
using Zenject;

public abstract class Enemy : MonoBehaviour
{
    [Inject] private readonly FightManager _fightManager;
    public int HP { get; private set; }

    private EnemyData _enemyData;
    protected abstract void Attack();

    public virtual void Initialize()
    {
        _enemyData = _fightManager.CurrentBattleData.EnemyData;
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
    }

    private void Die()
    {
        Debug.Log("DEAD!!!!!");
    }

}
