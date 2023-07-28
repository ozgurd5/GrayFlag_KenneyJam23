using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float flyingForce = 2000f;
    [SerializeField] private float flyingMovementForce = 2000f;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private AudioSource aus;
    
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private LineRenderer lr;
    private Rigidbody rb;
    public Transform hookGunTransform;

    private Vector3 hookedPosition;
    private Vector3 movingDirection;
    private bool flyingCondition;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        hookGunTransform = GameObject.Find("PlayerCamera/HookGun/LineOut").transform;
    }

    private void Update()
    {
        lr.SetPosition(0, hookGunTransform.position);

        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        if (!pim.isHookKeyDown) return;

        StartCoroutine(HandleGunAnimation());
        aus.Play();
        if (CrosshairManager.isLookingAtHookTarget) StartCoroutine(HandleShoot());
    }

    private void FixedUpdate()
    {
        HandleFlying();
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
        
        movingDirection = (hookedPosition - transform.position).normalized;
        rb.AddForce(movingDirection * flyingForce, ForceMode.Force);

        psd.currentMainState = PlayerStateData.PlayerMainState.HookState;
        flyingCondition = false;
    }

    private void OnCollisionEnter(Collision col)
    {
        HandleExitHookSubState();
    }

    private void HandleExitHookSubState()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.HookState)
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
    }
}