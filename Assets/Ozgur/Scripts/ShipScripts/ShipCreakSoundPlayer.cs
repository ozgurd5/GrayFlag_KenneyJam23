using System.Collections;
using UnityEngine;

public class ShipCreakSoundPlayer : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource aus;

    private ShipController sc;

    private void Awake()
    {
        StartCoroutine(PlayIdleSound());
    }

    private IEnumerator PlayIdleSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(40);
            aus.Play();
        }
    }
}
