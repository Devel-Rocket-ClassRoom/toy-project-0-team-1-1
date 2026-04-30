using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] protected float baseDamage = 10f;
    [SerializeField] protected float baseCooldown = 1f;
    [SerializeField] protected float baseRange = 10f;

    [Header("Target")]
    [SerializeField] protected LayerMask targetLayer;

    private float attackTimer;

    //나중에 스탯컨테이너 변경시 바꿀값
    protected virtual float Damage => baseDamage;
    protected virtual float Cooldown => baseCooldown;
    protected virtual float Range => baseRange;

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= Cooldown)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    //모든 무기는 이걸 구현해야 함, 상속받고 진행
    protected abstract void Attack();

    // ==============================
    // 타겟 탐색 함수
    // ==============================

    protected Transform FindNearestTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Range, targetLayer);

        Transform nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            float dist = (hit.transform.position - transform.position).sqrMagnitude;

            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }

    protected Transform FindRandomTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Range, targetLayer);

        if (hits.Length == 0)
            return null;

        int index = Random.Range(0, hits.Length);
        return hits[index].transform;
    }

    // ==============================
    // 방향 계산 (3D 핵심)
    // ==============================

    protected Vector3 GetDirectionToTarget(Transform target)
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        return dir.normalized;
    }
}