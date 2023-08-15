using UnityEngine;

public class InteractionTextManager : MonoBehaviour
{
    private static int lastID;
    
    [Header("SELECT IF IT'S CHEST")]
    [SerializeField] private bool isChest;
    
    [Header("Info - No Touch")]
    [SerializeField] private bool isOpen;
    public int id;
    
    private GameObject interactionTextCanvas;
    private ChestManager chestManager;

    private void Awake()
    {
        id = lastID;
        lastID++;
        
        interactionTextCanvas = transform.Find("InteractionTextCanvas").gameObject;
        if (isChest) chestManager = GetComponent<ChestManager>();
    }

    public void OpenInteractionText()
    {
        if (isOpen) return;
        if (isChest && chestManager.isChestOpened) return;
        
        isOpen = true;
        interactionTextCanvas.SetActive(true);
    }

    public void CloseInteractionText()
    {
        if (!isOpen) return;
        
        isOpen = false;
        interactionTextCanvas.SetActive(false);
    }
}
