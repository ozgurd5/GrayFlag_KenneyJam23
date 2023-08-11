using UnityEngine;

public class PlayerLookingController : MonoBehaviour
{
    public Vector3 movingDirection { get; private set; }
    
    private PlayerStateData psd;
    private Transform cameraTransform;
    
    private Vector3 currentRotation;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        cameraTransform = GameObject.Find("PlayerCamera").transform;
    }

    private void Update()
    {
        if (psd.currentMainState is not (PlayerStateData.PlayerMainState.NormalState or PlayerStateData.PlayerMainState.HookState)) return;
        
        HandleLooking();
        CalculateMovingDirection();
    }

    private void HandleLooking()
    {
        currentRotation.x -= PlayerInputManager.Singleton.lookInput.y;
        currentRotation.y += PlayerInputManager.Singleton.lookInput.x;
        
        currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation);
        
        transform.localRotation = Quaternion.Euler(new Vector3(0f, currentRotation.y, 0f));
    }
    
    private void CalculateMovingDirection()
    {
        Quaternion parentRotation = Quaternion.Euler(0,0,0);
        if (transform.parent != null) parentRotation = Quaternion.Euler(0, -1 * transform.parent.eulerAngles.y, 0);

        Vector3 newForward = parentRotation * transform.forward;
        Vector3 newRight = parentRotation * transform.right;
        
        movingDirection = newRight * PlayerInputManager.Singleton.moveInput.x + newForward * PlayerInputManager.Singleton.moveInput.y;
    }
}