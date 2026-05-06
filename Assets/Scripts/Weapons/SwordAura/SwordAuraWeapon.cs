using System.Collections;
using UnityEngine;

public class SwordAuraWeapon : ProjectileWeaponBase
{
    private ParticleSystem effectObject;
    [SerializeField] private float fireInterval = 0.3f;
    private Coroutine _currentFire;
    protected override void Awake()
    {
        base.Awake();
        effectObject = transform.Find("SwordAuraEffect")?.gameObject.GetComponentInChildren<ParticleSystem>();
    }
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
    private void UpdateEffectSize()
    {
        float sizeRatio = Size / weaponData.size;

        var main = effectObject.main;
        main.startSizeMultiplier = sizeRatio;
        Debug.Log(sizeRatio);
    }
    public override void AddModifier(StatType type, StatModifier modifier)
    {
        base.AddModifier(type, modifier);
        Debug.Log($"AddModifier: type={type}, effectObject={effectObject != null}, IsActive={IsActive}");
        if (type == StatType.Size && effectObject != null && IsActive)
        {
            UpdateEffectSize();
            Debug.Log($"이펙트 크기 갱신: {Size}");
        }
    }
}
