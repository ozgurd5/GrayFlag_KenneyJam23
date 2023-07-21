using Unity.Mathematics;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float normalSailSpeed = 3f;
    [SerializeField] private float fullSailSpeed = 10f;
    
    private ShipInputManager sim;
    private Transform cameraFollowTransform;
    public Transform cameraLookAtTransform;

    private Vector3 movingDirection;
    private float movingSpeed;

    private void Awake()
    {
        //Moving speed default must be walking speed
        movingSpeed = normalSailSpeed;

        sim = GetComponent<ShipInputManager>();
        cameraFollowTransform = transform.Find("ShipCameraFollow");
        cameraLookAtTransform = transform.Find("ShipCameraLookAt");
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
        HandleLooking();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
}