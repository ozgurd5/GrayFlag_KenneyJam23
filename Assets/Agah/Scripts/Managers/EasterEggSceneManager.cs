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
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        var _mushroomCollected = CoinChestMushroomManager.Singleton.mushroomNumber;
        mushroomCollected = _mushroomCollected;
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
