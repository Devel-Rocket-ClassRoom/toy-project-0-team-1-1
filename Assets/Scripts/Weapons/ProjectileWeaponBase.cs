using UnityEngine;

public abstract class ProjectileWeaponBase : DirectionalWeaponBase
{
    [Header("Projectile")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firePoint;

    protected GameObject SpawnProjectile(Vector3 direction)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(direction)
        );

        return projectile;
    }
}