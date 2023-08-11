using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource soundPlayer;

    private void Awake()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    public void PlayThisSoundEffect()
    {
        soundPlayer.Play();
    }
}
