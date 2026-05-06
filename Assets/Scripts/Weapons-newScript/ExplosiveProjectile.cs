using UnityEngine;

public class ExplosiveProjectile : ProjectileBase
{
    private float lifeTime = 5f;
    private float explosionRadius; //= 5f;
    [SerializeField] private GameObject explosionEffectPrefab;

    private float _timer;

    public void SetExplosionRadius(float size)
    {
        explosionRadius = size;
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
            ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 장애물이면 터지기만 하고 끝
        if (((1 << other.gameObject.layer) & _obstacleLayer) != 0)
        {
            Explode(); // 폭발 이펙트 + 주변 적 데미지
            ReturnToPool();
            return;
        }

        if (!IsTarget(other)) return;

        Explode();
        ReturnToPool();
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        foreach (Collider hit in hits)
        {
            hit.GetComponent<BaseEntity>()?.TakeDamage(damage);
        }
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(effect, ps.main.duration);
            else
                Destroy(effect, 2f);
        }
    }
}
