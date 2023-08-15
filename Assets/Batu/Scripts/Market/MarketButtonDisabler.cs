using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MarketButtonDisabler : MonoBehaviour
{
    [Header("Assign")] 
    [SerializeField] private GameObject fishButton;
    [SerializeField] private GameObject orangeButton;
    [SerializeField] private GameObject chickenButton;
    [SerializeField] private GameObject hookGunButton;

    private void Awake()
    {
        PlayerPowerUps.OnFishBought += DisableFishButton;
        PlayerPowerUps.OnOrangeBought += DisableOrangeButton;
        PlayerPowerUps.OnChickenBought += DisableChickenButton;
        PlayerPowerUps.OnHookGunBought += DisableHookGunButton;
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
    
    private void DisableHookGunButton()
    {
        hookGunButton.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerPowerUps.OnFishBought -= DisableFishButton;
        PlayerPowerUps.OnOrangeBought -= DisableOrangeButton;
        PlayerPowerUps.OnChickenBought -= DisableChickenButton;
        PlayerPowerUps.OnHookGunBought -= DisableHookGunButton;
    }
}

