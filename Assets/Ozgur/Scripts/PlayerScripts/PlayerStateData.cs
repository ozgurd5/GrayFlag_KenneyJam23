using UnityEngine;

public class PlayerStateData : MonoBehaviour
{
    public enum PlayerMainState
    {
        NormalState,
        ShipControllingState,
        PauseMenuState,
        HookState,
    }

    [Header("Main State")]
    public PlayerMainState currentMainState = PlayerMainState.NormalState;

    [Header("States")]
    public bool isIdle;     //PlayerController.cs
    public bool isWalking;  //PlayerController.cs
    public bool isRunning;  //PlayerController.cs
    public bool isJumping;

    [Header("Logic Only States")]
    public bool isMoving;           //PlayerController.cs
    public bool isGrounded;         //GroundCheck.cs
    public bool isSwimming;
    public bool isGettingDamage;    //PlayerHookController.cs
}