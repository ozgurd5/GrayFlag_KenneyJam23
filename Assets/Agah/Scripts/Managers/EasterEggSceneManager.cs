using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasterEggSceneManager : MonoBehaviour
{
    [SerializeField] Button button;
    int mushroomCollected;

    public static event Action<int> OnMushroomEvent;

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
        yield return new WaitForSeconds(3);
        InvokeMushroomEvent();
    }
}
