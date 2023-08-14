using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class SkyBoxEnabler : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Material newSkybox;
    
    private void Awake()
    {
        PlayerColorEnabler.OnBlueColorEnabled += ChangeSkybox; //abone ol
    }

    private void ChangeSkybox()
    {
        RenderSettings.skybox = newSkybox;
    }

    private void OnDestroy()
    {
        PlayerColorEnabler.OnBlueColorEnabled -= ChangeSkybox; //aboneliği bırak ki sahne geçişinde sorun çıkarmasın
    }
}
