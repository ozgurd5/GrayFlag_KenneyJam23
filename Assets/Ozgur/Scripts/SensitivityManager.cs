using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SensitivityManager : MonoBehaviour
{
    private static float mainMenuSliderValue = 5f;
    
    [Header("Assign for Main Menu")]
    [SerializeField] private Slider mainMenuSlider;
    [SerializeField] private TextMeshProUGUI mainMenuText;
    
    [Header("Assign for Pause Menu")]
    [SerializeField] private Slider pauseMenuSlider;
    [SerializeField] private TextMeshProUGUI pauseMenuText;

    [Header("Info - No Touch")]
    [SerializeField] private string currentSceneName;

    private void Awake()
    {
        SceneManager.activeSceneChanged += (a, currentScene) =>
        {
            currentSceneName = currentScene.name;

            if (currentSceneName == "Menu")
            {
                mainMenuSlider.onValueChanged.AddListener((float sliderValue) =>
                {
                    mainMenuSliderValue = sliderValue;
                    pauseMenuText.text = $"Sensitivity: {sliderValue}";
                });
            }

            else
            {
                //pauseMenuSlider = GameObject.Find("PauseMenu/PauseMenuCanvas/SensitivitySlider").GetComponent<Slider>();
                //pauseMenuText = pauseMenuSlider.transform.Find("SensitivityText").GetComponent<TextMeshProUGUI>();

                //Get the value from the main menu
                PlayerInputManager.Singleton.mouseSensitivity = CalculateSensitivity(mainMenuSliderValue);
                pauseMenuSlider.value = mainMenuSliderValue;
                pauseMenuText.text = $"Sensitivity: {mainMenuSliderValue}";
                
                pauseMenuSlider.onValueChanged.AddListener((float sliderValue) =>
                {
                    PlayerInputManager.Singleton.mouseSensitivity = CalculateSensitivity(sliderValue);
                    pauseMenuText.text = $"Sensitivity: {sliderValue}";
                });
            }
        };
    }

    //Default sensitivity is 0.2 and default slider value is 5. 25x difference
    private float CalculateSensitivity(float sliderValue)
    {
        return sliderValue / 25f;
    }
}
