using System.Collections;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    private void Start()
    {
        StartCoroutine(PlayPlaylist());
    }

    private IEnumerator PlayPlaylist()
    {
        int index = 0;
        while (true)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
            yield return new WaitForSeconds(audioClips[index].length);
            index = (index + 1) % audioClips.Length;
        }
    }
}
