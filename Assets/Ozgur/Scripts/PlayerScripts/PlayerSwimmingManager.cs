using System;
using UnityEngine;

public class PlayerSwimmingManager : MonoBehaviour
{
    public static event Action OnSwimmingEnter;
    public static event Action OnSwimmingExit;
    
    [Header("Assign")]
    [SerializeField] private AudioSource splashSource;
    [SerializeField] private AudioSource normalSwimSource;
    [SerializeField] private AudioSource fastSwimSource;
    [SerializeField] private  float splashVelocityLimit = -20;

    private PlayerStateData psd;
    private Rigidbody rb;
    
    private bool previousIsSwimming;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        rb = GetComponent<Rigidbody>();

        OnSwimmingEnter += PlaySplashSound;
    }

    private void Update()
    {
        if (psd.isMoving && psd.isSwimming) PlaySwimmingSound();
        
        CheckEnterExitSwimming();
    }

    private void CheckEnterExitSwimming()
    {
        if (previousIsSwimming && !psd.isSwimming) OnSwimmingExit?.Invoke();
        else if (!previousIsSwimming && psd.isSwimming) OnSwimmingEnter?.Invoke();

        previousIsSwimming = psd.isSwimming;
    }

    private void PlaySplashSound()
    {
        if (rb.velocity.y < splashVelocityLimit) splashSource.Play();
    }

    private void PlaySwimmingSound()
    {
        if (normalSwimSource.isPlaying || fastSwimSource.isPlaying) return;
        
        if (psd.isWalking) normalSwimSource.Play();
        else if (psd.isRunning) fastSwimSource.Play();
    }

    private void OnDestroy()
    {
        OnSwimmingEnter -= PlaySplashSound;
    }
}
