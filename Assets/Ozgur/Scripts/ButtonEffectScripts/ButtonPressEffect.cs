using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressEffect : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject defaultVersion;
    [SerializeField] private GameObject coin;
    [SerializeField] private float coinScaleDownRate = 0.95f;
    [SerializeField] private float transitionTime = 0.1f;

    private float coinSize;
    private float coinSizePressed;
    private Tweener coinAnimationTween;
    private bool isPressWorking;

    private void Awake()
    {
        button.onClick.AddListener(() => StartCoroutine(PressButton()));

        coinSize = coin.transform.localScale.x;
        coinSizePressed = coinSize * coinScaleDownRate;
    }

    private IEnumerator PressButton()
    {
        if (isPressWorking) yield break;
        
        isPressWorking = true;
        
        defaultVersion.SetActive(false);
        coinAnimationTween = coin.transform.DOScale(new Vector3(coinSizePressed, coinSizePressed, coinSizePressed), transitionTime);

        yield return new WaitForSeconds(transitionTime);

        defaultVersion.SetActive(true);
        coinAnimationTween.Kill();
        coinAnimationTween = coin.transform.DOScale(new Vector3(coinSize, coinSize, coinSize), transitionTime);

        isPressWorking = false;
    }
}
