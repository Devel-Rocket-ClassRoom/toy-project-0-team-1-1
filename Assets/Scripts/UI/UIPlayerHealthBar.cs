using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(10000)]
public class UIPlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private float pixelOffsetY = 60f;

    private Slider HpBar;
    private Camera cam;
    private RectTransform rt;
    private RectTransform canvasRect;
    private Canvas canvas;
    private Camera uiCam; // null이면 Overlay, 아니면 Canvas의 worldCamera
    private RectTransform parentRect;
    private void Awake()
    {
        cam = Camera.main;
        HpBar = GetComponent<Slider>();
        HpBar.value = 1f;
        rt = (RectTransform)transform;

        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
            ? null
            : canvas.worldCamera;
    }

    private void Start()
    {
        playerStatus.OnHpChange += HealthBarUpdate;
    }

    private void HealthBarUpdate(float hp)
    {
        HpBar.value = hp / playerStatus.MaxHp;
    }

    private void LateUpdate()
    {
        Vector3 viewport = cam.WorldToViewportPoint(playerStatus.transform.position);

        bool visible = viewport.z > 0f;
        rt.gameObject.SetActive(visible);
        if (!visible) return;

        Vector2 canvasSize = canvasRect.rect.size;
        rt.anchoredPosition = new Vector2(
            (viewport.x - 0.5f) * canvasSize.x,
            (viewport.y - 0.5f) * canvasSize.y + pixelOffsetY
        );
        //Debug.Log($"[{cam.name}] vp=({viewport.x:F3},{viewport.y:F3}) " +
        //  $"anchored={rt.anchoredPosition} " +
        //  $"screen={Screen.width}x{Screen.height} " +
        //  $"canvasRect={canvasRect.rect.size} " +
        //  $"canvasScale={canvas.scaleFactor} " +
        //  $"renderMode={canvas.renderMode}");
    }
}