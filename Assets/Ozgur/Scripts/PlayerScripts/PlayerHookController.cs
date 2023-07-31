using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float flyingForce = 2000f;
    [SerializeField] private float flyingMovingSpeed = 10f;
    [SerializeField] private AudioSource aus;
    
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerLookingController plc;
    private PlayerHookGunAnimationManager an;
    private LineRenderer lr;
    private Rigidbody rb;
    private Transform lineOutTransform;
    private Transform hookGunTransform;

    private Vector3 hookedPosition;
    private bool flyingCondition;

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
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        
        lr.SetPosition(0, lineOutTransform.position);
        HandleEnterHookState();
    }

    private void FixedUpdate()
    {
        HandleFlying();
        HandleFlyingMovement();
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
        if (psd.currentMainState != PlayerStateData.PlayerMainState.HookState) return;
        
        if (plc.movingDirection.x == 0 || plc.movingDirection.z == 0) return;
        
        Vector3 moving = plc.movingDirection * flyingMovingSpeed;
        rb.velocity = new Vector3(moving.x, rb.velocity.y, moving.z);
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
}