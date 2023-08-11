using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource soundPlayer;

    public void PlayThisSoundEffect()
    {
        soundPlayer.Play();
    }
}
