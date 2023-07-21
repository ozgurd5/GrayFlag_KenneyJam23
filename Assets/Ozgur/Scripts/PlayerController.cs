using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// <para>Controls player movement, rotation, jump</para>
/// <para>Works only for local player</para>
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float runningSpeed = 10f;
    
    [Header("Assign - 0-1 - Range broken")]
    [SerializeField] private float walkingRotatingSpeed = 0.2f;
    [SerializeField] private float runningRotatingSpeed = 0.4f;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Rigidbody rb;

    public float rotatingSpeed;
    private Vector3 movingDirection;
    private float movingSpeed;

    private void Awake()
    {
        //Moving and rotating speed defaults must be walking speeds
        movingSpeed = walkingSpeed;
        rotatingSpeed = walkingRotatingSpeed;

        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        rb = GetComponent<Rigidbody>();
    }
    
    /// <summary>
    /// <para>Calculates moving direction</para>
    /// <para>Should work in FixedUpdate</para>
    /// </summary>
    private void CalculateMovingDirection()
    {
        Vector3 lookingDirectionRight = Vector3.Cross(Vector3.up, pim.lookingDirectionForward);
        
        movingDirection = lookingDirectionRight * pim.moveInput.x + pim.lookingDirectionForward * pim.moveInput.y;
        movingDirection.y = 0f;
    }
    
    /// <summary>
    /// <para>Turns the player to looking direction</para>
    /// <para>Must work in FixedUpdate, because Update stutters, idk why</para>
    /// </summary>
    private void TurnLookingDirection()
    {
        if (psd.isMoving) transform.forward = Vector3.Slerp(transform.forward, movingDirection, rotatingSpeed);
        else if (psd.isIdle) transform.forward = Vector3.Slerp(transform.forward, pim.lookingDirectionForward, rotatingSpeed);
    }

    /// <summary>
    /// <para>Switches between idle and moving via rigidbody velocity x and z</para>
    /// <para>Must work in Update since it must work before DecideWalkingOrRunningStates</para>
    /// </summary>
    private void DecideIdleOrMovingStates()
    {
        Vector2 velocityXZ = new Vector2(rb.velocity.x, rb.velocity.z);
        
        psd.isMoving = math.abs(velocityXZ.magnitude) > 0.01f;  //It's never 0, idk why
        psd.isIdle = !psd.isMoving;
    }
    
    /// <summary>
    /// <para>Switches between walking and running state via input, sets moving and rotating speeds according to it</para>
    /// <para>Must work Update since it has input check</para>
    /// </summary>
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

        if (psd.isRunning)
        {
            movingSpeed = runningSpeed;
            rotatingSpeed = runningRotatingSpeed;
        }
        else
        {
            movingSpeed = walkingSpeed;
            rotatingSpeed = walkingRotatingSpeed;
        }
    }
    
    /// <summary>
    /// <para>Makes player move</para>
    /// <para>Must work in FixedUpdate</para>
    /// </summary>
    private void HandleMovement()
    {
        movingDirection *= movingSpeed;
        rb.velocity = new Vector3(movingDirection.x, rb.velocity.y, movingDirection.z);
    }

    private void Update()
    {
        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
    }

    private void FixedUpdate()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        CalculateMovingDirection();
        TurnLookingDirection();
        
        HandleMovement();
    }
}