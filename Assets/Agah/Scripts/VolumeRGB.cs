using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeRGB : MonoBehaviour
{
    UnityEngine.Rendering.VolumeProfile volumeProfile;
    UnityEngine.Rendering.Universal.Bloom bloom;
    UnityEngine.Rendering.Universal.Vignette vignette;

    Color bloomColor;
    [SerializeField][Range(0f, 25f)] float lerpTime;

    [SerializeField] Color[] myColors;
    [SerializeField]bool rgbBloom;
    [SerializeField]bool rgbVignette;

    int colorIndex = 0;

    float t = 0f;

    private void Start()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        
        if (!volumeProfile.TryGet(out bloom)) throw new System.NullReferenceException(nameof(bloom));
        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));



        bloom.tint.Override(bloomColor);
    }
    private void LateUpdate()
    {
        ChangeColors();
        //bloom.tint.Override(bloomColor);
    }

    void ChangeColors()
    {
        if(rgbBloom)
         bloom.tint.Override(Color.Lerp(((Color)bloom.tint), myColors[colorIndex], lerpTime * Time.deltaTime));
        if(rgbVignette)
         vignette.color.value = Color.Lerp(vignette.color.value ,myColors[colorIndex], lerpTime * Time.deltaTime);
        

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
        if (t > .99f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= myColors.Length) ? 0 : colorIndex;
        }
    }
}
