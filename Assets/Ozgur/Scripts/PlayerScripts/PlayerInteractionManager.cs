using Cinemachine;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerInputManager pim;
    private PlayerStateData psd;
    private Rigidbody rb;

    private CinemachineVirtualCamera playerCamera;
    private GameObject sword;
    private GameObject hookGun;
    
    private ShipController sc;

    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
        psd = GetComponent<PlayerStateData>();
        rb = GetComponent<Rigidbody>();

        playerCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        sword = playerCamera.transform.Find("Sword").gameObject;
        hookGun = playerCamera.transform.Find("HookGun").gameObject;
    }

    private void Update()
    {
        if (!pim.isInteractKeyDown) return;
        
        //TODO: this not the place of this, find a better script
        if (psd.currentMainState == PlayerStateData.PlayerMainState.ShipControllingState)
        {
            sc.DropControl();
            
            playerCamera.enabled = true;
            sword.SetActive(true);
            hookGun.SetActive(true);
            
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            //transform.parent = null;
        }
        
        if (!CrosshairManager.isLookingAtShipWheel) return;
        
        
        if (psd.currentMainState == PlayerStateData.PlayerMainState.NormalState && CrosshairManager.isLookingAtShipWheel)
        {
            sc = CrosshairManager.crosshairHit.collider.GetComponentInParent<ShipController>();
            sc.TakeControl();
            
            playerCamera.enabled = false;
            sword.SetActive(false);
            hookGun.SetActive(false);
            
            psd.currentMainState = PlayerStateData.PlayerMainState.ShipControllingState;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            //transform.parent = sc.transform;
        }
    }
}
