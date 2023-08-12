using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] float exitPressTime = 1.5f;
    private void Update()
    {
        while(Input.GetKey(KeyCode.E))
        {
            exitPressTime -= Time.deltaTime;
            if (exitPressTime < 0) {Application.Quit(); Debug.Log("Application Quits the Game"); }
            else return;
        }
    }
}
