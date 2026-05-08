using UnityEngine;
using UnityEngine.Audio;

public class BgmPlayerStart : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
