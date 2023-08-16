using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasterEggSceneManager : MonoBehaviour
{
    int mushroomCollected;
    public static event Action<int> OnMushroomEvent;
    [Tooltip("Wait for seconds before starting the effects. Default is 0.")][SerializeField] int waitTime = 0;
    private void Awake()
    {
        var _mushroomCollected = CoinChestMushroomManager.GetNumber("Mushroom");
        mushroomCollected = _mushroomCollected;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(CallMushroomEvent());
        //InvokeMushroomEvent(); 
    }
    public void InvokeMushroomEvent()
    {
        OnMushroomEvent?.Invoke(mushroomCollected);
    }
    IEnumerator CallMushroomEvent() // TODO: Build öncesi sil ve Start'a gerekli fonksiyonu koy.
    {
        yield return new WaitForSeconds(waitTime);
        InvokeMushroomEvent();
    }
}
