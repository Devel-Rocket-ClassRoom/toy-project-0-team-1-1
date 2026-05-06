using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpecialObject : BaseEntity
{
    [SerializeField] private BreakableObject data;
    [System.Serializable]
    public struct DropEntry
    {
        public GameObject itemPrefab;
        [Min(0f)] public float weight;
    }

    [SerializeField] private List<DropEntry> entries = new List<DropEntry>();
    protected override void Awake()
    {
        InitStats();
    }
    protected override void InitStats()
    {
        stats[StatType.MaxHp] = new StatContainer(50f);
        stats[StatType.Defense] = new StatContainer(0f);
        stats[StatType.Speed] = new StatContainer(0);
    }

    protected override void Die()
    {
        isDead = true;
        GetComponent<Collider>().enabled = false;
        OnDie();
    }

    protected override void OnDie()
    {
        SpawnDrops();

        if (data != null && data.prefab != null)
            PoolManager.Instance.Despawn(data.prefab, gameObject);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    private void SpawnDrops()
    {
        if (entries.Count == 0) return;

        float total = 0f;
        foreach (var e in entries) total += e.weight;
        if (total <= 0f) return;

        float roll = UnityEngine.Random.value * total;
        float cumulative = 0f;

        foreach (var entry in entries)
        {
            cumulative += entry.weight;
            if (roll < cumulative)
            {
                if (entry.itemPrefab != null) PoolManager.Instance.Spawn(entry.itemPrefab, transform.position, Quaternion.identity);
                return;
            }
        }
    }
}
