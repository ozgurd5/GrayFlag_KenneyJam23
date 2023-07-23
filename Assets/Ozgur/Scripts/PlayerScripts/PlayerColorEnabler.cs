using System;
using UnityEngine;

public class PlayerColorEnabler : MonoBehaviour
{
    public static event Action OnRedColorEnabled;
    public static event Action OnGreenColorEnabled;
    public static event Action OnBlueColorEnabled;
    public static event Action OnYellowColorEnabled;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) OnRedColorEnabled?.Invoke();
        else if (Input.GetKeyDown(KeyCode.K)) OnGreenColorEnabled?.Invoke();
        else if (Input.GetKeyDown(KeyCode.L)) OnBlueColorEnabled?.Invoke();
        else if (Input.GetKeyDown(KeyCode.H)) OnYellowColorEnabled?.Invoke();
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
