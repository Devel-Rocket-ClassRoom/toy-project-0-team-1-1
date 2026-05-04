using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStatus : BaseEntity
{
    public event Action<float> OnHpChange;
    public Dictionary<UpgradeItemData, int> upgradeItems = new Dictionary<UpgradeItemData, int>();

    private Renderer[] _renderers;
    [SerializeField] private float invincibleTime = 3f;
    private bool _isInvincible = false;


    protected override void Awake()
    {
        base.Awake();
        _renderers = GetComponentsInChildren<Renderer>();
    }

    protected override void InitStats()
    {
        stats[StatType.MaxHp] = new StatContainer(100f);
        stats[StatType.Defense] = new StatContainer(5f);
        stats[StatType.Speed] = new StatContainer(9f);
    }

    public override void TakeDamage(float damage)
    {
        if (_isInvincible) return;
        base.TakeDamage(damage);
        OnHpChange?.Invoke(currentHp);
        StartCoroutine(InvincibleRoutine());
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
    private IEnumerator InvincibleRoutine()
    {
        _isInvincible = true;

        // 깜박이기
        float elapsed = 0f;
        while (elapsed < invincibleTime)
        {
            foreach (var mesh in _renderers)
                mesh.enabled = !mesh.enabled;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        // 원래대로
        foreach (var mesh in _renderers)
            mesh.enabled = true;

        _isInvincible = false;
    }
}