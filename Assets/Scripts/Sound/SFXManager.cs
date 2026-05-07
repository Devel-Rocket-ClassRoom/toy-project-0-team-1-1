using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private int poolSize = 16;
    [SerializeField] private float minInterval = 0.05f;
    [SerializeField] private int maxConcurrentPerClip = 3;
    [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;

    [Header("3D Rolloff")]
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;

    private AudioSource source2D;
    private AudioSource[] pool3D;
    private int poolIndex = 0;

    private readonly Dictionary<AudioClip, float> lastPlay = new();

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

    public void Play2D(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        float now = Time.unscaledTime;
        if (lastPlay.TryGetValue(clip, out float t) && now - t < minInterval) return;
        lastPlay[clip] = now;

        source2D.PlayOneShot(clip, volume * masterVolume);
    }

    public void Play3D(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;

        float now = Time.unscaledTime;
        if (lastPlay.TryGetValue(clip, out float t) && now - t < minInterval) return;
        lastPlay[clip] = now;

        AudioSource src = PickSource(clip);
        src.Stop();                 // 점유 중이던 사운드 즉시 컷 (= voice stealing)
        src.clip = clip;
        src.volume = volume * masterVolume;
        src.transform.position = position;
        src.Play();
    }

    private AudioSource PickSource(AudioClip clip)
    {
        // 1. 비어있는 source 우선
        for (int i = 0; i < pool3D.Length; i++)
        {
            int idx = (poolIndex + i) % pool3D.Length;
            if (!pool3D[idx].isPlaying)
            {
                poolIndex = (idx + 1) % pool3D.Length;
                return pool3D[idx];
            }
        }

        // 2. 같은 클립이 한계 이상 재생 중이면, 그 중 첫 번째를 stealing
        int sameCount = 0;
        int stealIdx = -1;
        for (int i = 0; i < pool3D.Length; i++)
        {
            int idx = (poolIndex + i) % pool3D.Length;
            if (pool3D[idx].clip == clip && pool3D[idx].isPlaying)
            {
                sameCount++;
                if (stealIdx == -1) stealIdx = idx;
            }
        }
        if (sameCount >= maxConcurrentPerClip && stealIdx >= 0)
        {
            poolIndex = (stealIdx + 1) % pool3D.Length;
            return pool3D[stealIdx];
        }

        // 3. round-robin (다른 클립을 훔침)
        var ret = pool3D[poolIndex];
        poolIndex = (poolIndex + 1) % pool3D.Length;
        return ret;
    }
}