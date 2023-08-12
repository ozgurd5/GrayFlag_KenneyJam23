using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowExitTheGame : MonoBehaviour
{
    [SerializeField] TMP_Text exitText;
    [SerializeField] int shroomWaitTime = 5;
    [SerializeField] int exitDestroyTime = 3;

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
        yield return new WaitForSeconds(shroomWaitTime);
        ShowExitText();
        Destroy(transform.parent.gameObject, exitDestroyTime);
    }
}
