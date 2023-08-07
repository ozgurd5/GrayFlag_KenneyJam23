using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomPickup : MonoBehaviour
{
    public static event Action onPickup;
    public static int count;

    private void Start()
    {
        onPickup?.Invoke(); //Yap�lacak: Crosshair ile e�le ve Start()'tan sil...
    }
    public ShroomPickup()
    {
        count++;
    }
}
