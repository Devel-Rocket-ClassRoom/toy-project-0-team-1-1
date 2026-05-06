using System.Collections.Generic;
using UnityEngine;

public class ElectronicStaffWeapon : WeaponBase
{
    [SerializeField] private GameObject lightningPrefab; // 라이트닝스크립트 붙어있는 프리팹

    //private readonly int[] chainCounts = { 3, 4, 5, 6, 7 };

    private int ChainCount => ProjectileCount; // 에너미 타격 수
    private float ChainRange => Range; // 무기 발동 거리

    protected override void Attack()
    {
        BaseEnemy firstTarget = FindRandomEnemyInRange(transform.position, Range);
        if (firstTarget == null) return;
        ChainAttack(firstTarget);
    }

    private void ChainAttack(BaseEnemy startTarget)
    {
        BaseEnemy current = startTarget;
        List<BaseEnemy> attacked = new List<BaseEnemy>();

        // 첫 번째는 플레이어 → 첫 타겟
        if (lightningPrefab != null)
        {
            GameObject lightning = Instantiate(lightningPrefab, transform.position, Quaternion.identity);
            Transform hostTransform = lightning.transform.Find("Host");
            if (hostTransform != null)
            {
                LighteningScript script = hostTransform.GetComponent<LighteningScript>();
                if (script != null)
                {
                    script.target = current.gameObject;
                    script.Initialize();
                }
            }
            Destroy(lightning, 0.3f);
        }

        for (int i = 0; i < ChainCount; i++)
        {
            if (current == null) break;

            current.TakeDamage(Damage);
            attacked.Add(current);

            BaseEnemy next = FindNextTarget(current, attacked);
            if (next != null && lightningPrefab != null)
            {
                GameObject lightning = Instantiate(lightningPrefab, current.transform.position, Quaternion.identity);
                Transform hostTransform = lightning.transform.Find("Host");
                if (hostTransform != null)
                {
                    LighteningScript script = hostTransform.GetComponent<LighteningScript>();
                    if (script != null)
                    {
                        script.target = next.gameObject;
                        script.Initialize();
                    }
                }
                Destroy(lightning, 0.3f);
            }

            current = next;
        }
    }

    private BaseEnemy FindNextTarget(BaseEnemy from, List<BaseEnemy> attacked)
    {
        Collider[] hits = FindTargetsInRange(from.transform.position, ChainRange);
        List<BaseEnemy> candidates = new List<BaseEnemy>();

        foreach (Collider hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy == null || attacked.Contains(enemy)) continue;
            candidates.Add(enemy);
        }

        if (candidates.Count == 0) return null;
        return candidates[Random.Range(0, candidates.Count)];
    }

    private BaseEnemy FindRandomEnemyInRange(Vector3 center, float radius)
    {
        Collider[] hits = FindTargetsInRange(center, radius);
        List<BaseEnemy> enemies = new List<BaseEnemy>();

        foreach (Collider hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy != null) enemies.Add(enemy);
        }

        if (enemies.Count == 0) return null;
        return enemies[Random.Range(0, enemies.Count)];
    }
}