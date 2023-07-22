using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float maxSpeed = 0.5f;
    [SerializeField] private float reverseSpeed = 0.1f;
    [SerializeField] private float acceleration = 0.2f;
    [SerializeField] private float deceleration = 0.5f;
    [SerializeField] private float minRotationSpeed = 0.2f;
    [SerializeField] private float maxRotationSpeed = 0.5f;
    
    [Header("Info - No touch")]
    [SerializeField] private float movingSpeed;
    [SerializeField] private float rotationSpeed;
    
    private bool isShipControlled;
    
    private ShipInputManager sim;
    private Transform cameraFollowTransform;
    private Transform cameraLookAtTransform;
    private CinemachineVirtualCamera shipCamera;

    private void Awake()
    {
        rotationSpeed = minRotationSpeed;
        
        sim = GetComponent<ShipInputManager>();
        cameraFollowTransform = transform.Find("ShipCameraFollow");
        cameraLookAtTransform = transform.Find("ShipCameraLookAt");
        shipCamera = GameObject.Find("ShipCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void HandleLooking()
    {
        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, Vector3.up, sim.lookInput.x);
        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, Vector3.right, -sim.lookInput.y);
    }

    private void HandleMovement()
    {
        //Acceleration
        if (sim.moveInput.y != 0f)
        {
            if (sim.moveInput.y > 0f)
            {
                movingSpeed += acceleration * Time.deltaTime;
                movingSpeed = math.min(movingSpeed, maxSpeed);
            }
            
            else
            {
                movingSpeed -= acceleration * Time.deltaTime;
                movingSpeed = math.max(movingSpeed, -1 * reverseSpeed);
            }
            
            rotationSpeed -= acceleration * Time.deltaTime;
            rotationSpeed = math.max(rotationSpeed, minRotationSpeed);
        }

        //Deceleration
        else
        {
            if (movingSpeed < 0f)
            {
                movingSpeed += deceleration * Time.deltaTime;
                movingSpeed = math.min(movingSpeed, 0);
            }

            else
            {
                movingSpeed -= deceleration * Time.deltaTime;
                movingSpeed = math.max(movingSpeed, 0);
            }
            
            rotationSpeed += deceleration * Time.deltaTime;
            rotationSpeed = math.min(rotationSpeed, maxRotationSpeed);
        }

        transform.Translate(Vector3.forward * movingSpeed);
        transform.Rotate(0f, sim.moveInput.x * rotationSpeed, 0f);
    }

    private void Update()
    {
        if (isShipControlled) HandleLooking();
    }

    private void FixedUpdate()
    {
        if (isShipControlled) HandleMovement();
    }

    public void TakeControl()
    {
        isShipControlled = true;
        shipCamera.enabled = true;
    }

    public void DropControl()
    {
        isShipControlled = false;
        shipCamera.enabled = false;
    }
}