using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] protected LayerMask targetLayer;

    [SerializeField] protected WeaponData weaponData;
    public WeaponData WeaponData => weaponData;
    protected StatContainer damageStat;
    protected StatContainer cooldownStat;
    protected StatContainer rangeStat;

    public bool IsActive { get; private set; }

    //무기 레벨 관리용
    public int Level { get; private set; }

    public float Damage => damageStat.FinalValue;
    public float Cooldown => cooldownStat.FinalValue;
    public float Range => rangeStat.FinalValue;

    private float _timer;

    protected virtual void Awake()
    {
        if (weaponData == null)
        {
            Debug.LogError("WeaponData 없음");
        }
        damageStat = new StatContainer(weaponData.damage);
        cooldownStat = new StatContainer(weaponData.cooldown);
        rangeStat = new StatContainer(weaponData.size);
    }
    private void Update()
    {
        if (!IsActive) return;
        if (cooldownStat == null) return;

        _timer += Time.deltaTime;
        if (_timer >= Cooldown)
        {
            _timer = 0f;
            Attack();
        }
    }

    public virtual void Activate()
    {
        if (IsActive) return;

        IsActive = true;
        _timer = 0f;
        OnActivate();
    }

    public virtual void Deactivate()
    {
        if (!IsActive) return;

        IsActive = false;
        OnDeactivate();
    }

    protected abstract void Attack();

    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }

    //무기 레벨 증가 함수
    public void LevelUp()
    {
        Level++;
    }

    //public void AddDamageModifier(StatModifier modifier)
    //{
    //    damageStat.AddModifier(modifier);
    //}

    //public void AddCooldownModifier(StatModifier modifier)
    //{
    //    cooldownStat.AddModifier(modifier);
    //}

    //public void AddRangeModifier(StatModifier modifier)
    //{
    //    rangeStat.AddModifier(modifier);
    //}

    // WeaponData의 StatType에 따라 알맞은 스탯 컨테이너에 Modifier 적용
    public void AddModifier(WeaponStatType type, StatModifier modifier)
    {
        switch (type)
        {
            case WeaponStatType.Damage: damageStat.AddModifier(modifier); break;
            case WeaponStatType.Cooldown: cooldownStat.AddModifier(modifier); break;
            case WeaponStatType.Range: rangeStat.AddModifier(modifier); break;
            default: Debug.LogWarning($"지원하지 않는 무기 스탯 타입: {type}"); break;
        }
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