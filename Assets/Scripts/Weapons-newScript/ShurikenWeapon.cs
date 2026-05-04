//using System.Collections.Generic;
//using UnityEngine;

//public class ShurikenWeapon : WeaponBase
//{
//    [Header("Shuriken")]
//    [SerializeField] private GameObject shurikenPrefab;
//    [SerializeField] private float rotateSpeed = 180f;
//    [SerializeField] private int shurikenCount = 1;

//    private readonly List<ShurikenOrbit> shurikens = new List<ShurikenOrbit>();

//    protected override void OnActivate()
//    {
//        SpawnShurikens();
//    }

//    protected override void OnDeactivate()
//    {
//        ClearShurikens();
//    }

//    public override void Attack()
//    {
//        // 수리검은 쿨타임 공격이 아니라
//        // 생성된 오브젝트가 계속 회전하며 공격함
//    }

//    private void SpawnShurikens()
//    {
//        ClearShurikens();

//        if (shurikenPrefab == null)
//        {
//            Debug.LogError("Shuriken Prefab이 비어있음");
//            return;
//        }

//        float angleStep = 360f / shurikenCount;

//        for (int i = 0; i < shurikenCount; i++)
//        {
//            float startAngle = angleStep * i;

//            GameObject obj = Instantiate(shurikenPrefab);

//            ShurikenOrbit orbit = obj.GetComponent<ShurikenOrbit>();

//            if (orbit == null)
//            {
//                Debug.LogError("ShurikenPrefab에 ShurikenOrbit이 없음");
//                Destroy(obj);
//                continue;
//            }

//            orbit.Init(
//                transform,
//                Range,
//                rotateSpeed,
//                Damage,
//                targetLayer,
//                startAngle
//            );

//            shurikens.Add(orbit);
//        }
//    }

//    private void ClearShurikens()
//    {
//        foreach (ShurikenOrbit shuriken in shurikens)
//        {
//            if (shuriken != null)
//                Destroy(shuriken.gameObject);
//        }

//        shurikens.Clear();
//    }

//    public void SetLevel(int level)
//    {
//        shurikenCount = Mathf.Max(1, level);

//        if (IsActive)
//            SpawnShurikens();
//    }
//}