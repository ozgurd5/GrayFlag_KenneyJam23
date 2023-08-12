using System.Collections;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float flyingForce = 3000f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float maxSpeedXZ = 50f;
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
    [SerializeField] private bool isIncreasingSpeed;
    private Vector3 hookedPosition;
    private bool flyingCondition;
    private IEnumerator increaseMovingSpeed;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        pim = PlayerInputManager.Singleton;
        plc = GetComponent<PlayerLookingController>();
        an = GetComponent<PlayerHookGunAnimationManager>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        hookGunTransform = GameObject.Find("PlayerCamera/HookGun").transform;
        lineOutTransform = hookGunTransform.GetChild(0);
        
        increaseMovingSpeed = IncreaseMovingSpeed();
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        if (an.isHidden) return;
        
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
        lr.SetPosition(1, hookedPosition);

        if (CrosshairManager.isLookingAtEnemy || CrosshairManager.isLookingAtEnemyLong) hookedPosition = CrosshairManager.enemyHookPlace.position;
        else hookedPosition = CrosshairManager.crosshairHit.transform.position;

        flyingCondition = true;
        
        yield return new WaitForSeconds(an.attackAnimationHalfDuration);
        
        lr.enabled = false;
    }

    private void HandleFlying()
    {
        if (!flyingCondition) return;
        
        psd.currentMainState = PlayerStateData.PlayerMainState.HookState;
        
        Vector3 moveDirection = (hookedPosition - transform.position).normalized;
        rb.AddForce(moveDirection * flyingForce);

        flyingCondition = false;
    }

    private void HandleFlyingMovement()
    {
        if (flyingCondition) return;
        
        if (pim.moveInput.magnitude == 0 || psd.isGettingDamage || psd.currentMainState != PlayerStateData.PlayerMainState.HookState)
        {
            StopCoroutine(increaseMovingSpeed);
            isIncreasingSpeed = false;
            flyingMovingSpeed = 0f;
            return;
        }

        if (!isIncreasingSpeed)
        {
            increaseMovingSpeed = IncreaseMovingSpeed();
            StartCoroutine(increaseMovingSpeed);
        }
        
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
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (Vector3.Angle(horizontalVelocity.normalized, transform.forward) < 44 && pim.moveInput.y == 1)
        {
            flyingMovingSpeed = maxSpeedXZ;
            yield break;
        }
        
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