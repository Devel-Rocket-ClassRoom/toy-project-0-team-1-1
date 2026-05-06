using System;
using System.Collections;
using UnityEngine;

public class ShotgunWeapon : ProjectileWeaponBase
{
    [SerializeField] GameObject fireEffectPrefab;
    private float SpreadAngle => Range; // 발사 각도
    private int BulletCount => ProjectileCount;

    protected override void Attack()
    {
        if (fireEffectPrefab != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            spawnPos.z += 1f;
            GameObject flash = PoolManager.Instance.Spawn(fireEffectPrefab, spawnPos, Quaternion.identity);
            StartCoroutine(ReturnFlashToPool(flash));
        }
        Vector3 baseDir = GetDirectionToNearestTarget();
        for (int i = 0; i < BulletCount; i++)
        {
            float t = BulletCount == 1 ? 0.5f : (float)i / (BulletCount - 1);
            float angle = Mathf.Lerp(-SpreadAngle / 2, SpreadAngle / 2, t);
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;
            SpawnProjectile(dir);
        }
    }
    private IEnumerator ReturnFlashToPool(GameObject flash)
    {
        yield return new WaitForSeconds(1f);
        PoolManager.Instance.Despawn(fireEffectPrefab, flash);
    }
}