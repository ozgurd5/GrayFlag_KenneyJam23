using DG.Tweening;
using UnityEngine;

public class MainMenuBugFixer : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1;
        DOTween.KillAll();
    }
}
