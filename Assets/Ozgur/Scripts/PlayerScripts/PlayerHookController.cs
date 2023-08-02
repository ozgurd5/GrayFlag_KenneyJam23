using System.Collections;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    //TODO: oncollisionstay 2 seconds allowance
    
    [Header("Assign")]
    [SerializeField] private float flyingForce = 1000f;
    [SerializeField] private float acceleration = 40f;
    [SerializeField] private float maxSpeedXZ = 20f;
    [SerializeField] private float maxSpeedY = 50f;
    [SerializeField] private AudioSource aus;
    
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerLookingController plc;
    private PlayerHookGunAnimationManager an;
    private LineRenderer lr;
    private Rigidbody rb;
    private Transform lineOutTransform;
    private Transform hookGunTransform;

    [Header("Info - No Touch")]
    [SerializeField] private float flyingMovingSpeed;
    private bool isIncreasingSpeed;
    private Vector3 hookedPosition;
    private bool flyingCondition;

    //
    public Vector3 speedo;
    public float speedTowardsMovingDirection;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        plc = GetComponent<PlayerLookingController>();
        an = GetComponent<PlayerHookGunAnimationManager>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        hookGunTransform = GameObject.Find("PlayerCamera/HookGun").transform;
        lineOutTransform = hookGunTransform.GetChild(0);
    }

    private void Update()
    {
        //
        speedo = rb.velocity;
        speedTowardsMovingDirection = Vector3.Dot(rb.velocity, plc.movingDirection);
        
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        
        lr.SetPosition(0, lineOutTransform.position);
        HandleEnterHookState();
    }

    private void FixedUpdate()
    {
        HandleFlying();
        HandleFlyingMovement();
        HandleMaxSpeed();
    }

    private void HandleMaxSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // Limit the horizontal velocity magnitude
        if (horizontalVelocity.magnitude > maxSpeedXZ)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeedXZ;
            rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
        }
        
        if (rb.velocity.y > maxSpeedY) rb.velocity = new Vector3(rb.velocity.x, maxSpeedY, rb.velocity.z);
    }
    
    private IEnumerator HandleShoot()
    {
        lr.enabled = true;
        hookedPosition = CrosshairManager.crosshairHit.transform.position;
        lr.SetPosition(1, hookedPosition);

        flyingCondition = true;
        
        yield return new WaitForSeconds(an.attackAnimationHalfDuration);
        
        lr.enabled = false;
    }

    private void HandleFlying()
    {
        if (!flyingCondition) return;
        
        psd.currentMainState = PlayerStateData.PlayerMainState.HookState;
        
        Vector3 move = (hookedPosition - transform.position).normalized;
        rb.AddForce(move * flyingForce, ForceMode.Force);
        
        Vector3 moveDirection = (hookedPosition - transform.position).normalized;
        rb.AddForce(moveDirection * flyingForce); 
        flyingCondition = false;
    }

    private void HandleFlyingMovement()
    {
        if (pim.moveInput.magnitude == 0)
        {
            flyingMovingSpeed = 0f;
            return;
        }
        if (psd.isGettingDamage) return;
        if (psd.currentMainState != PlayerStateData.PlayerMainState.HookState) return;

        if (!isIncreasingSpeed) StartCoroutine(IncreaseMovingSpeed());
        
        Vector3 velocity = plc.movingDirection * flyingMovingSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    private void OnCollisionEnter(Collision col)
    {
        HandleExitHookState();
    }

    private void HandleEnterHookState()
    {
        if (!pim.isHookKeyDown) return;
        aus.Play();
        if (CrosshairManager.isLookingAtHookTarget) StartCoroutine(HandleShoot());
    }

    private void HandleExitHookState()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.HookState)
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
    }
    
    private IEnumerator IncreaseMovingSpeed()
    {
        isIncreasingSpeed = true;

        while (flyingMovingSpeed < maxSpeedXZ)
        {
            flyingMovingSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
        
        if (flyingMovingSpeed > maxSpeedXZ) flyingMovingSpeed = maxSpeedXZ;
        
        isIncreasingSpeed = false;
    }
}