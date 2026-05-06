using UnityEngine;

public class ShotgunWeapon : ProjectileWeaponBase
{
    private readonly float[] spreadAngles = { 30f, 35f, 40f, 45f, 50f };
    private float SpreadAngle => spreadAngles[Level];
    private int BulletCount => (int)stats[StatType.ProjectileCount].FinalValue;

    protected override void Attack()
    {
        Vector3 baseDir = GetDirectionToNearestTarget();
        for (int i = 0; i < BulletCount; i++)
        {
            float t = BulletCount == 1 ? 0.5f : (float)i / (BulletCount - 1);
            float angle = Mathf.Lerp(-SpreadAngle / 2, SpreadAngle / 2, t);
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;
            SpawnProjectile(dir);
        }
    }
}