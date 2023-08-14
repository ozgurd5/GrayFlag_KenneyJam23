using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    public static event Action OnHookGunBought;
    public static event Action OnFishBought;
    public static event Action OnOrangeBought;
    public static event Action OnChickenBought;
    
    
    public void BuyHookGun()
    {
        OnHookGunBought?.Invoke();
        Debug.Log("hookGunBought");
    }

    public void BuyFish()
    {
        OnFishBought?.Invoke();
        Debug.Log("fishBought");
    }
    
    public void BuyOrange()
    {
        OnOrangeBought?.Invoke();
        Debug.Log("orangeBought");
    }
    
    public void BuyChicken()
    {
        OnChickenBought?.Invoke();
        Debug.Log("chickenBought");
    }
}

