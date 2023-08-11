using System;
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
    
    [Header("Assign - Hiding")]
    [SerializeField] private float hidingTime = 0.3f;
    [SerializeField] private float hiddenPositionY = -0.6f;
    [SerializeField] private float hiddenPositionYBack = -0.2f;
    [SerializeField] private AudioSource hidingSource;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private GameObject hookGun;

    [Header("Info - No Touch")]
    public bool isHidden;
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
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        hookGun = GameObject.Find("PlayerCamera/HookGun");

        playMovingAnimation = PlayMovingAnimation();
        playHideWeaponAnimation = PlayHideWeaponAnimation();
        playExposeWeaponAnimation = PlayExposeWeaponAnimation();
        
        //Walking values are the default values
        DisableRunningMode();
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        
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

    private void DecideForMovingAnimationHalfDuration()
    {
        if (psd.isWalking) movingAnimationHalfDuration = walkingAnimationHalfDuration;
        else if (psd.isRunning) movingAnimationHalfDuration = runningAnimationHalfDuration;
    }

    private IEnumerator PlayMovingAnimation()
    {
        isMovingAnimationPlaying = true;
        
        movingTweenZ = hookGun.transform.DOLocalMoveZ(movingAnimationPositionZ, movingAnimationHalfDuration);
        movingTweenY = hookGun.transform.DOLocalMoveY(movingAnimationPositionY, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        movingTweenZ = hookGun.transform.DOLocalMoveY(movingAnimationPositionYBack, movingAnimationHalfDuration);
        movingTweenY = hookGun.transform.DOLocalMoveZ(movingAnimationPositionZBack, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);
        
        isMovingAnimationPlaying = false;
    }

    private void StopMovingAnimation()
    {
        StopCoroutine(playMovingAnimation);
        isMovingAnimationPlaying = false;
        
        movingTweenZ.Kill();
        movingTweenY.Kill();

        if (psd.isIdle) hookGun.transform.DOLocalMoveY(movingAnimationPositionYBack, 0.1f);
        hookGun.transform.DOLocalMoveZ(movingAnimationPositionZBack, 0.1f);
    }
    
    private void EnableRunningMode()
    {
        attackRotationXBack = runningModeRotationX;
        movingAnimationPositionZ = runningModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = runningModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();

        runningModeRotationTween.Kill();
        runningModeRotationTween = hookGun.transform.DOLocalRotate(new Vector3(runningModeRotationX, -170f, 0f), 0.1f);
        
        isRunningModeActive = true;
    }

    private void DisableRunningMode()
    {
        attackRotationXBack = walkingModeRotationX;
        movingAnimationPositionZ = walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = walkingModeMovingAnimationPositionZBack;
        
        StopMovingAnimation();
        
        runningModeRotationTween.Kill();
        runningModeRotationTween = hookGun.transform.DOLocalRotate(new Vector3(walkingModeRotationX, -170f, 0f), 0.1f);

        isRunningModeActive = false;
    }

    private void HandleAttack()
    {
        if (!pim.isHookKeyDown) return;
        StartCoroutine(PlayAttackAnimation());

        if (CrosshairManager.isLookingAtEnemy)
            CrosshairManager.crosshairHit.collider.GetComponent<EnemyManager>().GetHit(transform.forward);
    }
    
    private IEnumerator PlayAttackAnimation()
    {
        hookGun.transform.DOLocalRotate(new Vector3(attackRotationX, -170f, 0f), attackAnimationHalfDuration);
        yield return new WaitForSeconds(attackAnimationHalfDuration);
        hookGun.transform.DOLocalRotate(new Vector3(attackRotationXBack, -170f, 0f), attackAnimationHalfDuration);
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
        
        hideTween = hookGun.transform.DOLocalMoveY(hiddenPositionY, hidingTime);
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
        
        hideTween = hookGun.transform.DOLocalMoveY(hiddenPositionYBack, hidingTime);
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
