using UnityEngine;

public abstract class ProjectileWeaponBase : WeaponBase
{
    [Header("Projectile")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firePoint;

    protected ProjectileBase SpawnProjectile(Vector3 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"{name}: Projectile Prefab이 비어있음");
            return null;
        }

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position + Vector3.up * 1f;

        GameObject obj = PoolManager.Instance.Spawn(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));
        ProjectileBase projectile = obj.GetComponent<ProjectileBase>();
        projectile.Init(transform, direction, Damage, weaponData.projectileSpeed, targetLayer, projectilePrefab );

        return projectile;
    }
}