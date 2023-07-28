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
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
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
        currentRotation.x -= pim.lookInput.y;
        currentRotation.y += pim.lookInput.x;
        
        currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(currentRotation);
        
        //currentRotation.x = 0f; //very stupid that it broke the camera's x rotation
        transform.localRotation = Quaternion.Euler(new Vector3(0f, currentRotation.y, 0f));
    }
    
    private void CalculateMovingDirection()
    {
        movingDirection = transform.right * pim.moveInput.x + transform.forward * pim.moveInput.y;
        //movingDirection.y = 0f;
    }
}
