using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera islandCamera;

    public CinemachineVirtualCamera startCam;
    private CinemachineVirtualCamera currentCam;
    private void Start()
    {
        currentCam = startCam;

        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == currentCam)
            { 
                cameras[i].Priority = 30; 
            }
            else
            {
                cameras[i].Priority = 10;
            }
            
        }
    }
    public void SwitchCameras(CinemachineVirtualCamera nextCam)
    {
        currentCam = nextCam;

        for(int i = 0;i < cameras.Length; i++)
        {
            if (cameras[i] == currentCam)
            {
                cameras[i].Priority = 30;
            }
            else
            {
                cameras[i].Priority = 10;
            }
        }
    }
}
