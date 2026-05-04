using UnityEngine;

public abstract class ProjectileWeaponBase : WeaponBase
{
    [Header("Projectile")]
    [SerializeField] protected ProjectileBase projectilePrefab;
    [SerializeField] protected Transform firePoint;

    protected ProjectileBase SpawnProjectile(Vector3 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"{name}: Projectile Prefab이 비어있음");
            return null;
        }

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        ProjectileBase projectile = Instantiate(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(direction)
        );

        projectile.Init(
            owner: transform,
            direction: direction,
            damage: Damage,
            targetLayer: targetLayer
        );

        return projectile;
    }
}