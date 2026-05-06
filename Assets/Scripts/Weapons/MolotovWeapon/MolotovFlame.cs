using UnityEngine;

public class MolotovFlame : MonoBehaviour
{
    private float _existTime; // = 3f  장판 지속시간
    private float _tickInterval = 0.5f; // 도트 데미지 주기 
    private float _flameSize; // = 2.5f; // 장판 크기
    [SerializeField] private LayerMask targetLayer;

    private GameObject _prefab;
    private float _damage;
    private float _timer;
    private float _tickTimer;

    public void Init(GameObject prefab, float damage, float flameSize, float existTime)
    {
        _existTime = existTime;
        _flameSize = flameSize;
        _prefab = prefab;
        _damage = damage;
        _timer = 0f;
        _tickTimer = 0f;

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            var main = ps.main;
            main.startSizeMultiplier = flameSize / 0.4f;
        }
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

        if(_tickTimer > _tickInterval)
        {
            _tickTimer = 0f;
            Collider[] hits = Physics.OverlapSphere(transform.position, _flameSize, targetLayer);
            foreach (var  hit in hits)
            {
                hit.GetComponent<BaseEnemy>()?.TakeDamage(_damage);
            }
        }

        if(_timer > _existTime)
        {
            PoolManager.Instance.Despawn(_prefab, gameObject);
        }
    }
}
