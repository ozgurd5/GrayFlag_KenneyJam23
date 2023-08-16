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

    //Bu Kodda Yapýlacaklar TODO:
    //Wait() ENUMATORÜ SÝLÝNECEK, CUTSCENE YAPILIP REFERANSLANIP PlayCutscene() ÝÇÝNDE REFERANSLANICAK,
    //HandleIsland() FONKSÝYONU OnAllColorEnabled() ÝÇÝNE KONACAK.


    IEnumerator WaitForHandleIsland() //TODO Bu enumerator silinecek ve bunun yerine event dinlenerek çalýþýlacak.
    {
        yield return new WaitForSeconds(3);
        HandleIsland();
    }

    IEnumerator WaitForIslandAnim()
    {
        yield return new WaitForSeconds(11);
        cameraManager.SwitchCameras(cameraManager.playerCamera);
        yield return new WaitForSeconds(2);
        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.NormalState;
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
        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.PauseMenuState;
        cameraManager.SwitchCameras(cameraManager.islandCamera); //2 saniye sürüyor
        StartCoroutine(WaitForIslandAnim()); // 11 saniye sürüyor, adanýn kýmýldamasý 10 saniye sürüyor
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
