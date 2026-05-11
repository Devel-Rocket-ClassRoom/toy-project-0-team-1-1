using UnityEngine;

public class SwordAuraProjectile : ProjectileBase
{
    [SerializeField]
    private float lifeTime = 5f;
    private float _timer;

    public override void Init(ProjectileInitData data)
    {
        base.Init(data);
        transform.localScale = Vector3.one * data.size;

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startSizeMultiplier = data.size;
        }
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
