using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookGunActivated : MonoBehaviour
{
    [Header("Assign HookGun From Player Camera")]
    public GameObject HookGun;

    private void OnEnable()
    {
        PlayerPowerUps.OnHookGunBought += ActivateHookGunAndPoints;
    }
    private void ActivateHookGunAndPoints()
    {
        HookGun.SetActive(true);
    }
    private void OnDisable()
    {
        PlayerPowerUps.OnHookGunBought -= ActivateHookGunAndPoints;
    }
}