using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AdaPositionManager : MonoBehaviour
{
    [SerializeField] GameObject adaParent;
    [SerializeField] Transform adaTargetTr;
    [SerializeField] float moveTime;
    [Header("Assing Main Camera Here")][SerializeField] Camera mainCamera;
    LayerMask adaLayerMask;

    private void Awake()
    {
        PlayerColorEnabler.OnAllColorEnabled += OnAllColorEnabled;
    }

    private void Start()
    {
        Debug.Log(PlayerColorEnabler.IsAllColorEnabled());
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        HandleIsland();
    }
    private void OnAllColorEnabled()
    {
        //HandleIsland();
        Debug.Log("OnAllColorEnabled()");
    }

    public void ShowIsland()
    {
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("InvisibleLayer");
    }

    public void MoveIsland()
    {
        transform.DOMoveY(adaTargetTr.position.y,moveTime);
        Debug.Log("MoveIsland();");
    }

    public void PlayCutscene() 
    {
        Debug.Log("PlayCutscene()");
    }

    public void HandleIsland() 
    {
        ShowIsland();
        MoveIsland();
        PlayCutscene();
    }
    private void OnDestroy()
    {
        PlayerColorEnabler.OnAllColorEnabled -= OnAllColorEnabled;
    }

}
