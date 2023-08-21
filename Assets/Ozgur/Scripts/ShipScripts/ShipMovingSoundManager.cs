using System;
using System.Collections;
using UnityEngine;

public class ShipMovingSoundManager : MonoBehaviour
{
    [Header("Assign - Audio")]
    [SerializeField] private AudioSource movingSource;
    [SerializeField] private AudioSource rotatingSource;

    [Header("Assign")]
    [SerializeField] private float fadeTime = 0.5f;

    private float movingSourceVolume;
    private float rotatingSourceVolume;

    private IEnumerator movingCoroutine;
    private IEnumerator rotatingCoroutine;
    
    private void Awake()
    {
        ShipController.OnMovementStarted += PlayMovingSound;
        ShipController.OnMovementEnded += StopMovingSound;
        ShipController.OnRotationStarted += PlayRotationSound;
        ShipController.OnRotationEnded += StopRotationSound;

        movingSourceVolume = movingSource.volume;
        rotatingSourceVolume = rotatingSource.volume;

        movingSource.volume = 0f;
        rotatingSource.volume = 0f;
        
        movingCoroutine = Fade(movingSource, movingSourceVolume, true);
        rotatingCoroutine = Fade(rotatingSource, rotatingSourceVolume, true);
    }

    private void PlayMovingSound()
    {
        StopCoroutine(movingCoroutine);
        movingCoroutine = Fade(movingSource, movingSourceVolume, true);
        StartCoroutine(movingCoroutine);
    }

    private void StopMovingSound()
    {
        StopCoroutine(movingCoroutine);
        movingCoroutine = Fade(movingSource, movingSourceVolume, false);
        StartCoroutine(movingCoroutine);
    }

    private void PlayRotationSound()
    {
        StopCoroutine(rotatingCoroutine);
        rotatingCoroutine = Fade(rotatingSource, rotatingSourceVolume, true);
        StartCoroutine(rotatingCoroutine);
    }
    
    private void StopRotationSound()
    {
        StopCoroutine(rotatingCoroutine);
        rotatingCoroutine = Fade(rotatingSource, rotatingSourceVolume, false);
        StartCoroutine(rotatingCoroutine);
    }
    
    private IEnumerator Fade(AudioSource aus, float ausVolume, bool isFadeIn)
    {
        if (isFadeIn) aus.Play();
        
        float timePassed = 0f;
        float speed = ausVolume / fadeTime;

        while (timePassed <= fadeTime)
        {
            if (isFadeIn) aus.volume += speed * Time.deltaTime;
            else aus.volume -= speed * Time.deltaTime;
            
            if (isFadeIn && (aus.volume >= ausVolume)) break;
            
            timePassed += Time.deltaTime;
            yield return null;
        }

        if (isFadeIn)
        {
            aus.volume = ausVolume;
        }

        else
        {
            aus.volume = 0f;
            aus.Stop();
        }
    }

    private void OnDestroy()
    {
        ShipController.OnMovementStarted -= PlayMovingSound;
        ShipController.OnMovementEnded -= StopMovingSound;
        ShipController.OnRotationStarted -= PlayRotationSound;
        ShipController.OnRotationEnded -= StopRotationSound;
    }
}
