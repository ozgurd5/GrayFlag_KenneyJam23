using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerHookController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float releaseForce = 200f;
    [SerializeField] private float animationDuration = 0.2f;
    
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
        HandleExitHookSubState();
        
        if (!pim.isHookKeyDown) return;
        if (!CrosshairManager.isLookingAtHookTarget) return;
        StartCoroutine(HandleShoot());
    }

    private void FixedUpdate()
    {
        HandleFlying();
    }
    
    private IEnumerator HandleShoot()
    {
        hookGunTransform.parent.DOLocalRotate(new Vector3(30f, -170f, 0f), animationDuration);
        
        lr.enabled = true;
        hookedPosition = CrosshairManager.crosshairHit.transform.position;
        lr.SetPosition(1, hookedPosition);

        flyingCondition = true;
        
        yield return new WaitForSeconds(animationDuration);
        
        hookGunTransform.parent.DOLocalRotate(new Vector3(15f, -170f, 0f), animationDuration);
        lr.enabled = false;
    }
    
    private void HandleFlying()
    {
        if (!flyingCondition) return;
        
        movingDirection = (hookedPosition - transform.position).normalized;
        rb.AddForce(movingDirection * releaseForce, ForceMode.Force);
        
        psd.isHookFlying = true;
        flyingCondition = false;
    }

    private void HandleExitHookSubState()
    {
        if (!psd.isHookFlying) return;
        if (pim.moveInput.magnitude != 0f)  psd.isHookFlying = false;
    }
}