using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> musicList;
    //musicList ��yle ayarlanmal�:
    /*0 "Pirate 2" Oyun a��l���nda.
     *1 "Pirate 3" �lk Renk A��l���nda.
     *2 "Pirate 5" �kinci Renk A��l���nda.
     *3 "Pirate 4" Belki Son Ada ��k�nca.
     *4 "Pirate 6" veya "Pirate 8" Outro.
     *5 "Pirate 7" Belki Diyalog M�zi�i. 
     *6 "Earthquake 3 - Big" Ada ��k�� SFX
     */
    [Header("Assign")]
    [SerializeField] AudioSource musicSource; 

    private void Awake()
    {
        AdaPositionManager.OnAda5IsHere += Ada5Unlocked;
    }

    private void Ada5Unlocked()
    {
        musicSource.Stop();
        Debug.Log("adaunlocked");
        StartCoroutine(PlayEarthquakeAndMusic());
    }
    IEnumerator PlayEarthquakeAndMusic()
    {
        Debug.Log("play earthqu");
        yield return new WaitForSeconds(3f);
        ChangeMusic(musicList[6]);
        yield return new WaitForSeconds(10f);
        ChangeMusic(musicList[3]);
    }

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