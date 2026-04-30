using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon, IAutoAttackWeapon
{
    [Header("Base Stats")]
    [SerializeField] protected float baseDamage = 10f;
    [SerializeField] protected float baseCooldown = 1f;
    [SerializeField] protected float baseRange = 5f;

    [Header("Target")]
    [SerializeField] protected LayerMask targetLayer;

    private float attackTimer;

    protected virtual float Damage => baseDamage;
    public virtual float Cooldown => baseCooldown;
    public virtual float Range => baseRange;

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= Cooldown)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    public abstract void Attack();
}