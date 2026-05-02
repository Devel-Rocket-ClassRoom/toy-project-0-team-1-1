using UnityEngine;

public class MolotovWeapon : AreaWeaponBase
{
    [SerializeField] private GameObject molotovPrefab;
    [SerializeField] private float thorwRange = 8f; // 던지는 반지름
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 5;
    public override void Attack()
    {
        float startAngle = Random.Range(0f, 360f);
        for(int i = 0; i < level; i++)
        {
            Vector3 targetPos = GetThrowPosition(i, startAngle);
            GameObject obj = PoolManager.Instance.Spawn(molotovPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<MolotovProjectile>().Init(molotovPrefab, targetPos, Damage);
        }
    }
    public Vector3 GetThrowPosition(int index, float startAngle)
    {
        float angle = startAngle + (360 / level) * index;
        float rad = angle * Mathf.Deg2Rad;
        return transform.position + new Vector3(Mathf.Cos(rad) , 0f, Mathf.Sin(rad)) * thorwRange;
    }
}
