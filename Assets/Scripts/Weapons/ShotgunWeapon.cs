using UnityEngine;

//public class ShotgunWeapon : DirectionalWeaponBase
//{
//    [SerializeField] private GameObject bulletPrefab;
//    [SerializeField] private int level = 1;


//    //레벨별 총알 갯수와 퍼지는 각도
//    private readonly int[] bulletCounts = { 3, 4, 5, 6, 7 };
//    private readonly float[] spreadAngles = { 30f, 35f, 40f, 45f, 50f };

//    private int BulletCount => bulletCounts[level - 1];
//    private float SpreadAngle => spreadAngles[level - 1];
    

//    //public override void Attack()
//    //{
//    //    Vector3 baseDir = GetAttackDirection();
//    //    for (int i = 0; i < BulletCount; i++)
//    //    {
//    //        float t = BulletCount == 1 ? 0.5f : (float)i / (BulletCount - 1);
//    //        float angle = Mathf.Lerp(-SpreadAngle / 2, SpreadAngle / 2, t);
//    //        Vector3 dir = Quaternion.Euler(0f, angle, 0f) * baseDir;
//    //        GameObject obj = PoolManager.Instance.Spawn(bulletPrefab, transform.position, Quaternion.LookRotation(dir));
//    //        obj.GetComponent<PlayerProjectile>().Init(bulletPrefab, Damage, dir);
//    //    }
//    //}
//}
