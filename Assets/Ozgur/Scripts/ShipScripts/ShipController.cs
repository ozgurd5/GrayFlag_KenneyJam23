using Cinemachine;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float normalSailSpeed = 3f;
    [SerializeField] private float fullSailSpeed = 10f;

    private bool isShipControlled;
    
    private ShipInputManager sim;
    private Transform cameraFollowTransform;
    private Transform cameraLookAtTransform;
    private CinemachineVirtualCamera shipCamera;

    private Vector3 movingDirection;
    private float movingSpeed;

    private void Awake()
    {
        //Moving speed default must be walking speed
        movingSpeed = normalSailSpeed;

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
        transform.Translate(Vector3.forward * (sim.moveInput.y * movingSpeed));
        transform.Rotate(0f, sim.moveInput.x * movingSpeed, 0f);
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