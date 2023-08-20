using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterCanvasManager : MonoBehaviour
{
    public int requiredMushroomCount = 10;
    private GameObject questionCanvas;

    private CoinChestMushroomManager mushroomManager;

    private void Start()
    {
        questionCanvas = GameObject.Find("QuestionCanvas");
        questionCanvas.SetActive(false);

        mushroomManager = CoinChestMushroomManager.Singleton;
    }

    public void CheckMushroomCountAndProceed()
    {
        int collectedMushroomCount = mushroomManager.mushroomNumber;

        if (collectedMushroomCount >= requiredMushroomCount)
        {
            questionCanvas.SetActive(true);
        }
    }

    public void EatMushroomsAndProceed(bool eatMushrooms)
    {
        if (eatMushrooms) // true // yes button
        {
            SceneManager.LoadScene("Easter Egg"); // Load Easter Egg scene
        }
        else //false // no button
        {
            SceneManager.LoadScene("Menu"); // Load Menu scene
        }
    }
}
