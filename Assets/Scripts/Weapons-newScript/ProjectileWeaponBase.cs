using UnityEngine;

public abstract class ProjectileWeaponBase : WeaponBase
{
    [Header("Projectile")]
    [SerializeField]
    protected GameObject projectilePrefab;

    [SerializeField]
    protected Transform firePoint;

    protected override void InitStats()
    {
        base.InitStats();
        stats[StatType.ProjectileCount] = new StatContainer(weaponData.projectileCount);
    }

    protected ProjectileBase SpawnProjectile(Vector3 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError($"{name}: Projectile Prefab�� �������");
            return null;
        }

        Vector3 spawnPos =
            firePoint != null ? firePoint.position : transform.position + Vector3.up * 1f;

        GameObject obj = PoolManager.Instance.Spawn(
            projectilePrefab,
            spawnPos,
            Quaternion.LookRotation(direction)
        );
        ProjectileBase projectile = obj.GetComponent<ProjectileBase>();

        var data = new ProjectileInitData
        {
            owner = transform,
            direction = direction,
            damage = Damage,
            speed = weaponData.projectileSpeed,
            targetLayer = targetLayer,
            obstacleLayer = obstacleLayer,
            prefab = projectilePrefab,
            size = Size,
            knockBack = KnockBack,
        };
        projectile.Init(data);
        if (projectile is ExplosiveProjectile explosive)
        {
            explosive.SetExplosionRadius(Size);
        }

        return projectile;
    }
}
