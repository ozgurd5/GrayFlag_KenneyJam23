using System;
using UnityEngine;

public class PlayerColorEnabler : MonoBehaviour
{
    public static event Action OnRedColorEnabled;
    public static event Action OnGreenColorEnabled;
    public static event Action OnBlueColorEnabled;
    public static event Action OnYellowColorEnabled;
    public static event Action OnAllColorEnabled;

    private static bool isRedColorEnabled;
    private static bool isGreenColorEnabled; 
    private static bool isBlueColorEnabled;
    private static bool isYellowColorEnabled;

    //TODO: remove before build
    #region RemoveBeforeBuild

    private void Start()
    {
        //EnableColorsAutomatically();
    }
    
    private void Update()
    {
        EnableColorsManually();
    }

    #endregion

    public static void EnableRedColor()
    {
        OnRedColorEnabled?.Invoke();
        isRedColorEnabled = true;
        
        if (IsAllColorEnabled())
            OnAllColorEnabled?.Invoke();
    }

    public static void EnableGreenColor()
    {
        OnGreenColorEnabled?.Invoke();
        isGreenColorEnabled = true;
        
        if (IsAllColorEnabled())
            OnAllColorEnabled?.Invoke();
    }

    public static void EnableBlueColor()
    {
        OnBlueColorEnabled?.Invoke();
        isBlueColorEnabled = true;
        
        if (IsAllColorEnabled())
            OnAllColorEnabled?.Invoke();
    }

    public static void EnableYellowColor()
    {
        OnYellowColorEnabled?.Invoke();
        isYellowColorEnabled = true;
        
        if (IsAllColorEnabled())
            OnAllColorEnabled?.Invoke();
    }
    
    public static bool IsAllColorEnabled()
    {
        if(isRedColorEnabled &&  isGreenColorEnabled && isBlueColorEnabled && isYellowColorEnabled)
            return true;
        else return false;
    }

    private void EnableColorsAutomatically()
    {
        isRedColorEnabled = true;
        isGreenColorEnabled = true;
        isBlueColorEnabled = true;
        isYellowColorEnabled = true;
        
        OnRedColorEnabled?.Invoke();
        OnGreenColorEnabled?.Invoke();
        OnBlueColorEnabled?.Invoke();
        OnYellowColorEnabled?.Invoke();
        OnAllColorEnabled?.Invoke();
    }

    private void EnableColorsManually()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnRedColorEnabled?.Invoke();
            isRedColorEnabled = true;
        }
        
        else if (Input.GetKeyDown(KeyCode.G))
        {
            OnGreenColorEnabled?.Invoke();
            isGreenColorEnabled = true;
        }
        
        else if (Input.GetKeyDown(KeyCode.B))
        {
            OnBlueColorEnabled?.Invoke();
            isBlueColorEnabled = true;
        }
        
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            OnYellowColorEnabled?.Invoke();
            isYellowColorEnabled = true;
        }
    }
}
