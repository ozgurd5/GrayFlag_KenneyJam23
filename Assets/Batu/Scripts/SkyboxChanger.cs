using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyboxChanger : MonoBehaviour
{
    [SerializeField] private Material blueSkyboxMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlueRune"))
        {
            RenderSettings.skybox = blueSkyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }
    }
}
