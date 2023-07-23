using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnJump;
    
    [Header("Assign")]
    [SerializeField] private float walkingSpeed = 8f;
    [SerializeField] private float runningSpeed = 15f;
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 20f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Rigidbody rb;
    private Transform cameraTransform;

    private bool isJumpCondition;
    private Vector3 currentRotation;
    private Vector3 movingDirection;
    public float movingSpeed;

    private void Awake()
    {
        //Moving speed default must be walking speed
        movingSpeed = walkingSpeed;

        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = GameObject.Find("PlayerCamera").transform;
    }

    private void HandleLooking()
    {
        currentRotation.x -= pim.lookInput.y;
        currentRotation.y += pim.lookInput.x;
        
        currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation);
        
        //currentRotation.x = 0f; //very stupid that it broke the camera's x rotation
        transform.localRotation = Quaternion.Euler(new Vector3(0f, currentRotation.y, 0f));
    }
    
    private void CalculateMovingDirection()
    {
        if (psd.isHookFlying) return;
        
        movingDirection = transform.right * pim.moveInput.x + transform.forward * pim.moveInput.y;
        movingDirection.y = 0f;
    }

    private void HandleMovingSpeed()
    {
        //delete speed thing in walking running decide
        
        if (psd.isWalking) StartCoroutine(IncreaseMovingSpeed(walkingSpeed));
        else if (psd.isRunning) StartCoroutine(IncreaseMovingSpeed(runningSpeed));
        
        else StartCoroutine(DecreaseMovingSpeed(0f));
    }

    private void HandleMovement()
    {
        if (psd.isHookFlying) return;
        
        movingDirection *= movingSpeed;
        rb.velocity = new Vector3(movingDirection.x, rb.velocity.y, movingDirection.z);
    }

    private void CheckJumpCondition()
    {
        if (psd.isGrounded && pim.isJumpKeyDown) isJumpCondition = true;
    }
    
    private void HandleJump()
    {
        if (!isJumpCondition) return;
        
        rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        OnJump?.Invoke();
        
        psd.isJumping = true;
        isJumpCondition = false;
    }

    private void DecideJumpingState()
    {
        if (psd.isGrounded) psd.isJumping = false;
    }

    private void DecideIdleOrMovingStates()
    {
        Vector2 velocityXZ = new Vector2(rb.velocity.x, rb.velocity.z);
        
        psd.isMoving = math.abs(velocityXZ.magnitude) > 0.01f;  //It's never 0, idk why
        psd.isIdle = !psd.isMoving;
    }
    
    private void DecideWalkingOrRunningStates()
    {
        if (!psd.isMoving)
        {
            psd.isWalking = false;
            psd.isRunning = false;
            return;
        }
        
        psd.isRunning = pim.isRunKey;
        psd.isWalking = !psd.isRunning;

        if (psd.isRunning) movingSpeed = runningSpeed;
        else movingSpeed = walkingSpeed;
    }

    private void Update()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;

        DecideJumpingState();
        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
        HandleLooking();
        CheckJumpCondition();
    }

    private void FixedUpdate()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        CalculateMovingDirection();
        HandleMovement();
        HandleJump();
    }
    
    private IEnumerator IncreaseMovingSpeed(float movingSpeedToReach)
    {
        while (movingSpeed < movingSpeedToReach)
        {
            movingSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator DecreaseMovingSpeed(float movingSpeedToReach)
    {
        while (movingSpeed > movingSpeedToReach)
        {
            movingSpeed -= deceleration * Time.deltaTime;
            yield return null;
        }
    }
}