using UnityEngine;

public class ElectronicFiledWepon : WeaponBase
{
    protected override void Attack()
    {
        // 범위 안의 모든 타겟 탐색
        Collider[] hits = Physics.OverlapSphere(transform.position, Range, targetLayer);

        foreach (Collider hit in hits)
        {
            // 아직 몬스터 구현 안했으니까 로그로 확인
            Debug.Log($"전자기장 타격: {hit.name}");

            // 나중에 이렇게 바뀜
            // hit.GetComponent<IDamageable>()?.OnDamage(Damage);
        }
    }
}
