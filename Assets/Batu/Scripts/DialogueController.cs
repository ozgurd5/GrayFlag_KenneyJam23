using System;
using UnityEditor;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private GameObject thinkingObject;
    [SerializeField] private GameObject playerCanvas;    // Reference to PlayerCanvas
    
    public static bool isOpen = false;
    private Dialogue dialogue;

    private void Awake()
    {
        dialogue = dialogueObject.GetComponentInChildren<Dialogue>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        dialogueObject.SetActive(true);
        thinkingObject.SetActive(true);
        isOpen = true;
        dialogue.StartDialogue();
        
        playerCanvas.SetActive(false);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        dialogueObject.SetActive(false);
        thinkingObject.SetActive(false);
        isOpen = false;
        dialogue.ResetDialogue();
        
        playerCanvas.SetActive(true);
    }
}