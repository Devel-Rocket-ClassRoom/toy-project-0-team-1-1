using UnityEngine;
public class ElectricFieldWeapon : WeaponBase
{
    [SerializeField] private GameObject effectObject;
    private float _tickInterval = 0.5f; // 데미지 주기
    private float _tickTimer;

    protected override void Awake()
    {
        base.Awake();
        effectObject = transform.Find("SwordAuraEffect")?.gameObject;
    }

    protected override void Attack()
    {
    }

    private void Update()
    {
        if (!IsActive) return;
        _tickTimer += Time.deltaTime;
        if (_tickTimer > _tickInterval)
        {
            _tickTimer = 0f;
            DealDamage();
        }
    }
    private void DealDamage()
    {
        var targets = FindTargetsInRange(transform.position, Size);
        foreach (var target in targets)
        {
            target.GetComponent<BaseEnemy>()?.TakeDamage(Damage);
        }
    }
    

    private void UpdateEffectSize()
    {
        
        float sizeRatio = Size / weaponData.size; // 초기값 대비 비율
        effectObject.transform.localScale = Vector3.one * sizeRatio;
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