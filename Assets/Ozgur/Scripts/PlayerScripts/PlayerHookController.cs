using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float flyingForce = 2000f;
    [SerializeField] private float flyingMovingSpeed = 15f;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private AudioSource aus;
    
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerLookingController plc;
    private LineRenderer lr;
    private Rigidbody rb;
    public Transform hookGunTransform;

    private Vector3 hookedPosition;
    private bool flyingCondition;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        plc = GetComponent<PlayerLookingController>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        hookGunTransform = GameObject.Find("PlayerCamera/HookGun/LineOut").transform;
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        
        lr.SetPosition(0, hookGunTransform.position);
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
        
        yield return new WaitForSeconds(animationDuration);
        
        lr.enabled = false;
    }

    private IEnumerator HandleGunAnimation()
    {
        hookGunTransform.parent.DOLocalRotate(new Vector3(30f, -170f, 0f), animationDuration);
        yield return new WaitForSeconds(animationDuration);
        hookGunTransform.parent.DOLocalRotate(new Vector3(15f, -170f, 0f), animationDuration);
    }
    
    private void HandleFlying()
    {
        if (!flyingCondition) return;
        
        Vector3 move = (hookedPosition - transform.position).normalized;
        rb.AddForce(move * flyingForce, ForceMode.Force);

        psd.currentMainState = PlayerStateData.PlayerMainState.HookState;
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
        StartCoroutine(HandleGunAnimation());
        aus.Play();
        if (CrosshairManager.isLookingAtHookTarget) StartCoroutine(HandleShoot());
    }

    private void HandleExitHookState()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.HookState)
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
    }
}