using System;
using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipController : MonoBehaviour
{
    public static event Action<SailMode> OnSailChanged;
    
    [Header("Assign - Movement")]
    [SerializeField] private float fullSailSpeed = 60f;
    [SerializeField] private float halfSailSpeed = 40f;
    [SerializeField] private float reverseSpeed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 30f;

    [Header("Assign - Rotation")]
    [SerializeField] private float rotationAcceleration = 2f; //2 means nearly no acceleration at all
    [SerializeField] private float stationaryRotationSpeed = 1f;
    [SerializeField] private float halfSailRotationSpeed = 0.75f;
    [SerializeField] private float fullSailRotationSpeed = 0.5f;

    [Header("Assign - Look")]
    [SerializeField] private float cameraMinYPos = 10f;
    [SerializeField] private float cameraMaxYPos = 110f;

    [Header("Info - No touch")]
    public SailMode currentSailMode;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float rotationSpeed;
    public bool canMoveForward;
    public bool canMoveBackward;
    public bool canRotateRight;
    public bool canRotateLeft;

    private ShipInputManager sim;
    private ShipAnimationManager sam;
    private Transform cameraFollowTransform;
    private Transform cameraLookAtTransform;
    private CinemachineVirtualCamera shipCamera;
    private Rigidbody rb;
    private AudioSource aus;

    private PlayerStateData psd;    

    public IEnumerator stopShipWithoutPhysicsCoroutine { private set; get; }

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
        sam = GetComponent<ShipAnimationManager>();
        cameraFollowTransform = transform.Find("ShipCameraFollow");
        cameraLookAtTransform = transform.Find("ShipCameraLookAt");
        shipCamera = GameObject.Find("ShipCamera").GetComponent<CinemachineVirtualCamera>();
        rb = GetComponent<Rigidbody>();
        aus = GetComponent<AudioSource>();

        psd = PlayerStateData.Singleton;;

        stopShipWithoutPhysicsCoroutine = StopShipWithoutPhysics();
    }

    private void HandleLooking()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.ShipControllingState) return;

        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, Vector3.up, sim.lookInput.x);
        
        if ((cameraFollowTransform.position.y < cameraMinYPos && sim.lookInput.y < 0) ||
            cameraFollowTransform.position.y > cameraMaxYPos && sim.lookInput.y > 0) return;
        
        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, cameraFollowTransform.right, sim.lookInput.y);
    }

    private void Update()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.ShipControllingState) return;
        
        HandleLooking();
        HandleSailMode();
    }

    private void FixedUpdate()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.ShipControllingState) HandleMovement();
    }

    public void TakeControl()
    {
        shipCamera.enabled = true;
        rb.isKinematic = false;
    }

    public void DropControl()
    {
        shipCamera.enabled = false;
        rb.isKinematic = true;
        
        sam.SetStationary();
        StopAllCoroutines();
        StartCoroutine(stopShipWithoutPhysicsCoroutine);
        currentSailMode = SailMode.Stationary;
    }
    
    private void HandleSailMode()
    {
        if (sim.isSailUpKeyDown && currentSailMode != SailMode.FullSail) IncreaseSail();
        else if (sim.isSailDownKeyDown && currentSailMode != SailMode.Reverse) DecreaseSail();
    }

    private void HandleMovement()
    {
        if ((canMoveForward && currentSailMode != SailMode.Reverse) || (canMoveBackward && currentSailMode == SailMode.Reverse))
            rb.velocity = transform.forward * movingSpeed;
        else
        {
            rb.velocity = Vector3.zero;
            movingSpeed = 0f;
        }
        
        if ((sim.rotateInput == -1 && canRotateLeft) || (sim.rotateInput == 1 && canRotateRight))
            transform.Rotate(0f, sim.rotateInput * rotationSpeed, 0f);
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
            aus.Stop();
            aus.Play();
            
            StopAllCoroutines();
            StartCoroutine(IncreaseMovingSpeed(halfSailSpeed));
            StartCoroutine(IncreaseRotationSpeed(halfSailRotationSpeed));
        }
        
        else if (currentSailMode == SailMode.FullSail)
        {
            aus.Stop();
            aus.Play();
            
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
            aus.Stop();
            aus.Play();
            
            StopAllCoroutines();
            StartCoroutine(DecreaseMovingSpeed(0f));
            StartCoroutine(DecreaseRotationSpeed(stationaryRotationSpeed));
        }
        
        else if (currentSailMode == SailMode.HalfSail)
        {
            aus.Stop();
            aus.Play();
            
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

        //End result won't not be the exact movingSpeedToReach
        movingSpeed = movingSpeedToReach;
    }
    
    private IEnumerator DecreaseMovingSpeed(float movingSpeedToReach)
    {
        while (movingSpeed > movingSpeedToReach)
        {
            movingSpeed -= deceleration * Time.deltaTime;
            yield return null;
        }
        
        //End result won't not be the exact movingSpeedToReach
        movingSpeed = movingSpeedToReach;
    }
    
    private IEnumerator IncreaseRotationSpeed(float rotationSpeedToReach)
    {
        while (rotationSpeed > rotationSpeedToReach)
        {
            rotationSpeed -= rotationAcceleration * Time.deltaTime;
            yield return null;
        }
        
        //End result won't not be the exact rotationSpeedToReach
        rotationSpeed = rotationSpeedToReach;
    }

    private IEnumerator DecreaseRotationSpeed(float rotationSpeedToReach)
    {
        while (rotationSpeed < rotationSpeedToReach)
        {
            rotationSpeed += rotationAcceleration * Time.deltaTime;
            yield return null;
        }
        
        //End result won't not be the exact rotationSpeedToReach
        rotationSpeed = rotationSpeedToReach;
    }

    private IEnumerator StopShipWithoutPhysics()
    {
        StartCoroutine(DecreaseMovingSpeed(0));
        
        while (movingSpeed > 0)
        {
            transform.position += transform.forward * (Time.deltaTime * movingSpeed);
            yield return null;
        }
    }
}