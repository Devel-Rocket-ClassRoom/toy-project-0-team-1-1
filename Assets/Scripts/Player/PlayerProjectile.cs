using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifeTime = 5f;

    protected GameObject _prefab;
    protected Vector3 _direction;
    protected float _damage;
    protected float _timer;

    public virtual void Init(GameObject prefab, float damage, Vector3 direction)
    {
        _prefab = prefab;
        _damage = damage;
        _direction = direction;
        _timer = 0f;
    }
    private void OnEnable()
    {
        _timer = 0f;   
    }

    protected virtual void Update()
    {
        transform.position += Time.deltaTime * speed * _direction;
        _timer += Time.deltaTime;
        if( _timer > lifeTime)
        {
            PoolManager.Instance.Despawn(_prefab, gameObject);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<BaseEnemy>();
        if (enemy == null) return;
        enemy.TakeDamage(_damage);
        PoolManager.Instance.Despawn(_prefab, gameObject);
    }
}
