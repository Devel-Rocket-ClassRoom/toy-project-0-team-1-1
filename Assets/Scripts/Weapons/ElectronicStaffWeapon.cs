using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicStaffWeapon : WeaponBase
{
    [SerializeField]
    private GameObject lightningPrefab;

    [SerializeField]
    private AudioClip lightningClip;

    [SerializeField]
    private float chainDelay = 0.08f; // 호핑 간격

    [SerializeField]
    private float lightningLifetime = 0.3f;

    protected override void Attack()
    {
        //var targets = FindTargetsInRange(transform.position, Range);
        //if (targets == null || targets.Length == 0) return;
        for (int i = 0; i < ProjectileCount; i++)
        {
            var target = FindRandomEnemyInRange(transform.position, Range);
            if (target == null)
                continue;
            StartCoroutine(ChainAttackRoutine(target));
        }
        //BaseEnemy firstTarget = FindRandomEnemyInRange(transform.position, Range);
        //if (firstTarget == null) return;
        //StartCoroutine(ChainAttackRoutine(firstTarget));
    }

    private IEnumerator ChainAttackRoutine(BaseEnemy startTarget)
    {
        BaseEnemy current = startTarget;
        List<BaseEnemy> attacked = new List<BaseEnemy>();

        // 플레이어 → 첫 타겟
        SpawnLightning(transform, transform.position, current);

        for (int i = 0; i < 5; i++)
        {
            if (current == null)
                break;

            current.TakeDamage(Damage);
            attacked.Add(current);
            SFXManager.Instance.Play3D(lightningClip, current.transform.position, 0.5f);

            BaseEnemy next = FindNextTarget(current, attacked);
            if (next == null)
                break;

            yield return new WaitForSeconds(chainDelay);

            if (current == null || next == null)
                break;

            SpawnLightning(current.transform, current.transform.position, next);
            current = next;
        }
    }

    private void SpawnLightning(Transform parent, Vector3 pos, BaseEnemy target)
    {
        if (lightningPrefab == null)
            return;

        GameObject lightning = PoolManager.Instance.Spawn(
            lightningPrefab,
            pos,
            Quaternion.identity
        );
        lightning.transform.SetParent(parent);

        Transform hostTransform = lightning.transform.Find("Host");
        if (hostTransform != null)
        {
            LighteningScript script = hostTransform.GetComponent<LighteningScript>();
            if (script != null)
            {
                script.target = target.gameObject;
                script.Initialize();
            }
        }

        StartCoroutine(DespawnAfterDelay(lightning, lightningLifetime));
    }

    private BaseEnemy FindNextTarget(BaseEnemy from, List<BaseEnemy> attacked)
    {
        Collider[] hits = FindTargetsInRange(from.transform.position, Range);
        List<BaseEnemy> candidates = new List<BaseEnemy>();

        foreach (Collider hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy == null || attacked.Contains(enemy))
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

    private System.Collections.IEnumerator DespawnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            PoolManager.Instance.Despawn(lightningPrefab, obj);
        }
    }
}
