using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasterEggSceneLoader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait() //TODO: Bu Class geçiþi yapacak, Wait fonksiyonu silinecek. 
    {
        yield return new WaitForSeconds(45); // mantar toplamak için sahip olduðum süre olarak atamýþtým. Sil.
        DOTween.KillAll();
        ChangeSceneToEasterEgg();
    }
    public void ChangeSceneToEasterEgg()
    {
        SceneManager.LoadScene("Easter Egg");
    }
} 
