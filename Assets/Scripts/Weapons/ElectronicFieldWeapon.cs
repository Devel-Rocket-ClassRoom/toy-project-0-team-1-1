using UnityEngine;

public class ElectricFieldWeapon : AreaWeaponBase
{
    [SerializeField] private GameObject effectObject;

    protected override void OnActivate() // 선택을했을때 이펙트 켜기
    {
        if (effectObject != null)
            effectObject.SetActive(true);
    }

    protected override void OnDeactivate() //비활성화일때 이펙트 끄기
    {
        if (effectObject != null)
            effectObject.SetActive(false);
    }

    public override void Attack() //공격범위내에 있는 모든타겟에게 데미지를 주는 로직 구현 필요(단순로그만띄움)
    {
        Collider[] hits = FindTargetsInRange(transform.position, Range);

        foreach (Collider hit in hits)
        {
            Debug.Log($"전자기장 타격: {hit.name} / 데미지: {Damage}");
        }
    }
}