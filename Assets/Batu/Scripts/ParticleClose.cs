using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleClose : MonoBehaviour
{
    public static List<ParticleClose> energyParticles;

    public static bool flag;
    
    private ParticleSystem myparticleSystem;
    private Renderer particleRenderer;
    private void Awake()
    {
        if (!flag)
        {
            energyParticles = new List<ParticleClose>();
            flag = true;
        }

        energyParticles.Add(this);
        myparticleSystem = GetComponent<ParticleSystem>();
        particleRenderer = GetComponent<Renderer>();
    }

    public void PlayAnim()
    {
        ParticleSystem.MainModule mainModule = myparticleSystem.main;
        
        Color startColor = mainModule.startColor.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        
        mainModule.startColor = endColor;
        
        myparticleSystem.Play();
    
        Debug.Log("anim done");
    }
}
