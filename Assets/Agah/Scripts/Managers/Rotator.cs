using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]Transform tr;
    [SerializeField][Range(0f,400f)]float r, rMax;
    [SerializeField][Range(0f, 2160f)]float angle, angleMax;
    [SerializeField] float timeF= 1.001f;
    [SerializeField] int waitTime = 2;
    [SerializeField] bool x, y, z;
    bool didWait = false;

    void Rotato()
    {
       if(x) tr.Rotate(angle * Time.deltaTime * r, 0, 0, Space.Self);
       if(y) tr.Rotate(0, angle * Time.deltaTime * r, 0, Space.Self);
       if(z) tr.Rotate(0, 0, angle * Time.deltaTime * r, Space.Self);
    }
    private void Update()
    {
        if (!didWait) StartCoroutine(WaitForPlayerReadingTheScreenLol());
        else 
        {
            Rotato();
            r = Mathf.Lerp(r, rMax, timeF * Time.deltaTime);
            if (r >= 24) angle = Mathf.Lerp(angle, angleMax, timeF * Time.deltaTime);
        }
    }

    IEnumerator WaitForPlayerReadingTheScreenLol() { yield return new WaitForSeconds(waitTime); didWait = true; }

}
