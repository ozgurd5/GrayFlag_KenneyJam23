using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FinalCutSceneManager : MonoBehaviour
{
    [Tooltip("The change rate of the NORMALIZED position of the cameras.")]
    [SerializeField] float cameraMoveSpeed = 0.15f;

    [SerializeField]CameraManager cameraManager;

    private void Awake()
    {
        PlayerColorEnabler.OnAllColorEnabled += PlayerColorEnabler_OnAllColorEnabled;
    }

    private void PlayerColorEnabler_OnAllColorEnabled()
    {
        return;
    }
    private void Start()
    {
        cameraManager.ada1Camera.Priority = 0;
        cameraManager.ada2Camera.Priority = 0;
        cameraManager.ada3Camera.Priority = 0;
        cameraManager.ada4Camera.Priority = 0;
        cameraManager.ada5Camera.Priority = 0;
    }

    private void Update()
    {
        PlayFinalCutscene();
    }
    void PlayFinalCutscene()
    {
        
        MoveCamera(cameraManager.ada5Camera);

        if(HasCameraMoved(cameraManager.ada5Camera));
         MoveCamera(cameraManager.ada4Camera); //Kameranýn prio su sürekli yüksek kalýyor.
        //Fonksiyonun burdan sonrasnýn çalýþtýðýný görmedim.
        if(HasCameraMoved(cameraManager.ada4Camera))
        MoveCamera(cameraManager.ada3Camera);

        if(HasCameraMoved(cameraManager.ada3Camera))
        MoveCamera(cameraManager.ada2Camera);

        if(HasCameraMoved(cameraManager.ada2Camera))
        MoveCamera(cameraManager.ada1Camera);

        return;
    }

    void MoveCamera(CinemachineVirtualCamera cameraToMove)
    {
        if(!HasCameraMoved(cameraToMove))
        {
            cameraManager.SwitchCameras(cameraToMove); 
            cameraToMove.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += cameraMoveSpeed*Time.deltaTime; 
        }
        else cameraToMove.Priority = 0;
    }
    bool HasCameraMoved(CinemachineVirtualCamera cameraToMove)
    {
        if (cameraToMove.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition >= 1)
        {
            cameraToMove.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
            return true;
        }
        else
        {
            return false;
        }   
    }
}
