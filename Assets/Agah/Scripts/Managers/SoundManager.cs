using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> musicList;
    public AudioSource musicSource;

    private void ChangeMusic(AudioClip music)
    {
        musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
    }
}
/* Must react to color changes so it needs event references.
 * Must react to ADA 5 appearing so it needs its event reference.
 * Must react to diffrent conditions to change music accordingly so it needs the conditions:
 * -Color unlock counter.
 * -End of the game event reference
 * -
 */