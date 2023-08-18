using Cinemachine;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Rigidbody rb;

    private ShipController sc;
    private CinemachineVirtualCamera playerCamera;
    private CameraFollow cf;
    private GameObject sword;
    private GameObject hookGun;
    
    private ChestManager cm;
    private MushroomManager mm;
    private ColorAltarManager cam;

    private bool isLookingAtInteractable;
    
    private InteractionTextManager itm;
    private InteractionTextManager previousItm;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;
        pim = PlayerInputManager.Singleton;
        rb = GetComponent<Rigidbody>();

        sc = GameObject.Find("ShipParent").GetComponent<ShipController>();
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
        HandleColorAltarInteraction();
    }
    
    private void HandleInteractionText()
    {
        isLookingAtInteractable = CrosshairManager.isLookingAtChest || CrosshairManager.isLookingAtMushroom || CrosshairManager.isLookingAtShipWheel
                                  || CrosshairManager.isLookingAtColorAltar; //ADD NEW HERE 5 - First 4 is in CrosshairManager.cs
        
        if (isLookingAtInteractable)
        {
            //We may look at chest lid and it doesn't have any InteractionTextCanvas
            if (CrosshairManager.isLookingAtChest)
            {
                itm = CrosshairManager.crosshairHit.collider.transform.Find("InteractionTextCanvas")?.GetComponent<InteractionTextManager>();
                if (!itm) itm = CrosshairManager.crosshairHit.collider.transform.parent.Find("InteractionTextCanvas").GetComponent<InteractionTextManager>();
            }
            
            else
                itm = CrosshairManager.crosshairHit.collider.transform.Find("InteractionTextCanvas").GetComponent<InteractionTextManager>();
            
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
        
        //We may look at chest lid
        cm = CrosshairManager.crosshairHit.collider.transform.GetComponent<ChestManager>();
        if (!cm) cm = CrosshairManager.crosshairHit.collider.transform.parent.GetComponent<ChestManager>();
        
        if (cm.isChestOpened) return;
        
        itm.CloseInteractionText();
        cm.OpenChest();
    }

    private void HandleMushroomInteraction()
    {
        if (!CrosshairManager.isLookingAtMushroom) return;
        
        mm = CrosshairManager.crosshairHit.collider.GetComponent<MushroomManager>();
        if (!mm.isCollected) mm.CollectMushroom();
    }

    private void HandleColorAltarInteraction()
    {
        if (!CrosshairManager.isLookingAtColorAltar) return;
        
        cam = CrosshairManager.crosshairHit.collider.GetComponent<ColorAltarManager>();
        if (cam.isActivated) return;
        
        itm.CloseInteractionText();
        cam.EnableAltar();
    }
}
