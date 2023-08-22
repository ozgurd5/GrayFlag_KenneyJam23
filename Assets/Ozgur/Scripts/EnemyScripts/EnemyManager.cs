using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private int health; //100 zombie - 150 skeleton
    [SerializeField] private int damage; //20 zombie - 25 skeleton
    [SerializeField] private int knockBackForce = 300;
    [SerializeField] private float damageTakingAnimTime = 0.6f;
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
    private IEnumerator attackCoroutine;
    private IEnumerator resetTakingDamageCoroutine;

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
        attackCoroutine = AttackPlayer();
        resetTakingDamageCoroutine = ResetTakingDamage();
    }

    public void EnterAttackState()
    {
        if (currentState == EnemyState.Dead) return;
        
        attackCoroutine = AttackPlayer();
        StartCoroutine(attackCoroutine);
    }

    public void StopAttack()
    {
        StopCoroutine(attackCoroutine);
    }

    private IEnumerator AttackPlayer()
    {
        aus.Stop();
        
        currentState = EnemyState.Attack;
        an.Play("EnemyAttack");
        
        yield return new WaitForSeconds(attackPrepareTime);

        player.GetComponent<PlayerDamageManager>().GetHit(transform.forward, damage);
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

    public void GetHit(Vector3 playerTransformForward, int takenDamage)
    {
        if (currentState == EnemyState.Dead) return;
        
        StopAttack();
        currentState = EnemyState.GettingDamage;

        health -= takenDamage;
        healthBar.value = health;
        if (CheckForDeath()) return;
        
        an.Play("EnemyGetHit", 0, 0.2f);
        rb.AddForce(knockBackForce * playerTransformForward, ForceMode.Acceleration);

        StopCoroutine(resetTakingDamageCoroutine);
        resetTakingDamageCoroutine = ResetTakingDamage();
        StartCoroutine(resetTakingDamageCoroutine);
    }
    
    private IEnumerator ResetTakingDamage()
    {
        if (currentState == EnemyState.Dead) yield break;

        yield return new WaitForSeconds(damageTakingAnimTime);
        
        currentState = EnemyState.Walking;
        rb.velocity = Vector3.zero;
    }
    
    private bool CheckForDeath()
    {
        aus.Stop();
        
        if (health <= 0)
        {
            StopCoroutine(resetTakingDamageCoroutine);
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