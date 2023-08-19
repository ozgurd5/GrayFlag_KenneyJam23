using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject MainScreen;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    
    public void Play()
    {
        LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("BYE BYE");
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        enemy1.SetActive(false);
        enemy2.SetActive(false);
        enemy3.SetActive(false);
        MainScreen.SetActive(false);
        LoadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}