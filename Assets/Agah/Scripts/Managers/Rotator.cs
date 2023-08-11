using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]Transform tr;
    [SerializeField][Range(0f,400f)]float r;
    [SerializeField][Range(0f, 2160f)]float angle;
    [SerializeField] float timeF= 1.001f;

    void Rotato()
    {
        tr.Rotate(0, angle*Time.deltaTime*r, 0, Space.Self);
    }
    private void Update()
    {
       Rotato();
       r = Mathf.Lerp(r, 400f, timeF * Time.deltaTime);
        if (r >= 24) angle = Mathf.Lerp(angle, 2160f, timeF * Time.deltaTime);
    }

}
