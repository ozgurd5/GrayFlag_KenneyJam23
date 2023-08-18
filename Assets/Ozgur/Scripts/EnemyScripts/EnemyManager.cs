using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private int health; //10 zombie - 15 skeleton
    [SerializeField] private int defaultTakenDamage = 3;
    [SerializeField] private int powerUpTakenDamage = 5;
    [SerializeField] private int knockBackForce = 2500;
    [SerializeField] private float damageTakingAnimTime = 0.8f;
    public float attackPrepareTime; //1 zombie - 0.7 skeleton
    
    [Header("Assign - Colliders")]
    [SerializeField] private Collider aliveCollider;
    [SerializeField] private Collider deadCollider;

    [Header("Assign - Sounds")]
    [SerializeField] private float idleSoundTimeInterval = 2f;
    [SerializeField] private float idleSoundProbability = 20f;
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip idleSound;

    private Rigidbody rb;
    private Animator an;
    private Slider healthBar;
    private GameObject player;
    
    private RaycastHit hit;
    private IEnumerator idleSoundCoroutine;
    private IEnumerator attackRoutine;
    
    private int takenDamage;

    public enum EnemyState
    {
        Walking,
        Running,
        Attack,
        GettingDamage,
        Dead
    }
    
    [Header("Info - No Touch")]
    public EnemyState currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
        healthBar = GetComponentInChildren<Slider>();
        player = GameObject.Find("Player");
        
        idleSoundCoroutine = HandleIdleSound();
        attackRoutine = EnterAttackState();
        
        takenDamage = defaultTakenDamage;
        MarketManager.OnChickenBought += IncreaseTakenDamage;
    }

    public void AttackPlayer()
    {
        if (currentState == EnemyState.Dead) return;
        
        attackRoutine = EnterAttackState();
        StartCoroutine(attackRoutine);
    }

    public void StopAttack()
    {
        StopCoroutine(attackRoutine);
    }

    private IEnumerator EnterAttackState()
    {
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
     
        StopCoroutine(idleSoundCoroutine);
        currentState = EnemyState.Running;
        an.Play("EnemyRunning");
    }

    private IEnumerator HandleIdleSound()
    {
        while (true)
        {
            bool isSoundPlayed = false;
            while (!isSoundPlayed)
            {
                if (GetProbability(idleSoundProbability))
                {
                    aus.PlayOneShot(idleSound);
                    isSoundPlayed = true;
                }

                yield return new WaitForSeconds(idleSoundTimeInterval);
            }
        }
    }

    private bool GetProbability(float probability)
    {
        float random = Random.Range(0, 100);

        if (random <= probability) return true;
        else return false;
    }

    public void GetHit(Vector3 playerTransformForward)
    {
        if (currentState == EnemyState.Dead) return;

        StopAttack();
        currentState = EnemyState.GettingDamage;

        health -= takenDamage;
        healthBar.value = health;
        if (CheckForDeath()) return;
        
        an.Play("EnemyGetHit");
        rb.AddForce(knockBackForce * playerTransformForward, ForceMode.Acceleration);
        Invoke(nameof(ResetTakingDamage), damageTakingAnimTime);
    }
    
    private void ResetTakingDamage()
    {
        if (currentState == EnemyState.Dead) return;
        
        currentState = EnemyState.Walking;
        rb.velocity = Vector3.zero;
    }
    
    private bool CheckForDeath()
    {
        aus.Stop();
        
        if (health <= 0)
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

    private void IncreaseTakenDamage()
    {
        takenDamage = powerUpTakenDamage;
    }

    private void OnDestroy()
    {
        MarketManager.OnChickenBought -= IncreaseTakenDamage;
    }
}