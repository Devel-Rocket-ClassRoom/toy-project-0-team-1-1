using UnityEngine;
using UnityEngine.Audio;

public class BgmPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private PlayerStatus playerStatus;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    private void Start()
    {
        playerStatus.OnDead += GameOverPlay;
    }
    public void GameOverPlay()
    {
        audioSource.clip = gameOverClip;
        audioSource.loop = false;
        audioSource.Play();
    }
}
