using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
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
    }

    private void Start()
    {
        StartCoroutine(CallMushroomEvent());
    }

    public void InvokeMushroomEvent()
    {
        OnMushroomEvent?.Invoke(mushroomCollected);
    }

    IEnumerator CallMushroomEvent()
    {
        yield return new WaitForSeconds(3);
        InvokeMushroomEvent();

    }

}
