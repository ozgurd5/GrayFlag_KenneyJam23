using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    private void Awake()
    {
        sensitivitySlider.onValueChanged.AddListener((float sliderValue) =>
        {
            PlayerInputManager.Singleton.mouseSensitivity = CalculateSensitivity(sliderValue);
            sensitivityText.text = $"Sensitivity: {sliderValue}";
        });
    }

    //Default sensitivity is 0.2 and default slider value is 5. 25x difference
    private float CalculateSensitivity(float sliderValue)
    {
        return sliderValue / 25f;
    }
}
