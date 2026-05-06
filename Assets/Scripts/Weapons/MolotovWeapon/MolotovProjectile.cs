using UnityEngine;

public class MolotovProjectile : MonoBehaviour
{
    [SerializeField] private GameObject flamePrefab; // 장판 프리팹
    [SerializeField] private float arcHeight = 3f; // 포물선 높이
    [SerializeField] private float speed = 8f;
    [SerializeField] private float rotationSpeed = 360f;

    private GameObject _prefab;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _damage;
    private float _progress;
    private float _duration;
    private float _flameRange;

    public void Init(GameObject prefab, Vector3 targetPos, float damage, float flameRange, float duration)
    {
        _prefab = prefab;
        _targetPos = targetPos;
        _damage = damage;
        _startPos = transform.position;
        _progress = 0;
        _flameRange = flameRange;
        _duration = duration;
    }
    private void OnEnable()
    {
        _progress = 0;
    }
    private void Update()
    {
        _progress += speed * Time.deltaTime / Vector3.Distance(_startPos, _targetPos);
        
        Vector3 flatPos = Vector3.Lerp(_startPos, _targetPos, _progress);
        float height = Mathf.Sin(_progress * Mathf.PI) * arcHeight;
        transform.position = flatPos + Vector3.up * height;

        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if (_progress >= 1)
        {
            Debug.Log($"화염병 도착! 장판 생성: {_targetPos}");
            GameObject flame = PoolManager.Instance.Spawn(flamePrefab, _targetPos, Quaternion.identity);
            Debug.Log($"Flame 스폰됨: {flame.name}");
            flame.GetComponent<MolotovFlame>().Init(flamePrefab, _damage, _flameRange, _duration);
            PoolManager.Instance.Despawn(_prefab, gameObject);
        }
    }
}
