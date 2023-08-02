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
    [SerializeField] private float runningModeRotationX = 25f;
    [SerializeField] private float walkingModeRotationX = 0f;
    [SerializeField] private float movingAnimationPositionY = -0.35f;
    [SerializeField] private float movingAnimationPositionYBack = -0.4f;
    [SerializeField] private float walkingModeMovingAnimationPositionZ = 0.62f;
    [SerializeField] private float walkingModeMovingAnimationPositionZBack = 0.6f;
    [SerializeField] private float runningModeMovingAnimationPositionZ = 0.52f;
    [SerializeField] private float runningModeMovingAnimationPositionZBack = 0.5f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private GameObject sword;

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
        sword = GameObject.Find("PlayerCamera/Sword");

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
        
        swordMovingTweenZ = sword.transform.DOLocalMoveZ(movingAnimationPositionZ, movingAnimationHalfDuration);
        swordMovingTweenY = sword.transform.DOLocalMoveY(movingAnimationPositionY, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        swordMovingTweenZ = sword.transform.DOLocalMoveY(movingAnimationPositionYBack, movingAnimationHalfDuration);
        swordMovingTweenY = sword.transform.DOLocalMoveZ(movingAnimationPositionZBack, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        isMovingAnimationPlaying = false;
    }

    private void StopMovingAnimation()
    {
        StopCoroutine(playMovingAnimation);
        isMovingAnimationPlaying = false;
        
        swordMovingTweenZ.Kill();
        swordMovingTweenY.Kill();

        if (psd.isIdle) sword.transform.DOLocalMoveY(movingAnimationPositionYBack, 0.1f);
        sword.transform.DOLocalMoveZ(movingAnimationPositionZBack, 0.1f);
    }
    
    private void EnableRunningMode()
    {
        attackRotationXBack = runningModeRotationX;
        movingAnimationPositionZ = runningModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = runningModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();

        swordRunningModeRotationTween.Kill();
        swordRunningModeRotationTween = sword.transform.DOLocalRotate(new Vector3(runningModeRotationX, 0f, 0f), 0.1f);
        
        isRunningModeActive = true;
    }

    private void DisableRunningMode()
    {
        attackRotationXBack = walkingModeRotationX;
        movingAnimationPositionZ = walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = walkingModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();
        
        swordRunningModeRotationTween.Kill();
        swordRunningModeRotationTween = sword.transform.DOLocalRotate(new Vector3(walkingModeRotationX, 0f, 0f), 0.1f);

        isRunningModeActive = false;
    }
}
