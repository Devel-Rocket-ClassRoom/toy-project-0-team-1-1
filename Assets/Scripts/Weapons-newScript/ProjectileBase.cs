using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Transform owner;
    protected Vector3 direction;
    protected float damage;
    protected LayerMask targetLayer;
    protected GameObject _prefab; // 

    public virtual void Init(
        Transform owner,
        Vector3 direction,
        float damage,
        LayerMask targetLayer)
    {
        this.owner = owner;
        this.direction = direction.normalized;
        this.damage = damage;
        this.targetLayer = targetLayer;
    }

    protected bool IsTarget(Collider other)
    {
        return ((1 << other.gameObject.layer) & targetLayer) != 0;
    }

    protected virtual void Hit(Collider other)
    {
        if (!IsTarget(other))
            return;

        Debug.Log($"{name} 타격: {other.name} / 데미지: {damage}");

        // 나중에 몬스터 구현 후
        // other.GetComponent<IDamageable>()?.OnDamage(damage);
    }
    protected void ReturnToPool()
    {
        if (_prefab == null) return;

        PoolManager.Instance.Despawn(_prefab, gameObject);
    }
}