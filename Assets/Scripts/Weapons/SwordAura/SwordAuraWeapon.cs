using System.Collections;
using UnityEngine;

public class SwordAuraWeapon : ProjectileWeaponBase
{
    private ParticleSystem effectObject;

    [SerializeField]
    private float fireInterval = 0.3f;

    [SerializeField]
    private AudioClip auraClip;
    private Coroutine _currentFire;

    protected override void Attack()
    {
        if (_currentFire != null)
            return;

        _currentFire = StartCoroutine(SwordAuraFire());
    }

    private IEnumerator SwordAuraFire()
    {
        //var dir = GetDirectionToNearestTarget();
        for (int i = 0; i < ProjectileCount; i++)
        {
            var targets = FindTargetsInRange(transform.position, Range);
            Vector3 dir;
            if (targets.Length > 0)
            {
                // 적이 있으면 랜덤 적 방향
                dir = (
                    targets[Random.Range(0, targets.Length)].transform.position - transform.position
                ).normalized;
            }
            else
            {
                // 적이 없으면 플레이어가 보는 방향
                dir = transform.forward;
            }
            SFXManager.Instance.Play3D(auraClip, transform.position, 1f);
            SpawnProjectile(dir);
            if (i < ProjectileCount - 1)
            {
                yield return new WaitForSeconds(fireInterval);
            }
        }
        _currentFire = null;
    }
}
