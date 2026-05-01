using UnityEngine;

public class SlasherWeapon : DirectionalWeaponBase
{
    [Header("Slasher")]
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private Transform effectPoint;

    public override void Attack()
    {
        Vector3 attackDir = GetAttackDirection();

        Collider[] hits = Physics.OverlapSphere(transform.position, Range, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 dirToTarget = hit.transform.position - transform.position;
            dirToTarget.y = 0f;

            if (dirToTarget.sqrMagnitude <= 0.001f)
                continue;

            dirToTarget.Normalize();

            float angle = Vector3.Angle(attackDir, dirToTarget);

            if (angle <= attackAngle * 0.5f)
            {
                Debug.Log($"슬래셔 타격: {hit.name} / 데미지: {Damage}");
            }
        }

        SpawnSlashEffect(attackDir);
    }

    private void SpawnSlashEffect(Vector3 attackDir)
    {
        if (slashEffectPrefab == null)
            return;

        Vector3 spawnPos = effectPoint != null ? effectPoint.position : transform.position;

        Instantiate(
            slashEffectPrefab,
            spawnPos,
            Quaternion.LookRotation(attackDir)
        );
    }
}