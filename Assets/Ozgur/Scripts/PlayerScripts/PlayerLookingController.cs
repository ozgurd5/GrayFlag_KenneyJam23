using UnityEngine;

public class PlayerLookingController : MonoBehaviour
{
    public Vector3 movingDirection { get; private set; }
    
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Transform cameraTransform;
    
    private Vector3 currentRotation;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        pim = PlayerInputManager.Singleton;
        cameraTransform = GameObject.Find("PlayerCamera").transform;
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState
            or PlayerStateData.PlayerMainState.DialogueState)) return;
        
        HandleLooking();
        CalculateMovingDirection();
    }

    private void HandleLooking()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.DialogueState) return;
        
        currentRotation.x -= pim.lookInput.y;
        currentRotation.y += pim.lookInput.x;
        
        currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation);

        Vector3 rotationEuler = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(new Vector3(rotationEuler.x, currentRotation.y, rotationEuler.z));
    }
    
    private void CalculateMovingDirection()
    {
        Quaternion parentRotation = Quaternion.Euler(0,0,0);
        if (transform.parent != null) parentRotation = Quaternion.Euler(0, -1 * transform.parent.eulerAngles.y, 0);

        Vector3 newForward = parentRotation * transform.forward;
        Vector3 newRight = parentRotation * transform.right;
        
        movingDirection = newRight * pim.moveInput.x + newForward * pim.moveInput.y;
    }
}