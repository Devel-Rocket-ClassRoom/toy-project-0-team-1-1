using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float Resistance => stats[StatType.Resistance].FinalValue;

    public float CurrentHp => currentHp;

    protected virtual void Awake()
    {
        InitStats();
        animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        currentHp = stats[StatType.MaxHp].FinalValue;
    }

    protected abstract void InitStats();

    public virtual void AddModifier(StatType type, StatModifier mod)
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
        if (isDead)
            return;
        float finalDamage = Mathf.Max(1, damage - stats[StatType.Defense].FinalValue);
        currentHp -= finalDamage;
        if (currentHp <= 0 && !isDead) // �ߺ� Die() ����
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if (isDead)
            return;
        isDead = true;
        animator.SetTrigger("Die");
        animator.SetBool("Run", false);
        StartCoroutine(DieRoutine());
    }

    protected virtual IEnumerator DieRoutine()
    {
        yield return null;
        OnDie();
    }

    protected virtual void OnDie() { }
}
