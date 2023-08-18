using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerSwordController : WeaponAnimationManagerBase
{
    [Header("Assign")]
    [SerializeField] private float attackAnimationHalfDuration = 0.1f;
    [SerializeField] private float walkingAnimationHalfDuration = 0.5f;
    [SerializeField] private float runningAnimationHalfDuration = 0.2f;
    [SerializeField] private AudioSource attackSource;
    [SerializeField] private ParticleSystem attackParticle;

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

    [Header("Assign - Hiding")]
    [SerializeField] private float hidingTime = 0.3f;
    [SerializeField] private float hiddenPositionY = -1.1f;
    [SerializeField] private float hiddenPositionYBack = -0.4f;
    [SerializeField] private AudioSource hidingSource;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Transform sword;

    [Header("Info - No Touch")]
    [SerializeField] private bool isHidden;
    [SerializeField] private bool isHidingAnimationPlaying;
    [SerializeField] private bool isRunningModeActive;
    [SerializeField] private bool isMovingAnimationPlaying;
    [SerializeField] private float movingAnimationHalfDuration;
    [SerializeField] private float attackRotationXBack;
    [SerializeField] private float movingAnimationPositionZ;
    [SerializeField] private float movingAnimationPositionZBack;

    private IEnumerator playMovingAnimation;
    private Tweener movingTweenY;
    private Tweener movingTweenZ;
    private Tweener runningModeRotationTween;
    
    private IEnumerator playHideWeaponAnimation;
    private IEnumerator playExposeWeaponAnimation;
    private Tweener hideTween;
    
    private bool previousIsDialogueOpen;
    private bool previousIsSwimming;
    private bool didExitSwimming;
    private bool didExitDialogue;
    
    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        pim = PlayerInputManager.Singleton;
        sword = GameObject.Find("PlayerCamera/Sword").transform;

        playMovingAnimation = PlayMovingAnimation();
        playHideWeaponAnimation = PlayHideWeaponAnimation();
        playExposeWeaponAnimation = PlayExposeWeaponAnimation();
        
        //Walking values are the default values
        DisableRunningMode();
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState
            or PlayerStateData.PlayerMainState.DialogueState)) return;
        
        HandleHiddenStatus();
        CheckDialogueAndSwimmingConditions();
        if (isHidden || isHidingAnimationPlaying) return;
        
        DecideForMovingAnimationHalfDuration();
        
        if (psd.isMoving && !isMovingAnimationPlaying)
        {
            playMovingAnimation = PlayMovingAnimation();
            StartCoroutine(playMovingAnimation);
        }
        else if (psd.isIdle && isMovingAnimationPlaying) StopMovingAnimation();
        
        if (psd.isRunning && !isRunningModeActive) EnableRunningMode();
        else if (!psd.isRunning && isRunningModeActive) DisableRunningMode();

        HandleAttack();
    }
    
    private void HandleAttack()
    {
        if (!pim.isAttackKeyDown) return;
        StartCoroutine(PlayAttackAnimation(sword, attackRotationX, attackRotationXBack, 0f, attackAnimationHalfDuration));
        attackSource.Play();

        if (CrosshairManager.isLookingAtEnemy)
        {
            CrosshairManager.crosshairHit.collider.GetComponent<EnemyManager>().GetHit(transform.forward);
            attackParticle.Play();
        }
    }

    private void DecideForMovingAnimationHalfDuration()
    {
        if (psd.isWalking) movingAnimationHalfDuration = walkingAnimationHalfDuration;
        else if (psd.isRunning) movingAnimationHalfDuration = runningAnimationHalfDuration;
    }
    
    private IEnumerator PlayMovingAnimation()
    {
        isMovingAnimationPlaying = true;
        
        movingTweenZ = sword.transform.DOLocalMoveZ(movingAnimationPositionZ, movingAnimationHalfDuration);
        movingTweenY = sword.transform.DOLocalMoveY(movingAnimationPositionY, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        movingTweenZ = sword.transform.DOLocalMoveY(movingAnimationPositionYBack, movingAnimationHalfDuration);
        movingTweenY = sword.transform.DOLocalMoveZ(movingAnimationPositionZBack, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        isMovingAnimationPlaying = false;
    }
    
    private void EnableRunningMode()
    {
        attackRotationXBack = runningModeRotationX;
        movingAnimationPositionZ = runningModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = runningModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();

        runningModeRotationTween.Kill();
        runningModeRotationTween = sword.transform.DOLocalRotate(new Vector3(runningModeRotationX, 0f, 0f), 0.1f);
        
        isRunningModeActive = true;
    }

    private void DisableRunningMode()
    {
        attackRotationXBack = walkingModeRotationX;
        movingAnimationPositionZ = walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = walkingModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();
        
        runningModeRotationTween.Kill();
        runningModeRotationTween = sword.transform.DOLocalRotate(new Vector3(walkingModeRotationX, 0f, 0f), 0.1f);

        isRunningModeActive = false;
    }

    private void StopMovingAnimation()
    {
        StopCoroutine(playMovingAnimation);
        isMovingAnimationPlaying = false;
        
        movingTweenZ.Kill();
        movingTweenY.Kill();

        if (psd.isIdle) sword.transform.DOLocalMoveY(movingAnimationPositionYBack, 0.1f);
        sword.transform.DOLocalMoveZ(movingAnimationPositionZBack, 0.1f);
    }

    private void HandleHiddenStatus()
    {
        if (!isHidden && (pim.isWeaponHideKeyDown || psd.isSwimming || DialogueController.isOpen ))
        {
            isHidden = true;
            
            playHideWeaponAnimation = PlayHideWeaponAnimation();
            StartCoroutine(playHideWeaponAnimation);
        }

        else if (didExitSwimming || didExitDialogue || (isHidden && pim.isWeaponHideKeyDown && !psd.isSwimming && !DialogueController.isOpen))
        {
            isHidden = false;
            
            playExposeWeaponAnimation = PlayExposeWeaponAnimation();
            StartCoroutine(playExposeWeaponAnimation);
        }
    }

    private IEnumerator PlayHideWeaponAnimation()
    {
        StopMovingAnimation();
        StopCoroutine(playExposeWeaponAnimation);
        hideTween.Kill();
        hidingSource.Play();
        
        isHidingAnimationPlaying = true;
        
        hideTween = sword.transform.DOLocalMoveY(hiddenPositionY, hidingTime);
        yield return new WaitForSeconds(hidingTime);

        isHidingAnimationPlaying = false;
    }

    private IEnumerator PlayExposeWeaponAnimation()
    {
        if (psd.isSwimming) yield break;
        
        StopCoroutine(playHideWeaponAnimation);
        hideTween.Kill();
        hidingSource.Play();
        
        isHidingAnimationPlaying = true;
        
        hideTween = sword.transform.DOLocalMoveY(hiddenPositionYBack, hidingTime);
        yield return new WaitForSeconds(hidingTime);

        isHidingAnimationPlaying = false;
    }
    
    private void CheckDialogueAndSwimmingConditions()
    {
        didExitSwimming = previousIsSwimming && !psd.isSwimming;
        didExitDialogue = previousIsDialogueOpen && !DialogueController.isOpen;

        previousIsSwimming = psd.isSwimming;
        previousIsDialogueOpen = DialogueController.isOpen;
    }
}