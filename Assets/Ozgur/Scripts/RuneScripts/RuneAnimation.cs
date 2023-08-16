using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RuneAnimation : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float positionChange = 0.2f;
    [SerializeField] private float positionTime = 1f;
    [SerializeField] private float rotationTime = 4f;

    [Header("Info - No Touch")]
    [SerializeField] private bool isUpDownPlaying;
    [SerializeField] private bool isRotationPlaying;
    
    void Update()
    {
        if (!isUpDownPlaying) StartCoroutine(PlayUpAndDownAnimation());
        if (!isRotationPlaying) StartCoroutine(PlayRotationAnimation());
    }

    private IEnumerator PlayUpAndDownAnimation()
    {
        isUpDownPlaying = true;
        
        transform.DOMoveY(transform.position.y + positionChange, positionTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(positionTime);
        
        transform.DOMoveY(transform.position.y - positionChange, positionTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(positionTime);
        
        isUpDownPlaying = false;
    }

    private IEnumerator PlayRotationAnimation()
    {
        isRotationPlaying = true;
        
        transform.DORotate(new Vector3(90, 360, 0), rotationTime, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        yield return new WaitForSeconds(rotationTime);
        
        isRotationPlaying = false;
    }
}
