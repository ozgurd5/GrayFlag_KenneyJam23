using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAnimationManagerBase : MonoBehaviour
{
    [Header("Assign")] public WeaponAnimationValue wav; //PlayerHookController.cs needs access

    protected Transform weaponTransform;
    protected Slider cooldownSlider;

    //Moving
    private IEnumerator movingAnimation;
    private Tweener movingTweenY;
    private Tweener movingTweenZ;
    private bool isMovingAnimationPlaying;
    private float movingAnimationHalfDuration;
    private float movingAnimationPositionZ;
    private float movingAnimationPositionZBack;

    //Running
    private Tweener runningModeRotationTween;
    private bool isRunningModeActive;

    //Hiding
    protected AudioSource baseHidingSource;
    private IEnumerator hideWeaponAnimation;
    private IEnumerator exposeWeaponAnimation;
    private Tweener hideTween;
    [Header("Info - No Touch")] public bool isHidden; //PlayerHookController.cs needs access
    private bool isHidingAnimationPlaying;

    //Attacking
    private Tweener attackAnimationTween;
    public bool isAttackAnimationPlaying; //PlayerHookController.cs needs access
    private bool isAttackAnimationReturning; //Returning means weapon getting back to attackRotationXBack. Detailed explanation is in PlayAttackAnimation()
    private float attackRotationXBack;

    /// <summary>
    /// Call this method in the Awake method of the child scripts
    /// </summary>
    protected void OnAwake()
    {
        movingAnimation = PlayMovingAnimation();
        hideWeaponAnimation = PlayHideWeaponAnimation();
        exposeWeaponAnimation = PlayExposeWeaponAnimation();

        //Walking values are the default values
        attackRotationXBack = wav.walkingModeRotationX;
        movingAnimationPositionZ = wav.walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = wav.walkingModeMovingAnimationPositionZBack;
    }

    /// <summary>
    /// Call this method in the Update method of the child scripts
    /// </summary>
    protected void OnUpdate()
    {
        if (PlayerStateData.Singleton.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;

        HandleHiddenStatus();
        CheckDialogueAndSwimmingConditions();

        if (isHidden || isHidingAnimationPlaying) return;

        DecideForMovingAnimationHalfDuration();

        if (PlayerStateData.Singleton.isMoving && !isMovingAnimationPlaying)
        {
            movingAnimation = PlayMovingAnimation();
            StartCoroutine(movingAnimation);
        }
        else if (PlayerStateData.Singleton.isIdle && isMovingAnimationPlaying) StopMovingAnimation();

        if (PlayerStateData.Singleton.isRunning && !isRunningModeActive) EnableRunningMode();
        else if (!PlayerStateData.Singleton.isRunning && isRunningModeActive) DisableRunningMode();
    }

    //TODO: IMPLEMENT SWIMMING EVENT SYSTEM
    #region DialogueAndSwimming
    private bool previousIsDialogueOpen;
    private bool previousIsSwimming;
    private bool didExitSwimming;
    private bool didExitDialogue;

    private void CheckDialogueAndSwimmingConditions()
    {
        didExitSwimming = previousIsSwimming && !PlayerStateData.Singleton.isSwimming;
        didExitDialogue = previousIsDialogueOpen && !DialogueController.isOpen;

        previousIsSwimming = PlayerStateData.Singleton.isSwimming;
        previousIsDialogueOpen = DialogueController.isOpen;
    }
    #endregion

    #region Moving

    private void DecideForMovingAnimationHalfDuration()
    {
        if (PlayerStateData.Singleton.isWalking) movingAnimationHalfDuration = wav.walkingAnimationHalfDuration;
        else if (PlayerStateData.Singleton.isRunning) movingAnimationHalfDuration = wav.runningAnimationHalfDuration;
    }

    private IEnumerator PlayMovingAnimation()
    {
        isMovingAnimationPlaying = true;

        movingTweenY = weaponTransform.DOLocalMoveY(wav.movingAnimationPositionY, movingAnimationHalfDuration);
        movingTweenZ = weaponTransform.DOLocalMoveZ(movingAnimationPositionZ, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);

        movingTweenY = weaponTransform.DOLocalMoveY(wav.movingAnimationPositionYBack, movingAnimationHalfDuration);
        movingTweenZ = weaponTransform.DOLocalMoveZ(movingAnimationPositionZBack, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);

        isMovingAnimationPlaying = false;
    }

    private void StopMovingAnimation()
    {
        StopCoroutine(movingAnimation);
        isMovingAnimationPlaying = false;

        movingTweenZ.Kill();
        movingTweenY.Kill();

        //If character stops, weapon must be in it's default position
        if (PlayerStateData.Singleton.isIdle)
        {
            weaponTransform.DOLocalMoveY(wav.movingAnimationPositionYBack, 0.1f);
            weaponTransform.DOLocalMoveZ(movingAnimationPositionZBack, 0.1f);
        }
    }

    #endregion

    #region Running

    private void EnableRunningMode()
    {
        isRunningModeActive = true;

        attackRotationXBack = wav.runningModeRotationX;
        movingAnimationPositionZ = wav.runningModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = wav.runningModeMovingAnimationPositionZBack;

        StopMovingAnimation();
        runningModeRotationTween.Kill();

        //Both attack animation and running mode changing animation controls X rotation. Their conflict causes strange things and we must prevent it.
        //Attack animation has higher priority, so when it's playing, this must not.
        if (!isAttackAnimationPlaying)
            runningModeRotationTween = weaponTransform.DOLocalRotate(new Vector3(wav.runningModeRotationX, wav.defaultRotationY, 0f), wav.runningModeSwitchDuration);

        //Detailed explanation about why we are doing this is down bellow in PlayAttackAnimation()
        else if (isAttackAnimationReturning)
        {
            attackAnimationTween.Kill();
            attackAnimationTween = weaponTransform.DOLocalRotate(new Vector3(attackRotationXBack, wav.defaultRotationY, 0f), wav.attackAnimationHalfDuration);
        }
    }

    private void DisableRunningMode()
    {
        isRunningModeActive = false;

        attackRotationXBack = wav.walkingModeRotationX;
        movingAnimationPositionZ = wav.walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = wav.walkingModeMovingAnimationPositionZBack;

        StopMovingAnimation();
        runningModeRotationTween.Kill();

        //Both attack animation and running mode changing animation controls X rotation. Their conflict causes strange things and we must prevent it.
        //Attack animation has higher priority, so when it's playing, this must not.
        if (!isAttackAnimationPlaying)
            runningModeRotationTween = weaponTransform.DOLocalRotate(new Vector3(wav.walkingModeRotationX, wav.defaultRotationY, 0f), wav.runningModeSwitchDuration);

        //Detailed explanation about why we are doing this is down bellow in PlayAttackAnimation()
        else if (isAttackAnimationReturning)
        {
            attackAnimationTween.Kill();
            attackAnimationTween = weaponTransform.DOLocalRotate(new Vector3(attackRotationXBack, wav.defaultRotationY, 0f), wav.attackAnimationHalfDuration);
        }
    }

    #endregion

    #region Hiding

    private void HandleHiddenStatus()
    {
        if (!isHidden && (PlayerInputManager.Singleton.isWeaponHideKeyDown || PlayerStateData.Singleton.isSwimming || DialogueController.isOpen ))
        {
            isHidden = true;

            hideWeaponAnimation = PlayHideWeaponAnimation();
            StartCoroutine(hideWeaponAnimation);
        }

        else if (didExitSwimming || didExitDialogue || (isHidden && PlayerInputManager.Singleton.isWeaponHideKeyDown && !PlayerStateData.Singleton.isSwimming && !DialogueController.isOpen))
        {
            isHidden = false;

            exposeWeaponAnimation = PlayExposeWeaponAnimation();
            StartCoroutine(exposeWeaponAnimation);
        }
    }

    private IEnumerator PlayHideWeaponAnimation()
    {
        StopMovingAnimation();
        StopCoroutine(exposeWeaponAnimation);
        hideTween.Kill();
        baseHidingSource.Play();

        isHidingAnimationPlaying = true;

        hideTween = weaponTransform.DOLocalMoveY(wav.hiddenPositionY, wav.hidingTime);
        yield return new WaitForSeconds(wav.hidingTime);

        isHidingAnimationPlaying = false;
    }

    private IEnumerator PlayExposeWeaponAnimation()
    {
        if (PlayerStateData.Singleton.isSwimming) yield break;

        StopCoroutine(hideWeaponAnimation);
        hideTween.Kill();
        baseHidingSource.Play();

        isHidingAnimationPlaying = true;

        hideTween = weaponTransform.DOLocalMoveY(wav.hiddenPositionYBack, wav.hidingTime);
        yield return new WaitForSeconds(wav.hidingTime);

        isHidingAnimationPlaying = false;
    }

    #endregion

    #region Attacking

    protected IEnumerator PlayAttackAnimation()
    {
        runningModeRotationTween.Kill();

        isAttackAnimationPlaying = true;

        //cooldownSlider tween is linear because default tween ease is outQuad which is getting slower while it's getting closer to the destination.
        //This is confusing for the player and it's better for slider to move at constant speed, so the player can **feel** when the slider is full..
        //..and when he/she can attack again. OutQuad makes the *feeling* harder because it's getting slower when it's closer.

        attackAnimationTween = weaponTransform.DOLocalRotate(new Vector3(wav.attackRotationX, wav.defaultRotationY, 0f), wav.attackAnimationHalfDuration);
        DOTween.To(() => cooldownSlider.value, value => cooldownSlider.value = value, 0, wav.attackAnimationHalfDuration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(wav.attackAnimationHalfDuration);

        //We need to keep track of returning part of the animation, because when player change it's running/walking state while returning part is playing,..
        //..it goes to the previous state's attackRotationXBack value.

        //For example: While returning part is playing, player is in running mode ant the DOLocalRotate tween down bellow started with runningModeRotationX..
        //..and it x value goes there. But the player switches from running to walking. We are now in walking state and x value must reach walkingModeRotationX..
        //..but it's still going runningModeRotationX

        //So in order to do that we must keep track of when returning animation plays. If it's playing and player switches state, we must stop the active..
        //..tween and create another one with the correct attackRotationXBack value which is either runningModeRotationX or walkingModeRotationX.

        isAttackAnimationReturning = true;
        attackAnimationTween = weaponTransform.DOLocalRotate(new Vector3(attackRotationXBack, wav.defaultRotationY, 0f), wav.attackAnimationHalfDuration);
        DOTween.To(() => cooldownSlider.value, value => cooldownSlider.value = value, 1, wav.attackAnimationHalfDuration).SetEase(Ease.Linear);
        yield return new WaitForSeconds(wav.attackAnimationHalfDuration);
        isAttackAnimationReturning = false;

        isAttackAnimationPlaying = false;
    }

    #endregion
}
