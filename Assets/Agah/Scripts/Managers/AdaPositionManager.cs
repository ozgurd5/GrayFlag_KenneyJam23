using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class AdaPositionManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject adaObject;
    [SerializeField] private Transform adaTargetTr;
    [SerializeField] private float moveTime = 10f;
    
    [Header("Assign - Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CameraManager cameraManager;

    public static event Action OnAda5IsHere; //for music change

    private void Awake()
    {
        PlayerColorEnabler.OnAllColorEnabled += HandleIsland;
    }

    private void HandleIsland()
    {
        StartCoroutine(HandleIslandWithTimeDelay());
    }

    private IEnumerator HandleIslandWithTimeDelay()
    {
        OnAda5IsHere?.Invoke();
        yield return new WaitForSeconds(3f);
        ShowIsland();
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene() 
    {
        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.PauseMenuState;
        
        adaObject.transform.DOMoveY(adaTargetTr.position.y,moveTime);
        cameraManager.SwitchCameras(cameraManager.islandShakeCamera);
        yield return new WaitForSeconds(10);
        
        cameraManager.SwitchCameras(cameraManager.playerCamera);
        yield return new WaitForSeconds(2);
        
        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.NormalState;
    }
    
    private void ShowIsland()
    {
        mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("InvisibleLayer");
    }
    
    private void OnDestroy()
    {
        PlayerColorEnabler.OnAllColorEnabled -= HandleIsland;
    }
}
