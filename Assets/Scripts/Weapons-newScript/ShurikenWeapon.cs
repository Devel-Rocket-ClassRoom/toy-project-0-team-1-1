using UnityEngine;
using UnityEngine.AI;

public class ShurikenWeapon : WeaponBase
{
    [Header("Trap")]
    [SerializeField] private ShurikenTrap trapPrefab;

    [Header("Spawn Check")]
    [SerializeField] private float navMeshSampleDistance = 2f;
    [SerializeField] private int maxSpawnTryCount = 20;
    [SerializeField] private float groundOffset = 0.03f;

    protected override void Attack()
    {
        int count = Mathf.Max(1, ProjectileCount);

        for (int i = 0; i < count; i++)
        {
            SpawnTrap();
        }
    }

    private void SpawnTrap()
    {
        if (trapPrefab == null)
        {
            Debug.LogError($"{name}: Trap Prefab이 비어있습니다.");
            return;
        }

        if (!TryGetValidSpawnPosition(out Vector3 pos))
        {
            Debug.LogWarning("수리검 트랩 생성 실패: 유효한 위치를 찾지 못했습니다.");
            return;
        }

        GameObject obj = PoolManager.Instance.Spawn(
            trapPrefab.gameObject,
            pos,
            Quaternion.identity
        );

        ShurikenTrap trap = obj.GetComponent<ShurikenTrap>();

        trap.InitTrap(
            owner: transform,
            damage: Damage,
            duration: ExistTime,
            size: Size,
            targetLayer: targetLayer,
            obstacleLayer: obstacleLayer,
            prefab: trapPrefab.gameObject
        );
    }

    private bool TryGetValidSpawnPosition(out Vector3 result)
    {
        for (int i = 0; i < maxSpawnTryCount; i++)
        {
            Vector2 random = Random.insideUnitCircle * Range;

            Vector3 randomPos = transform.position + new Vector3(
                random.x,
                0f,
                random.y
            );

            if (NavMesh.SamplePosition(
                randomPos,
                out NavMeshHit hit,
                navMeshSampleDistance,
                NavMesh.AllAreas))
            {
                result = hit.position + Vector3.up * groundOffset;
                return true;
            }
        }

        result = transform.position;
        return false;
    }
}