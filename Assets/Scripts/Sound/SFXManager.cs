using UnityEngine;
using UnityEngine.VFX;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] private AudioSource source2D;  // UI나 위치 무관 사운드
    [SerializeField] private int poolSize = 16;
    private AudioSource[] pool3D;
    private int poolIndex = 0;

    private void Awake()
    {
        Instance = this;
        pool3D = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            var go = new GameObject($"SfxSource_{i}");
            go.transform.SetParent(transform);
            var src = go.AddComponent<AudioSource>();
            src.spatialBlend = 1f;
            src.playOnAwake = false;
            pool3D[i] = src;
        }
    }

    public void Play2D(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        source2D.PlayOneShot(clip, volume);
    }

    public void Play3D(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        var src = pool3D[poolIndex];
        poolIndex = (poolIndex + 1) % pool3D.Length;

        src.transform.position = position;
        src.PlayOneShot(clip, volume);
    }
}
