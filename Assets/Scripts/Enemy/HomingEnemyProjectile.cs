using UnityEngine;

public class HomingEnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    // 초당 최대 회전각도 (낮을수록 보정이 약함)
    [SerializeField]
    private float homingAnglePerSecond = 60f;

    [SerializeField]
    private float lifeTime = 5f;

    private GameObject _prefab;
    private float _damage;
    private Vector3 _direction;
    private Transform _player;
    private float _timer;

    public void Init(GameObject prefab, float damage, Vector3 direction)
    {
        _prefab = prefab;
        _damage = damage;
        _direction = direction;

        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;
    }

    private void OnEnable()
    {
        _timer = 0f;
    }

    private void Update()
    {
        if (_player != null)
        {
            Vector3 toPlayer = (_player.position + Vector3.up * 0.8f - transform.position).normalized;
            float maxDeg = homingAnglePerSecond * Time.deltaTime;
            _direction = Vector3.RotateTowards(_direction, toPlayer, maxDeg * Mathf.Deg2Rad, 0f);
        }

        transform.position += _direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(_direction);

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
            ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerStatus = other.GetComponent<PlayerStatus>();
        if (playerStatus == null)
            return;
        playerStatus.TakeDamage(_damage);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        PoolManager.Instance.Despawn(_prefab, gameObject);
    }
}
