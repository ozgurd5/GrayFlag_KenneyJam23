using System;
using UnityEditor;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject dialogueObject;
    [SerializeField] private GameObject thinkingObject;
    
    private bool isOpen = true;
    private Dialogue dialogue;
    //private Animator animator;

    private void Start()
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
        //animator.SetBool("isTalking", true);
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player"))
        {
            dialogueObject.SetActive(false);
        }
        
        dialogueObject.SetActive(false);
        thinkingObject.SetActive(false);
        isOpen = false;
        dialogue.ResetDialogue();
        //animator.SetBool("isTalking", false);
    }
}