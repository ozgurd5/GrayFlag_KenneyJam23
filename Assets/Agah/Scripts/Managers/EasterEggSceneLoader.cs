using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasterEggSceneLoader : MonoBehaviour
{
    bool playerYes;
    bool playerNo;

    bool isCutsceneOver = false;

    [SerializeField] float fadeTime;

    [SerializeField] CanvasGroup canvasGroup;



    private void Awake()
    {
        FinalCutSceneManager.IsCutSceneOver += FinalCutSceneManager_IsCutSceneOver;
    }

    private void Start()
    {
        canvasGroup.interactable = false;
    }

    private void FinalCutSceneManager_IsCutSceneOver()
    {
        StartCoroutine(Fade(canvasGroup, true));
        isCutsceneOver =true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (isCutsceneOver)
        {
            if(playerYes) StartCoroutine(Wait());
        }

        if (playerNo)
            SceneManager.LoadScene("Menu");
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Z))
            PlayerYesButton();
#endif
    }
    IEnumerator Wait() //TODO: Bu Class geçiþi yapacak, Wait fonksiyonu silinecek. 
    {
        yield return new WaitForSeconds(0); 
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
    public void PlayerNoButton()
    {
        playerNo = true;
        Debug.Log("No Button Clicked!!");
    }

    private IEnumerator Fade(CanvasGroup canvas, bool isFadeIn)
    {
        Debug.Log("Debug");
        canvas.interactable=true;
        float timePassed = 0f;
        float increaseSpeed = 1 / fadeTime;

        while (timePassed <= fadeTime)
        {
            if (isFadeIn) canvas.alpha += increaseSpeed * Time.deltaTime;
            else canvas.alpha -= increaseSpeed * Time.deltaTime;

            timePassed += Time.deltaTime;
            yield return null;
        }

        if (isFadeIn) canvas.alpha = 1f;
        else canvas.alpha = 0f;
    }
} 
