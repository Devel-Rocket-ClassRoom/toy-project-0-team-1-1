using System.Collections;
using UnityEngine;

public class SwordAuraWeapon : ProjectileWeaponBase
{
    private ParticleSystem effectObject;
    [SerializeField] private float fireInterval = 0.3f;
    private Coroutine _currentFire;
    protected override void Attack()
    {
        if (_currentFire != null) return;

        _currentFire = StartCoroutine(SwordAuraFire());
    }
    private IEnumerator SwordAuraFire()
    {
        var dir = GetDirectionToNearestTarget();
        for (int i = 0; i < ProjectileCount; i++)
        {
            SpawnProjectile(dir);
            if (i < ProjectileCount - 1)
            {
                yield return new WaitForSeconds(fireInterval);
            }
        }
        _currentFire = null;
    }
}
