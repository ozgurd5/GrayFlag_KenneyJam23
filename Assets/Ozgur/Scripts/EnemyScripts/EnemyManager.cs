using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private int health = 10;
    [SerializeField] private int knockbackForce = 2500;
    [SerializeField] private float damageTakingAnimTime = 0.8f;
    public float attackPrepareTime = 0.5f;
    
    [Header("Assign - Colliders")]
    [SerializeField] private Collider aliveCollider;
    [SerializeField] private Collider deadCollider;

    [Header("Assign - Sounds")]
    [SerializeField] private float idleSoundInterval = 10f;
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip idleSound;

    private Rigidbody rb;
    private Animator an;
    private Slider healthBar;
    
    private GameObject player;
    private IEnumerator idleSoundCoroutine;
    private RaycastHit hit;
    
    public enum EnemyState
    {
        Walking,
        Running,
        Attack,
        Dead
    }

    public EnemyState currentState;
    public bool isTakingDamage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
        healthBar = GetComponentInChildren<Slider>();
        
        player = GameObject.Find("Player");
        idleSoundCoroutine = HandleIdleSound();
    }

    public IEnumerator EnterAttackState()
    {
        if (currentState == EnemyState.Dead) yield break;
        
        StopCoroutine(idleSoundCoroutine);
        aus.Stop();
        
        currentState = EnemyState.Attack;
        an.Play("EnemyAttack");

        yield return new WaitForSeconds(attackPrepareTime);
        
        player.GetComponent<PlayerDamageManager>().GetHit(transform.forward);
        aus.PlayOneShot(attackSound);
    }

    public void EnterWalkingState()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Walking;
        an.Play("EnemyWalking");

        idleSoundCoroutine = HandleIdleSound();
        StartCoroutine(idleSoundCoroutine);
    }
    
    public void EnterRunningState()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Running;
        an.Play("EnemyRunning");

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
        isTakingDamage = true;
        
        health -= 3;
        healthBar.value = health;
        if (CheckForDeath()) return;
        
        an.Play("EnemyGetHit");
        rb.AddForce(knockbackForce * playerTransformForward, ForceMode.Acceleration);
        Invoke(nameof(ResetAfterDamage), damageTakingAnimTime);
    }
    
    private void ResetAfterDamage()
    {
        an.applyRootMotion = true;
        isTakingDamage = false;
        
        EnterWalkingState();
    }
    
    private bool CheckForDeath()
    {
        StopCoroutine(idleSoundCoroutine);
        aus.Stop();
        
        if (health < 0)
        {
            currentState = EnemyState.Dead;
            an.Play("EnemyDeath");
            
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