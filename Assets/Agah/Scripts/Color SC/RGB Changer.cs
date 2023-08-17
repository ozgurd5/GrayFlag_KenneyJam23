using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RGBChanger : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [SerializeField][Range(0f, 5f)] float lerpTime;

    [SerializeField] Color[] myColors;

    int colorIndex = 0;

    bool isAllColorsEnabled;
    bool isGameOver;

    float t = 0f;

    private void Awake()
    {
        PlayerColorEnabler.OnAllColorEnabled += PlayerColorEnabler_OnAllColorEnabled;
        ColorAltarManager.OnGameCompleted += ColorAltarManager_OnGameCompleted;
    }

    private void ColorAltarManager_OnGameCompleted()
    {
        isGameOver = true;
    }

    private void PlayerColorEnabler_OnAllColorEnabled()
    {
        isAllColorsEnabled = true;
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isAllColorsEnabled && isGameOver)
            ChangeColors();
    }

    void ChangeColors()
    {
        meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .99f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= myColors.Length) ? 0 : colorIndex;
        }
    }

}


