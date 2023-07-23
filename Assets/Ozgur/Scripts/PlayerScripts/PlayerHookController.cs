using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float releaseForce = 200f;
    
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private LineRenderer lr;
    private Rigidbody rb;
    private Transform hookGunTransform;

    private Vector3 hookedPosition;
    private Vector3 movingDirection;
    private bool releasingCondition;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        hookGunTransform = GameObject.Find("PlayerCamera/HookGun").transform;
    }

    private void Update()
    {
        lr.SetPosition(0, hookGunTransform.position);
        
        HandleShooting();
        HandleReleasingCondition();
        HandleExitHookSubState();
    }

    private void FixedUpdate()
    {
        HandleReleasing();
    }

    private void HandleShooting()
    {
        if (!pim.isHookKeyDown) return;
        if (!CrosshairManager.isLookingAtHookTarget) return;

        psd.isHooked = true;
        lr.enabled = true;
        
        hookedPosition = CrosshairManager.crosshairHit.transform.position;
        lr.SetPosition(1, hookedPosition);
    }

    private void HandleReleasingCondition()
    {
        if (pim.isHookKeyUp && psd.isHooked) releasingCondition = true;
    }

    private void HandleReleasing()
    {
        if (!releasingCondition) return;
        
        movingDirection = (hookedPosition - transform.position).normalized;
        rb.AddForce(movingDirection * releaseForce, ForceMode.Force);
        
        psd.isHookFlying = true;
        psd.isHooked = false;
        
        lr.enabled = false;
        releasingCondition = false;
    }

    private void HandleExitHookSubState()
    {
        if (!psd.isHookFlying) return;
        if (pim.moveInput.magnitude != 0f) psd.isHookFlying = false;
    }
}