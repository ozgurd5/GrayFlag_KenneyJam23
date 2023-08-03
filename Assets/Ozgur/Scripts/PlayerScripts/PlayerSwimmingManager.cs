using UnityEngine;

public class PlayerSwimmingManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource normalSwimSource;
    [SerializeField] private AudioSource fastSwimSource;
    
    private PlayerStateData psd;
    
    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
    }

    private void Update()
    {
        if (psd.isMoving && psd.isSwimming) PlaySound();
    }

    private void PlaySound()
    {
        if (normalSwimSource.isPlaying || fastSwimSource.isPlaying) return;
        
        if (psd.isWalking) normalSwimSource.Play();
        else if (psd.isRunning) fastSwimSource.Play();
    }
}
