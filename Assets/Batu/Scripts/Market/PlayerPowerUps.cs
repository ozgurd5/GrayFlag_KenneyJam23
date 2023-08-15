using System;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    //public static event Action OnHookGunBought;
    public static event Action OnFishBought;
    public static event Action OnOrangeBought;
    public static event Action OnChickenBought;
    
    

    private void Start()
    {
        PoorGuyShake.Instance.ShakeCamera(0f, .0f);
    }

    // int hookGunPrice = 3;
    public int fishPrice = 3;
    public int orangePrice = 6;
    public int chickenPrice = 4;

    
    private bool IsCoinEnough(int price)
    {
        bool isEnough;
        if (price<=CoinChestMushroomManager.Singleton.coinNumber)
        {
            isEnough = true;
        }
        else
        {
            isEnough = false;
        }
        return isEnough;
    }
    
    /*public void BuyHookGun()
    {
        if(IsCoinEnough(hookGunPrice))
        {
            OnHookGunBought?.Invoke();
            Debug.Log("HookGunBought");
        }
        else
        {
            PoorGuyShake.Instance.ShakeCamera(5f,.1f);
            Debug.Log("fakirküpek");
        }
    }*/

    public void BuyFish()
    {
        if(IsCoinEnough(fishPrice))
        {
            OnFishBought?.Invoke();
            Debug.Log("fishBought");
        }
        else
        {
            PoorGuyShake.Instance.ShakeCamera(5f,.1f);
            Debug.Log("notEnoughMoneyForFish");
        }
    }
    
    public void BuyOrange()
    {
        if(IsCoinEnough(orangePrice))
        {
            OnOrangeBought?.Invoke();
            Debug.Log("orangeBought");
        }
        else
        {
            PoorGuyShake.Instance.ShakeCamera(5f,.1f);
            Debug.Log("notEnoughMoneyForOrange");
        }
    }
    
    public void BuyChicken()
    {
        if(IsCoinEnough(chickenPrice))
        {
            OnChickenBought?.Invoke();
            Debug.Log("chickenBought");
        }
        else
        {
            PoorGuyShake.Instance.ShakeCamera(5f,.1f);
            Debug.Log("notEnoughMoneyForChicken");
        }
    }
}
