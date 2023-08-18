using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasterEggSceneLoader : MonoBehaviour
{

    bool playerYes;
    //private void Awake()
    //{
    //    ColorAltarManager.OnGameCompleted += ColorAltarManager_OnGameCompleted;
    //}

    //private void ColorAltarManager_OnGameCompleted()
    //{

    //}

    private void Update()
    {
        if (FinalCutSceneManager.isCutSceneOver && playerYes)
            StartCoroutine(Wait());
    }

    IEnumerator Wait() //TODO: Bu Class geçiþi yapacak, Wait fonksiyonu silinecek. 
    {
        yield return new WaitForSeconds(5); 
        DOTween.KillAll();
        ChangeSceneToEasterEgg();
    }
    public void ChangeSceneToEasterEgg()
    {
        SceneManager.LoadScene("Easter Egg");
    }

    public void PlayerYesButton()
    {
        playerYes = true;
        Debug.Log("Button Clicked !! ");
    }
} 
