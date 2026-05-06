using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ThunderWeapon : WeaponBase
{
    [SerializeField] private float boundaryWidth = 15;
    [SerializeField] private float boundaryHeight = 15;
    [SerializeField] GameObject thunderProjectilePrefab;
    private Coroutine thunderCo;

    protected override void Attack()
    {
        if (thunderCo != null) return;
        thunderCo = StartCoroutine(SpawnThunders());
    }
    public IEnumerator SpawnThunders()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            Vector3 spawnPos = GetRandomPosition();
            GameObject flame = PoolManager.Instance.Spawn(thunderProjectilePrefab, spawnPos, Quaternion.identity);
            flame.GetComponent<ThunderPorjectile>().Init(thunderProjectilePrefab, Damage, Size, spawnPos);
            flame.GetComponent<ThunderPorjectile>().SpawnThunder();
            yield return new WaitForSeconds(0.2f);
        }
        thunderCo = null;
    }
    public Vector3 GetRandomPosition()
    {
        float x = transform.position.x + Random.Range(-boundaryWidth / 2f, boundaryWidth / 2f);
        float z = transform.position.z + Random.Range(-boundaryHeight / 2f, boundaryHeight / 2f);
        var pos = new Vector3(x, 0f, z);
        return pos;
    }
}
