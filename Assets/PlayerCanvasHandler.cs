using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasHandler : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasToHandle;

    private void Awake()
    {
        ColorAltarManager.OnGameCompleted += ColorAltarManager_OnGameCompleted;
    }

    private void ColorAltarManager_OnGameCompleted()
    {
        canvasToHandle.alpha = 0;
    }
}

