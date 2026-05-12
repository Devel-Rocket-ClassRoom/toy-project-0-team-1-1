using UnityEngine;

public class ElectricFieldWeapon : WeaponBase
{
    private GameObject effectObject;

    // private float _tickInterval = 1f; // 데미지 주기
    private float _tickTimer;

    [SerializeField]
    private float visualRadiusAtScaleOne = 2f;

    [Header("Lifesteal")]
    [SerializeField, Range(0f, 1f)]
    private float lifestealRatio = 0.05f; // 적 한 마리 적중당 데미지의 몇 % 회복

    private PlayerStatus _ownerStatus;

    protected override void Awake()
    {
        base.Awake();
        effectObject = transform.Find("ElectronicFieldEffect")?.gameObject;
        _ownerStatus = GetComponentInParent<PlayerStatus>();
        UpdateEffectSize();
    }

    protected override void Attack() { }

    private void Update()
    {
        if (!IsActive)
            return;
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
            var entity = target.GetComponent<BaseEntity>();
            if (entity == null || entity.IsDead) continue;
            entity.TakeDamage(Damage);
            if (_ownerStatus != null && lifestealRatio > 0f)
            {
                _ownerStatus.Heal(Damage * lifestealRatio);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return;
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
