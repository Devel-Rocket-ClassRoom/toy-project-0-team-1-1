using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float baseDamage = 10f;
    [SerializeField] protected float baseCooldown = 1f;
    [SerializeField] protected float baseRange = 10f;

    [Header("Target")]
    [SerializeField] protected LayerMask targetLayer;

    protected StatContainer damageStat;
    protected StatContainer cooldownStat;
    protected StatContainer rangeStat;

    private float attackTimer;

    protected float Damage => damageStat.FinalValue;
    protected float Cooldown => cooldownStat.FinalValue;
    protected float Range => rangeStat.FinalValue;
    protected virtual void Awake()
    {
        damageStat = new StatContainer(baseDamage);
        cooldownStat = new StatContainer(baseCooldown);
        rangeStat = new StatContainer(baseRange);
    }
    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= Cooldown)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    protected abstract void Attack();
}
