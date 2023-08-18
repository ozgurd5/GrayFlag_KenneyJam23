using System;
using Cinemachine;
using UnityEngine;

public class MarketManager : MonoBehaviour
{
    public static event Action OnHookGunBought;
    public static event Action OnFishBought;
    public static event Action OnOrangeBought;
    public static event Action OnChickenBought;
    public static event Action OnMarketCanvasClosed;

    [Header("Assign Canvases")]
    [SerializeField] private Canvas foodCanvas;
    [SerializeField] private Canvas gunCanvas;
    
    [Header("Assign Prices")]
    [SerializeField] private int hookGunPrice = 3;
    [SerializeField] private int fishPrice = 3;
    [SerializeField] private int orangePrice = 6;
    [SerializeField] private int chickenPrice = 4;

    private CinemachineImpulseSource impulse;

    private void Awake()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    private bool IsCoinEnough(int price)
    {
        if (price <= CoinChestMushroomManager.Singleton.coinNumber) return true;
        else return false;
    }
    
    public void BuyHookGun()
    {
        if (IsCoinEnough(hookGunPrice))
        {
            OnHookGunBought?.Invoke();
            CoinChestMushroomManager.Singleton.DecreaseCoinNumber(hookGunPrice);
            Debug.Log("Hook Gun Bought");
        }
        
        else
        {
            impulse.GenerateImpulse();
            Debug.Log("Not Enough Money for Hook Gun");
        }
    }

    public void BuyFish()
    {
        if( IsCoinEnough(fishPrice))
        {
            OnFishBought?.Invoke();
            CoinChestMushroomManager.Singleton.DecreaseCoinNumber(fishPrice);
            Debug.Log("Fish Bought");
        }
        
        else
        {
            impulse.GenerateImpulse();
            Debug.Log("Not Enough Money for Fish");
        }
    }
    
    public void BuyOrange()
    {
        if (IsCoinEnough(orangePrice))
        {
            OnOrangeBought?.Invoke();
            CoinChestMushroomManager.Singleton.DecreaseCoinNumber(orangePrice);
            Debug.Log("Orange Bought");
        }
        
        else
        {
            impulse.GenerateImpulse();
            Debug.Log("Not Enough Money for Orange");
        }
    }
    
    public void BuyChicken()
    {
        if (IsCoinEnough(chickenPrice))
        {
            OnChickenBought?.Invoke();
            CoinChestMushroomManager.Singleton.DecreaseCoinNumber(chickenPrice);
            Debug.Log("Chicken Bought");
        }
        
        else
        {
            impulse.GenerateImpulse();
            Debug.Log("Not Enough Money for Chicken");
        }
    }

    public void BuyLockedItem()
    {
        impulse.GenerateImpulse();
        Debug.Log("Can not buy");
    }
    
    public void CloseMarketCanvas()
    {
        foodCanvas.enabled = false;
        gunCanvas.enabled = false;

        OnMarketCanvasClosed?.Invoke();
    }
}

