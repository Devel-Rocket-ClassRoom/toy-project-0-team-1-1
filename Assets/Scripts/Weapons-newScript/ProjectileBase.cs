using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Transform owner;
    protected Vector3 direction;
    protected float damage;
    protected float speed;
    protected LayerMask targetLayer;
    protected GameObject _prefab;

    public virtual void Init(Transform owner, Vector3 direction, float damage,  float speed, LayerMask targetLayer, GameObject prefab)
    {
        this.owner = owner;
        this.direction = direction.normalized;
        this.damage = damage;
        this.speed = speed;
        this.targetLayer = targetLayer;
        this._prefab = prefab;
    }

    protected bool IsTarget(Collider other)
    {
        return ((1 << other.gameObject.layer) & targetLayer) != 0;
    }

    protected virtual void Hit(Collider other)
    {
        if (!IsTarget(other)) return;
        other.GetComponent<BaseEnemy>()?.TakeDamage(damage);
    }

    protected void ReturnToPool()
    {
        if (_prefab == null) return;
        PoolManager.Instance.Despawn(_prefab, gameObject);
    }
}