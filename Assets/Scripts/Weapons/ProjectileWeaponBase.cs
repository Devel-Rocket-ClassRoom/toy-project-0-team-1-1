using UnityEngine;

public abstract class ProjectileWeaponBase : DirectionalWeaponBase //투사체형 무기의 베이스 코드
{
    [Header("Projectile")]
    [SerializeField] protected ProjectileBase projectilePrefab;
    [SerializeField] protected Transform firePoint;

    protected ProjectileBase SpawnProjectile(Vector3 direction)
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        ProjectileBase projectile = Instantiate(projectilePrefab,spawnPos,Quaternion.LookRotation(direction));

        projectile.Init(owner: transform, direction: direction, damage: Damage, targetLayer: targetLayer);

        return projectile;
    }
}