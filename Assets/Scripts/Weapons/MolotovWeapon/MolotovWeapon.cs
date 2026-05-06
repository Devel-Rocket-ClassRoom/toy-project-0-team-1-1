using UnityEngine;

public class MolotovWeapon : WeaponBase
{
    [SerializeField] private GameObject molotovProjectilePrefab;
    [SerializeField] private float throwRange = 8f;
    protected override void Attack()
    {
        int count = weaponData.projectileCount;
        float startAngle = Random.Range(0f, 360f);
        for (int i = 0; i < count; i++)
        {
            Vector3 targetPos = GetThrowPosition(i, count, startAngle);
            GameObject obj = PoolManager.Instance.Spawn(molotovProjectilePrefab, transform.position, Quaternion.identity);
            obj.GetComponent<MolotovProjectile>().Init(molotovProjectilePrefab, targetPos, Damage, Size, ExistTime);
        }
    }

    private Vector3 GetThrowPosition(int index, int count, float startAngle)
    {
        float angle = startAngle + (360f / count) * index;
        float rad = angle * Mathf.Deg2Rad;
        return transform.position + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * throwRange;
    }
}
