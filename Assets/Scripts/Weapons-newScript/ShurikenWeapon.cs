using System.Collections.Generic;
using UnityEngine;

public class ShurikenWeapon : WeaponBase
{
    [Header("Shuriken")]
    [SerializeField] private ShurikenOrbit shurikenPrefab;

    private readonly List<ShurikenOrbit> shurikens = new();
    
    // ProjectileCount는 WeaponBase의 StatContainer 기반 값
    private int ShurikenCount => Mathf.Max(1, ProjectileCount);

    // [변경] projectileSpeed는 아직 안 쓰기로 했으므로 고정값 유지
    // 나중에 ProjectileSpeed StatContainer 추가하면 이 부분만 바꾸면 됨
    private const float RotateSpeed = 180f;

    protected override void OnActivate()
    {
        SpawnShurikens();
    }

    protected override void OnDeactivate()
    {
        ClearShurikens();
    }

    protected override void Attack()
    {
        // 수리검은 쿨타임 공격이 아니라
        // 생성된 ShurikenOrbit이 계속 회전하면서 충돌 데미지 처리
    }

    private void SpawnShurikens()
    {
        ClearShurikens();

        if (shurikenPrefab == null)
        {
            Debug.LogError($"{name}: Shuriken Prefab이 비어있음");
            return;
        }

        int count = ShurikenCount;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float startAngle = angleStep * i;

            GameObject obj = PoolManager.Instance.Spawn(
                shurikenPrefab.gameObject,
                transform.position,
                Quaternion.identity
            );

            ShurikenOrbit shuriken = obj.GetComponent<ShurikenOrbit>();

            if (shuriken == null)
            {
                Debug.LogError("ShurikenOrbit 컴포넌트 없음");
                continue;
            }

            shuriken.Init(
                owner: transform,
                direction: Vector3.forward,
                damage: Damage,
                speed: RotateSpeed,
                targetLayer: targetLayer,
                obstacleLayer: obstacleLayer,
                prefab: shurikenPrefab.gameObject
            );

            shuriken.SetOrbitData(
                radius: Range,
                startAngle: startAngle
            );

            shurikens.Add(shuriken);
        }
    }
    

    private void ClearShurikens()
    {
        foreach (ShurikenOrbit shuriken in shurikens)
        {
            if (shuriken != null)
            {
                shuriken.Return();
            }
        }

        shurikens.Clear();
    }

    // [추가 추천] 수리검 개수가 업그레이드되었을 때 다시 생성
    public override void AddModifier(StatType type, StatModifier modifier)
    {
        base.AddModifier(type, modifier);

        if (!IsActive)
            return;

        if (type == StatType.ProjectileCount || type == StatType.Range || type == StatType.Attack)
        {
            SpawnShurikens();
        }
    }
}