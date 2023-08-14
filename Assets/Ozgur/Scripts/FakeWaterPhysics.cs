using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FakeWaterPhysics : MonoBehaviour
{
    [Header("IMPORTANT - SELECT IF PLAYER")] 
    [SerializeField] private bool isPlayer;

    [Header("Assign")] [SerializeField]
    private float movingAmount; //ship: 1 - player: 0.5
    [SerializeField] private float movingTime; //ship: 1 - player: 0.75
    [SerializeField] private float rotatingAmount = 1f;
    [SerializeField] private float rotatingSpeed = 1f;

    [Header("Info - No Touch")]
    [SerializeField] private bool isXPositive;
    [SerializeField] private bool isZPositive;
    
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

        GroundCheck.OnSwimmingEnter += EnablePlayerSwimmingAnimation;
        GroundCheck.OnSwimmingExit += DisablePlayerSwimmingAnimation;
    }

    private void Update()
    {
        if (isPlayer) HandlePlayerSwimmingAnimations();
        else PlaySwimmingAnimations();
    }

    private void HandlePlayerSwimmingAnimations()
    {
        if (!PlayerStateData.Singleton.isSwimming) return;
        PlaySwimmingAnimations();
    }

    private void EnablePlayerSwimmingAnimation()
    {
        playerEg.enabled = false;
        playerRb.useGravity = false;
    }
    
    private void DisablePlayerSwimmingAnimation()
    {
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

            Vector3 rotation = Vector3.zero;
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
        
        GroundCheck.OnSwimmingEnter -= EnablePlayerSwimmingAnimation;
        GroundCheck.OnSwimmingExit -= DisablePlayerSwimmingAnimation;
    }
}
