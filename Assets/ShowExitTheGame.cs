using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowExitTheGame : MonoBehaviour
{
    [SerializeField] TMP_Text exitText;
    [Tooltip("Time the object waits before appearing for the shroom text to move and disappear")][SerializeField] int shroomTextWaitTime = 5;
    [Tooltip("Time the object waits before disappearing")][SerializeField] int exitDestroyTime = 45;

    private void Awake()
    {
        EasterEggSceneManager.OnMushroomEvent += EasterEggSceneManager_OnMushroomEvent;
    }

    private void EasterEggSceneManager_OnMushroomEvent(int obj)
    {
        StartCoroutine(WaitForShroomText());
    }

    void ShowExitText()
    {
        exitText.text = "Game is Over, hold <b> E </b> to exit.";
    }
    IEnumerator WaitForShroomText()
    {
        yield return new WaitForSeconds(shroomTextWaitTime);
        ShowExitText();
        Destroy(transform.parent.gameObject, exitDestroyTime);
    }
}
