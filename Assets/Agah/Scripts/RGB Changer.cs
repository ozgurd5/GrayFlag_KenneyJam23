using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RGBChanger : MonoBehaviour
{
    MeshRenderer cubeMeshRenderer;
    [SerializeField][Range(0f, 5f)] float lerpTime;

    [SerializeField] Color[] myColors;

    int colorIndex = 0;

    float t = 0f;


    bool redColorEnabled;
    bool greenColorEnabled;
    bool blueColorEnabled;
    bool yellowColorEnabled;
    bool isGameOver; // �zg�r� bekle


    private void Awake()
    {
        PlayerColorEnabler.OnBlueColorEnabled += PlayerColorEnabler_OnBlueColorEnabled;
        PlayerColorEnabler.OnRedColorEnabled += PlayerColorEnabler_OnRedColorEnabled;
        PlayerColorEnabler.OnGreenColorEnabled += PlayerColorEnabler_OnGreenColorEnabled;
        PlayerColorEnabler.OnYellowColorEnabled += PlayerColorEnabler_OnYellowColorEnabled;
    }
    
    #region ReceiveEvents
    private void PlayerColorEnabler_OnYellowColorEnabled()
    {
        yellowColorEnabled = true;
    }

    private void PlayerColorEnabler_OnGreenColorEnabled()
    {
        greenColorEnabled = true;
    }

    private void PlayerColorEnabler_OnRedColorEnabled()
    {
        redColorEnabled = true;
    }

    private void PlayerColorEnabler_OnBlueColorEnabled()
    {
        blueColorEnabled = true;
    }
    #endregion
    void Start()
    {
        cubeMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (IsAllColorsEnabled())
            ChangeColors();
    }

    void ChangeColors()
    {
        cubeMeshRenderer.material.color = Color.Lerp(cubeMeshRenderer.material.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .99f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= myColors.Length) ? 0 : colorIndex;
        }
    }

    bool IsAllColorsEnabled()
    {
        if(redColorEnabled && blueColorEnabled && greenColorEnabled && yellowColorEnabled) return true;
        else return false;
    }

}


