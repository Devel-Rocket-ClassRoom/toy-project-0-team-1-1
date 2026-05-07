using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private int poolSize = 16;
    [SerializeField] private float minInterval = 0.05f;
    [SerializeField] private int maxConcurrentPerClip = 3;
    [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;

    private AudioSource source2D;
    private AudioSource[] pool3D;
    private int poolIndex = 0;

    private readonly Dictionary<AudioClip, float> lastPlay = new();
    private readonly Dictionary<AudioClip, int> activeCount = new();

    [Header("3D Rolloff")]
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

    private void Awake()
    {
        Instance = this;
        source2D = GetComponent<AudioSource>();
        pool3D = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            var go = new GameObject($"SfxSource_{i}");
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.spatialBlend = 1f;
            src.playOnAwake = false;
            src.rolloffMode = rolloffMode;
            src.minDistance = minDistance;
            src.maxDistance = maxDistance;
            src.dopplerLevel = 0f;
            pool3D[i] = src;
        }
    }

    private bool TryReserve(AudioClip clip)
    {
        float now = Time.unscaledTime;

        if (lastPlay.TryGetValue(clip, out float t) && now - t < minInterval)
            return false;

        activeCount.TryGetValue(clip, out int n);
        if (n >= maxConcurrentPerClip) return false;

        lastPlay[clip] = now;
        activeCount[clip] = n + 1;
        StartCoroutine(ReleaseAfter(clip, clip.length));
        return true;
    }

    private IEnumerator ReleaseAfter(AudioClip clip, float dur)
    {
        yield return new WaitForSecondsRealtime(dur);
        if (activeCount.TryGetValue(clip, out int n) && n > 0)
            activeCount[clip] = n - 1;
    }

    public void Play2D(AudioClip clip, float volume = 1f)
    {
        if (clip == null || !TryReserve(clip)) return;
        source2D.PlayOneShot(clip, volume * masterVolume);
    }

    public void Play3D(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null || !TryReserve(clip)) return;
        var src = pool3D[poolIndex];
        poolIndex = (poolIndex + 1) % pool3D.Length;
        src.transform.position = position;
        src.PlayOneShot(clip, volume * masterVolume);
    }
}