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
    
    private InteractionTextManager itm;
    private InteractionTextManager previousItm;

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
        HandleInteractionText();
        
        if (!pim.isInteractKeyDown) return;
        
        HandleShipInteraction();
        HandleChestInteraction();
        HandleMushroomInteraction();
    }
    
    private void HandleInteractionText()
    {
        if (CrosshairManager.isLookingAtChest || CrosshairManager.isLookingAtMushroom || CrosshairManager.isLookingAtShipWheel)
        {
            if (CrosshairManager.isLookingAtChest)
                itm = CrosshairManager.crosshairHit.transform.root.GetComponent<InteractionTextManager>();
            else
                itm = CrosshairManager.crosshairHit.collider.GetComponent<InteractionTextManager>();
            
            itm.OpenInteractionText();
        }
        
        else if (itm)
        {
            itm.CloseInteractionText();
        }
        
        //When we look another interactable while looking an interactable, without looking to any other object, interaction..
        //..text of the first interactable stays open because we never triggered the else if above because we have never..
        //..looked anywhere other than an interactable. If statement above stayed true while we look from one interactable..
        //..to another. Anyways, if statement below this line and holding the data of the previousItm fix this issue
        if (itm && previousItm)
        {
            if (itm.id != previousItm.id)
            {
                previousItm.CloseInteractionText();
            }
        }

        previousItm = itm;
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
        if (!CrosshairManager.isLookingAtChest || cm.isChestOpened) return;
        
        cm = CrosshairManager.crosshairHit.transform.root.GetComponent<ChestManager>(); //lid can be selected
        itm.CloseInteractionText();
        cm.OpenChest();
    }

    private void HandleMushroomInteraction()
    {
        if (!CrosshairManager.isLookingAtMushroom) return;
        mm = CrosshairManager.crosshairHit.collider.GetComponent<MushroomManager>();
        if (!mm.isCollected) mm.CollectMushroom();
    }
}
