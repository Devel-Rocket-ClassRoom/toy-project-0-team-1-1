using UnityEngine;

public class SlasherWeapon : ProjectileWeaponBase
{
    protected override void Attack()
    {
        Vector3 dir = GetDirectionToNearestTarget();

        SpawnProjectile(dir);
    }
}