using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeOpener : MonoBehaviour
{
    Transform volume1, volume2, volume3, volume4, volume5;
    private void Awake()
    {
        volume1 = transform.GetChild(0);
        volume2 = transform.GetChild(1);
        volume3 = transform.GetChild(2);
        volume4 = transform.GetChild(3);
        volume5 = transform.GetChild(4);
        EasterEggSceneManager.OnMushroomEvent += EasterEggSceneManager_OnMushroomEvent;
    }

    private void EasterEggSceneManager_OnMushroomEvent(int eatenMushroomAmount)
    {
        switch (eatenMushroomAmount)
        {
            case 0: Debug.LogError("0 Mantar ile Sahneye Geçiþ Yapmak Efektleri Çalýþtýrmaz!"); break;
            case 1: volume1.gameObject.SetActive(true); break;
            case 2: volume2.gameObject.SetActive(true); break;
            case 3: volume3.gameObject.SetActive(true); break;
            case 4: volume4.gameObject.SetActive(true); break;
            case 5: volume5.gameObject.SetActive(true); break;
            default: Debug.LogError("1 ila 5 Mantar ile Sahneye Geçiþ Yapman Lazým!"); break;
        }
    }

    private void OnDestroy()
    {
        EasterEggSceneManager.OnMushroomEvent -= EasterEggSceneManager_OnMushroomEvent;
    }
}
