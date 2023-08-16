using UnityEngine;

public class CursorHider : MonoBehaviour
{
    public static CursorHider Singleton;
    
    private PlayerStateData psd;
    private PlayerStateData.PlayerMainState previousState;

    private void Awake()
    {
        Singleton = GetComponent<CursorHider>();
        
        psd = PlayerStateData.Singleton;

        PauseMenu.OnGamePause += PauseGame;
        PauseMenu.OnGameContinue += ContinueGame;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        previousState = psd.currentMainState;
        psd.currentMainState = PlayerStateData.PlayerMainState.PauseMenuState;
    }

    public void ContinueGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        psd.currentMainState = previousState;
    }

    private void OnDestroy()
    {
        PauseMenu.OnGamePause -= PauseGame;
        PauseMenu.OnGameContinue -= ContinueGame;
    }
}