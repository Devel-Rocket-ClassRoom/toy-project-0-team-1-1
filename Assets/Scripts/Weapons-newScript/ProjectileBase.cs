using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Transform owner;
    protected Vector3 direction;
    protected float damage;
    protected float speed;
    protected LayerMask targetLayer;
    protected LayerMask _obstacleLayer;
    protected GameObject _prefab;
    protected float _size;
    protected float _knockBack;
    private HashSet<Collider> _hitTargets = new HashSet<Collider>();

    //public virtual void Init(Transform owner, Vector3 direction, float damage,  float speed, LayerMask targetLayer, LayerMask obstacleLayer, GameObject prefab, float KnockBack, float size = 1f )
    //{
    //    this.owner = owner;
    //    this.direction = direction.normalized;
    //    this.damage = damage;
    //    this.speed = speed;
    //    this.targetLayer = targetLayer;
    //    this._prefab = prefab;
    //    _obstacleLayer = obstacleLayer;
    //    _size = size;
    //    _knockBack = KnockBack;
    //}
    public virtual void Init(ProjectileInitData data)
    {
        this.owner = data.owner;
        this.direction = data.direction.normalized;
        this.damage = data.damage;
        this.speed = data.speed;
        this.targetLayer = data.targetLayer;
        this._prefab = data.prefab;
        _obstacleLayer = data.obstacleLayer;
        _size = data.size;
        _knockBack = data.knockBack;

        _hitTargets.Clear();
    }

    protected bool IsTarget(Collider other)
    {
        return ((1 << other.gameObject.layer) & targetLayer) != 0;
    }

    protected virtual void Hit(Collider other)
    {
        if (!IsTarget(other))
            return;
        if (_hitTargets.Contains(other))
            return;
        _hitTargets.Add(other);
        other.GetComponent<BaseEntity>()?.TakeDamage(damage);
        other.GetComponent<BaseEnemy>()?.KnockBack(_knockBack);
    }

    protected void ReturnToPool()
    {
        if (_prefab == null)
            return;
        PoolManager.Instance.Despawn(_prefab, gameObject);
    }
}
