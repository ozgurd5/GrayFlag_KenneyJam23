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
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float jumpBufferLimit = 0.2f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerLookingController plc;
    private Rigidbody rb;
    
    [Header("No Touch")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private bool isIncreasingSpeed;
    private bool isJumpCondition;
    private float jumpBufferTimer;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        plc = GetComponent<PlayerLookingController>();
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
        HandleMovingSpeed();
        HandleJumpCondition();
    }

    private void FixedUpdate()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        HandleMovement();
        HandleJump();
    }

    private void HandleMovingSpeed()
    {
        if (psd.isIdle) movingSpeed = 0f;
        
        else if (psd.isWalking)
        {
            if (movingSpeed > walkingSpeed) movingSpeed = walkingSpeed;    //from running to moving
            else if (!isIncreasingSpeed && movingSpeed != walkingSpeed) StartCoroutine(IncreaseMovingSpeed(walkingSpeed));
        }
        
        else if (psd.isRunning && !isIncreasingSpeed && movingSpeed != runningSpeed) StartCoroutine(IncreaseMovingSpeed(runningSpeed));
    }

    private void HandleMovement()
    {
        if (psd.isGettingDamage) return;

        Vector3 moving = plc.movingDirection * movingSpeed;
        rb.velocity = new Vector3(moving.x, rb.velocity.y, moving.z);
    }

    private void HandleJumpCondition()
    {
        if (pim.isJumpKeyDown)
        {
            psd.isJumping = false;
            jumpBufferTimer = jumpBufferLimit;
        }
        
        else jumpBufferTimer -= Time.deltaTime;
        
        if (jumpBufferTimer > 0f && psd.isGrounded) isJumpCondition = true;
    }

    private void HandleJump()
    {
        if (!isJumpCondition) return;
        
        rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
        OnJump?.Invoke();
        
        psd.isJumping = true;
        isJumpCondition = false;
    }

    private void DecideIdleOrMovingStates()
    { 
        psd.isMoving = pim.moveInput.magnitude > 0;
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
    }

    private IEnumerator IncreaseMovingSpeed(float movingSpeedToReach)
    {
        isIncreasingSpeed = true;
        
        while (movingSpeed < movingSpeedToReach)
        {
            movingSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
        
        if (movingSpeed > movingSpeedToReach) movingSpeed = movingSpeedToReach;
        
        isIncreasingSpeed = false;
    }
}