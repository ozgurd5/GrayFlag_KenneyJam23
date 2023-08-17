using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorTintEffect : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button button;
    [SerializeField] private float transitionTime = 0.1f;
    [SerializeField] private Color pressedGrayColor;
    [SerializeField] private Color pressedRedColor;
    [SerializeField] private List<Image> images;

    private bool isColorTintWorking;
    private Color pressedColor;
    
    private void Awake()
    {
        button.onClick.AddListener(() => StartCoroutine(PlayColorTint()));

        PlayerColorEnabler.OnRedColorEnabled += ChangePressedColor;
        pressedColor = pressedGrayColor;
    }

    private void ChangePressedColor()
    {
        pressedColor = pressedRedColor;
    }
    
    private IEnumerator PlayColorTint()
    {
        if (isColorTintWorking) yield break;

        foreach (Image item in images)
        {
            item.color = pressedColor;
        }
        
        isColorTintWorking = true;

        yield return new WaitForSeconds(transitionTime);
        
        foreach (Image item in images)
        {
            item.color = Color.white;
        }

        isColorTintWorking = false;
    }

    private void OnDestroy()
    {
        PlayerColorEnabler.OnRedColorEnabled -= ChangePressedColor;
    }
}
