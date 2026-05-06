using System.Collections;
using UnityEngine;

public class ThunderPorjectile : MonoBehaviour
{
    private GameObject _prefab;
    private float _damage;
    private float _thunderSize;
    private Vector3 _spawnPos;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float lifeTime = 2f;
    public void Init(GameObject prefab, float damage, float thunderSize, Vector3 spawnPos )
    {
        _thunderSize = thunderSize;
        _prefab = prefab;
        _damage = damage;
        _spawnPos = spawnPos;
    }

    public void SpawnThunder()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _thunderSize, _targetLayer);
        foreach (var hit in hits)
        {
            hit.GetComponent<BaseEnemy>()?.TakeDamage(_damage);
        }
        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(lifeTime);
        PoolManager.Instance.Despawn(_prefab, gameObject);
    }
}
