using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class RGBChanger : MonoBehaviour
{
    MeshRenderer cubeMeshRenderer;
    [SerializeField][Range(0f, 5f)] float lerpTime;

    [SerializeField] Color[] myColors;

    int colorIndex = 0;

    float t = 0f;

    void Start()
    {
        cubeMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        cubeMeshRenderer.material.color = Color.Lerp(cubeMeshRenderer.material.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .9f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= myColors.Length) ? 0 : colorIndex;
        }
    }
}


