using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    private Slider HpBar;
    private void Awake()
    {
        playerStatus.OnHpChange += HealthBarUpdate;
        HpBar = GetComponent<Slider>();
    }

    private void HealthBarUpdate(float hp)
    {

    }

    private void LateUpdate()
    {
        // 1) 플레이어 머리 위치를 화면 좌표로 변환
        Vector3 worldPos = target.transform.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        // 카메라 뒤로 가면 숨김
        bool visible = screenPos.z > 0;
        barRoot.gameObject.SetActive(visible);
        if (!visible) return;

        barRoot.position = screenPos;

        // 2) 게이지 부드럽게 보간
        displayedFill = Mathf.Lerp(displayedFill, targetFill, Time.deltaTime * lerpSpeed);
        fillImage.fillAmount = displayedFill;
    }
}
