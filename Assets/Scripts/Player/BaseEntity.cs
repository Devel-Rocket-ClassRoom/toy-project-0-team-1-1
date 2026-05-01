using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    protected StatContainer maxHp = new StatContainer(100f);
    protected StatContainer defense = new StatContainer(5f);
    protected StatContainer speed = new StatContainer(10f);

    protected float currentHp;
    protected bool isDead;
    protected Animator animator;

    public bool IsDead => isDead;
    public float MaxHp => maxHp.FinalValue;
    public float Defense => defense.FinalValue;
    public float Speed => speed.FinalValue;

    public float CurrentHp => currentHp;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        isDead = true;
        InitStats();
        currentHp = maxHp.FinalValue;
    }

    protected abstract void InitStats();

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;
        float finalDamage = Mathf.Max(0, damage - defense.FinalValue);
        currentHp -= finalDamage;
        if (currentHp <= 0) Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
    }
}