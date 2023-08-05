using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    [SerializeField]TMP_Text _textMeshPro;
    static bool timeIsRunning;
    float startTime;

    private void Start()
    {
        timeIsRunning = true;
        startTime = Time.time;
        
    }
    private void Update()
    {
        float timeToDisplay = Time.time - startTime; 
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = timeToDisplay % 60;
        float miliseconds = seconds % 99;
        var first2DecimalPlaces = (int)(((decimal)miliseconds % 1) * 100);


        if (timeIsRunning)
        {
            _textMeshPro.text =  minutes.ToString("00") + " : " + Mathf.FloorToInt(seconds).ToString("00") + " : " + first2DecimalPlaces.ToString("00");
        }
    }

    public static void StopTimer()
    {
        timeIsRunning = false;
    }
        
}


