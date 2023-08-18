using UnityEngine;

public class MarketButtonDisabler : MonoBehaviour
{
    [Header("Assign")] 
    [SerializeField] private GameObject hookGunButton;
    [SerializeField] private GameObject fishButton;
    [SerializeField] private GameObject orangeButton;
    [SerializeField] private GameObject chickenButton;
    
    private void Awake()
    {
        MarketManager.OnHookGunBought += DisableHookGunButton;
        MarketManager.OnFishBought += DisableFishButton;
        MarketManager.OnOrangeBought += DisableOrangeButton;
        MarketManager.OnChickenBought += DisableChickenButton;

        DialogueController.OnPlayerExitNpcCollider += DisableCanvas;
    }

    private void DisableCanvas()
    {
        gameObject.SetActive(false);
    }
    
    private void DisableHookGunButton()
    {
        hookGunButton.SetActive(false);
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
        MarketManager.OnHookGunBought -= DisableHookGunButton;
        MarketManager.OnFishBought -= DisableFishButton;
        MarketManager.OnOrangeBought -= DisableOrangeButton;
        MarketManager.OnChickenBought -= DisableChickenButton;
        
        DialogueController.OnPlayerExitNpcCollider -= DisableCanvas;
    }
}

