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

    private void Awake()
    {
        PlayerColorEnabler.OnAllColorEnabled += OnAllColorEnabled;
    }

    private void Start()
    {
        Debug.Log(PlayerColorEnabler.IsAllColorEnabled());
        StartCoroutine(Wait()); 
    }

    //Bu Kodda Yapýlacaklar TODO:
    //Wait() ENUMATORÜ SÝLÝNECEK, CUTSCENE YAPILIP REFERANSLANIP PlayCutscene() ÝÇÝNDE REFERANSLANICAK,
    //HandleIsland() FONKSÝYONU OnAllColorEnabled() ÝÇÝNE KONACAK.


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        HandleIsland();
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
