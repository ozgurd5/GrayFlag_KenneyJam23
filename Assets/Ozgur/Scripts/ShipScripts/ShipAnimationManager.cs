using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class ShipAnimationManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float sailAnimationTime = 1f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float rotationLimit = 20f;
    
    private Transform frontSail;
    private Transform midSail;
    private Transform backSail;

    private Transform frontPivot;
    private Transform midPivot;
    private Transform backPivot;

    private Tweener frontSailTweenPosition;
    private Tweener frontSailTweenScale;
    private Tweener midSailTweenPosition;
    private Tweener midSailTweenScale;
    private Tweener backSailTweenPosition;
    private Tweener backSailTweenScale;

    private ShipInputManager sim;

    private bool isRotatingAnimationPlaying;
    private float rotationAmount;
    private int currentPosition;    //-1 left 0 middle 1 right

    private void Awake()
    {
        frontSail = transform.Find("FrontSail/FrontSail");
        midSail = transform.Find("MidSail/MidSail");
        backSail = transform.Find("BackSail/BackSail");

        frontPivot = frontSail.parent.Find("FrontPivot");
        midPivot = midSail.parent.Find("MidPivot");
        backPivot = backSail.parent.Find("BackPivot");

        sim = GetComponent<ShipInputManager>();
        ShipController.OnSailChanged += PlayAnimation;
    }

    private void Update()
    {
        PlayRotationAnimation();
    }

    #region SailAnimation

    private void PlayAnimation(ShipController.SailMode sailMode)
    {
        if (sailMode == ShipController.SailMode.Stationary) SetStationary();
        else if (sailMode == ShipController.SailMode.HalfSail) SetHalfSail();
        else if (sailMode == ShipController.SailMode.FullSail) SetFullSail();
    }
    
    private void SetStationary()
    {
        PlayMidSailUpAnimation();
    }

    private void SetHalfSail()
    {
        PlayMidSailDownAnimation();
        PlayFrontSailUpAnimation();
        PlayBackSailUpAnimation();
    }

    private void SetFullSail()
    {
        PlayFrontSailDownAnimation();
        PlayBackSailDownAnimation();
    }

    private void PlayFrontSailUpAnimation()
    {
        frontSailTweenPosition.Kill();
        frontSailTweenScale.Kill();
        
        frontSailTweenPosition = frontSail.DOLocalMoveY(3.5f, sailAnimationTime);
        frontSailTweenScale = frontSail.DOScaleY(0.5f, sailAnimationTime);
    }

    private void PlayFrontSailDownAnimation()
    {
        frontSailTweenPosition.Kill();
        frontSailTweenScale.Kill();
        
        frontSailTweenPosition = frontSail.DOLocalMoveY(-3.5f, sailAnimationTime);
        frontSailTweenScale = frontSail.DOScaleY(2f, sailAnimationTime);
    }

    private void PlayMidSailUpAnimation()
    {
        midSailTweenPosition.Kill();
        midSailTweenScale.Kill();

        midSailTweenPosition = midSail.DOLocalMoveY(8f, sailAnimationTime);
        midSailTweenScale = midSail.DOScaleY(0.5f, sailAnimationTime);
    }

    private void PlayMidSailDownAnimation()
    {
        midSailTweenPosition.Kill();
        midSailTweenScale.Kill();

        midSailTweenPosition = midSail.DOLocalMoveY(0f, sailAnimationTime);
        midSailTweenScale = midSail.DOScaleY(2f, sailAnimationTime);
    }

    private void PlayBackSailUpAnimation()
    {
        backSailTweenPosition.Kill();
        backSailTweenScale.Kill();

        backSailTweenPosition = backSail.DOLocalMoveY(10.5f, sailAnimationTime);
        backSailTweenScale = backSail.DOScaleY(0.5f, sailAnimationTime);
    }

    private void PlayBackSailDownAnimation()
    {
        backSailTweenPosition.Kill();
        backSailTweenScale.Kill();

        backSailTweenPosition = backSail.DOLocalMoveY(0f, sailAnimationTime);
        backSailTweenScale = backSail.DOScaleY(2f, sailAnimationTime);
    }
    
    #endregion

    #region RotationAnimation

    private void PlayRotationAnimation()
    {
        if (sim.rotateInput > 0 && currentPosition != 1 && !isRotatingAnimationPlaying)
        {
            if (currentPosition == 0) rotationAmount = rotationLimit;
            else if (currentPosition == -1) rotationAmount = rotationLimit * 2;

            StartCoroutine(PlayRotationAnimation(true));

            currentPosition = 1;
        }
        
        else if (sim.rotateInput < 0 && currentPosition != -1 && !isRotatingAnimationPlaying)
        {
            if (currentPosition == 0) rotationAmount = rotationLimit;
            else if (currentPosition == 1) rotationAmount = rotationLimit * 2;

            StartCoroutine(PlayRotationAnimation(false));

            currentPosition = -1;
        }
        
        else if (sim.rotateInput == 0f && currentPosition != 0 && !isRotatingAnimationPlaying)
        {
            rotationAmount = rotationLimit;
            
            if (currentPosition == 1) StartCoroutine(PlayRotationAnimation(false));
            else if (currentPosition == -1) StartCoroutine(PlayRotationAnimation(true));
            
            currentPosition = 0;
        }
    }
    
    private IEnumerator PlayRotationAnimation(bool isPositive)
    {
        isRotatingAnimationPlaying = true;
        
        float totalAngle = 0;
        while (totalAngle <= rotationAmount)
        {
            float angle = rotationSpeed * Time.deltaTime;
            totalAngle += angle;

            if (totalAngle > rotationAmount) angle -= totalAngle - rotationAmount;

            if (!isPositive) angle = -angle;
            
            frontSail.RotateAround(frontPivot.position, Vector3.up, angle);
            midSail.RotateAround(midPivot.position, Vector3.up, angle);
            backSail.RotateAround(backPivot.position, Vector3.up, angle);

            yield return null;
        }
        
        isRotatingAnimationPlaying = false;
    }
    
    #endregion
}
