using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static event Action OnGamePause;
    
    public static event Action OnGameContinue;
    
    public static bool paused = false;
    public GameObject pauseMenuCanvas;
    private GameObject playerCanvas;
    private GameObject dialogueCanvasları;

    void Awake()
    {
        playerCanvas = GameObject.Find("Player/PlayerCanvas");
        dialogueCanvasları = GameObject.Find("DialogueCanvasları");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        playerCanvas.SetActive(false);
        dialogueCanvasları.SetActive(false);
        OnGamePause?.Invoke();
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        paused = true; 
    }

    public void Play()
    {
        playerCanvas.SetActive(true);
        dialogueCanvasları.SetActive(true);
        OnGameContinue?.Invoke();
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}