using UnityEngine;

public class SlasherWeapon : ProjectileWeaponBase
{
    public override void Attack()
    {
        Vector3 dir = GetAttackDirection();
        SpawnProjectile(dir);
    }
}