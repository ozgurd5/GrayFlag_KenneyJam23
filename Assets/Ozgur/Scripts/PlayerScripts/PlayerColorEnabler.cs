using System;
using UnityEngine;

public class PlayerColorEnabler : MonoBehaviour
{
    public static event Action OnRedColorEnabled;
    public static event Action OnGreenColorEnabled;
    public static event Action OnBlueColorEnabled;
    public static event Action OnYellowColorEnabled;

    //TODO: remove before build
    private void Start()
    {
        OnRedColorEnabled?.Invoke();
        OnGreenColorEnabled?.Invoke();
        OnBlueColorEnabled?.Invoke();
        OnYellowColorEnabled?.Invoke();
    }

    public static void EnableRedColor()
    {
        OnRedColorEnabled?.Invoke();
    }

    public static void EnableGreenColor()
    {
        OnGreenColorEnabled?.Invoke();
    }

    public static void EnableBlueColor()
    {
        OnBlueColorEnabled?.Invoke();
    }

    public static void EnableYellowColor()
    {
        OnYellowColorEnabled?.Invoke();
    }
}
