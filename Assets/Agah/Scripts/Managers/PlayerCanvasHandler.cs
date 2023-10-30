using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasHandler : MonoBehaviour
{
    [SerializeField] Canvas[] canvasToHandle;

    private void Awake()
    {
        ColorAltarManager.OnGameCompleted += ColorAltarManager_OnGameCompleted;
    }

    private void ColorAltarManager_OnGameCompleted()
    {
        foreach (var canvas in canvasToHandle) 
        { 
          canvas.gameObject.SetActive(false);
        }
    }
}

