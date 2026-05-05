using UnityEngine;

public class SlasherWeapon : ProjectileWeaponBase
{
    private readonly float[] swingAngles = { 60f, 75f, 90f, 110f, 130f }; // 슬래쉬 범위
    private float SwingAngle => swingAngles[Mathf.Clamp(Level, 0, swingAngles.Length - 1)];

    private int SlashCount => 1; //슬래시는 보통 1번 휘두름

    protected override void Attack()
    {
        Vector3 baseDir = GetDirectionToNearestTarget();

        for (int i = 0; i < SlashCount; i++)
        {
            float t = SlashCount == 1 ? 0.5f : (float)i / (SlashCount - 1);

            float angle = Mathf.Lerp(-SwingAngle / 2, SwingAngle / 2, t);

            Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;

            SpawnProjectile(dir);
        }
    }
}