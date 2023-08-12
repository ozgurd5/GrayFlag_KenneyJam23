using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyboxChanger : MonoBehaviour
{
    [SerializeField] private Material blueSkyboxMaterial;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.CompareTag("BlueRune"))
        {
            Debug.Log("Triggered by object with bluerune tag");
            Debug.Log("Triggered by object with bluerune tag");
            RenderSettings.skybox = blueSkyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }
    }
}
