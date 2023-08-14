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
        psd = PlayerStateData.Singleton;
        rb = GetComponent<Rigidbody>();

        GroundCheck.OnSwimmingEnter += PlaySplashSound;
    }

    private void Update()
    {
        if (psd.isMoving && psd.isSwimming) PlaySwimmingSound();
    }

    private void PlaySplashSound()
    {
        if (rb.velocity.y < splashVelocityLimit)
        {
            splashSource.Play();
            splashCondition = false;
        }
    }

    private void PlaySwimmingSound()
    {
        if (normalSwimSource.isPlaying || fastSwimSource.isPlaying) return;
        
        if (psd.isWalking) normalSwimSource.Play();
        else if (psd.isRunning) fastSwimSource.Play();
    }
}
