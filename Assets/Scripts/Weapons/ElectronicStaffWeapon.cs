using System.Collections.Generic;
using UnityEngine;

public class ElectronicStaffWeapon : WeaponBase
{
    [Header("Staff")]
    [SerializeField] private int level = 1;
    [SerializeField] private float chainRange = 5f;

    private readonly int[] chainCounts = { 3, 4, 5, 6, 7 };

    private int ChainCount
    {
        get
        {
            int index = Mathf.Clamp(level - 1, 0, chainCounts.Length - 1);
            return chainCounts[index];
        }
    }

    protected override void Attack()
    {
        BaseEnemy firstTarget = FindRandomEnemyInRange(transform.position, Range);

        if (firstTarget == null)
            return;

        ChainAttack(firstTarget);
    }

    private void ChainAttack(BaseEnemy startTarget)
    {
        BaseEnemy current = startTarget;
        List<BaseEnemy> attacked = new List<BaseEnemy>();

        for (int i = 0; i < ChainCount; i++)
        {
            if (current == null)
                break;

            current.TakeDamage(Damage);
            attacked.Add(current);

            Debug.Log($"스태프 연쇄 타격: {current.name} / 데미지: {Damage}");

            current = FindNextTarget(current, attacked);
        }
    }

    private BaseEnemy FindNextTarget(BaseEnemy from, List<BaseEnemy> attacked)
    {
        Collider[] hits = FindTargetsInRange(from.transform.position, chainRange);

        List<BaseEnemy> candidates = new List<BaseEnemy>();

        foreach (Collider hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();

            if (enemy == null)
                continue;

            if (attacked.Contains(enemy))
                continue;

            candidates.Add(enemy);
        }

        if (candidates.Count == 0)
            return null;

        return candidates[Random.Range(0, candidates.Count)];
    }

    private BaseEnemy FindRandomEnemyInRange(Vector3 center, float radius)
    {
        Collider[] hits = FindTargetsInRange(center, radius);

        List<BaseEnemy> enemies = new List<BaseEnemy>();

        foreach (Collider hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();

            if (enemy != null)
                enemies.Add(enemy);
        }

        if (enemies.Count == 0)
            return null;

        return enemies[Random.Range(0, enemies.Count)];
    }
}
