using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleClose : MonoBehaviour
{
    public static List<ParticleClose> energyParticles;

    public static bool flag;
    
    private Animator animator;
    private void Awake()
    {
        if (!flag)
        {
            energyParticles = new List<ParticleClose>();
            flag = true;
        }

        energyParticles.Add(this);
        animator = GetComponent<Animator>();
    }

    public void PlayAnim()
    {
        animator.Play("particleClose");
    }
}
