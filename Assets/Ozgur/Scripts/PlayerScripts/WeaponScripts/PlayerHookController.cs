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

    private Camera mainCamera;
    private Camera swordAndGunRenderer;

    [Header("Info - No Touch")]
    [SerializeField] private float flyingMovingSpeed;
    [SerializeField] private bool isIncreasingSpeed;

    public Vector3 realLineOutPosition;
    private Vector3 hookedPosition;
    private bool flyingCondition;
    private IEnumerator increaseMovingSpeed;
    private float collisionTimer;

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

        mainCamera = Camera.main;
        swordAndGunRenderer = mainCamera!.transform.GetChild(0).GetComponent<Camera>();
        
        increaseMovingSpeed = IncreaseMovingSpeed();
    }

    private void LateUpdate()
    {
        //Since the weapons are rendered from another camera (and line renderer is rendered from mainCamera) the lineOutTransform.position is not..
        //..the line out position we want. We need the line renderer to start from the tip of the gun. It actually starts from there but relative to..
        //..the main camera. Main camera and 'sword and gun renderer' camera renders the lineOutTransform in different positions because their FOVs..
        //..are different.

        //So in this line we are converting the position from 'sword and gun renderer' camera to the main camera using the viewport as a common and..
        //..objective point of view.
        realLineOutPosition = mainCamera.ViewportToWorldPoint(swordAndGunRenderer.WorldToViewportPoint(lineOutTransform.position));
        lr.SetPosition(0, realLineOutPosition);

        //NOTE: To make this work smooth, we need to make this script executed after the cinemachine scripts in the script execution order menu from..
        //..project settings. Why? Because this script needs the final position from the cameras. If we don't do that the line stutters.

        //Also we need to calculate realLineOutPosition in LateUpdate because script execution order works like this: All Awake() methods by their..
        //..order, all Start() methods by their order, all Update() methods by their order, all LateUpdate() methods by their order etc. Cinemachine..
        //..scripts works in LateUpdate() so the calculations must work after that.

        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        if (an.isHidden || (PlayerInputManager.Singleton.isHookKeyDown && !an.isAttackAnimationPlaying)) return; //Explanation is down bellow
        EnterHookState();

        //Explanation that is down bellow: Since this script executed at the very end of the frame and the animator is executed before, the variable..
        //..an.isAttackAnimationPlaying becomes always true when this script is executed and the player never enters hook state. To prevent this..
        //..we need to ignore the an.isAttackAnimationPlaying variable when the hook key is down. This is the best logical thing i can make, i know..
        //..it's ugly and hard to understand, that's why we have this wall of comment.
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
        if (CrosshairManager.isLookingAtEnemy || CrosshairManager.isLookingAtEnemyLong) hookedPosition = CrosshairManager.enemyHookPlace.position;
        else hookedPosition = CrosshairManager.crosshairHit.collider.transform.position;

        lr.enabled = true;
        lr.SetPosition(1, hookedPosition);
        
        flyingCondition = true;
        
        yield return new WaitForSeconds(an.wav.attackAnimationHalfDuration);
        
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
        ExitHookState();
    }

    private void EnterHookState()
    {
        if (!pim.isHookKeyDown) return;
        aus.Play();
        if (CrosshairManager.isLookingAtHookTarget) StartCoroutine(HandleShoot());
    }

    private void ExitHookState()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.HookState)
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
    }
    
    private IEnumerator IncreaseMovingSpeed()
    {
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (Vector3.Angle(horizontalVelocity.normalized, transform.forward) < 15 && pim.moveInput.y == 1)
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