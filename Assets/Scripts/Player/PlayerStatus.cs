using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStatus : BaseEntity
{
    public event Action<float> OnHpChange;
    public Dictionary<UpgradeItemData, int> upgradeItems = new Dictionary<UpgradeItemData, int>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void InitStats()
    {
        stats[StatType.MaxHp] = new StatContainer(100f);
        stats[StatType.Defense] = new StatContainer(5f);
        stats[StatType.Speed] = new StatContainer(9f);
        //maxHp = new StatContainer(100f);
        //defense = new StatContainer(5f);
        //speed = new StatContainer(9f);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        OnHpChange?.Invoke(currentHp);
    }

    public void Heal(float amount)
    {
        currentHp = Mathf.Min(currentHp + amount, stats[StatType.MaxHp].FinalValue);
        OnHpChange?.Invoke(currentHp);
    }

    protected override void Die()
    {
        base.Die();
    }
}