using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBLights : MonoBehaviour
{
    Light light;
    [SerializeField][Range(0f, 5f)] float lerpTime;

    [SerializeField] Color[] myColors;

    int colorIndex = 0;

    float t = 0f;

    void Start()
    {
        light = GetComponent<Light>();
    }

    void Update()
    {
          ChangeColors();
    }

    void ChangeColors()
    {
        light.color = Color.Lerp(light.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .99f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= myColors.Length) ? 0 : colorIndex;
        }
    }
}
