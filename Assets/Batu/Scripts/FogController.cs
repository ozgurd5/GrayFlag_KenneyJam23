using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class FogController : MonoBehaviour
{
    [Header("Fog Settings")]
    public bool enableFog = true;
    public Color fogColor = Color.gray;
    public float fogDensity = 0.01f;
    public float fogStartDistance = 10f;
    public float fogEndDistance = 100f;

    private void Update()
    {
        RenderSettings.fog = enableFog;
        if (enableFog)
        {
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogStartDistance = fogStartDistance;
            RenderSettings.fogEndDistance = fogEndDistance;
        }
    }
}
