using System.Collections;
using UnityEngine;

public class TitleFadeIn : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private CanvasGroup titleGroup;
    [SerializeField] private CanvasGroup[] buttonGroups;

    [Header("Timing")]
    [SerializeField] private float titleFadeDuration = 1.5f;
    [SerializeField] private float buttonFadeDuration = 0.6f;
    [SerializeField] private float delayAfterTitle = 0.3f;
    [SerializeField] private float delayBetweenButtons = 0.15f;

    [Header("Easing (선택)")]
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Start()
    {
        // 시작 시 전부 투명
        titleGroup.alpha = 0f;
        foreach (var bg in buttonGroups) bg.alpha = 0f;

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // 1) 타이틀 페이드 인
        yield return FadeIn(titleGroup, titleFadeDuration);

        yield return new WaitForSeconds(delayAfterTitle);

        // 2) 버튼들 순차적으로 페이드 인 (겹치게 하려면 StartCoroutine, 하나씩 끝나고 다음으로 가려면 yield return)
        foreach (var bg in buttonGroups)
        {
            StartCoroutine(FadeIn(bg, buttonFadeDuration));
            yield return new WaitForSeconds(delayBetweenButtons);
        }
    }

    private IEnumerator FadeIn(CanvasGroup cg, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            cg.alpha = fadeCurve.Evaluate(t);
            yield return null;
        }
        cg.alpha = 1f;
    }
}