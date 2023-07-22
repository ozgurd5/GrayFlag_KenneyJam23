using System;
using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public static event Action<SailMode> OnSailChanged; 
    public static bool isShipControlled;
    
    [Header("Assign - Movement")]
    [SerializeField] private float fullSailSpeed = 40f;
    [SerializeField] private float halfSailSpeed = 30f;
    [SerializeField] private float reverseSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;

    [Header("Assign - Rotation")]
    [SerializeField] private float rotationAcceleration = 2f; //2 means nearly no acceleration at all
    [SerializeField] private float stationaryRotationSpeed = 1f;
    [SerializeField] private float halfSailRotationSpeed = 0.75f;
    [SerializeField] private float fullSailRotationSpeed = 0.5f;

    [Header("Info - No touch")]
    public SailMode currentSailMode;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float rotationSpeed;

    private ShipInputManager sim;
    private Transform cameraFollowTransform;
    private Transform cameraLookAtTransform;
    private CinemachineVirtualCamera shipCamera;
    private Rigidbody rb;
    
    public enum SailMode
    {
        Reverse,
        Stationary,
        HalfSail,
        FullSail
    }

    private void Awake()
    {
        currentSailMode = SailMode.Stationary;
        rotationSpeed = stationaryRotationSpeed;
        
        sim = GetComponent<ShipInputManager>();
        cameraFollowTransform = transform.Find("ShipCameraFollow");
        cameraLookAtTransform = transform.Find("ShipCameraLookAt");
        shipCamera = GameObject.Find("ShipCamera").GetComponent<CinemachineVirtualCamera>();
        rb = GetComponent<Rigidbody>();
    }

    private void HandleLooking()
    {
        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, Vector3.up, sim.lookInput.x);
        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, Vector3.right, -sim.lookInput.y);
    }

    private void HandleSailMode()
    {
        if (sim.isSailUpKeyDown) IncreaseSail();
        else if (sim.isSailDownKeyDown) DecreaseSail();
    }

    private void HandleMovement()
    {
        rb.velocity = transform.forward * movingSpeed;
        transform.Rotate(0f, sim.rotateInput * rotationSpeed, 0f);
    }

    private void Update()
    {
        if (isShipControlled)
        {
            HandleLooking();
            HandleSailMode();
        }
    }

    private void FixedUpdate()
    {
        if (isShipControlled) HandleMovement();
    }

    public void TakeControl()
    {
        isShipControlled = true;
        shipCamera.enabled = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void DropControl()
    {
        isShipControlled = false;
        shipCamera.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        currentSailMode = SailMode.Stationary;
    }

    private void IncreaseSail()
    {
        currentSailMode++;
        
        if (currentSailMode == SailMode.Stationary)
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseMovingSpeed(0f));
            StartCoroutine(IncreaseRotationSpeed(stationaryRotationSpeed));
        }
        
        else if (currentSailMode == SailMode.HalfSail)
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseMovingSpeed(halfSailSpeed));
            StartCoroutine(IncreaseRotationSpeed(halfSailRotationSpeed));
        }
        
        else if (currentSailMode == SailMode.FullSail)
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseMovingSpeed(fullSailSpeed));
            StartCoroutine(IncreaseRotationSpeed(fullSailRotationSpeed));
        }
        
        currentSailMode = (SailMode)math.clamp((int)currentSailMode, 0, 3);
        OnSailChanged?.Invoke(currentSailMode);
    }
    
    private void DecreaseSail()
    {
        currentSailMode--;

        if (currentSailMode == SailMode.Reverse)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseMovingSpeed(-1 * reverseSpeed));
            StartCoroutine(DecreaseRotationSpeed(stationaryRotationSpeed));
        }
        
        else if (currentSailMode == SailMode.Stationary)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseMovingSpeed(0f));
            StartCoroutine(DecreaseRotationSpeed(stationaryRotationSpeed));
        }
        
        else if (currentSailMode == SailMode.HalfSail)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseMovingSpeed(halfSailSpeed));
            StartCoroutine(DecreaseRotationSpeed(halfSailRotationSpeed));
        }
        
        currentSailMode = (SailMode)math.clamp((int)currentSailMode, 0, 3);
        OnSailChanged?.Invoke(currentSailMode);
    }

    private IEnumerator IncreaseMovingSpeed(float movingSpeedToReach)
    {
        while (movingSpeed < movingSpeedToReach)
        {
            movingSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
        
        if (currentSailMode == SailMode.Stationary) movingSpeed = 0f;
    }
    
    private IEnumerator DecreaseMovingSpeed(float movingSpeedToReach)
    {
        while (movingSpeed > movingSpeedToReach)
        {
            movingSpeed -= deceleration * Time.deltaTime;
            yield return null;
        }

        if (currentSailMode == SailMode.Stationary) movingSpeed = 0f;
    }
    
    private IEnumerator IncreaseRotationSpeed(float rotationSpeedToReach)
    {
        while (rotationSpeed > rotationSpeedToReach)
        {
            rotationSpeed -= rotationAcceleration * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DecreaseRotationSpeed(float rotationSpeedToReach)
    {
        while (rotationSpeed < rotationSpeedToReach)
        {
            rotationSpeed += rotationAcceleration * Time.deltaTime;
            yield return null;
        }
    }
}