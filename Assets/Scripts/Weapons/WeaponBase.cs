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
    protected bool isActive = false;

    protected virtual float Damage => baseDamage;
    public virtual float Cooldown => baseCooldown;
    public virtual float Range => baseRange;

    public virtual void Activate()
    {
        isActive = true;
        attackTimer = Cooldown;
        OnActivate();
    }

    public virtual void Deactivate()
    {
        isActive = false;
        OnDeactivate();
    }

    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }

    protected virtual void Update()
    {
        if (!isActive)
            return;

        UpdateWeapon();

        attackTimer += Time.deltaTime;

        if (attackTimer >= Cooldown)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    protected virtual void UpdateWeapon() { }

    public abstract void Attack();
}