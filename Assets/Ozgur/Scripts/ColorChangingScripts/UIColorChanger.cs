using UnityEngine;
using UnityEngine.UI;

public class UIColorChanger : MonoBehaviour
{
    private enum GrayFlagColor
    {
        None,
        Red,
        Green,
        Blue,
        Yellow
    }

    [Header("Assign")]
    [SerializeField] private GrayFlagColor firstColor;
    [SerializeField] private Sprite firstColorOnlySprite;
    [SerializeField] private GrayFlagColor secondColor;
    [SerializeField] private Sprite secondColorOnlySprite;
    [SerializeField] private Sprite allColorsSprite;

    [Header("Info - No Touch")]
    [SerializeField] private bool isFirstColorEnabled;
    [SerializeField] private bool isSecondColorEnabled;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        
        if (firstColor == GrayFlagColor.Red) PlayerColorEnabler.OnRedColorEnabled += EnableFirstColor;
        else if (firstColor == GrayFlagColor.Green) PlayerColorEnabler.OnGreenColorEnabled += EnableFirstColor;
        else if (firstColor == GrayFlagColor.Blue) PlayerColorEnabler.OnBlueColorEnabled += EnableFirstColor;
        else if (firstColor == GrayFlagColor.Yellow) PlayerColorEnabler.OnYellowColorEnabled += EnableFirstColor;
        else if (firstColor == GrayFlagColor.None) isFirstColorEnabled = true;
        
        if (secondColor == GrayFlagColor.Red) PlayerColorEnabler.OnRedColorEnabled += EnableSecondColor;
        else if (secondColor == GrayFlagColor.Green) PlayerColorEnabler.OnGreenColorEnabled += EnableSecondColor;
        else if (secondColor == GrayFlagColor.Blue) PlayerColorEnabler.OnBlueColorEnabled += EnableSecondColor;
        else if (secondColor == GrayFlagColor.Yellow) PlayerColorEnabler.OnYellowColorEnabled += EnableSecondColor;
        else if (secondColor == GrayFlagColor.None) isSecondColorEnabled = true;
    }

    private void EnableFirstColor()
    {
        Debug.Log("first: " + name);
        isFirstColorEnabled = true;

        image.enabled = true;
        image.sprite = firstColorOnlySprite;
        
        if (secondColor == GrayFlagColor.None) return;
        else if (isFirstColorEnabled && isSecondColorEnabled) EnableAllColors();
    }

    private void EnableSecondColor()
    {
        Debug.Log("second: " + name);
        isSecondColorEnabled = true;

        image.enabled = true;
        image.sprite = secondColorOnlySprite;
        
        if (firstColor == GrayFlagColor.None) return;
        else if (isFirstColorEnabled && isSecondColorEnabled) EnableAllColors();
    }

    private void EnableAllColors()
    {
        Debug.Log("all: " + name);
        image.enabled = true;
        image.sprite = allColorsSprite;
    }

    private void OnDestroy()
    {
        if (firstColor == GrayFlagColor.Red) PlayerColorEnabler.OnRedColorEnabled -= EnableFirstColor;
        else if (firstColor == GrayFlagColor.Green) PlayerColorEnabler.OnGreenColorEnabled -= EnableFirstColor;
        else if (firstColor == GrayFlagColor.Blue) PlayerColorEnabler.OnBlueColorEnabled -= EnableFirstColor;
        else if (firstColor == GrayFlagColor.Yellow) PlayerColorEnabler.OnYellowColorEnabled -= EnableFirstColor;
        
        if (secondColor == GrayFlagColor.Red) PlayerColorEnabler.OnRedColorEnabled -= EnableSecondColor;
        else if (secondColor == GrayFlagColor.Green) PlayerColorEnabler.OnGreenColorEnabled -= EnableSecondColor;
        else if (secondColor == GrayFlagColor.Blue) PlayerColorEnabler.OnBlueColorEnabled -= EnableSecondColor;
        else if (secondColor == GrayFlagColor.Yellow) PlayerColorEnabler.OnYellowColorEnabled -= EnableSecondColor;
    }
}
