using UnityEngine;

public class PlayerSwimmingManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource splashSource;
    [SerializeField] private AudioSource normalSwimSource;
    [SerializeField] private AudioSource fastSwimSource;
    [SerializeField] private  float splashVelocityLimit = -30;

    private PlayerStateData psd;
    private Rigidbody rb;
    
    public bool splashCondition;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (splashCondition && psd.isSwimming) PlaySplashSound();
        if (psd.isMoving && psd.isSwimming) PlaySwimmingSound();

        if (!splashCondition) splashCondition = rb.velocity.y < splashVelocityLimit;
    }

    private void PlaySplashSound()
    {
        splashSource.Play();
        splashCondition = false;
    }

    private void PlaySwimmingSound()
    {
        if (normalSwimSource.isPlaying || fastSwimSource.isPlaying) return;
        
        if (psd.isWalking) normalSwimSource.Play();
        else if (psd.isRunning) fastSwimSource.Play();
    }
}
