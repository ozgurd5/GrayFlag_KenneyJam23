using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera islandShakeCamera;
    [Header("Final Cutscene Cameras")]
    public CinemachineVirtualCamera ada1Camera;
    public CinemachineVirtualCamera ada2Camera;
    public CinemachineVirtualCamera ada3Camera;
    public CinemachineVirtualCamera ada4Camera;
    [Tooltip("Bu kamera 5. Adanýn FÝNAL cutscene'i için, yerden çýkýþ kamerasý 'islandShakeCamera'")]
    public CinemachineVirtualCamera ada5Camera; //Bu kamera final cutscene için, karýþtýrma!



    public CinemachineVirtualCamera startCam;
    private CinemachineVirtualCamera currentCam;
    private void Start()
    {
        currentCam = startCam;
        SwitchCameras(startCam);    
    }
    public void SwitchCameras(CinemachineVirtualCamera nextCam)
    {
        currentCam = nextCam;

        foreach(var cam in cameras) 
        {
            if (cam == currentCam)
            {
                cam.Priority = 100;
            }
            else
            {
                cam.Priority = 10;
            }
        }
    }
}
