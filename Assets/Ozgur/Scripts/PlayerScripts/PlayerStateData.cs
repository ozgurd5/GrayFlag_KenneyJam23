using System;
using UnityEngine;

public class PlayerStateData : MonoBehaviour
{
    public static PlayerStateData Singleton;

    private void Awake()
    {
        Singleton = GetComponent<PlayerStateData>();
    }

    public enum PlayerMainState
    {
        NormalState,
        HookState,  //Custom movement
        ShipControllingState,   //No looking - No movement - Ship input and control
        PauseMenuState, //No looking - No movement
        DialogueState, //No looking
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