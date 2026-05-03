using UnityEngine;

public abstract class WeaponProjectileBase : MonoBehaviour
{
    protected float damage;
    protected LayerMask targetLayer;
    protected Transform owner;

    public virtual void Init(Transform owner, float damage, LayerMask targetLayer)
    {
        this.owner = owner;
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

        
    }
}