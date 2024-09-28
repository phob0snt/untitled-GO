using System.Linq;
using UnityEngine;

public enum EntityType { Enemy, Player }

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _destroyEffect;
    protected EntityType _user;
    protected int _damage;
    private bool _isInitialized = false;
    private bool _isDestroying = false;
    private Collider[] _sphereCastBuffer = new Collider[10];

    public void Initialize(EntityType user, int damage)
    {
        _user = user;
        _damage = damage;
        _isInitialized = true;
        Debug.Log("INIT");
    }

    private void FixedUpdate()
    {
        if (!_isInitialized || _isDestroying) return;
        Physics.OverlapSphereNonAlloc(transform.position, 1.6f, _sphereCastBuffer);
        switch (_user)
        {
            case EntityType.Enemy:
                foreach (var collider in _sphereCastBuffer)
                {
                    if (collider?.GetComponent<Player>() != null)
                    {
                        collider.GetComponent<Player>().ApplyDamage(_damage);
                        DestroyWithEffect();
                    }
                }
                break;
            case EntityType.Player:
                foreach (var collider in _sphereCastBuffer)
                {
                    if (collider?.GetComponent<Enemy>() != null)
                    {
                        collider.GetComponent<Enemy>().ApplyDamage(_damage);
                        DestroyWithEffect();
                    }
                }
                break;
        }
        _sphereCastBuffer = new Collider[10];
    }

    //protected virtual void OnTriggerEnter(Collider other)
    //{
    //    if (!_isInitialized) return;
    //    switch (_user)
    //    {
    //        case EntityType.Enemy:
    //            if (other.GetComponent<Player>() != null)
    //            {
    //                other.GetComponent<Player>().ApplyDamage(_damage);
    //                DestroyWithEffect();
    //            }
    //            break;
    //        case EntityType.Player:
    //            if (other.GetComponent<Enemy>() != null)
    //            {
    //                other.GetComponent<Enemy>().ApplyDamage(_damage);
    //                DestroyWithEffect();
    //            }
    //            break;
    //    }

    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        DestroyWithEffect();
    //    }
    //}

    private void DestroyWithEffect()
    {
        _isDestroying = true;
        _destroyEffect.gameObject.SetActive(true);
        _destroyEffect.transform.SetParent(null);
        Destroy(gameObject, 1);
    }
}
