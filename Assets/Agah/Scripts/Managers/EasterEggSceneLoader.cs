using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasterEggSceneLoader : MonoBehaviour
{
    [SerializeField]Button buttonYes;
    [SerializeField]Button buttonNo;

    void Start()
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait() //TODO: Bu Class ge�i�i yapacak, Wait fonksiyonu silinecek. 
    {
        yield return new WaitForSeconds(45); // mantar toplamak i�in sahip oldu�um s�re olarak atam��t�m. Sil.
        DOTween.KillAll();
        ChangeSceneToEasterEgg();
    }
    void ChangeSceneToEasterEgg()
    {
        SceneManager.LoadScene("Easter Egg"); 
    }
}
