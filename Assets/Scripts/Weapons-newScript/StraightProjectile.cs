using UnityEngine;

public class StraightProjectile : ProjectileBase
{
    [SerializeField] private float lifeTime = 5f;
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
        Hit(other);
    }
}