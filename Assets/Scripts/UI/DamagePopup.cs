using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private Vector3 randomOffsetRange = new(0.3f, 0.2f, 0f);
    [SerializeField] private DamagePopupData popupData;

    private TextMeshPro _text;
    private Camera _cam;
    private float _elapsed;
    private Color _baseColor;
    private Vector3 _floatDir;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
        _cam = Camera.main;
    }

    public void Setup(int damage, bool isCritical = false)
    {
        _text.text = damage.ToString();
        _baseColor = isCritical ? Color.yellow : Color.white;
        _text.color = _baseColor;
        _text.fontSize = isCritical ? 48 : 36;

        // 같은 위치에 겹치지 않도록 약간의 랜덤 오프셋
        Vector3 offset = new(
            Random.Range(-randomOffsetRange.x, randomOffsetRange.x),
            Random.Range(0, randomOffsetRange.y),
            0f
        );
        transform.position += offset;

        _floatDir = (Vector3.up + offset.normalized * 0.5f).normalized;
        _elapsed = 0f;
    }

    private void LateUpdate()
    {
        _elapsed += Time.deltaTime;

        // 위로 떠오르기
        transform.position += _floatDir * floatSpeed * Time.deltaTime;

        // 카메라 향하게 (Billboard)
        if (_cam != null)
            transform.rotation = _cam.transform.rotation;

        // 페이드 아웃
        float t = _elapsed / lifetime;
        Color c = _baseColor;
        c.a = Mathf.Lerp(1f, 0f, t);
        _text.color = c;

        if (_elapsed >= lifetime) PoolManager.Instance.Despawn(popupData.damagePopup, this.gameObject);
            // Destroy(gameObject); // 풀링 쓰면 ReturnToPool로 교체
    }
}