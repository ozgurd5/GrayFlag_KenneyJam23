using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinalCutSceneManager : MonoBehaviour
{
    [Tooltip("The change rate of the NORMALIZED position of the cameras.")]
    [SerializeField] float cameraMoveSpeed = 0.15f;
    [Tooltip("The max NORMALIZED lenght of the dolly path camera can go before switching to the next camera." +
        " The result of cameraMoveLimit / cameraMoveSpeed gives the time one camera takes to rotate around one island")]
    [SerializeField] float cameraMoveLimit = 1.5f;

    [SerializeField]CameraManager cameraManager;

    bool isGameComplete;
    bool isCutSceneOver;


    private void Awake()
    {
        ColorAltarManager.OnGameCompleted += ColorAltarManager_OnGameCompleted;
    }

    private void ColorAltarManager_OnGameCompleted()
    {
        Debug.Log("Game Complete Event Recieved " + isGameComplete);
        isGameComplete = true;
        Debug.Log(isGameComplete);
    }

    private void Start()
    {
        isCutSceneOver = false;

        cameraManager.ada1Camera.Priority = 0;
        cameraManager.ada2Camera.Priority = 0;
        cameraManager.ada3Camera.Priority = 0;
        cameraManager.ada4Camera.Priority = 0;
        cameraManager.ada5Camera.Priority = 0;

        ResetCamera(cameraManager.ada1Camera);
        ResetCamera(cameraManager.ada2Camera);
        ResetCamera(cameraManager.ada3Camera);
        ResetCamera(cameraManager.ada4Camera);
        ResetCamera(cameraManager.ada5Camera);
    }

    private void Update()
    {
        if (isGameComplete && !isCutSceneOver)
            PlayFinalCutscene();
    }
    void PlayFinalCutscene()
    {

        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.PauseMenuState;

        MoveCamera(cameraManager.ada5Camera);

        if (HasCameraMoved(cameraManager.ada5Camera))
        MoveCamera(cameraManager.ada4Camera); 

        if (HasCameraMoved(cameraManager.ada4Camera))
            MoveCamera(cameraManager.ada3Camera);

        if (HasCameraMoved(cameraManager.ada3Camera))
            MoveCamera(cameraManager.ada2Camera);

        if (HasCameraMoved(cameraManager.ada2Camera))
            MoveCamera(cameraManager.ada1Camera);

        if (HasCameraMoved(cameraManager.ada1Camera))
            cameraManager.SwitchCameras(cameraManager.playerCamera);

        StartCoroutine(WaitFor(2));
        PlayerStateData.Singleton.currentMainState = PlayerStateData.PlayerMainState.NormalState;

        isCutSceneOver = true;
    }

    IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void MoveCamera(CinemachineVirtualCamera cameraToMove)
    {
            cameraManager.SwitchCameras(cameraToMove); 
            cameraToMove.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += cameraMoveSpeed*Time.deltaTime; 
    }
    bool HasCameraMoved(CinemachineVirtualCamera cameraToMove)
    {
        if (cameraToMove.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition >= cameraMoveLimit)
        {
            return true;
        }
        else
        {
            return false;
        }   
    }
    void ResetCamera(CinemachineVirtualCamera cameraToReset)
    {
        cameraToReset.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
    }

    private void OnDestroy()
    {
        ColorAltarManager.OnGameCompleted -= ColorAltarManager_OnGameCompleted;
    }
}