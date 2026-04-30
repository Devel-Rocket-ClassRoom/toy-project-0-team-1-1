using System;
using UnityEngine;

public class PlayerStatus : BaseEntity
{
    public event Action<float> OnHpChange;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitStats()
    {
        maxHp = new StatContainer(100f);
        defense = new StatContainer(5f);
        speed = new StatContainer(9f);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHpChange?.Invoke(currentHp);
    }

    public void Heal(float amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp.FinalValue);
        OnHpChange?.Invoke(currentHp);
    }

    protected override void Die()
    {
        base.Die();
    }
}