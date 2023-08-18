using System;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private GameObject thinkingObject;
    [SerializeField] private GameObject playerCanvas;    // Reference to PlayerCanvas
    
    public static bool isOpen;
    public static event Action OnPlayerExitNpcCollider;
    private Dialogue dialogue;
    
    //bruh
    private Canvas dialogueCanvas;

    private void Awake()
    {
        dialogue = dialogueObject.GetComponentInChildren<Dialogue>();

        dialogue.OnDialogueEnd += CloseDialogue;
        OnPlayerExitNpcCollider += CloseDialogue;
        MarketManager.OnMarketCanvasClosed += CloseDialogue;
        
        //bruh
        dialogueCanvas = dialogueObject.GetComponent<Canvas>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        dialogueCanvas.enabled = true;
        thinkingObject.SetActive(true);
        isOpen = true;
        dialogue.StartDialogue();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.DialogueState;
        
        playerCanvas.SetActive(false);
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player")) OnPlayerExitNpcCollider?.Invoke();
    }

    private void CloseDialogue()
    {
        dialogueCanvas.enabled = false;
        thinkingObject.SetActive(false);
        isOpen = false;
        dialogue.ResetDialogue();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.NormalState;
        
        playerCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        dialogue.OnDialogueEnd -= CloseDialogue;
        OnPlayerExitNpcCollider += CloseDialogue;
        MarketManager.OnMarketCanvasClosed -= CloseDialogue;
    }
}