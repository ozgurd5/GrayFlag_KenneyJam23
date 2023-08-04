using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource jumpingSource;
    [SerializeField] private AudioSource walkingSource;
    [SerializeField] private AudioSource runningSource;
    
    private PlayerStateData psd;
    
    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        PlayerController.OnJump += () => jumpingSource.Play();
    }

    private void Update()
    {
        if (psd.isMoving && psd.isGrounded) PlaySound();
    }

    private void PlaySound()
    {
        if (walkingSource.isPlaying || runningSource.isPlaying) return;
        
        if (psd.isWalking) walkingSource.Play();
        else if (psd.isRunning) runningSource.Play();
    }
}
