using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerHookGunAnimationManager : MonoBehaviour
{
    [Header("Assign")]
    public float attackAnimationHalfDuration = 0.1f;
    [SerializeField] private float walkingAnimationHalfDuration = 0.5f;
    [SerializeField] private float runningAnimationHalfDuration = 0.2f;

    [Header("Assign")]
    [SerializeField] private float attackRotationX = 50f;
    [SerializeField] private float runningModeRotationX = 15f;
    [SerializeField] private float walkingModeRotationX = 0f;
    [SerializeField] private float movingAnimationPositionY = -0.15f;
    [SerializeField] private float movingAnimationPositionYBack = -0.2f;
    [SerializeField] private float walkingModeMovingAnimationPositionZ = 0.62f;
    [SerializeField] private float walkingModeMovingAnimationPositionZBack = 0.6f;
    [SerializeField] private float runningModeMovingAnimationPositionZ = 0.52f;
    [SerializeField] private float runningModeMovingAnimationPositionZBack = 0.5f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private GameObject hookGun;

    [Header("Info - No Touch")]
    [SerializeField] private bool isRunningModeActive;
    [SerializeField] private bool isMovingAnimationPlaying;
    [SerializeField] private float movingAnimationHalfDuration;
    [SerializeField] private float attackRotationXBack;
    [SerializeField] private float movingAnimationPositionZ;
    [SerializeField] private float movingAnimationPositionZBack;

    private IEnumerator playMovingAnimation;
    private Tweener swordMovingTweenY;
    private Tweener swordMovingTweenZ;
    private Tweener swordRunningModeRotationTween;
    
    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        hookGun = GameObject.Find("PlayerCamera/HookGun");

        playMovingAnimation = PlayMovingAnimation();
        
        //Walking values are the default values
        DisableRunningMode();
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;

        DecideForMovingAnimationHalfDuration();
        
        if (psd.isMoving && !isMovingAnimationPlaying)
        {
            playMovingAnimation = PlayMovingAnimation();
            StartCoroutine(playMovingAnimation);
        }
        else if (psd.isIdle && isMovingAnimationPlaying) StopMovingAnimation();
        
        if (psd.isRunning && !isRunningModeActive) EnableRunningMode();
        else if (!psd.isRunning && isRunningModeActive) DisableRunningMode();

        if (!pim.isHookKeyDown) return;
        StartCoroutine(PlayAttackAnimation());

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
        hookGun.transform.DOLocalRotate(new Vector3(attackRotationX, -170f, 0f), attackAnimationHalfDuration);
        yield return new WaitForSeconds(attackAnimationHalfDuration);
        hookGun.transform.DOLocalRotate(new Vector3(attackRotationXBack, -170f, 0f), attackAnimationHalfDuration);
    }
    
    private IEnumerator PlayMovingAnimation()
    {
        isMovingAnimationPlaying = true;
        
        swordMovingTweenZ = hookGun.transform.DOLocalMoveZ(movingAnimationPositionZ, movingAnimationHalfDuration);
        swordMovingTweenY = hookGun.transform.DOLocalMoveY(movingAnimationPositionY, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        swordMovingTweenZ = hookGun.transform.DOLocalMoveY(movingAnimationPositionYBack, movingAnimationHalfDuration);
        swordMovingTweenY = hookGun.transform.DOLocalMoveZ(movingAnimationPositionZBack, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        isMovingAnimationPlaying = false;
    }

    private void StopMovingAnimation()
    {
        StopCoroutine(playMovingAnimation);
        isMovingAnimationPlaying = false;
        
        swordMovingTweenZ.Kill();
        swordMovingTweenY.Kill();

        if (psd.isIdle) hookGun.transform.DOLocalMoveY(movingAnimationPositionYBack, 0.1f);
        hookGun.transform.DOLocalMoveZ(movingAnimationPositionZBack, 0.1f);
    }
    
    private void EnableRunningMode()
    {
        attackRotationXBack = runningModeRotationX;
        movingAnimationPositionZ = runningModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = runningModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();

        swordRunningModeRotationTween.Kill();
        swordRunningModeRotationTween = hookGun.transform.DOLocalRotate(new Vector3(runningModeRotationX, -170f, 0f), 0.1f);
        
        isRunningModeActive = true;
    }

    private void DisableRunningMode()
    {
        attackRotationXBack = walkingModeRotationX;
        movingAnimationPositionZ = walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = walkingModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();
        
        swordRunningModeRotationTween.Kill();
        swordRunningModeRotationTween = hookGun.transform.DOLocalRotate(new Vector3(walkingModeRotationX, -170f, 0f), 0.1f);

        isRunningModeActive = false;
    }
}
