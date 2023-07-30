using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float attackAnimationHalfDuration = 0.1f;
    [SerializeField] private float walkingAnimationHalfDuration = 0.5f;
    [SerializeField] private float runningAnimationHalfDuration = 0.2f;
    [SerializeField] private AudioSource aus;

    [Header("Assign")]
    [SerializeField] private float attackRotationX = 50f;
    [SerializeField] private float attackRotationXBack = 0f;
    [SerializeField] private float movingAnimationPositionY = -0.35f;
    [SerializeField] private float movingAnimationPositionYBack = -0.4f;
    [SerializeField] private float walkingModeMovingAnimationPositionZ = 0.62f;
    [SerializeField] private float walkingModeMovingAnimationPositionZBack = 0.6f;
    [SerializeField] private float runningModeMovingAnimationPositionZ = 0.52f;
    [SerializeField] private float runningModeMovingAnimationPositionZBack = 0.5f;
    [SerializeField] private float runningModeRotationX = 25f;
    [SerializeField] private float runningModeRotationXBack = 0f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private GameObject sword;

    private bool isRunningModeActive;
    private bool isMovingAnimationPlaying;
    private float movingAnimationHalfDuration;
    private float movingAnimationPositionZ;
    private float movingAnimationPositionZBack;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        sword = GameObject.Find("PlayerCamera/Sword");
        
        //Walking values are the default values
        DisableRunningMode();
    }

    private void Update()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;

        DecideForMovingAnimationHalfDuration();
        if (psd.isMoving && !isMovingAnimationPlaying) StartCoroutine(PlayMovingAnimation());
        else if (psd.isIdle) StopMovingAnimation();
        
        if (psd.isRunning && !isRunningModeActive) EnableRunningMode();
        else if (!psd.isRunning && isRunningModeActive) DisableRunningMode();

        if (!pim.isAttackKeyDown) return;
        StartCoroutine(PlayAttackAnimation());
        aus.Play();
        
        if (CrosshairManager.isLookingAtEnemy)
            CrosshairManager.crosshairHit.collider.GetComponent<EnemyManager>().GetHit(transform.forward);
    }

    private void DecideForMovingAnimationHalfDuration()
    {
        if (psd.isWalking) movingAnimationHalfDuration = walkingAnimationHalfDuration;
        else if (psd.isRunning) movingAnimationHalfDuration = runningAnimationHalfDuration;
    }

    private IEnumerator PlayAttackAnimation()
    {
        sword.transform.DOLocalRotate(new Vector3(attackRotationX, 0f, 0f), attackAnimationHalfDuration);
        yield return new WaitForSeconds(attackAnimationHalfDuration);
        sword.transform.DOLocalRotate(new Vector3(attackRotationXBack, 0f, 0f), attackAnimationHalfDuration);
    }
    
    private IEnumerator PlayMovingAnimation()
    {
        isMovingAnimationPlaying = true;
        
        sword.transform.DOLocalMoveZ(movingAnimationPositionZ, movingAnimationHalfDuration);
        sword.transform.DOLocalMoveY(movingAnimationPositionY, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        sword.transform.DOLocalMoveY(movingAnimationPositionYBack, movingAnimationHalfDuration);
        sword.transform.DOLocalMoveZ(movingAnimationPositionZBack, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);

        isMovingAnimationPlaying = false;
    }

    private void StopMovingAnimation()
    {
        sword.transform.DOLocalMoveY(movingAnimationPositionYBack, 0.1f);
        sword.transform.DOLocalMoveZ(movingAnimationPositionZBack, 0.1f);

        StopCoroutine(PlayMovingAnimation());
        isMovingAnimationPlaying = false;
    }
    
    private void EnableRunningMode()
    {
        movingAnimationPositionZ = runningModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = runningModeMovingAnimationPositionZBack;
        
        sword.transform.DOLocalRotate(new Vector3(runningModeRotationX, 0f, 0f), movingAnimationHalfDuration);
        
        isRunningModeActive = true;
    }

    private void DisableRunningMode()
    {
        movingAnimationPositionZ = walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = walkingModeMovingAnimationPositionZBack;
        
        sword.transform.DOLocalRotate(new Vector3(runningModeRotationXBack, 0f, 0f), movingAnimationHalfDuration);

        isRunningModeActive = false;
    }
}
