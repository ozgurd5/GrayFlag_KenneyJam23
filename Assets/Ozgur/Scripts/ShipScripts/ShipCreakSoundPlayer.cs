using System.Collections;
using UnityEngine;

public class ShipCreakSoundPlayer : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource creakSource;

    private void Awake()
    {
        StartCoroutine(PlayIdleSound());
    }
    
    private IEnumerator PlayIdleSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(40);
            creakSource.Play();
        }
    }
}