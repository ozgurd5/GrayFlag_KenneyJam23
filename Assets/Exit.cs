using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [Tooltip("Exit button hold time.")][SerializeField] float exitPressTime = 1.5f;
    private void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            exitPressTime -= Time.deltaTime;
        }

        if (exitPressTime <= 0)
        {
            /*Application.Quit();*/ Debug.Log("Application Quits the Game");
            exitPressTime = 1.5f;
        }

        else return;
    }
}
