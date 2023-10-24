using System;
using System.Collections;
using System.ComponentModel;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipController : MonoBehaviour
{
    public static event Action<SailMode> OnSailChanged;
    public static event Action OnMovementStarted;
    public static event Action OnMovementEnded;
    public static event Action OnRotationStarted;
    public static event Action OnRotationEnded;
    
    [Header("Assign - Movement")]
    [SerializeField] private float fullSailSpeed = 60f;
    [SerializeField] private float halfSailSpeed = 40f;
    [SerializeField] private float reverseSpeed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 30f;

    [Header("Assign - Rotation")]
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
    [SerializeField] private bool isRotating;

    private ShipInputManager sim;
    private ShipAnimationManager sam;
    private Transform cameraFollowTransform;
    private Transform cameraLookAtTransform;
    private CinemachineVirtualCamera shipCamera;
    private Rigidbody rb;
    private AudioSource aus;
    private PlayerStateData psd;

    private bool previousIsRotating;
    
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

    private void Update()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.ShipControllingState) return;
        
        HandleLooking();
        HandleSailMode();
        HandleRotatingEvents();
    }

    private void FixedUpdate()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.ShipControllingState) return;
        
        HandleMovement();
        HandleRotating();
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
    
    private void HandleLooking()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.ShipControllingState) return;

        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, Vector3.up, sim.lookInput.x);
        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, cameraFollowTransform.right, sim.lookInput.y);

        if ((cameraFollowTransform.position.y < cameraMinYPos && sim.lookInput.y < 0) || (cameraFollowTransform.position.y > cameraMaxYPos && sim.lookInput.y > 0))
        {
            cameraFollowTransform.RotateAround(cameraLookAtTransform.position, cameraFollowTransform.right, -sim.lookInput.y);
        }
    }

    private void HandleMovement()
    {
        if ((canMoveForward && currentSailMode != SailMode.Reverse) || (canMoveBackward && currentSailMode == SailMode.Reverse))
        {
            rb.velocity = transform.forward * movingSpeed;
        }
        
        else
        {
            rb.velocity = Vector3.zero;
            movingSpeed = 0f;
        }
    }

    private void HandleRotating()
    {
        if ((sim.rotateInput == -1 && canRotateLeft) || (sim.rotateInput == 1 && canRotateRight))
        {
            transform.Rotate(0f, sim.rotateInput * rotationSpeed, 0f);
            isRotating = true;
        }
        
        else isRotating = false; 
    }

    private void HandleSailMode()
    {
        if (sim.isSailUpKeyDown && currentSailMode != SailMode.FullSail) IncreaseSail();
        else if (sim.isSailDownKeyDown && currentSailMode != SailMode.Reverse) DecreaseSail();
    }

    private void IncreaseSail()
    {
        currentSailMode++;
        
        if (currentSailMode == SailMode.Stationary)
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseMovingSpeed(0f));
            rotationSpeed = stationaryRotationSpeed;
        }
        
        else if (currentSailMode == SailMode.HalfSail)
        {
            aus.Stop();
            aus.Play();
            
            StopAllCoroutines();
            StartCoroutine(IncreaseMovingSpeed(halfSailSpeed));
            rotationSpeed = halfSailRotationSpeed;
            
            OnMovementStarted?.Invoke();
        }
        
        else if (currentSailMode == SailMode.FullSail)
        {
            aus.Stop();
            aus.Play();
            
            StopAllCoroutines();
            StartCoroutine(IncreaseMovingSpeed(fullSailSpeed));
            rotationSpeed = fullSailRotationSpeed;
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
            rotationSpeed = stationaryRotationSpeed;
        }
        
        else if (currentSailMode == SailMode.Stationary)
        {
            aus.Stop();
            aus.Play();
            
            StopAllCoroutines();
            StartCoroutine(DecreaseMovingSpeed(0f));
            rotationSpeed = stationaryRotationSpeed;
            
            OnMovementEnded?.Invoke();
        }
        
        else if (currentSailMode == SailMode.HalfSail)
        {
            aus.Stop();
            aus.Play();
            
            StopAllCoroutines();
            StartCoroutine(DecreaseMovingSpeed(halfSailSpeed));
            rotationSpeed = halfSailRotationSpeed;
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

    private IEnumerator StopShipWithoutPhysics()
    {
        StartCoroutine(DecreaseMovingSpeed(0));
        
        while (movingSpeed > 0)
        {
            transform.position += transform.forward * (Time.deltaTime * movingSpeed);
            yield return null;
        }
    }

    private void HandleRotatingEvents()
    {
        if (!previousIsRotating && isRotating) OnRotationStarted?.Invoke();
        else if (previousIsRotating && !isRotating) OnRotationEnded?.Invoke(); 

        previousIsRotating = isRotating;
    }
}