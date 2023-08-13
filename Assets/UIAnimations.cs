using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] float z;
    [Tooltip("Time it takes to move the object.")][SerializeField] float moveTime;
    [Tooltip("Time in seconds before the object moves.")][SerializeField] float waitTime;


    [SerializeField] Transform objTr;

    private void Awake()
    {
        EasterEggSceneManager.OnMushroomEvent += EasterEggSceneManager_OnMushroomEvent;
    }

    private void EasterEggSceneManager_OnMushroomEvent(int obj)
    {
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    { 
        yield return new WaitForSeconds(waitTime);
        objTr.DOMove(new Vector3(x,y,z) ,moveTime);
    }
    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
