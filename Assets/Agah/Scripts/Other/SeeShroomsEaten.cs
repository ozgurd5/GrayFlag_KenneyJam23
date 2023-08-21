using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SeeShroomsEaten : MonoBehaviour
{
    [SerializeField] TMP_Text shroomText;
    [Tooltip("Time it takes before destroying the object.")][SerializeField]int disapperTime;

    int eatenShroomAmount;

    private void Awake()
    {
        EasterEggSceneManager.OnMushroomEvent += EasterEggSceneManager_OnMushroomEvent;  
    }

    private void EasterEggSceneManager_OnMushroomEvent(int obj)
    {
        eatenShroomAmount = obj;
        ShowShroomText();
        Destroy(transform.parent.gameObject, disapperTime);
    }
    void ShowShroomText()
    {
        shroomText.text = $"{eatenShroomAmount} Shrooms Eaten!!";
    }

    private void OnDestroy()
    {
        EasterEggSceneManager.OnMushroomEvent -= EasterEggSceneManager_OnMushroomEvent;
    }
}
