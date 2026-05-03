using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicStaffWeapon : AreaWeaponBase
{
    [SerializeField] private int level = 1;
    [SerializeField] private float chainRange = 5f;

    private int[] chainCount = { 3, 4, 5, 6, 7 };
    private int ChainCount => chainCount[level - 1];

    public override void Attack()
    {
        Collider[] hits = FindTargetsInRange(transform.position, Range);
        if (hits.Length <= 0) return;

        BaseEnemy current = hits[Random.Range(0, hits.Length)].GetComponent<BaseEnemy>();
        if(current ==  null) return;

        List<BaseEnemy> attacked = new List<BaseEnemy>();

        for(int i = 0; i < ChainCount; i++)
        {
            if (current == null) break;
            current.TakeDamage(Damage);
            attacked.Add(current);
            current = FindNextTarget(current, attacked);
        }
    }

    public BaseEnemy FindNextTarget(BaseEnemy from, List<BaseEnemy> attacked)
    {
        Collider[] hits = FindTargetsInRange(from.transform.position, chainRange);

        List<BaseEnemy> candidates = new List<BaseEnemy>();
        foreach (var hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy == null || attacked.Contains(enemy)) continue;
            candidates.Add(enemy);
        }

        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }
}
