using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]Transform tr;
    [SerializeField][Range(0f,5f)]float r;
    [SerializeField][Range(0f, 360f)]float angle;
    [SerializeField] float timeF= 1.001f;

    void Rotato()
    {
        tr.Rotate(0, angle*Time.deltaTime*r, 0, Space.Self);
    }
    private void Update()
    {
       Rotato();
       r = Mathf.Lerp(r, 5, timeF * Time.deltaTime);
    }

}
