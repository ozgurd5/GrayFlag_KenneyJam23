using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    private Coroutine typingCoroutine;

    private bool dialogueInProgress;
    
    public event Action OnDialogueEnd;
    
    void Start()
    {
        textComponent.text = string.Empty;
        dialogueInProgress = false;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopCoroutine(typingCoroutine); // Stop the current typing coroutine
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        if (!dialogueInProgress)
        {
            textComponent.text = string.Empty;
            gameObject.SetActive(true); // Ensure the canvas is active
            dialogueInProgress = true;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            typingCoroutine = StartCoroutine(TypeLine());
        }
        else
        {
            dialogueInProgress = false;
            gameObject.SetActive(false); // Disable the canvas when dialogue ends
            
            OnDialogueEnd?.Invoke();
        }
    }
    public void ResetDialogue()
    {
        dialogueInProgress = false;
        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
    }
}