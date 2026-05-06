using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fadePanel;          // 검정 Image
    [SerializeField] private TMP_Text gameOverText;    // "GAME OVER"
    [SerializeField] private CanvasGroup buttonGroup;  // 버튼 묶음만 CanvasGroup

    [Header("Timing")]
    [SerializeField] private float fadeToBlackDuration = 1.5f;
    [SerializeField] private float textFadeInDuration = 1.0f;
    [SerializeField] private float buttonFadeInDuration = 0.5f;
    [SerializeField] private float delayBeforeText = 0.3f;
    [SerializeField] private float delayBeforeButtons = 0.4f;

    [Header("Scene")]
    [SerializeField] private string titleSceneName = "TitleScene";

    [Header("Options")]
    [SerializeField] private bool pauseGameOnComplete = true;

    [SerializeField] private PlayerStatus playerStatus;

    private bool isPlaying;

    private void Awake()
    {
        SetImageAlpha(fadePanel, 0f);
        fadePanel.raycastTarget = false;

        SetTextAlpha(gameOverText, 0f);

        buttonGroup.alpha = 0f;
        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;

        playerStatus.OnDead += ShowGameOver;
    }

    public void ShowGameOver()
    {
        if (isPlaying) return;
        isPlaying = true;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // 1) 검은 패널 페이드. 페이드 시작과 동시에 뒷 입력 차단.
        fadePanel.raycastTarget = true;
        yield return Fade(fadeToBlackDuration,
            t => SetImageAlpha(fadePanel, t));

        yield return new WaitForSecondsRealtime(delayBeforeText);

        // 2) GAME OVER 텍스트
        yield return Fade(textFadeInDuration,
            t => SetTextAlpha(gameOverText, t));

        yield return new WaitForSecondsRealtime(delayBeforeButtons);

        // 3) 버튼 묶음 페이드 + alpha 1 도달 후에 클릭 허용
        yield return Fade(buttonFadeInDuration,
            t => buttonGroup.alpha = t);
        buttonGroup.interactable = true;
        buttonGroup.blocksRaycasts = true;

        if (pauseGameOnComplete)
            Time.timeScale = 0f;
    }

    /// <summary>0→1 보간을 콜백으로 흘려보내는 공용 페이드.</summary>
    private static IEnumerator Fade(float duration, System.Action<float> apply)
    {
        if (duration <= 0f) { apply(1f); yield break; }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            apply(Mathf.Clamp01(elapsed / duration));
            yield return null;
        }
        apply(1f);
    }

    private static void SetImageAlpha(Image img, float a)
    {
        Color c = img.color; c.a = a; img.color = c;
    }

    private static void SetTextAlpha(TMP_Text tmp, float a)
    {
        tmp.alpha = a;   // TMP는 전용 프로퍼티가 있음
    }

    // ─── Button OnClick에서 연결 ───
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnTitleButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }
}