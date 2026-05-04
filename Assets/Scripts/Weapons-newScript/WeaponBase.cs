using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float baseDamage = 10f;
    [SerializeField] protected float baseCooldown = 1f;
    [SerializeField] protected float baseRange = 5f;

    [Header("Target")]
    [SerializeField] protected LayerMask targetLayer;

    protected StatContainer damageStat;
    protected StatContainer cooldownStat;
    protected StatContainer rangeStat;

    public bool IsActive { get; private set; }

    public float Damage => damageStat.FinalValue;
    public float Cooldown => cooldownStat.FinalValue;
    public float Range => rangeStat.FinalValue;

    protected virtual void Awake()
    {
        damageStat = new StatContainer(baseDamage);
        cooldownStat = new StatContainer(baseCooldown);
        rangeStat = new StatContainer(baseRange);
    }

    public virtual void Activate()
    {
        if (IsActive) return;

        IsActive = true;
        OnActivate();
    }

    public virtual void Deactivate()
    {
        if (!IsActive) return;

        IsActive = false;
        OnDeactivate();
    }

    public void Use()
    {
        if (!IsActive) return;

        Attack();
    }

    protected abstract void Attack();

    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }

    public void AddDamageModifier(StatModifier modifier)
    {
        damageStat.AddModifier(modifier);
    }

    public void AddCooldownModifier(StatModifier modifier)
    {
        cooldownStat.AddModifier(modifier);
    }

    public void AddRangeModifier(StatModifier modifier)
    {
        rangeStat.AddModifier(modifier);
    }

    public void RemoveModifiersBySource(object source)
    {
        damageStat.RemoveBySource(source);
        cooldownStat.RemoveBySource(source);
        rangeStat.RemoveBySource(source);
    }

    protected Collider[] FindTargetsInRange(Vector3 center, float radius)
    {
        return Physics.OverlapSphere(center, radius, targetLayer);
    }

    protected Transform FindNearestTarget()
    {
        Collider[] hits = FindTargetsInRange(transform.position, Range);

        Transform nearest = null;
        float nearestDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;

            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }

    protected Vector3 GetDirectionToNearestTarget()
    {
        Transform target = FindNearestTarget();

        if (target == null)
            return transform.forward;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude <= 0.001f)
            return transform.forward;

        return dir.normalized;
    }
}