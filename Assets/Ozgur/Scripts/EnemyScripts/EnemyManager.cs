using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private int health = 10;
    [SerializeField] private int knockbackForce = 1000;
    [SerializeField] private float damageTakingAnimTime = 0.5f;
    
    [Header("Assign - Colliders")]
    [SerializeField] private Collider aliveCollider;
    [SerializeField] private Collider deadCollider;

    [Header("Assign - Sounds")]
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float idleSoundInterval;
    [SerializeField] private AudioClip idleSound;

    private Rigidbody rb;
    private Animator an;
    private Slider healthBar;
    
    private IEnumerator idleSoundCoroutine;
    private RaycastHit hit;
    
    public enum EnemyState
    {
        Walking,
        Punching,
        Dead
    }

    public EnemyState currentState;
    public bool isDamageTaking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
        healthBar = GetComponentInChildren<Slider>();
        idleSoundCoroutine = HandleIdleSound();
    }

    public void EnterPunchingState()
    {
        if (currentState == EnemyState.Dead) return;
        
        StopCoroutine(idleSoundCoroutine);
        aus.Stop();
        
        currentState = EnemyState.Punching;
        an.Play("Zombie Punching");
        aus.PlayOneShot(attackSound);
    }

    public void EnterWalkingState()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Walking;
        an.Play("ZombieWalking");

        idleSoundCoroutine = HandleIdleSound();
        StartCoroutine(idleSoundCoroutine);
    }

    private IEnumerator HandleIdleSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleSoundInterval);
            aus.PlayOneShot(idleSound);
        }
    }

    public void GetHit(Vector3 playerTransformForward)
    {
        if (currentState == EnemyState.Dead) return;
        
        an.applyRootMotion = false;
        isDamageTaking = true;
        
        health -= 3;
        healthBar.value = health;
        if (CheckForDeath()) return;
        
        an.Play("Zombie Reaction Hit More");
        rb.AddForce(knockbackForce * playerTransformForward, ForceMode.Acceleration);
        Invoke(nameof(ResetAfterDamage), damageTakingAnimTime);
    }
    
    private void ResetAfterDamage()
    {
        an.applyRootMotion = true;
        isDamageTaking = false;
    }
    
    private bool CheckForDeath()
    {
        StopCoroutine(idleSoundCoroutine);
        aus.Stop();
        
        if (health < 0)
        {
            currentState = EnemyState.Dead;
            an.Play("Zombie Death");
            
            aus.PlayOneShot(deathSound);
            
            aliveCollider.enabled = false;
            deadCollider.enabled = true;

            healthBar.gameObject.SetActive(false);
            
            return true;
        }

        else
        {
            aus.PlayOneShot(damageSound);
            return false;
        }
    }
}