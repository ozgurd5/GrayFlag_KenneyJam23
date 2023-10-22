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
    public bool isHidden; //PlayerHookController.cs needs access
    private bool isHidingAnimationPlaying;

    //Attacking
    private Tweener attackRotationTween;
    protected bool isAttackAnimationPlaying;
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
        DisableRunningMode();
    }

    /// <summary>
    /// Call this method in the Update method of the child scripts
    /// </summary>
    protected void OnUpdate()
    {
        if (PlayerStateData.Singleton.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState
            or PlayerStateData.PlayerMainState.DialogueState)) return;

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

        movingTweenZ = weaponTransform.DOLocalMoveZ(movingAnimationPositionZ, movingAnimationHalfDuration);
        movingTweenY = weaponTransform.DOLocalMoveY(wav.movingAnimationPositionY, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);

        movingTweenZ = weaponTransform.DOLocalMoveY(wav.movingAnimationPositionYBack, movingAnimationHalfDuration);
        movingTweenY = weaponTransform.DOLocalMoveZ(movingAnimationPositionZBack, movingAnimationHalfDuration);
        yield return new WaitForSeconds(movingAnimationHalfDuration);

        isMovingAnimationPlaying = false;
    }

    private void StopMovingAnimation()
    {
        StopCoroutine(movingAnimation);
        isMovingAnimationPlaying = false;

        movingTweenZ.Kill();
        movingTweenY.Kill();

        if (PlayerStateData.Singleton.isIdle) weaponTransform.DOLocalMoveY(wav.movingAnimationPositionYBack, 0.1f);
        weaponTransform.DOLocalMoveZ(movingAnimationPositionZBack, 0.1f);
    }

    #endregion

    #region Running

    private void EnableRunningMode()
    {
        attackRotationXBack = wav.runningModeRotationX;
        movingAnimationPositionZ = wav.runningModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = wav.runningModeMovingAnimationPositionZBack;

        StopMovingAnimation();

        runningModeRotationTween.Kill();
        runningModeRotationTween = weaponTransform.DOLocalRotate(new Vector3(wav.runningModeRotationX, wav.defaultRotationY, 0f), 0.1f);

        isRunningModeActive = true;
    }

    private void DisableRunningMode()
    {
        attackRotationXBack = wav.walkingModeRotationX;
        movingAnimationPositionZ = wav.walkingModeMovingAnimationPositionZ;
        movingAnimationPositionZBack = wav.walkingModeMovingAnimationPositionZBack;

        StopMovingAnimation();

        runningModeRotationTween.Kill();
        runningModeRotationTween = weaponTransform.DOLocalRotate(new Vector3(wav.walkingModeRotationX, wav.defaultRotationY, 0f), 0.1f);

        isRunningModeActive = false;
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
        isAttackAnimationPlaying = true;

        attackRotationTween = weaponTransform.DOLocalRotate(new Vector3(wav.attackRotationX, wav.defaultRotationY, 0f), wav.attackAnimationHalfDuration);
        DOTween.To(() => cooldownSlider.value, value => cooldownSlider.value = value, 0, wav.attackAnimationHalfDuration);
        yield return new WaitForSeconds(wav.attackAnimationHalfDuration);

        attackRotationTween = weaponTransform.DOLocalRotate(new Vector3(attackRotationXBack, wav.defaultRotationY, 0f), wav.attackAnimationHalfDuration);
        DOTween.To(() => cooldownSlider.value, value => cooldownSlider.value = value, 1, wav.attackAnimationHalfDuration);
        yield return new WaitForSeconds(wav.attackAnimationHalfDuration);

        isAttackAnimationPlaying = false;
    }

    #endregion
}
