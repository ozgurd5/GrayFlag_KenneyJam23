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
   

    public static void EnableRedColor()
    {
        OnRedColorEnabled?.Invoke();
        redColorEnabled = true;
        if (redColorEnabled && greenColorEnabled && blueColorEnabled && yellowColorEnabled)
            OnAllColorEnabled?.Invoke();
    }

    public static void EnableGreenColor()
    {
        OnGreenColorEnabled?.Invoke();
        greenColorEnabled = true;
        if (redColorEnabled && greenColorEnabled && blueColorEnabled && yellowColorEnabled)
            OnAllColorEnabled?.Invoke();
    }

    public static void EnableBlueColor()
    {
        OnBlueColorEnabled?.Invoke();
        blueColorEnabled = true;
        if (redColorEnabled && greenColorEnabled && blueColorEnabled && yellowColorEnabled)
            OnAllColorEnabled?.Invoke();
    }

    public static void EnableYellowColor()
    {
        OnYellowColorEnabled?.Invoke();
        yellowColorEnabled = true;
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
