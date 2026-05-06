using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerStatus : BaseEntity
{
    public event Action<float> OnHpChange;
    public event Action OnDead;
    public Dictionary<UpgradeItemData, int> upgradeItems = new Dictionary<UpgradeItemData, int>();
    private List<(StatType type, StatModifier mod)> weaponModifiers = new List<(StatType type, StatModifier mod)>();
    private Renderer[] _renderers;
    [SerializeField] private float invincibleTime = 0.5f;
    [SerializeField] private ParticleSystem hitEffect;
    private bool _isInvincible = false;

    public List<(StatType type, StatModifier mod)> WeaponModifiers => weaponModifiers;

    protected override void Awake()
    {
        base.Awake();
        _renderers = GetComponentsInChildren<Renderer>();
    }
    public void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stats[StatType.LootingArea].FinalValue, LayerMask.GetMask("Item"));
        foreach (Collider collider in hitColliders)
        {
            collider.GetComponent<ILootable>()?.StartLooting(this.transform);
        }


        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    OnDead?.Invoke();
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

    protected override void InitStats()
    {
        stats[StatType.MaxHp] = new StatContainer(100f);
        stats[StatType.Defense] = new StatContainer(5f);
        stats[StatType.Speed] = new StatContainer(9f);
        stats[StatType.LootingArea] = new StatContainer(2f);
    }

    public override void TakeDamage(float damage)
    {
        if (_isInvincible) return;
        base.TakeDamage(damage);
        OnHpChange?.Invoke(currentHp);

        hitEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        hitEffect.Play(true);

        StartCoroutine(InvincibleRoutine());
    }

    public override void AddModifier(StatType type, StatModifier mod)
    {
        if (stats.ContainsKey(type))
        {
            stats[type].AddModifier(mod);
        }
        else
        {
            weaponModifiers.Add((type, mod));
            var weapons = this.GetComponent<PlayerWeapon>().Weapons.Values.ToList();
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].AddModifier(type, mod);
            }
        }
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
    protected override void OnDie()
    {
        OnDead?.Invoke();
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