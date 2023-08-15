using System;
using UnityEditor;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private GameObject thinkingObject;
    [SerializeField] private GameObject playerCanvas;    // Reference to PlayerCanvas

    //[SerializeField] private GameObject sword;           // Reference to the sword GameObject
    //[SerializeField] private GameObject hookGun;         // Reference to the hook gun GameObject
    
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

        // Deactivate sword and hook gun
        //sword.SetActive(false);
        //hookGun.SetActive(false);
        
        playerCanvas.SetActive(false);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        dialogueObject.SetActive(false);
        thinkingObject.SetActive(false);
        isOpen = false;
        dialogue.ResetDialogue();

        // Activate sword and hook gun
        //sword.SetActive(true);
        //hookGun.SetActive(true);
        
        playerCanvas.SetActive(true);
    }
}