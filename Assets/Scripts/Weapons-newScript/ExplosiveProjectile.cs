using UnityEngine;

public class ExplosiveProjectile : ProjectileBase
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject explosionEffectPrefab;

    private float _timer;

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
        if (!IsTarget(other)) return;

        Explode();
        ReturnToPool();
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayer);
        foreach (Collider hit in hits)
        {
            hit.GetComponent<BaseEnemy>()?.TakeDamage(damage);
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
