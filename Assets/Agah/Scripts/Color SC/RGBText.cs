using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RGBText : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField][Range(0f, 15f)] float lerpTime;

    [SerializeField] Color[] myColors;

    int colorIndex = 0;

    float t = 0f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        ChangeColors();
    }

    void ChangeColors()
    {
        text.color = Color.Lerp(text.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .99f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= myColors.Length) ? 0 : colorIndex;
        }
    }
}
