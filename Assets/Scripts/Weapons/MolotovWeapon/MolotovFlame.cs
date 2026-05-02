using UnityEngine;

public class MolotovFlame : MonoBehaviour
{
    [SerializeField] private float duration = 3f; // 장판 지속시간
    [SerializeField] private float tickInterval = 0.5f; // 도트 데미지 주기 
    [SerializeField] private float flameRange = 2.5f; // 장판 크기
    [SerializeField] private LayerMask targetLayer;

    private GameObject _prefab;
    private float _damage;
    private float _timer;
    private float _tickTimer;

    public void Init(GameObject prefab, float damage)
    {
        _prefab = prefab;
        _damage = damage;
        _timer = 0f;
        _tickTimer = 0f;
    }
    private void OnEnable()
    {
        _timer = 0f;
        _tickTimer = 0f;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        _tickTimer += Time.deltaTime;

        if(_tickTimer > tickInterval)
        {
            _tickTimer = 0f;
            Collider[] hits = Physics.OverlapSphere(transform.position, flameRange, targetLayer);
            foreach (var  hit in hits)
            {
                hit.GetComponent<BaseEnemy>()?.TakeDamage(_damage);
            }
        }

        if(_timer > duration)
        {
            PoolManager.Instance.Despawn(_prefab, gameObject);
        }
    }
}
