using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasterEggSceneLoader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Wait());
        
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(45);
        DOTween.KillAll();
        ChangeSceneToEasterEgg();
    }
    void ChangeSceneToEasterEgg()
    {
        SceneManager.LoadScene("Easter Egg");
        
    }
}
