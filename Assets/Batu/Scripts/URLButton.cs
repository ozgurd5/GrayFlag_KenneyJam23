using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButton : MonoBehaviour
{
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
        Debug.Log("Opening URL: " + url);
    }
}