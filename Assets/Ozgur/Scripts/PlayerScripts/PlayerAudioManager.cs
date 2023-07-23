using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip runningSound;
    [SerializeField] private AudioSource jumpingAudioSource;
    [SerializeField] private AudioSource aus;
    
    private PlayerStateData psd;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        PlayerController.OnJump += () => jumpingAudioSource.Play();
    }

    private void Update()
    {
        if (psd.isMoving && psd.isGrounded)
        {
            SelectClip();
            PlaySound();
        }
        
        else
        {
            //Setting loop property to false feels more natural then using aus.Stop();
            aus.loop = false;
        }
    }
    
    private void SelectClip()
    {
        if (psd.isWalking) aus.clip = walkingSound;
        else if (psd.isRunning) aus.clip = runningSound;
    }

    private void PlaySound()
    {
        if (aus.isPlaying) return;
        
        //Making loop property to false feels more natural then using Stop method, but we have to enable it everytime
        aus.loop = true;
        aus.Play();
    }
}
