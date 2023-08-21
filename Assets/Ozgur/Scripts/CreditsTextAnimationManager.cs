using System.Collections;
using TMPro;
using UnityEngine;

public class CreditsTextAnimationManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private TextMeshProUGUI[] texts;
    [Tooltip("Wait time BEFORE the animation starts playing.")]
    [SerializeField] private float waitTimeBeforeStart = 3f;
    [Tooltip("The total time it takes to play all text animations. The time one text takes = displayTime = (totalCreditsTime / 5) - (fadeTime * 2) - waitTime;")]
    [SerializeField] private float totalCreditsTime;
    [Tooltip("The time texts takes to fade in and out into the screen.")]
    [SerializeField] private float fadeTime = 1f;
    [Tooltip("The wait time before the next text to show.")]
    [SerializeField] private float waitTime = 0.5f;

    private void Start()
    {
        foreach (var text in texts) text.alpha = 0f;

        ColorAltarManager.OnGameCompleted += PlayTextAnimationMethod;
    }

    private void PlayTextAnimationMethod()
    {
        StartCoroutine(PlayTextAnimation());
    }

    private IEnumerator PlayTextAnimation()
    {
        yield return new WaitForSeconds(waitTimeBeforeStart);
        
        float displayTime = (totalCreditsTime / 5) - (fadeTime * 2) - waitTime;
        
        foreach (var text in texts)
        {
            StartCoroutine(Fade(text, true));
            yield return new WaitForSeconds(fadeTime);

            yield return new WaitForSeconds(displayTime);

            StartCoroutine(Fade(text, false));
            yield return new WaitForSeconds(fadeTime);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator Fade(TextMeshProUGUI text, bool isFadeIn)
    {
        float timePassed = 0f;
        float increaseSpeed = 1 / fadeTime;

        while (timePassed <= fadeTime)
        {
            if (isFadeIn) text.alpha += increaseSpeed * Time.deltaTime;
            else text.alpha -= increaseSpeed * Time.deltaTime;
            
            timePassed += Time.deltaTime;
            yield return null;
        }

        if (isFadeIn) text.alpha = 1f;
        else text.alpha = 0f;
    }

    private void OnDestroy()
    {
        ColorAltarManager.OnGameCompleted -= PlayTextAnimationMethod;
    }
}