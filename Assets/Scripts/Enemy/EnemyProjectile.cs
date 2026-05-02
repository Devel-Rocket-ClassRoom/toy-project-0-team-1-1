using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float _lifeTime = 5f;

    private GameObject _prefab;
    private float _damage;
    private Vector3 _direction;
    private float _timer;

    public void Init(GameObject prefab, float damage, Vector3 direction)
    {
        _prefab = prefab;
        _damage = damage;
        _direction = direction;
    }

    public void OnEnable()
    {
        _timer = 0;
    }
    private void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
        _timer += Time.deltaTime;
        if(_lifeTime < _timer)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerStatus = other.GetComponent<PlayerStatus>();
        if (playerStatus == null) return;
        playerStatus.TakeDamage(_damage);
        ReturnToPool();
    }
    private void ReturnToPool()
    {
        PoolManager.Instance.Despawn(_prefab, gameObject);
    }
}
