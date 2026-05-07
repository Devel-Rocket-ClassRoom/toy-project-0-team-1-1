using UnityEngine;
public class ElectricFieldWeapon : WeaponBase
{
    private GameObject effectObject;
    // private float _tickInterval = 1f; // 데미지 주기
    private float _tickTimer;
    [SerializeField] private float visualRadiusAtScaleOne = 2f;
    protected override void Awake()
    {
        base.Awake();
        effectObject = transform.Find("ElectronicFieldEffect")?.gameObject;
        UpdateEffectSize();
    }

    protected override void Attack()
    {
    }

    private void Update()
    {
        if (!IsActive) return;
        _tickTimer += Time.deltaTime;
        if (_tickTimer > Cooldown)
        {
            _tickTimer = 0f;
            DealDamage();
        }
    }
    private void DealDamage()
    {
        float damageRadius = Size * visualRadiusAtScaleOne;
        var targets = FindTargetsInRange(transform.position, damageRadius);
        foreach (var target in targets)
        {
            target.GetComponent<BaseEntity>()?.TakeDamage(Damage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Size * visualRadiusAtScaleOne);
    }

    private void UpdateEffectSize()
    {
        effectObject.transform.localScale = Vector3.one * Size;
    }
    public override void AddModifier(StatType type, StatModifier modifier)
    {
        base.AddModifier(type, modifier);
        if (type == StatType.Size && effectObject != null && IsActive)
        {
            UpdateEffectSize();
            Debug.Log($"이펙트 크기 갱신: {Size}");
        }
    }
}