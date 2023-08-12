using System;
using UnityEngine;

public class PlayerColorEnabler : MonoBehaviour
{
    public static event Action OnRedColorEnabled;
    public static event Action OnGreenColorEnabled;
    public static event Action OnBlueColorEnabled;
    public static event Action OnYellowColorEnabled;
    public static event Action OnAllColorEnabled;

    static bool redColorEnabled;
    static bool greenColorEnabled; 
    static bool blueColorEnabled ;
    static bool yellowColorEnabled;

    //TODO: remove before build
    private void Awake()
    {
        redColorEnabled = true;
        greenColorEnabled = true;
        blueColorEnabled = true;
        yellowColorEnabled = true;
    }
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
        redColorEnabled = true;
    }

    public static void EnableGreenColor()
    {
        OnGreenColorEnabled?.Invoke();
        greenColorEnabled = true;
    }

    public static void EnableBlueColor()
    {
        OnBlueColorEnabled?.Invoke();
        blueColorEnabled = true;
    }

    public static void EnableYellowColor()
    {
        OnYellowColorEnabled?.Invoke();
        yellowColorEnabled = true;
    }

    public static void InvokeAllColorsEnabled()
    {
        if (redColorEnabled && greenColorEnabled && blueColorEnabled && yellowColorEnabled)
            OnAllColorEnabled?.Invoke();
    }
    
    public static bool IsAllColorEnabled()
    {
        if(redColorEnabled &&  greenColorEnabled && blueColorEnabled && yellowColorEnabled)
            return true;
        else return false;
    }
}
