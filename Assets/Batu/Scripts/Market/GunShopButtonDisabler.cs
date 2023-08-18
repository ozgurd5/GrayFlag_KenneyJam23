using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GunShopButtonDisabler : MonoBehaviour
{
    [Header("Assign")] 
    [SerializeField] private GameObject hookGunButton;
    void Awake()
    {
        MarketManager.OnHookGunBought += DisableHookGunButton;
    }

    private void DisableHookGunButton()
    {
         hookGunButton.SetActive(false);
    }

    private void OnDestroy()
    {
        MarketManager.OnHookGunBought -= DisableHookGunButton;
    }
}
