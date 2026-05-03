using UnityEngine;
using System.Collections.Generic;

public abstract class BaseEntity : MonoBehaviour
{
    //protected StatContainer maxHp = new StatContainer(100f);
    //protected StatContainer defense = new StatContainer(5f);
    //protected StatContainer speed = new StatContainer(10f);

    protected Dictionary<StatType, StatContainer> stats = new Dictionary<StatType, StatContainer>();

    protected float currentHp;
    protected bool isDead;
    protected Animator animator;

    public bool IsDead => isDead;
    public float MaxHp => stats[StatType.MaxHp].FinalValue;
    public float Defense => stats[StatType.Defense].FinalValue;
    public float Speed => stats[StatType.Speed].FinalValue;

    public float CurrentHp => currentHp;

    protected virtual void Awake()
    {
        InitStats();
        animator = GetComponent<Animator>();
    }

    protected void OnEnable()
    {
        isDead = false;
        currentHp = stats[StatType.MaxHp].FinalValue;
    }

    protected abstract void InitStats();

    public void AddModifier(StatType type, StatModifier mod)
    {
        if (stats.ContainsKey(type))
        {
            stats[type].AddModifier(mod);
        }
    }

    public void RemoveBySource(StatType type, object source)
    {
        if (stats.ContainsKey(type))
        {
            stats[type].RemoveBySource(source);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        float finalDamage = Mathf.Max(0, damage - stats[StatType.Defense].FinalValue);
        currentHp -= finalDamage;
        if (currentHp <= 0) Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
    }
}