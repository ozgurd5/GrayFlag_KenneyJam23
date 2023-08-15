using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FakeWaterPhysics : MonoBehaviour
{
    [Header("IMPORTANT - SELECT IF PLAYER")] 
    [SerializeField] private bool isPlayer;

    [Header("Assign")]
    [SerializeField] private float movingAmount = 1f;
    [SerializeField] private float movingTime = 1f;
    [SerializeField] private float rotatingAmount = 1f;
    [SerializeField] private float rotatingSpeed = 1f;
    [SerializeField] private float swimPosition = 2f;

    [Header("Info - No Touch")]
    [SerializeField] private bool isXPositive;
    [SerializeField] private bool isZPositive;
    [SerializeField] private bool canPlayerSwimAnimationPlay;
    [SerializeField] private bool isPlayerSinking;
    
    private bool isMoving;
    private bool isXRotating;
    private bool isZRotating;
    
    private Rigidbody playerRb;
    private ExtraGravity playerEg;
    
    private enum Axis
    {
        X,Z
    }
    
    private void Awake()
    {
        if (!isPlayer) return;
        
        playerRb = GetComponent<Rigidbody>();
        playerEg = GetComponent<ExtraGravity>();

        PlayerSwimmingManager.OnSwimmingEnter += EnablePlayerSwimmingAnimation;
        PlayerSwimmingManager.OnSwimmingExit += DisablePlayerSwimmingAnimation;
    }

    private void Update()
    {
        if (isPlayer) HandlePlayerSwimmingAnimations();
        else PlaySwimmingAnimations();
    }

    private void HandlePlayerSwimmingAnimations()
    {
        if (canPlayerSwimAnimationPlay) PlaySwimmingAnimations();
    }

    private void EnablePlayerSwimmingAnimation()
    {
        StartCoroutine(SinkPlayer());
    }

    private IEnumerator SinkPlayer()
    {
        isPlayerSinking = true;
        
        while (isPlayerSinking)
        {
            if (transform.position.y <= swimPosition)
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
                playerEg.enabled = false;
                playerRb.useGravity = false;

                isPlayerSinking = false;
                canPlayerSwimAnimationPlay = true;
            }

            else
            {
                yield return null;
            }
        }
    }
    
    private void DisablePlayerSwimmingAnimation()
    {
        canPlayerSwimAnimationPlay = false;
        
        playerEg.enabled = true;
        playerRb.useGravity = true;
    }
    
    private void PlaySwimmingAnimations()
    {
        if (!isMoving) StartCoroutine(Move());

        if (!isXRotating)
        {
            if (isXPositive) StartCoroutine(Rotate(false, Axis.X));
            else StartCoroutine(Rotate(true, Axis.X));
        }
        
        if (!isZRotating)
        {
            if (isZPositive) StartCoroutine(Rotate(false, Axis.Z));
            else StartCoroutine(Rotate(true, Axis.Z));
        }
    }

    private IEnumerator Move()
    {
        isMoving = true;

        transform.DOMoveY(transform.position.y + movingAmount, movingTime).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(movingTime);

        transform.DOMoveY(transform.position.y - movingAmount, movingTime).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(movingTime);
        
        isMoving = false;
    }
    
    private IEnumerator Rotate(bool isPositive, Axis axis)
    {
        if (axis == Axis.X) isXRotating = true;
        else isZRotating = true;
        
        float totalAngle = 0;
        while (totalAngle <= rotatingAmount)
        {
            float angle = rotatingSpeed * Time.deltaTime;
            totalAngle += angle;

            if (totalAngle > rotatingAmount) angle -= totalAngle - rotatingAmount;

            if (!isPositive) angle = -angle;

            Vector3 rotation;
            if (axis == Axis.X) rotation = new Vector3(angle, 0f, 0f);
            else rotation = new Vector3(0f, 0f, angle);
            
            transform.Rotate(rotation);

            yield return null;
        }

        if (axis == Axis.X) isXPositive = isPositive;
        else isZPositive = isPositive;

        if (axis == Axis.X) isXRotating = false;
        else isZRotating = false;
    }

    private void OnDestroy()
    {
        if (!isPlayer) return;
        
        PlayerSwimmingManager.OnSwimmingEnter -= EnablePlayerSwimmingAnimation;
        PlayerSwimmingManager.OnSwimmingExit -= DisablePlayerSwimmingAnimation;
    }
}
