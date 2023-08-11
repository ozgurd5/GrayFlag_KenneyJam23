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
        psd = PlayerStateData.Singleton;
        PlayerController.OnJump += PlayJumpSound;
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.HookState or PlayerStateData.PlayerMainState.NormalState)) return;
        
        if (psd.isMoving && psd.isGrounded) PlaySound();
    }

    private void PlaySound()
    {
        if (walkingSource.isPlaying || runningSource.isPlaying) return;
        
        if (psd.isWalking) walkingSource.Play();
        else if (psd.isRunning) runningSource.Play();
    }

    private void PlayJumpSound()
    {
        jumpingSource.Play();
    }

    private void OnDestroy()
    {
        PlayerController.OnJump -= PlayJumpSound;
    }
}
