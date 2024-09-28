using System.Collections;
using UnityEngine;

public class BaseEnemy : Enemy
{
    [SerializeField] private Projectile _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _shotStrength;
    [SerializeField] private float _attackDelay;

    protected override void Attack()
    {
        //Vector3 shotVector = new (Random.Range(-0.2f, 0.2f), 0, -1);
        //GameObject bullet = Instantiate(_bulletPrefab.gameObject, _bulletSpawnPoint.position, Quaternion.identity);
        //bullet.GetComponent<Rigidbody>().AddForce(shotVector * _shotStrength, ForceMode.Impulse);
    }
    public override void Initialize(EnemyData data)
    {
        base.Initialize(data);
        StartCoroutine(SprayAttack());
    }

    private IEnumerator SprayAttack()
    {
        Attack();
        yield return new WaitForSeconds(_attackDelay);
        StartCoroutine(SprayAttack());
    }
}

