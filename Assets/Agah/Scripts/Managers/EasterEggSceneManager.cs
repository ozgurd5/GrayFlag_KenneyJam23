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
    [Tooltip("Invokes the mushroom event with the int object being the number of mushrooms collected.")]
    public void InvokeMushroomEvent()
    {
        OnMushroomEvent?.Invoke(mushroomCollected); //TODO 5'i sil, commenti kaldýr.
    }
    IEnumerator CallMushroomEvent() // TODO: Build öncesi sil ve Start'a gerekli fonksiyonu koy.
    {
        yield return new WaitForSeconds(waitTime);
        InvokeMushroomEvent();
    }
}
