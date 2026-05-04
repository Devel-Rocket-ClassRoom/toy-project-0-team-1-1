using UnityEngine;

public class SlasherWeapon : ProjectileWeaponBase //변경된 구조의 근접 기본공격 코드
{
    public override void Attack()
    {
        Vector3 dir = GetAttackDirection();
        SpawnProjectile(dir);
    }
}