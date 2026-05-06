using UnityEngine;

public class SwordAuraProjectile : ProjectileBase
{
    [SerializeField] private float lifeTime = 5f;
    private float _timer;

    public override void Init(Transform owner, Vector3 direction, float damage, float speed, LayerMask targetLayer, LayerMask obstacleLayer, GameObject prefab, float size)
    {
        base.Init(owner, direction, damage, speed, targetLayer, obstacleLayer, prefab, size);
        transform.localScale = Vector3.one * size;
    }

    private void OnEnable()
    {
        _timer = 0f;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Hit(other);
    }
}