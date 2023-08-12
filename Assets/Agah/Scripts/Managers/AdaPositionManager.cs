using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaPositionManager : MonoBehaviour
{
    [SerializeField] GameObject adaParent;
    [SerializeField] Transform adaTargetTr;

    Transform ada5Obj;

    private void Awake()
    {
        ada5Obj = adaParent.transform.GetChild(0);
        PlayerColorEnabler.OnAllColorEnabled += OnAllColorEnabled;
    }

    private void OnAllColorEnabled()
    {
        HandleAda();
    }

    public void ShowIsland()
    {
        ada5Obj.gameObject.SetActive(true);
    }

    public void MoveIsland()
    {
        //Özgür: DoTween ile yap?
        Debug.Log("MoveIsland();");
    }
    public void PlayCutscene() 
    {
        Debug.Log("PlayCutscene()");
    }

    public void HandleAda() 
    {
        if(PlayerColorEnabler.IsAllColorEnabled())
        { 
            ShowIsland();
            MoveIsland();
            PlayCutscene();
        }
    }

}
