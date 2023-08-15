using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AdaPositionManager : MonoBehaviour
{
    [SerializeField] GameObject AdaObject;
    [SerializeField] Transform adaTargetTr;
    [SerializeField] float moveTime;
    [Header("Assing Main Camera Here")][SerializeField] Camera mainCamera;
    [SerializeField] CameraManager cameraManager;

    private void Awake()
    {
        PlayerColorEnabler.OnAllColorEnabled += OnAllColorEnabled;
    }

    private void Start()
    {
        Debug.Log(PlayerColorEnabler.IsAllColorEnabled());
        StartCoroutine(WaitForHandleIsland()); 
    }

    //Bu Kodda Yap�lacaklar TODO:
    //Wait() ENUMATOR� S�L�NECEK, CUTSCENE YAPILIP REFERANSLANIP PlayCutscene() ���NDE REFERANSLANICAK,
    //HandleIsland() FONKS�YONU OnAllColorEnabled() ���NE KONACAK.


    IEnumerator WaitForHandleIsland() //TODO Bu enumerator silinecek ve bunun yerine event dinlenerek �al���lacak.
    {
        yield return new WaitForSeconds(3);
        HandleIsland();
    }

    IEnumerator WaitForIslandAnim()
    {
        yield return new WaitForSeconds(11);
        cameraManager.SwitchCameras(cameraManager.playerCamera);
    }
    private void OnAllColorEnabled()
    {
        //HandleIsland();
        Debug.Log("OnAllColorEnabled()"); // bu da silinecek.
    }

    public void ShowIsland()
    {
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("InvisibleLayer");
    }

    public void MoveIsland()
    {
        AdaObject.transform.DOMoveY(adaTargetTr.position.y,moveTime);
        Debug.Log("MoveIsland();");
    }

    public void PlayCutscene() 
    {
        Debug.Log("PlayCutscene()");
        cameraManager.SwitchCameras(cameraManager.islandCamera);
        StartCoroutine(WaitForIslandAnim());
        
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
