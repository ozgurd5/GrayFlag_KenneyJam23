using System.Collections;
using TMPro;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI[] textsToToggle; // Array of TMP objects to open and close
    public float openDuration = 13.4f;        // Time each text stays open
    public float switchInterval = 2f;       // Time between switching on and off

    private int currentIndex = 0;
    private Coroutine toggleCoroutine;

    private void Start()
    {
        // Deactivate all texts at the beginning
        foreach (TextMeshProUGUI text in textsToToggle)
        {
            text.gameObject.SetActive(false);
        }

        // Start the toggle coroutine
        toggleCoroutine = StartCoroutine(ToggleTexts());
    }

    private IEnumerator ToggleTexts()
    {
        while (true)
        {
            // Open the current text
            textsToToggle[currentIndex].gameObject.SetActive(true);
            yield return new WaitForSeconds(openDuration);

            // Close the current text
            textsToToggle[currentIndex].gameObject.SetActive(false);
            currentIndex = (currentIndex + 1) % textsToToggle.Length;

            yield return new WaitForSeconds(switchInterval);
        }
    }
}