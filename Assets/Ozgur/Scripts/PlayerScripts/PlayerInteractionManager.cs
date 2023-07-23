using Cinemachine;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerInputManager pim;
    private PlayerStateData psd;

    private CinemachineVirtualCamera playerCamera;
    private ShipController sc;

    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
        psd = GetComponent<PlayerStateData>();

        playerCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (!pim.isInteractKeyDown) return;
        
        //TODO: this not the place of this, find a better script
        if (psd.currentMainState == PlayerStateData.PlayerMainState.ShipControllingState)
        {
            sc.DropControl();
            
            playerCamera.enabled = true;
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
            transform.parent = null;
        }
        
        if (!CrosshairManager.isLookingAtShipWheel) return;
        
        
        if (psd.currentMainState == PlayerStateData.PlayerMainState.NormalState && CrosshairManager.isLookingAtShipWheel)
        {
            sc = CrosshairManager.crosshairHit.collider.GetComponentInParent<ShipController>();
            sc.TakeControl();
            
            playerCamera.enabled = false;
            psd.currentMainState = PlayerStateData.PlayerMainState.ShipControllingState;
            transform.parent = sc.transform;
        }
    }
}
