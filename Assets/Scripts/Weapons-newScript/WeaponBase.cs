using UnityEngine;
using System.Collections.Generic;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected LayerMask obstacleLayer;
    [SerializeField] protected WeaponData weaponData;

    public WeaponData WeaponData => weaponData;
    public bool IsActive { get; private set; }
    public int Level { get; private set; } = 0;

    protected Dictionary<StatType, StatContainer> stats = new Dictionary<StatType, StatContainer>();

    public float Damage => stats.ContainsKey(StatType.Attack) ? stats[StatType.Attack].FinalValue : 0f;
    public float Cooldown => stats.ContainsKey(StatType.Cool) ? stats[StatType.Cool].FinalValue : 1f;
    public float Range => stats.ContainsKey(StatType.Range) ? stats[StatType.Range].FinalValue : 0f;

    private float _timer;

    protected virtual void Awake()
    {
        if (weaponData == null)
        {
            Debug.LogError($"{gameObject.name}: WeaponData가 할당되지 않았습니다.");
            return;
        }

        InitStats();
    }

    protected virtual void InitStats()
    {
        stats[StatType.Attack] = new StatContainer(weaponData.damage);
        stats[StatType.Cool] = new StatContainer(weaponData.cooldown);
        stats[StatType.Range] = new StatContainer(weaponData.Range); 
    }

    private void Update()
    {
        if (!IsActive) return;

        _timer += Time.deltaTime;
        if (_timer >= Cooldown)
        {
            _timer = 0f;
            Attack();
        }
    }

    public void AddModifier(StatType type, StatModifier modifier)
    {
        if (stats.TryGetValue(type, out var container))
        {
            container.AddModifier(modifier);
        }
        else
        {
            Debug.LogWarning($"{type} 스탯이 이 무기({weaponData.weaponName})에 존재하지 않습니다.");
        }
    }

    public void RemoveModifiersBySource(object source)
    {
        foreach (var stat in stats.Values)
        {
            stat.RemoveBySource(source);
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

    public void LevelUp() => Level++;

    protected abstract void Attack();
    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }

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
        if (target == null) return transform.forward;

        Vector3 dir = target.position - transform.position;
        //dir.y = 0f;
        return dir.sqrMagnitude <= 0.001f ? transform.forward : dir.normalized;
    }
}