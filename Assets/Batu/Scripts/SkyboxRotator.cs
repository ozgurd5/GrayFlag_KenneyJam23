using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float speed; 

    void FixedUpdate()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speed);
    }
}