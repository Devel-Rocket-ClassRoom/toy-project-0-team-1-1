using System.Collections.Generic;
using UnityEngine;

public class ShurikenWeapon : WeaponBase
{
    [Header("Shuriken")]
    [SerializeField] private ShurikenOrbit shurikenPrefab;


    private readonly List<ShurikenOrbit> shurikens = new();

    private int ShurikenCount => weaponData != null ? weaponData.projectileCount + Level : 1 + Level;
    private float RotateSpeed => weaponData != null ? weaponData.projectileSpeed : 180f;

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
        // 수리검은 쿨타임마다 공격하지 않음
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

        int count = Mathf.Max(1, ShurikenCount);
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float startAngle = angleStep * i;

            ShurikenOrbit shuriken = Instantiate(shurikenPrefab);

            shuriken.Init(
                owner: transform,
                direction: Vector3.forward,
                damage: Damage,
                speed: RotateSpeed,
                targetLayer: targetLayer,
                obstacleLayer: obstacleLayer,
                prefab: shurikenPrefab.gameObject,
                size: Size
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
}