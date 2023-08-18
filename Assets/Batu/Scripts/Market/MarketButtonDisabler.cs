using UnityEngine;

public class MarketButtonDisabler : MonoBehaviour
{
    [Header("Assign")] 
    [SerializeField] private GameObject fishButton;
    [SerializeField] private GameObject orangeButton;
    [SerializeField] private GameObject chickenButton;
    
    private void Awake()
    {
        MarketManager.OnFishBought += DisableFishButton;
        MarketManager.OnOrangeBought += DisableOrangeButton;
        MarketManager.OnChickenBought += DisableChickenButton;
    }
    
    private void DisableFishButton()
    {
        fishButton.SetActive(false);
    }
    
    private void DisableOrangeButton()
    {
        orangeButton.SetActive(false);
    }
    
    private void DisableChickenButton()
    {
        chickenButton.SetActive(false);
    }
    
    private void OnDestroy()
    {
        MarketManager.OnFishBought -= DisableFishButton;
        MarketManager.OnOrangeBought -= DisableOrangeButton;
        MarketManager.OnChickenBought -= DisableChickenButton;
    }
}

