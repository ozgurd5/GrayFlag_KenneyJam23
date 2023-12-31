using UnityEngine;

public class CursorHider : MonoBehaviour
{
    private PlayerStateData psd;
    private PlayerStateData.PlayerMainState previousState;

    private void Awake()
    {
        psd = PlayerStateData.Singleton;

        PauseMenu.OnGamePause += PauseGame;
        PauseMenu.OnGameContinue += ContinueGame;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        previousState = psd.currentMainState;
        psd.currentMainState = PlayerStateData.PlayerMainState.PauseMenuState;
    }

    private void ContinueGame()
    {
        psd.currentMainState = previousState;
        if (psd.currentMainState == PlayerStateData.PlayerMainState.DialogueState) return;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        PauseMenu.OnGamePause -= PauseGame;
        PauseMenu.OnGameContinue -= ContinueGame;
    }
}