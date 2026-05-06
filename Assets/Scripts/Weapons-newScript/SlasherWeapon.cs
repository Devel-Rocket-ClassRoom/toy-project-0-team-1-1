using UnityEngine;

public class SlasherWeapon : ProjectileWeaponBase
{
    
    private float SwingAngle => Range;
    
    private int SlashCount => Mathf.Max(1, ProjectileCount);

    protected override void Attack()
    {
        Vector3 baseDir = GetDirectionToNearestTarget();

        for (int i = 0; i < SlashCount; i++)
        {
            float t = SlashCount == 1 ? 0.5f : (float)i / (SlashCount - 1);
            float angle = Mathf.Lerp(-SwingAngle / 2f, SwingAngle / 2f, t);
            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;

            SpawnProjectile(dir);
        }
    }
}