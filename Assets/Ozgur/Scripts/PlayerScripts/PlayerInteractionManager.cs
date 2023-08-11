using Cinemachine;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Rigidbody rb;

    private CinemachineVirtualCamera playerCamera;
    private CameraFollow cf;
    private GameObject sword;
    private GameObject hookGun;
    
    private ShipController sc;
    private ChestManager cm;
    private MushroomManager mm;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        pim = PlayerInputManager.Singleton;
        rb = GetComponent<Rigidbody>();

        playerCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        cf = playerCamera.GetComponent<CameraFollow>();
        sword = playerCamera.transform.Find("Sword").gameObject;
        hookGun = playerCamera.transform.Find("HookGun").gameObject;
    }

    private void Update()
    {
        if (!pim.isInteractKeyDown) return;
        
        HandleShipInteraction();
        HandleChestInteraction();
        HandleMushroomInteraction();
    }

    private void HandleShipInteraction()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.ShipControllingState)
        {
            sc.DropControl();
            
            playerCamera.enabled = true;
            cf.canLookAt = false;
            sword.SetActive(true);
            hookGun.SetActive(true);
            
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
        if (!CrosshairManager.isLookingAtShipWheel) return;

        if (psd.currentMainState == PlayerStateData.PlayerMainState.NormalState && CrosshairManager.isLookingAtShipWheel)
        {
            sc = CrosshairManager.crosshairHit.collider.GetComponentInParent<ShipController>();
            sc.TakeControl();
            
            playerCamera.enabled = false;
            cf.canLookAt = true;
            sword.SetActive(false);
            hookGun.SetActive(false);
            
            psd.currentMainState = PlayerStateData.PlayerMainState.ShipControllingState;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void HandleChestInteraction()
    {
        if (!CrosshairManager.isLookingAtChest) return;
        cm = CrosshairManager.crosshairHit.collider.GetComponent<ChestManager>();
        if (!cm) cm = CrosshairManager.crosshairHit.transform.parent.GetComponent<ChestManager>(); //if lid is selected
        if (!cm.isChestOpened) cm.OpenChest();
    }

    private void HandleMushroomInteraction()
    {
        if (!CrosshairManager.isLookingAtMushroom) return;
        mm = CrosshairManager.crosshairHit.collider.GetComponent<MushroomManager>();
        if (!mm.isCollected) mm.CollectMushroom();
    }
}
