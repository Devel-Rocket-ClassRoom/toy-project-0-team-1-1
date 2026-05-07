using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenWeapon : WeaponBase
{
    [Header("Shuriken Orbit")]
    [SerializeField] private ShurikenOrbit shurikenPrefab;

    private readonly List<ShurikenOrbit> shurikens = new();
    private Coroutine orbitRoutine;

    private int ShurikenCount => Mathf.Max(1, ProjectileCount);
    private float OrbitDuration => Mathf.Max(0.1f, ExistTime);
    protected override bool CanAttack => orbitRoutine == null;
    protected override void OnActivate()
    {
        // 켜지자마자 한 번 나오게 하고 싶으면 유지
        Attack();
    }

    protected override void OnDeactivate()
    {
        if (orbitRoutine != null)
        {
            StopCoroutine(orbitRoutine);
            orbitRoutine = null;
        }

        ClearShurikens();
    }

    protected override void Attack()
    {
        // 이미 수리검이 떠 있는 중이면 중복 생성 방지
        if (orbitRoutine != null)
            return;

        orbitRoutine = StartCoroutine(OrbitRoutine());
    }

    private IEnumerator OrbitRoutine()
    {
        SpawnShurikens();

        yield return new WaitForSeconds(OrbitDuration);

        ClearShurikens();

        orbitRoutine = null;
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
    private void SpawnShurikens()
    {
        ClearShurikens();

        if (shurikenPrefab == null)
        {
            Debug.LogError($"{name}: Shuriken Prefab이 비어있습니다.");
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
                Debug.LogError("ShurikenOrbit 컴포넌트가 없습니다.");
                continue;
            }

            var data = new ProjectileInitData
            {
                owner = transform,
                direction = Vector3.forward,
                damage = Damage,
                speed = weaponData.projectileSpeed,
                targetLayer = targetLayer,
                obstacleLayer = obstacleLayer,
                prefab = shurikenPrefab.gameObject,
                size = Size,
                knockBack = 0f // 넉백 없으면 0
            };

            shuriken.Init(data);
            shuriken.SetOrbitData(radius: Range, startAngle: startAngle);

            shurikens.Add(shuriken);
        }
    }


}