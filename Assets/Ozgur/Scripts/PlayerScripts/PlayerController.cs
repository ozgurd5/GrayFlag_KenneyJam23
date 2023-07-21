using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float runningSpeed = 10f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Rigidbody rb;
    private Transform cameraTransform;

    public Vector3 currentRotation;
    private Vector3 movingDirection;
    private float movingSpeed;

    private void Awake()
    {
        //Moving speed default must be walking speed
        movingSpeed = walkingSpeed;

        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = GameObject.Find("PlayerCamera").transform;
    }
    
    void Start()
    {
        currentRotation = transform.localRotation.eulerAngles;
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
        movingDirection = transform.right * pim.moveInput.x + transform.forward * pim.moveInput.y;
        movingDirection.y = 0f;
    }

    private void HandleMovement()
    {
        movingDirection *= movingSpeed;
        rb.velocity = new Vector3(movingDirection.x, rb.velocity.y, movingDirection.z);
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
        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
        HandleLooking();
    }

    private void FixedUpdate()
    {
        CalculateMovingDirection();
        HandleMovement();
    }
}